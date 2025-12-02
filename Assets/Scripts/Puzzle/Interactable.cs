using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public enum InteractType
{
    ShowText,
    OpenPanel,
    PhaseFinalObject
}

[System.Serializable]
public class TextGroupElement
{
    [TextArea]
    public string[] texts;

    public int panelIndex;
    public bool playOnlyOnce;
    [HideInInspector]
    public bool hasPlayed = false;
}

[System.Serializable]
public class ButtonTextGroup
{
    public Button button;
    [TextArea]
    public string[] texts;
    public bool playOnlyOnce = false;
    [HideInInspector]
    public bool hasPlayed = false;
}

public class Interactable : MonoBehaviour
{
    public InteractType interactType;

    [Header("Single Panel (Default)")]
    public GameObject panelToOpen;

    [Header("Multiple Panels (Optional)")]
    public GameObject[] switchablePanels;

    [Header("UI Buttons (Optional)")]
    public Button upButton;
    public Button downButton;
    public Button backButton;

    [Header("Dialogue")]
    public GameObject textBackground;
    public TextMeshProUGUI textUI;
    public static TextMeshProUGUI sharedTextUI;

    [Header("Text Group System")]
    public TextGroupElement[] textGroups;

    [Header("Button Text Groups")]
    public ButtonTextGroup[] buttonTextGroups;

    [Header("Phase Manager Reference (Optional for PhaseFinalObject)")]
    public PhaseManager phaseManager;
    public GameObject targetObjectToDestroy;

    private bool isPlayingText = false;
    private int currentTextIndex = 0;
    public int currentPanelIndex = 0;
    private int currentGroupIndex = 0;
    private bool switchingEnabled = false;
    private bool waitForNextKey = false;
    private CanvasGroup panelCanvasGroup;

