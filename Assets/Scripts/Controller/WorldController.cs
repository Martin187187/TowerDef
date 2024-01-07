using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class WorldController : MonoBehaviour
{

    public Vector2Int start = new Vector2Int(0, 0);
    public Vector2Int size = new Vector2Int(10, 10);
    public Vector2Int boundary = new Vector2Int(2, 0);
    public bool[,] map;
    public Turret[,] turrets;
    public Base basis;
    public int[,] heatmap;
    public Tilemap tilemap;
    public TileBase defaultTile;
    public ShowTurretsUI prefabToPlace;

    public int turretCost = 100;
    public UIStateManager state;

    public GameObject prefab;
    private EntityManager manager;
    void Start()
    {
        manager = EntityManager.Instance;
        map = new bool[size.x, size.y];
        heatmap = new int[size.x, size.y];
        turrets = new Turret[size.x, size.y];

        for (int i = 0; i < size.x; i++)
        {
            
            for (int j = 0; j < size.y; j++)
            {
                Vector3Int position = new Vector3Int(i + start.x, j + start.y, 0);
                //tilemap.SetTile(position, defaultTile);
                TileBase tile = tilemap.GetTile(position);
                map[i, j] = !defaultTile.Equals(tile);
                if (!defaultTile.Equals(tile) && i > 1 && i < (size.x -2))
                {
                    var block =Instantiate(prefab, new Vector3(i + start.x, 0.15f, j + start.y), Quaternion.identity);
                    block.transform.parent = transform;
                }
                heatmap[i, j] = 0;

            }
        }


        int amount = GameObject.FindGameObjectsWithTag("Turret").Length;


    }



    void Update()
    {

        // Check for left mouse button click
        if (Stater.TURRET_PLACEMENT == state.GetState() && Input.GetMouseButtonDown(0))
        {

            state.SetState(Stater.NONE);

            int cost = prefabToPlace.turret.GetComponent<Turret>().baseCost;
            // Check if the main camera exists
            if (Camera.main != null && cost <= manager.GetMoney())
            {

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                // Define the plane where z = 0 or z = 1
                Plane plane = new Plane(Vector3.up, -0.4f); // Z = 0 plane

                // Check if the ray intersects with the plane
                if (plane.Raycast(ray, out float distance))
                {
                    // Get the intersection point
                    Vector3 mousePosition = ray.GetPoint(distance);

                    // Do something with the intersection point (e.g., visualize it or perform actions)
                    Debug.DrawRay(ray.origin, ray.direction * distance, Color.green);
                    Debug.Log("Intersection Point: " + mousePosition);

                    // Round the position to integers
                    Vector3Int roundedPosition = new Vector3Int(Mathf.RoundToInt(mousePosition.x), 0, Mathf.RoundToInt(mousePosition.z));

                    Vector2Int index = new Vector2Int(roundedPosition.x - start.x, roundedPosition.z - start.y);

                    if (index.x >= 2 && index.x < size.x-2 && index.y >=0 && index.y < size.y && turrets[index.x, index.y] == null && map[index.x, index.y])
                    {
                        var turretGameObject = Instantiate(prefabToPlace.turret, new Vector3(roundedPosition.x, mousePosition.y, roundedPosition.z), Quaternion.identity);
                        turretGameObject.transform.SetParent(manager.getTurretsTransform());
                        Turret turret = turretGameObject.GetComponentInChildren<Turret>();
                        turrets[index.x, index.y] = turret;
                        manager.SetMoney(manager.GetMoney() - cost);
                        
                    }
                }
            }
        }
    }


    public Vector2Int getCellLocation(Vector3 position)
    {
        
        return new Vector2Int((int)position.x, (int)position.z);
    }

    public List<Vector3> FindPath(Vector3 startPos, Vector3 goalPos)
    {
        Vector2Int a = getCellLocation(startPos);
        Vector2Int b = getCellLocation(goalPos);
        
        var res = FindPath(a, b);
        if (res == null)
            return new List<Vector3>();
        List<Vector3> result = new List<Vector3>();
        for (int i = 0; i < res.Count; i++)
        {
            Vector2Int pos = res[i];
            result.Add(new Vector3Int(pos.x, 0, pos.y));
        }
        return result;
    }


    // A* Pathfinding Algorithm
    private List<Vector2Int> FindPath(Vector2Int startPos, Vector2Int goalPos)
    {
        if (!IsWithinBounds(startPos) || !IsWithinBounds(goalPos))
        {
            Debug.LogError("Start or goal position is outside the map bounds.");
            return null;
        }

        // Create a priority queue for open nodes
        PriorityQueue<Vector2Int> openSet = new PriorityQueue<Vector2Int>();
        openSet.Enqueue(startPos, 0);

        // Create dictionaries to store costs and parents
        Dictionary<Vector2Int, float> gScore = new Dictionary<Vector2Int, float>();
        Dictionary<Vector2Int, Vector2Int> cameFrom = new Dictionary<Vector2Int, Vector2Int>();

        gScore[startPos] = 0;

        int counter = 0;
        // Main pathfinding loop
        while (openSet.Count > 0 && counter++ < 1000)
        {
            Vector2Int current = openSet.Dequeue();

            if (current == goalPos)
            {
                // Reconstruct the path
                return ReconstructPath(cameFrom, current);
            }

            foreach (Vector2Int neighbor in GetNeighbors(current))
            {
                float tentativeGScore = gScore[current] + Mathf.Max(0, heatmap[neighbor.x - start.x, neighbor.y - start.y]) + 1;

                if (!gScore.ContainsKey(neighbor) || tentativeGScore < gScore[neighbor])
                {
                    gScore[neighbor] = tentativeGScore;
                    float priority = tentativeGScore + HeuristicCostEstimate(neighbor, goalPos);

                    openSet.Enqueue(neighbor, priority);
                    cameFrom[neighbor] = current;
                }
            }
        }

        // No path found
        Debug.Log("No path found");
        return null;
    }

    // Check if a position is within the map bounds
    private bool IsWithinBounds(Vector2Int pos)
    {
        return pos.x >= start.x && pos.x < start.x + size.x &&
               pos.y >= start.y && pos.y < start.y + size.y;
    }

    // Get neighboring positions
    private List<Vector2Int> GetNeighbors(Vector2Int pos)
    {
        List<Vector2Int> neighbors = new List<Vector2Int>();

        // Assuming 4-connected neighbors
        neighbors.Add(new Vector2Int(pos.x + 1, pos.y));
        neighbors.Add(new Vector2Int(pos.x - 1, pos.y));
        neighbors.Add(new Vector2Int(pos.x, pos.y + 1));
        neighbors.Add(new Vector2Int(pos.x, pos.y - 1));

        // Filter out positions outside the map bounds
        neighbors.RemoveAll(n => !IsWithinBounds(n));

        // Filter out blocked positions
        neighbors.RemoveAll(n => map[n.x - start.x, n.y - start.y]);

        return neighbors;
    }

    // Heuristic function (Euclidean distance)
    private float HeuristicCostEstimate(Vector2Int a, Vector2Int b)
    {
        return Vector2Int.Distance(a, b);
    }

    // Reconstruct the path from the goal to the start
    private List<Vector2Int> ReconstructPath(Dictionary<Vector2Int, Vector2Int> cameFrom, Vector2Int current)
    {
        List<Vector2Int> path = new List<Vector2Int>();
        while (cameFrom.ContainsKey(current))
        {
            path.Add(current);
            current = cameFrom[current];
        }
        path.Reverse();
        return path;
    }

    // Priority queue implementation
    private class PriorityQueue<T>
    {
        private List<T> elements = new List<T>();
        private List<float> priorities = new List<float>();

        public int Count { get { return elements.Count; } }

        public void Enqueue(T element, float priority)
        {
            elements.Add(element);
            priorities.Add(priority);
        }

        public T Dequeue()
        {
            if (Count == 0)
                throw new System.Exception("Queue is empty");

            int index = 0;
            float highestPriority = priorities[0];

            for (int i = 1; i < priorities.Count; i++)
            {
                if (priorities[i] < highestPriority)
                {
                    index = i;
                    highestPriority = priorities[i];
                }
            }

            T element = elements[index];
            elements.RemoveAt(index);
            priorities.RemoveAt(index);
            return element;
        }
    }
}
