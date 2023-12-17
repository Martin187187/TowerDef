using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldController : MonoBehaviour
{

    public List<GameObject> enemies = new List<GameObject>();
    public Vector2Int start = new Vector2Int(0,0);
    public Vector2Int size = new Vector2Int(10,10);
    public bool[,] map;
    public Tilemap tilemap;
    public TileBase defaultTile;
    void Start ()
    {
        map = new bool[size.x, size.y];
        
        for(int i = 0; i < size.x; i++){
            for(int j = 0; j < size.y; j++){
                Vector3Int position = new Vector3Int(i+start.x, j+start.y, 0);
                //tilemap.SetTile(position, defaultTile);
                TileBase tile = tilemap.GetTile(position);
                map[i,j] = tile != null;
                
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<Vector3> FindPath(Vector3 startPos, Vector3 goalPos)
    {
        Vector3Int a = tilemap.WorldToCell(startPos);
        Vector3Int b = tilemap.WorldToCell(goalPos);
        var res = FindPath(new Vector2Int(a.x, a.y), new Vector2Int(b.x, b.y));
        if(res == null)
            return new List<Vector3>();
        List<Vector3> result = new List<Vector3>();
        for (int i = 0; i < res.Count; i++)
        {
            Vector2Int pos = res[i];
            result.Add(tilemap.CellToWorld(new Vector3Int(pos.x, pos.y, 0)));
        }
        return result;
    }
    private float getScore(Vector2Int score){
        
        float count = 1;
        foreach (var item in enemies)
        {
            
            
            Vector3Int a = tilemap.WorldToCell(item.transform.position);
            Vector2Int b = new Vector2Int(a.x, a.y);

            float distance = Vector2Int.Distance(score, b);
            if(distance < 1){
                count = count + (1- distance)*0.2f;
            }
        }

        return count;
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

        // Main pathfinding loop
        while (openSet.Count > 0)
        {
            Vector2Int current = openSet.Dequeue();

            if (current == goalPos)
            {
                // Reconstruct the path
                return ReconstructPath(cameFrom, current);
            }

            foreach (Vector2Int neighbor in GetNeighbors(current))
            {
                float tentativeGScore = gScore[current] + getScore(neighbor); // Assuming a cost of 1 to move to a neighboring cell

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

        // You can add diagonal neighbors for 8-connected movement if needed

        // Filter out positions outside the map bounds
        neighbors.RemoveAll(n => !IsWithinBounds(n));
        // Filter out blocked positions
        neighbors.RemoveAll(n => map[n.x-start.x, n.y-start.y]);

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
