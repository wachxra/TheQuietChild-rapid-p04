using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BookSlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [HideInInspector] public BookPuzzleManager manager;
    [HideInInspector] public bool canDrag = true;
    [HideInInspector] public int currentSlotIndex;

    private RectTransform rectTransform;
    private Canvas canvas;

    private GameObject placeholder;
    private Transform parentBeforeDrag;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!canDrag) return;

        parentBeforeDrag = transform.parent;

        placeholder = new GameObject("Placeholder");
        placeholder.name = "Placeholder for " + gameObject.name;

        var rt = placeholder.AddComponent<RectTransform>();
        rt.sizeDelta = rectTransform.sizeDelta;

        placeholder.AddComponent<LayoutElement>();
        placeholder.AddComponent<CanvasGroup>().blocksRaycasts = false;

        placeholder.transform.SetParent(parentBeforeDrag);
        placeholder.transform.SetSiblingIndex(transform.GetSiblingIndex());

        transform.SetParent(canvas.transform);

        LayoutRebuilder.ForceRebuildLayoutImmediate(parentBeforeDrag as RectTransform);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!canDrag) return;

        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;

        float draggedX = rectTransform.position.x;
        int newIndex = parentBeforeDrag.childCount - 1;
        bool indexChanged = false;

        for (int i = 0; i < parentBeforeDrag.childCount; i++)
        {
            var child = parentBeforeDrag.GetChild(i);
            if (child == placeholder.transform) continue;

            float childCenterX = child.position.x;

            if (draggedX < childCenterX)
            {
                newIndex = i;
                break;
            }
        }

        if (placeholder.transform.GetSiblingIndex() != newIndex)
        {
            placeholder.transform.SetSiblingIndex(newIndex);
            indexChanged = true;
        }

        if (indexChanged)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(parentBeforeDrag as RectTransform);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!canDrag || manager == null) return;

        int targetIndex = placeholder.transform.GetSiblingIndex();
        transform.SetParent(parentBeforeDrag);
        transform.SetSiblingIndex(targetIndex);
        manager.SwapBooks(this, targetIndex);

        Destroy(placeholder);
        rectTransform.anchoredPosition = Vector2.zero;
        LayoutRebuilder.ForceRebuildLayoutImmediate(parentBeforeDrag as RectTransform);
    }
}