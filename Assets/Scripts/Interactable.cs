using UnityEngine;

public enum InteractType
{
    ShowText,
    OpenPanel
}

public class Interactable : MonoBehaviour
{
    public InteractType interactType;
    [TextArea] public string message;
    public GameObject panelToOpen;

    public void Interact()
    {
        switch (interactType)
        {
            case InteractType.ShowText:
                UIManager.Instance.ToggleDialogue(message);
                break;
            case InteractType.OpenPanel:
                if (panelToOpen != null)
                    UIManager.Instance.TogglePuzzlePanel(panelToOpen);
                break;
        }
    }
}