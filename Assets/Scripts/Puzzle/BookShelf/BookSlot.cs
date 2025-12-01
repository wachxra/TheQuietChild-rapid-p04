using UnityEngine;
using UnityEngine.EventSystems;

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

    void Update()
    {
        if (placeholder != null)
        {
            placeholder.transform.localPosition = Vector3.Lerp(
                placeholder.transform.localPosition,
                parentBeforeDrag.GetChild(placeholder.transform.GetSiblingIndex()).localPosition,
                Time.deltaTime * 15f
            );
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!canDrag) return;

        parentBeforeDrag = transform.parent;

        placeholder = new GameObject("Placeholder");
        var rt = placeholder.AddComponent<RectTransform>();
        rt.sizeDelta = rectTransform.sizeDelta;
        placeholder.transform.SetParent(parentBeforeDrag);
        placeholder.transform.SetSiblingIndex(transform.GetSiblingIndex());

        transform.SetParent(canvas.transform);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!canDrag) return;

        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;

        float draggedX = rectTransform.position.x;

        int newIndex = parentBeforeDrag.childCount - 1;

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

        placeholder.transform.SetSiblingIndex(newIndex);
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
    }
}