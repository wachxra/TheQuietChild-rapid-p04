using UnityEngine;
using UnityEngine.UI;

public class TentacleButton : MonoBehaviour
{
    public int buttonID;
    private SquidTentaclePuzzleManager manager;
    public Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        manager = Object.FindFirstObjectByType<SquidTentaclePuzzleManager>();
        button.onClick.AddListener(OnClick);
    }

    public void OnClick()
    {
        if (manager != null)
        {
            manager.OnButtonPressed(buttonID);
        }
    }

    public void FreezeButton()
    {
        button.interactable = false;
    }

    public void ResetButton()
    {
        button.interactable = true;
    }
}