using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SnapScrolling : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public ScrollRect scrollRect;
    public RectTransform content;
    public RectTransform[] items;
    private RectTransform rectTransform;
    public float offset = 0;

    private float targetHorizontalPosition;
    private bool isDragging = false;

    void Start()
    {
        targetHorizontalPosition = content.anchoredPosition.x;
        rectTransform = scrollRect.GetComponent<RectTransform>();
    }

    void Update()
    {
        if (!isDragging)
        {
            LerpToItem(targetHorizontalPosition);
        }
    }

    
    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;
        Debug.Log(rectTransform.sizeDelta.x);
    }

    public void OnDrag(PointerEventData eventData)
    {
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
        FindNearestItem();
    }


    void LerpToItem(float targetPosition)
    {
        float newX = Mathf.Lerp(content.anchoredPosition.x, targetPosition, Time.deltaTime * 10f);
        content.anchoredPosition = new Vector2(newX, 0);
    }

    void FindNearestItem()
    {
        float minDistance = float.MaxValue;
        for (int i = 0; i < items.Length; i++)
        {
            float distance = Mathf.Abs(content.anchoredPosition.x + i * (rectTransform.rect.width+offset)+offset/2);
            if (distance < minDistance)
            {
                minDistance = distance;
                targetHorizontalPosition = -i*(rectTransform.rect.width+offset)-offset/2;
            }
        }
    }

}