    public static bool IsAnyTextPlaying = false;

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
                panelCanvasGroup = activePanel.AddComponent<CanvasGroup>();
        }

        if (textBackground != null)
            textBackground.SetActive(false);

        foreach (var btg in buttonTextGroups)
        {
            if (btg.button != null)
                btg.button.onClick.AddListener(() => PlayButtonTextGroup(btg));
        }
    }

    void Update()
    {
        if (isPlayingText)
        {
            if (!waitForNextKey)
                waitForNextKey = true;
            else
            {
                if (Input.anyKeyDown &&
                    !Input.GetMouseButtonDown(0) &&
                    !Input.GetMouseButtonDown(1) &&
                    !Input.GetMouseButtonDown(2))
                {
                    ContinueText();
                }
            }

            if (panelCanvasGroup != null)
                panelCanvasGroup.blocksRaycasts = false;
        }
        else
        {
            if (panelCanvasGroup != null)
                panelCanvasGroup.blocksRaycasts = true;
        }

        if (!switchingEnabled) return;
        if (Input.GetKeyDown(KeyCode.UpArrow)) SwitchPanel(-1);
        else if (Input.GetKeyDown(KeyCode.DownArrow)) SwitchPanel(1);
    }

    private bool IsUsingTextGroups()
    {
        return (textGroups != null && textGroups.Length > 0);
    }

    public void PlayTextGroup(bool ignorePlayOnce = false, int groupIndex = -1)
    {
        if (!IsUsingTextGroups()) return;

        if (groupIndex >= 0)
            currentGroupIndex = groupIndex;

        TextGroupElement group = textGroups[currentGroupIndex];

        if (group.playOnlyOnce && group.hasPlayed && !ignorePlayOnce)
        {
            if (interactType == InteractType.PhaseFinalObject)
                group.hasPlayed = false;
            else
                return;
        }

        group.hasPlayed = true;
        isPlayingText = true;
        IsAnyTextPlaying = true;
        currentTextIndex = 0;
        waitForNextKey = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (switchablePanels != null && switchablePanels.Length > 0 &&
            group.panelIndex >= 0 && group.panelIndex < switchablePanels.Length)
        {
            foreach (var p in switchablePanels)
                p.SetActive(false);

            switchablePanels[group.panelIndex].SetActive(true);
            panelCanvasGroup = switchablePanels[group.panelIndex].GetComponent<CanvasGroup>();
            if (panelCanvasGroup == null)
                panelCanvasGroup = switchablePanels[group.panelIndex].AddComponent<CanvasGroup>();
        }
        else if (panelToOpen != null)
        {
            panelToOpen.SetActive(true);
            panelCanvasGroup = panelToOpen.GetComponent<CanvasGroup>();
            if (panelCanvasGroup == null)
                panelCanvasGroup = panelToOpen.AddComponent<CanvasGroup>();
        }

        ShowCurrentText();
    }

    public void PlayButtonTextGroup(ButtonTextGroup btg)
    {
        if (btg.playOnlyOnce && btg.hasPlayed) return;

        btg.hasPlayed = true;
        isPlayingText = true;
        IsAnyTextPlaying = true;
        currentTextIndex = 0;
        waitForNextKey = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (textBackground != null)
            textBackground.SetActive(true);

        if (sharedTextUI != null)
        {
            sharedTextUI.gameObject.SetActive(true);
            sharedTextUI.text = btg.texts[0];
        }

        currentButtonTextGroup = btg;
    }

    private ButtonTextGroup currentButtonTextGroup = null;

    public void ContinueText()
    {
        if (!isPlayingText) return;

        currentTextIndex++;

        if (currentButtonTextGroup != null)
        {
            if (currentTextIndex >= currentButtonTextGroup.texts.Length)
            {
                EndText();
                currentButtonTextGroup = null;
                return;
            }
            sharedTextUI.text = currentButtonTextGroup.texts[currentTextIndex];
            return;
        }

        if (IsUsingTextGroups())
        {
            TextGroupElement group = textGroups[currentGroupIndex];
            if (currentTextIndex >= group.texts.Length)
            {
                EndText();
                return;
            }
        }

        ShowCurrentText();
    }

    private void EndText()
    {
        isPlayingText = false;
        IsAnyTextPlaying = false;
        waitForNextKey = false;
        currentButtonTextGroup = null;

        if (sharedTextUI != null)
            sharedTextUI.gameObject.SetActive(false);

        if (textBackground != null)
            textBackground.SetActive(false);

        if (panelCanvasGroup != null)
            panelCanvasGroup.blocksRaycasts = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // --- สำหรับ PhaseFinalObject ---
        if (interactType == InteractType.PhaseFinalObject && phaseManager != null && phaseManager.CurrentPhaseIsLast())
        {
            if (targetObjectToDestroy != null)
            {
                Destroy(targetObjectToDestroy);
            }
        }
    }

    private void ShowCurrentText()
    {
        if (sharedTextUI == null) return;

        if (textBackground != null)
            textBackground.SetActive(true);

        sharedTextUI.gameObject.SetActive(true);

        if (IsUsingTextGroups())
        {
            TextGroupElement group = textGroups[currentGroupIndex];

            if (currentTextIndex >= 0 && currentTextIndex < group.texts.Length)
                sharedTextUI.text = group.texts[currentTextIndex];
        }
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

        switchablePanels[currentPanelIndex].SetActive(false);

        currentPanelIndex += direction;
        if (currentPanelIndex < 0)
            currentPanelIndex = switchablePanels.Length - 1;
        else if (currentPanelIndex >= switchablePanels.Length)
            currentPanelIndex = 0;

        switchablePanels[currentPanelIndex].SetActive(true);
        currentTextIndex = 0;

        for (int i = 0; i < textGroups.Length; i++)
        {
            if (textGroups[i].panelIndex == currentPanelIndex)
            {
                PlayTextGroup(false, i);
                break;
            }
        }

        if (UIManager.Instance != null)
            UIManager.Instance.TogglePuzzlePanel(switchablePanels[currentPanelIndex]);
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
        if (IsAnyTextPlaying)
            return;

        if (interactType == InteractType.PhaseFinalObject)
        {
            if (textGroups != null && textGroups.Length > 0)
            {
                PlayTextGroup(ignorePlayOnce: true, groupIndex: 0);
            }

            return;
        }

        if (gameObject.CompareTag("Door"))
        {
            Diary diary = Object.FindFirstObjectByType<Diary>();
            if (diary != null && diary.IsAllPreparedPagesCollected())
            {
                SceneManager.LoadScene("Outro");
                return;
            }
            else
            {
                return;
            }
        }

        if (switchablePanels != null && switchablePanels.Length > 0)
        {
            switchingEnabled = true;

            currentPanelIndex = 0;
            foreach (var p in switchablePanels) p.SetActive(false);
            switchablePanels[currentPanelIndex].SetActive(true);

            if (upButton != null) upButton.gameObject.SetActive(true);
            if (downButton != null) downButton.gameObject.SetActive(true);
            if (backButton != null) backButton.gameObject.SetActive(true);

            UIManager.Instance?.TogglePuzzlePanel(switchablePanels[currentPanelIndex]);

            for (int i = 0; i < textGroups.Length; i++)
            {
                if (textGroups[i].panelIndex == currentPanelIndex)
                {
                    PlayTextGroup(false, i);
                    break;
                }
            }
        }
        else if (panelToOpen != null)
        {
            panelToOpen.SetActive(true);
            switchingEnabled = false;
            UIManager.Instance?.TogglePuzzlePanel(panelToOpen);
        }
    }
}