using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class NestedScrollView : MonoBehaviour
{
    public RectTransform rectTransform;
    private RectTransform myRect;

    void Start()
    {
        myRect = GetComponent<RectTransform>();
    }

    void Update()
    {
        myRect.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, myRect.anchoredPosition.y);
    }
}