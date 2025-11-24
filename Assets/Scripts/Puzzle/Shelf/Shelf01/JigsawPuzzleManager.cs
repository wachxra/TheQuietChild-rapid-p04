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


    public void OpenPuzzle()
    {
        puzzlePanel.SetActive(true);

        CheckCompletion();
    }

    public void ClosePuzzle()
    {
        puzzlePanel.SetActive(false);
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