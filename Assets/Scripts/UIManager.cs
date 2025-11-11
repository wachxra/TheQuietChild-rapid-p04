using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("UI References")]
    public GameObject dialoguePanel;
    public TMP_Text dialogueText;

    private GameObject currentPuzzlePanel;

    public bool IsUIOpen
    {
        get
        {
            return (dialoguePanel != null && dialoguePanel.activeSelf) ||
                   (currentPuzzlePanel != null && currentPuzzlePanel.activeSelf);
        }
    }

    void Awake()
    {
        Instance = this;

        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);
    }

    public void ToggleDialogue(string message)
    {
        if (dialoguePanel == null || dialogueText == null)
            return;

        if (dialoguePanel.activeSelf)
        {
            CloseDialogue();
        }
        else
        {
            dialoguePanel.SetActive(true);
            dialogueText.text = message;
        }
    }

    public void CloseDialogue()
    {
        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);
    }

    public void TogglePuzzlePanel(GameObject panel)
    {
        if (panel == null) return;

        if (currentPuzzlePanel == panel && panel.activeSelf)
        {
            ClosePuzzlePanel();
        }
        else
        {
            if (currentPuzzlePanel != null)
                currentPuzzlePanel.SetActive(false);

            currentPuzzlePanel = panel;
            panel.SetActive(true);
        }
    }

    public void ClosePuzzlePanel()
    {
        if (currentPuzzlePanel != null)
        {
            currentPuzzlePanel.SetActive(false);
            currentPuzzlePanel = null;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseDialogue();
            ClosePuzzlePanel();
        }
    }
}