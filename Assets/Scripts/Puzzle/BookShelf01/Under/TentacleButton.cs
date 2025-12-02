using UnityEngine;
using UnityEngine.UI;

public class TentacleButton : MonoBehaviour
{
    public int buttonID;
    private SquidTentaclePuzzleManager manager;
    public Button button;

    [Header("Pressed Image (child object)")]
    public GameObject pressedImage;

    private void Awake()
    {
        button = GetComponent<Button>();
        manager = Object.FindFirstObjectByType<SquidTentaclePuzzleManager>();

        if (pressedImage != null)
            pressedImage.SetActive(false);

        button.onClick.AddListener(OnClick);
    }

    public void OnClick()
    {
        if (manager != null)
        {
            if (AudioManager.Instance != null)
                AudioManager.Instance.PlaySFX("Click");

            if (button.image != null)
                button.image.enabled = false;

            if (pressedImage != null)
                pressedImage.SetActive(true);

            manager.OnButtonPressed(buttonID);
        }
    }

    public void FreezeButton()
    {
        button.interactable = false;
        button.onClick.RemoveAllListeners();
    }

    public void ResetButton()
    {
        if (button.image != null)
            button.image.enabled = true;

        if (pressedImage != null)
            pressedImage.SetActive(false);

        button.interactable = true;
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnClick);
    }
}