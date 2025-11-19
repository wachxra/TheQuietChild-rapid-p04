using UnityEngine;
using UnityEngine.UI;

public enum InteractType
{
    ShowText,
    OpenPanel
}

public class Interactable : MonoBehaviour
{
    public InteractType interactType;

    [TextArea] public string message;

    [Header("Single Panel (Default)")]
    public GameObject panelToOpen;

    [Header("Multiple Panels (Optional)")]
    public GameObject[] switchablePanels;

    [Header("UI Buttons (Optional)")]
    public Button upButton;
    public Button downButton;
    public Button backButton;

    private int currentIndex = 0;
    private bool switchingEnabled = false;

    void Start()
    {
        if (upButton != null)
            upButton.onClick.AddListener(SwitchUpUI);

        if (downButton != null)
            downButton.onClick.AddListener(SwitchDownUI);

        if (backButton != null)
            backButton.onClick.AddListener(BackUI);
    }

    void Update()
    {
        if (!switchingEnabled) return;

        if (Input.GetKeyDown(KeyCode.UpArrow))
            SwitchPanel(-1);
        else if (Input.GetKeyDown(KeyCode.DownArrow))
            SwitchPanel(1);
    }

    public void Interact()
    {
        switch (interactType)
        {
            case InteractType.ShowText:
                UIManager.Instance.ToggleDialogue(message);
                break;

            case InteractType.OpenPanel:
                if (panelToOpen != null && panelToOpen.CompareTag("DoorPanel"))
                {
                    Diary diary = Object.FindFirstObjectByType<Diary>();
                    if (diary != null && !diary.IsAllPreparedPagesCollected())
                    {
                        Debug.Log("Cannot open door yet! Collect all pages first.");
                        return;
                    }
                }

                if (UIManager.Instance != null && UIManager.Instance.IsUIOpen)
                    return;

                if (switchablePanels != null && switchablePanels.Length > 1)
                {
                    switchingEnabled = true;

                    if (upButton != null) upButton.gameObject.SetActive(true);
                    if (downButton != null) downButton.gameObject.SetActive(true);
                    if (backButton != null) backButton.gameObject.SetActive(true);

                    foreach (var p in switchablePanels)
                        p.SetActive(false);

                    currentIndex = 0;
                    switchablePanels[currentIndex].SetActive(true);

                    UIManager.Instance.TogglePuzzlePanel(switchablePanels[currentIndex]);
                }
                else
                {
                    switchingEnabled = false;

                    if (upButton != null) upButton.gameObject.SetActive(false);
                    if (downButton != null) downButton.gameObject.SetActive(false);
                    if (backButton != null) backButton.gameObject.SetActive(true);

                    if (panelToOpen != null)
                    {
                        UIManager.Instance.TogglePuzzlePanel(panelToOpen);
                    }
                }
                break;
        }
    }

    private void SwitchPanel(int direction)
    {
        switchablePanels[currentIndex].SetActive(false);

        currentIndex += direction;
        if (currentIndex < 0)
            currentIndex = switchablePanels.Length - 1;
        else if (currentIndex >= switchablePanels.Length)
            currentIndex = 0;

        switchablePanels[currentIndex].SetActive(true);

        UIManager.Instance.TogglePuzzlePanel(switchablePanels[currentIndex]);
    }

    public void SwitchUpUI()
    {
        if (switchingEnabled)
            SwitchPanel(-1);
    }

    public void SwitchDownUI()
    {
        if (switchingEnabled)
            SwitchPanel(1);
    }

    public void BackUI()
    {
        switchingEnabled = false;

        if (upButton != null) upButton.gameObject.SetActive(false);
        if (downButton != null) downButton.gameObject.SetActive(false);
        if (backButton != null) backButton.gameObject.SetActive(false);

        if (switchablePanels != null && switchablePanels.Length > 0)
        {
            foreach (var p in switchablePanels)
                p.SetActive(false);
        }

        if (panelToOpen != null)
            panelToOpen.SetActive(false);

        if (UIManager.Instance != null)
        {
            UIManager.Instance.ClosePuzzlePanel();
            UIManager.Instance.CloseDialogue();
        }
    }
}