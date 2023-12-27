using UnityEngine;
using UnityEngine.UI;

public class ClickHandler : MonoBehaviour
{
    public Text uiText; // Reference to the Text component in the UI

    void Update()
    {
        // Check for mouse click
        if (Input.GetMouseButtonDown(0))
        {
            // Cast a ray from the screen point where the mouse was clicked
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            // Check if the ray hits a collider
            if (hit.collider != null && hit.collider.CompareTag("Turret"))
            {
                Debug.Log("hit");
                // Get the game object that was clicked
                GameObject clickedObject = hit.collider.gameObject;

                // Set the text of the UI component to the name of the clicked game object
                if (uiText != null)
                {
                    uiText.text = clickedObject.name;
                }
                else
                {
                    Debug.LogError("UI Text component is not assigned.");
                }
            }
        }
    }
}
