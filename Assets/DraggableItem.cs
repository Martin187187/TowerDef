using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{

    public GameObject draggedItemPrefab;
    private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private GameObject draggedItem;
    private Button button;
    public AbstractEffect content;
    private void Start()
    {
        canvas = GetComponentInParent<Canvas>();
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        button = GetComponent<Button>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        
        button.interactable = false;
        // Instantiate the dragged item
        draggedItem = Instantiate(draggedItemPrefab, canvas.transform);

        // Set its position to the initial position of the item being dragged
        draggedItem.GetComponent<RectTransform>().position = rectTransform.position;

        // Disable raycasting for the dragged item (to prevent it from blocking raycasts)
        draggedItem.GetComponent<CanvasGroup>().blocksRaycasts = false;
        if(content && content.sprite)
            draggedItem.GetComponent<Image>().overrideSprite = content.sprite;
        else
            draggedItem.GetComponent<Image>().overrideSprite = GetComponent<Image>().sprite;

        // Make the original item invisible while dragging
        canvasGroup.alpha = 0.5f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Update the position of the dragged item
        draggedItem.GetComponent<RectTransform>().position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        
        button.interactable = true;
        Destroy(draggedItem);

        // Restore visibility of the original item
        canvasGroup.alpha = 1f;
    }
    public void EndDrag()
    {
        
        if(draggedItem)
            Destroy(draggedItem);

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Handle button click (optional, you can remove this if you don't need it)
        Debug.Log("Button Clicked");
    }
}
