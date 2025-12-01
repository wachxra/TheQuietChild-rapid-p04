using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    [Header("Text List System")]
    public string[] textList;
    public bool playOnlyOnce = true;
    private bool listPlayed = false;

    private bool isPlayingText = false;
    private int currentTextIndex = 0;

    public static TextMeshProUGUI sharedTextUI;
    public TextMeshProUGUI textUI;

    private Button[] cachedButtons;

    private int currentIndex = 0;
    private bool switchingEnabled = false;

    private bool waitForNextKey = false;
    private CanvasGroup panelCanvasGroup;

    void Start()
    {
        if (upButton != null) upButton.onClick.AddListener(SwitchUpUI);
        if (downButton != null) downButton.onClick.AddListener(SwitchDownUI);
        if (backButton != null) backButton.onClick.AddListener(BackUI);

        if (sharedTextUI == null && textUI != null)
            sharedTextUI = textUI;

        GameObject activePanel = GetActivePanel();
        if (activePanel != null)
        {
            panelCanvasGroup = activePanel.GetComponent<CanvasGroup>();
            if (panelCanvasGroup == null)
            {
                panelCanvasGroup = activePanel.AddComponent<CanvasGroup>();
            }
        }
    }

    void Update()
    {
        if (isPlayingText)
        {
            if (!waitForNextKey)
            {
                waitForNextKey = true;
            }
            else
            {
                if (Input.anyKeyDown && !Input.GetMouseButtonDown(0) && !Input.GetMouseButtonDown(1) && !Input.GetMouseButtonDown(2))
                {
                    ContinueText();
                }
            }

            if (cachedButtons != null)
            {
                foreach (var b in cachedButtons)
                    b.interactable = false;
            }

            if (panelCanvasGroup != null)
            {
                panelCanvasGroup.blocksRaycasts = false;
            }
        }
        else
        {
            if (panelCanvasGroup != null)
            {
                panelCanvasGroup.blocksRaycasts = true;
            }
        }

        if (!switchingEnabled) return;
        if (Input.GetKeyDown(KeyCode.UpArrow)) SwitchPanel(-1);
        else if (Input.GetKeyDown(KeyCode.DownArrow)) SwitchPanel(1);
    }

    public void PlayTextList(bool ignorePlayOnce = false)
    {
        if (textList == null || textList.Length == 0)
            return;

        if (playOnlyOnce && listPlayed && !ignorePlayOnce)
            return;

        listPlayed = true;
        isPlayingText = true;
        currentTextIndex = 0;
        waitForNextKey = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        ShowCurrentText();
    }

    private void EndText()
    {
        isPlayingText = false;

        waitForNextKey = false;

        if (sharedTextUI != null)
            sharedTextUI.gameObject.SetActive(false);

        if (panelCanvasGroup != null)
            panelCanvasGroup.blocksRaycasts = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void ShowCurrentText()
    {
        if (sharedTextUI == null) return;

        sharedTextUI.gameObject.SetActive(true);
        sharedTextUI.text = textList[currentTextIndex];
    }

    public void ContinueText()
    {
        if (!isPlayingText) return;

        currentTextIndex++;

        if (currentTextIndex >= textList.Length)
        {
            EndText();
            return;
        }

        ShowCurrentText();
    }

    private GameObject GetActivePanel()
    {
        if (switchablePanels != null)
        {
            foreach (var p in switchablePanels)
                if (p.activeSelf) return p;
        }

        if (panelToOpen != null && panelToOpen.activeSelf)
            return panelToOpen;

        return null;
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

    private void SwitchPanel(int direction)
    {
        if (switchablePanels == null || switchablePanels.Length == 0) return;

        switchablePanels[currentIndex].SetActive(false);

        currentIndex += direction;
        if (currentIndex < 0)
            currentIndex = switchablePanels.Length - 1;
        else if (currentIndex >= switchablePanels.Length)
            currentIndex = 0;

        switchablePanels[currentIndex].SetActive(true);

        if (UIManager.Instance != null)
            UIManager.Instance.TogglePuzzlePanel(switchablePanels[currentIndex]);
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
                        Debug.Log("Cannot open door yet! Collect all pages first");
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

                    PlayTextList(false);
                }
                else
                {
                    switchingEnabled = false;

                    if (upButton != null) upButton.gameObject.SetActive(false);
                    if (downButton != null) downButton.gameObject.SetActive(false);
                    if (backButton != null) backButton.gameObject.SetActive(true);

                    if (panelToOpen != null)
                        UIManager.Instance.TogglePuzzlePanel(panelToOpen);

                    PlayTextList(false);
                }
                break;
        }
    }
}