using UnityEngine;

public class BedPuzzle : MonoBehaviour
{
    [Header("Lamp Puzzle Reference")]
    public LampPuzzle lampPuzzle;
    public Interactable targetInteractable;
    public GameObject newPanel;

    private bool redirected = false;

    private void Update()
    {
        if (!redirected && lampPuzzle != null && targetInteractable != null && newPanel != null)
        {
            if (LampPuzzle.hasLampPart1 && LampPuzzle.hasLampPart2 && LampPuzzle.hasLampPart3)
            {
                targetInteractable.panelToOpen = newPanel;
                redirected = true;
                Debug.Log("Lamp Puzzle complete: Interactable now opens Panel B instead of Panel A");
            }
        }
    }
}