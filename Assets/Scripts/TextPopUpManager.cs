using UnityEngine;
using TMPro;

public class TextPopUpManager : MonoBehaviour
{
    public static TextPopUpManager Instance;

    [Header("UI")]
    public GameObject background;
    public TextMeshProUGUI messageText;

    private string[] messages;
    private int index = 0;
    private bool isShowing = false;
    private bool waitForKey = false;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        if (background != null)
            background.SetActive(false);
    }

    void Update()
    {
        if (!isShowing) return;

        if (!waitForKey)
        {
            waitForKey = true;
            return;
        }

        if (Input.anyKeyDown &&
            !Input.GetMouseButtonDown(0) &&
            !Input.GetMouseButtonDown(1) &&
            !Input.GetMouseButtonDown(2))
        {
            NextMessage();
        }
    }

    public static void ShowMessage(string[] msgs)
    {
        if (Instance == null) return;

        Instance.messages = msgs;
        Instance.index = 0;
        Instance.isShowing = true;
        Instance.waitForKey = false;

        Instance.background.SetActive(true);
        Instance.messageText.gameObject.SetActive(true);

        Instance.PlaySFX();
        Instance.messageText.text = msgs[0];

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void NextMessage()
    {
        index++;

        if (index >= messages.Length)
        {
            CloseMessage();
            return;
        }

        messageText.text = messages[index];
        PlaySFX();
    }

    private void PlaySFX()
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlaySFX("Click");
    }

    private void CloseMessage()
    {
        isShowing = false;
        waitForKey = false;

        background.SetActive(false);
        messageText.gameObject.SetActive(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}