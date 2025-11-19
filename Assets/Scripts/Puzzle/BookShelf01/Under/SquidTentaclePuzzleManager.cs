using System.Collections.Generic;
using UnityEngine;

public class SquidTentaclePuzzleManager : MonoBehaviour
{
    [Header("Settings")]
    public int rounds = 5;
    public int sequenceLength = 5;
    public List<int> correctSequence;

    [Header("Tentacle Buttons")]
    public TentacleButton[] buttons;

    [Header("Diary Reference")]
    public Diary diary;

    private int currentIndex = 0;
    private int currentRound = 1;

    public void OnButtonPressed(int buttonID)
    {
        if (currentIndex >= sequenceLength)
            return;

        if (buttonID == correctSequence[currentIndex])
        {
            currentIndex++;
            Debug.Log("Correct press: " + buttonID);

            if (currentIndex >= sequenceLength)
            {
                FreezeCurrentSequence();
                NextRound();
            }
        }
        else
        {
            Debug.Log("Wrong! Reset round.");
            ResetCurrentRound();
        }
    }

    private void FreezeCurrentSequence()
    {
        foreach (var id in correctSequence)
        {
            buttons[id].FreezeButton();
        }
    }

    private void ResetCurrentRound()
    {
        currentIndex = 0;

        foreach (var btn in buttons)
        {
            if (btn.button.interactable) btn.ResetButton();
        }
    }

    private void NextRound()
    {
        currentRound++;
        currentIndex = 0;

        if (currentRound > rounds)
        {
            Debug.Log("Puzzle Complete!");

            if (diary != null)
                diary.AddNextPreparedPage();
        }
        else
        {
            Debug.Log("Start Round " + currentRound);
        }
    }
}