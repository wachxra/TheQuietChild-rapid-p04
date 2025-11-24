using System.Collections.Generic;
using UnityEngine;

public class SquidTentaclePuzzleManager : MonoBehaviour
{
    public int rounds = 5;
    public List<int> correctSequence;
    public TentacleButton[] buttons;
    public Diary diary;

    private int currentRound = 1;
    private List<int> playerInputs = new List<int>();

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
            Debug.Log("Puzzle Correct for Round: " + currentRound);
            FreezeCurrentSequence();
            NextRound();
        }
        else
        {
            Debug.Log("Puzzle Wrong → Reset Round: " + currentRound);
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

    private void NextRound()
    {
        currentRound++;
        Debug.Log("Next Round: " + currentRound);

        if (currentRound > rounds)
        {
            Debug.Log("Puzzle Completed All Rounds.");
            if (diary != null) diary.AddNextPreparedPage();
        }
    }
}