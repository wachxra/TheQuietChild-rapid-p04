using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BookSlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [HideInInspector] public BookPuzzleManager manager;
    [HideInInspector] public int bookIndex;
    [HideInInspector] public bool canDrag = true;

    private RectTransform rectTransform;
    private Canvas canvas;
    private Transform originalParent;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!canDrag) return;

        originalParent = transform.parent;
        transform.SetParent(canvas.transform);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!canDrag) return;

        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!canDrag || manager == null) return;

        int closestIndex = 0;
        float closestDistance = float.MaxValue;

        for (int i = 0; i < manager.bookSlots.Count; i++)
        {
            var slot = manager.bookSlots[i];
            if (!slot.gameObject.activeSelf) continue;

            float distance = Vector3.Distance(rectTransform.position, slot.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestIndex = slot.transform.GetSiblingIndex();
            }
        }

        manager.SwapBooks(this, closestIndex);

        transform.SetParent(originalParent);
        rectTransform.anchoredPosition = Vector2.zero;
    }
}