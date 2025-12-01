using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class JigsawPuzzleManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject puzzlePanel;

    [Header("Puzzle Slots (Order: 1 to 6)")]
    public List<JigsawSlot> slots = new List<JigsawSlot>();

    [Header("Missing Piece Setup")]
    public GameObject missingPieceObject;

    public Sprite finalPieceSprite;

    private bool hasMissingPiece = false;
    private bool wasPanelActive = false;

    void Start()
    {
        if (slots.Count != 6)
        {
        }
        if (missingPieceObject != null)
        {
            Button collectButton = missingPieceObject.GetComponent<Button>();
            if (collectButton != null)
            {
                collectButton.onClick.AddListener(CollectMissingPiece);
            }
        }
    }

    void Update()
    {
        bool isActive = puzzlePanel.activeSelf;

        if (isActive != wasPanelActive)
        {
            if (isActive) OpenPuzzle();
            else ClosePuzzle();

            wasPanelActive = isActive;
        }
    }

    public void OpenPuzzle()
    {
        puzzlePanel.SetActive(true);

        JigsawPiece[] allPieces = puzzlePanel.GetComponentsInChildren<JigsawPiece>(true);
        foreach (var piece in allPieces)
        {
            var cg = piece.GetComponent<CanvasGroup>();
            if (cg != null) cg.blocksRaycasts = true;
        }

        CheckCompletion();
    }

    public void ClosePuzzle()
    {
        ResetPuzzlePieces();

        puzzlePanel.SetActive(false);

        foreach (var slot in slots)
        {
            if (slot.placedPiece != null)
            {
                var piece = slot.placedPiece;
                var cg = piece.GetComponent<CanvasGroup>();
                if (cg != null) cg.blocksRaycasts = false;
            }
        }

        JigsawPiece[] allPieces = puzzlePanel.GetComponentsInChildren<JigsawPiece>(true);
        foreach (var piece in allPieces)
        {
            var cg = piece.GetComponent<CanvasGroup>();
            if (cg != null) cg.blocksRaycasts = false;
        }
    }

    private void ResetPuzzlePieces()
    {
        bool pieces1To5Correct = CheckPieces1To5();
        if (!(pieces1To5Correct && hasMissingPiece))
        {
            foreach (var slot in slots)
            {
                if (slot.placedPiece != null)
                {
                    slot.placedPiece.ReturnToOriginalPosition();
                    slot.ClearSlot();
                }
            }
        }
    }

    public void CollectMissingPiece()
    {
        hasMissingPiece = true;
        if (missingPieceObject != null)
        {
            Destroy(missingPieceObject);
        }
        Debug.Log("Collected the missing piece (ID 6)!");
        CheckCompletion();
    }

    public void CheckCompletion()
    {
        bool pieces1To5Correct = CheckPieces1To5();

        if (pieces1To5Correct && hasMissingPiece)
        {
            PlaceFinalPieceAutomatically();

            PuzzleCompleted();
        }
        else if (pieces1To5Correct && !hasMissingPiece)
        {
            Debug.Log("Pieces 1-5 are correct! Need to find the last piece.");
        }
    }

    private bool CheckPieces1To5()
    {
        for (int i = 0; i < 5; i++)
        {
            if (slots[i].currentPieceID != slots[i].correctPieceID || slots[i].currentPieceID == 0)
            {
                return false;
            }
        }
        return true;
    }

    private void PlaceFinalPieceAutomatically()
    {
        JigsawSlot finalSlot = slots[5];
        finalSlot.SetPiece(finalSlot.correctPieceID, finalPieceSprite);
        Debug.Log("Final Piece (ID 6) placed automatically.");
    }

    private void PuzzleCompleted()
    {
        Debug.Log("Puzzle Completed");

        for (int i = 0; i < 5; i++)
        {
            if (slots[i].placedPiece != null)
            {
                slots[i].placedPiece.LockPiece(slots[i].transform);
            }
        }

        if (slots[5].placedPiece != null)
        {
            slots[5].placedPiece.LockPiece(slots[5].transform);
        }

        LampPuzzle lampPuzzle = Object.FindFirstObjectByType<LampPuzzle>();

        if (lampPuzzle != null)
        {
            lampPuzzle.ReceiveLampPart(1);
        }
    }
}