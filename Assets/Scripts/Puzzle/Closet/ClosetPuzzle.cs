using UnityEngine;
using UnityEngine.UI;

public class ClosetPuzzle : MonoBehaviour
{
    public Button batteryButton;

    private void Awake()
    {
        if (batteryButton != null)
            batteryButton.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        LampPuzzle lampPuzzle = Object.FindFirstObjectByType<LampPuzzle>();
        if (lampPuzzle != null)
        {
            lampPuzzle.ReceiveLampPart(3);
        }

        Destroy(batteryButton.gameObject);
    }
}