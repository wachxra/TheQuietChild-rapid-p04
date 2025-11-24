using UnityEngine;
using UnityEngine.UI;

public class JigsawSlot : MonoBehaviour
{
    public int correctPieceID;

    [HideInInspector]
    public int currentPieceID = 0;

    [HideInInspector]
    public JigsawPiece placedPiece = null;

    private Image slotImage;

    void Awake()
    {
        slotImage = GetComponent<Image>();
        if (slotImage != null)
        {
            slotImage.color = new Color(1, 1, 1, 0);
        }
    }

    public void OnPiecePlaced(JigsawPiece piece, Sprite pieceSprite)
    {
        if (placedPiece != null)
        {
            placedPiece.ReturnToOriginalPosition();
        }

        placedPiece = piece;
        currentPieceID = piece.pieceID;
        SetPiece(piece.pieceID, pieceSprite);

        var img = piece.GetComponent<Image>();
        if (img != null)
        {
            var color = img.color;
            color.a = 0f;
            img.color = color;
        }

        UnityEngine.Object.FindAnyObjectByType<JigsawPuzzleManager>().CheckCompletion();
    }

    public void ClearSlot()
    {
        if (placedPiece != null)
        {
            var img = placedPiece.GetComponent<Image>();
            if (img != null)
            {
                var color = img.color;
                color.a = 1f;
                img.color = color;
            }
        }

        placedPiece = null;
        currentPieceID = 0;

        if (slotImage != null)
        {
            slotImage.color = new Color(1, 1, 1, 0);
        }
    }

    public void SetPiece(int pieceID, Sprite pieceSprite)
    {
        currentPieceID = pieceID;
        if (slotImage != null)
        {
            slotImage.sprite = pieceSprite;
            slotImage.color = new Color(1, 1, 1, 1);
        }
    }
}