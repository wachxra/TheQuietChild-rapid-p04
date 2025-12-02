using UnityEngine;
using UnityEngine.UI;

public class KeyCloset : MonoBehaviour
{
    [Header("Optional Interactable Redirect")]
    public Interactable closetInteractable;
    public GameObject newPanelAfterKey;

    [Header("SpriteRenderer Change")]
    public SpriteRenderer closetSpriteRenderer;
    public Sprite spriteAfterKey;

    [Header("GameObject B Setup")]
    public GameObject gameObjectB;
    public Button openBButton;

    [Header("Switch Panel Setup")]
    public GameObject[] switchablePanels;
    public Button upButton;
    public Button downButton;
    public Button backButton;

    private int currentIndex = 0;
    private bool switchingEnabled = false;

    private bool redirected = false;

    private void Awake()
    {
        if (openBButton != null)
            openBButton.onClick.AddListener(OpenSwitchPanels);

        if (upButton != null)
            upButton.onClick.AddListener(SwitchUpUI);

        if (downButton != null)
            downButton.onClick.AddListener(SwitchDownUI);

        if (backButton != null)
            backButton.onClick.AddListener(BackUI);

        if (gameObjectB != null)
            gameObjectB.SetActive(false);

        if (switchablePanels != null)
        {
            foreach (var p in switchablePanels)
                p.SetActive(false);
        }

        if (upButton != null) upButton.gameObject.SetActive(false);
        if (downButton != null) downButton.gameObject.SetActive(false);
        if (backButton != null) backButton.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (PlantKeyPuzzle.hasKey && !redirected)
        {
            if (closetInteractable != null && newPanelAfterKey != null)
                closetInteractable.panelToOpen = newPanelAfterKey;

            if (closetSpriteRenderer != null && spriteAfterKey != null)
                closetSpriteRenderer.sprite = spriteAfterKey;

            redirected = true;
        }
    }

    private void OpenSwitchPanels()
    {
        if (!PlantKeyPuzzle.hasKey)
        {
            return;
        }

        if (switchablePanels != null && switchablePanels.Length > 0)
        {
            switchingEnabled = true;
            currentIndex = 0;

            foreach (var p in switchablePanels)
                p.SetActive(false);

            switchablePanels[currentIndex].SetActive(true);

            if (upButton != null) upButton.gameObject.SetActive(true);
            if (downButton != null) downButton.gameObject.SetActive(true);
            if (backButton != null) backButton.gameObject.SetActive(true);
        }
    }

    private void SwitchPanel(int direction)
    {
        if (!switchingEnabled || switchablePanels == null || switchablePanels.Length == 0)
            return;

        switchablePanels[currentIndex].SetActive(false);

        currentIndex += direction;
        if (currentIndex < 0)
            currentIndex = switchablePanels.Length - 1;
        else if (currentIndex >= switchablePanels.Length)
            currentIndex = 0;

        switchablePanels[currentIndex].SetActive(true);
    }

    public void SwitchUpUI() => SwitchPanel(-1);
    public void SwitchDownUI() => SwitchPanel(1);

    public void BackUI()
    {
        if (!switchingEnabled) return;

        switchingEnabled = false;

        if (switchablePanels != null)
        {
            foreach (var p in switchablePanels)
                p.SetActive(false);
        }

        if (upButton != null) upButton.gameObject.SetActive(false);
        if (downButton != null) downButton.gameObject.SetActive(false);
        if (backButton != null) backButton.gameObject.SetActive(false);
    }
}