using UnityEngine;
using UnityEngine.EventSystems;

public class BookSlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Book Data")]
    public int bookNumber;
    [HideInInspector] public BookPuzzleManager manager;

    private Transform originalParent;
    private int originalIndex;
    private Canvas canvas;
    private RectTransform rectTransform;

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
        if (rectTransform == null || canvas == null) return;
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (originalParent == null || manager == null) return;

        foreach (Transform child in originalParent)
        {
            if (child == transform) continue;

            RectTransform otherRect = child.GetComponent<RectTransform>();
            if (RectTransformUtility.RectangleContainsScreenPoint(otherRect, eventData.position))
            {
                manager.SwapBooks(this, child.GetComponent<BookSlot>());
                transform.SetParent(originalParent);
                transform.SetSiblingIndex(originalIndex);
                return;
            }
        }

        transform.SetParent(originalParent);
        transform.SetSiblingIndex(originalIndex);
    }
}