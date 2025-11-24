using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JigsawPiece : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public int pieceID;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector2 originalPosition;
    private Sprite pieceSprite;
    private Transform originalParent;
    private Transform canvasRoot;

    private JigsawSlot currentSlot = null;
    private bool isLocked = false;

    public bool IsLocked => isLocked;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        pieceSprite = GetComponent<Image>().sprite;
        originalPosition = rectTransform.anchoredPosition;

        originalParent = transform.parent;
        canvasRoot = transform.root;

        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        var manager = UnityEngine.Object.FindAnyObjectByType<JigsawPuzzleManager>();
        if (manager != null && !manager.puzzlePanel.activeSelf) return;

        if (isLocked) return;

        if (currentSlot != null)
        {
            currentSlot.ClearSlot();
            currentSlot = null;
        }

        originalPosition = rectTransform.anchoredPosition;

        if (canvasGroup != null)
        {
            canvasGroup.blocksRaycasts = false;
        }

        transform.SetParent(canvasRoot);
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        var manager = UnityEngine.Object.FindAnyObjectByType<JigsawPuzzleManager>();
        if (manager != null && !manager.puzzlePanel.activeSelf) return;

        if (isLocked) return;

        rectTransform.anchoredPosition += eventData.delta / transform.root.GetComponent<Canvas>().scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        var manager = UnityEngine.Object.FindAnyObjectByType<JigsawPuzzleManager>();
        if (manager != null && !manager.puzzlePanel.activeSelf) return;

        if (isLocked) return;

        if (canvasGroup != null)
        {
            canvasGroup.blocksRaycasts = true;
        }

        GameObject droppedSlot = eventData.pointerCurrentRaycast.gameObject;
        JigsawSlot targetSlot = (droppedSlot != null) ? droppedSlot.GetComponent<JigsawSlot>() : null;

        if (targetSlot != null)
        {
            targetSlot.OnPiecePlaced(this, pieceSprite);
            currentSlot = targetSlot;

            return;
        }

        ReturnToOriginalPosition();
    }

    public void LockPiece(Transform slotTransform)
    {
        isLocked = true;
        rectTransform.SetParent(slotTransform);
        rectTransform.anchoredPosition = Vector2.zero;

        if (GetComponent<Image>() != null)
        {
            GetComponent<Image>().enabled = false;
        }
    }

    public void UnlockPiece()
    {
        isLocked = false;
        if (GetComponent<Image>() != null)
        {
            GetComponent<Image>().enabled = true;
        }
    }

    public void ReturnToOriginalPosition()
    {
        isLocked = false;

        rectTransform.SetParent(originalParent);
        rectTransform.anchoredPosition = originalPosition;
    }
}