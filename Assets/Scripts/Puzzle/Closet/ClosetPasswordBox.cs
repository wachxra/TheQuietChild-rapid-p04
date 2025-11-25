using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ClosetPasswordBox : MonoBehaviour
{
    [Header("Panel References")]
    public GameObject panelA;
    public GameObject panelB;
    public GameObject panelC;

    [Header("Panel A Button to Change panel")]
    public Button panelAButton;

    [Header("Panel B Button to Change Sprite")]
    public Button panelBButton;
    public Sprite completedSprite;

    [Header("Panel B Input Fields")]
    public TMP_InputField input1;
    public TMP_InputField input2;
    public TMP_InputField input3;
    public TMP_InputField input4;

    [Header("Panel B Submit Button")]
    public Button submitButton;

    [Header("Panel C Button")]
    public Button destroyObjectButton;
    public GameObject objectToDestroy;

    [Header("Password Settings")]
    public string correctPassword = "1234";

    [Header("Diary Reference")]
    public Diary diary;

    [Header("Back Button")]
    public Button backButton;

    private bool passwordUnlocked = false;

    private void Start()
    {
        passwordUnlocked = false;

        SetupInputField(input1);
        SetupInputField(input2);
        SetupInputField(input3);
        SetupInputField(input4);

        panelAButton.onClick.AddListener(OnPanelAClick);
        submitButton.onClick.AddListener(OnSubmitPassword);
        destroyObjectButton.onClick.AddListener(() =>
        {
            if (objectToDestroy != null)
                Destroy(objectToDestroy);

            if (diary != null)
                diary.AddNextPreparedPage();
        });

        if (backButton != null)
        {
            backButton.onClick.AddListener(() =>
            {
                panelB.SetActive(false);
                panelC.SetActive(false);
            });
        }

        panelB.SetActive(false);
        panelC.SetActive(false);
    }

    private void SetupInputField(TMP_InputField input)
    {
        input.contentType = TMP_InputField.ContentType.IntegerNumber;
        input.characterLimit = 1;
        input.onValueChanged.AddListener((value) =>
        {
            if (value.Length > 1)
            {
                input.text = value.Substring(0, 1);
            }
        });
    }

    private void OnPanelAClick()
    {
        if (passwordUnlocked)
        {
            panelC.SetActive(true);
        }
        else
        {
            panelB.SetActive(true);
        }
    }

    private void OnSubmitPassword()
    {
        string enteredPassword = input1.text + input2.text + input3.text + input4.text;

        if (enteredPassword == correctPassword)
        {
            passwordUnlocked = true;

            if (panelBButton != null && completedSprite != null)
            {
                panelBButton.image.sprite = completedSprite;
            }

            panelB.SetActive(false);
            panelC.SetActive(true);
        }
        else
        {
            input1.text = "";
            input2.text = "";
            input3.text = "";
            input4.text = "";
        }
    }
}