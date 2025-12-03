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

        AudioManager.Instance.PlaySFX("PickUp");
        Destroy(batteryButton.gameObject);

        TextPopUpManager.ShowMessage(new string[]
            {
                "It looks like the lamp is out of battery",
                "This little thing should help"
            });
    }
}