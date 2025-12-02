using System.Collections.Generic;
using UnityEngine;

public class SquidTentaclePuzzleManager : MonoBehaviour
{
    public int rounds = 5;
    public List<int> correctSequence;
    public TentacleButton[] buttons;
    public Diary diary;
    private List<int> playerInputs = new List<int>();

    private bool isPuzzleCompleted = false;

    public void OnButtonPressed(int buttonID)
    {
        if (playerInputs.Count >= correctSequence.Count) return;
        playerInputs.Add(buttonID);
        Debug.Log("Pressed: " + buttonID);

        if (playerInputs.Count == correctSequence.Count)
            CheckSequence();
    }

    private void CheckSequence()
    {
        bool isCorrect = true;

        for (int i = 0; i < correctSequence.Count; i++)
        {
            Debug.Log("Compare index " + i + " → Player: " + playerInputs[i] + " | Correct: " + correctSequence[i]);
            if (playerInputs[i] != correctSequence[i])
            {
                isCorrect = false;
                break;
            }
        }

        if (isCorrect)
        {
            Debug.Log("Puzzle Completed");
            isPuzzleCompleted = true;
            FreezeCurrentSequence();

            LampPuzzle lampPuzzle = Object.FindFirstObjectByType<LampPuzzle>();

            if (lampPuzzle != null)
            {
                lampPuzzle.ReceiveLampPart(2);
            }
        }
        else
        {
            Debug.Log("Puzzle Wrong");
            ResetCurrentRound();
        }

        playerInputs.Clear();
    }

    private void FreezeCurrentSequence()
    {
        foreach (var id in correctSequence)
        {
            if (id >= 0 && id < buttons.Length)
                buttons[id].FreezeButton();
        }
    }

    private void ResetCurrentRound()
    {
        foreach (var btn in buttons)
        {
            btn.ResetButton();
        }
    }

    public void ResetTentaclePuzzle()
    {
        if (isPuzzleCompleted)
        {
            return;
        }

        ResetCurrentRound();
        playerInputs.Clear();
    }
}