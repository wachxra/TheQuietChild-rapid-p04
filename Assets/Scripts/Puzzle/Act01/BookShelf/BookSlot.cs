using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BookSlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [HideInInspector] public BookPuzzleManager manager;

    private RectTransform rectTransform;
    private Canvas canvas;
    private Transform originalParent;
    private int originalIndex;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        originalIndex = transform.GetSiblingIndex();
        transform.SetParent(canvas.transform);
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (originalParent == null || manager == null) return;

        int closestIndex = manager.GetClosestIndex(rectTransform.position, this);
        manager.ReorderBook(this, closestIndex);
        transform.SetParent(originalParent);
        rectTransform.anchoredPosition = Vector2.zero;
    }
}