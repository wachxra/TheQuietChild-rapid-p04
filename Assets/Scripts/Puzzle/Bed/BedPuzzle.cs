using UnityEngine;
using UnityEngine.UI;

public class BedPuzzle : MonoBehaviour
{
    [Header("Lamp Puzzle Reference")]
    public LampPuzzle lampPuzzle;
    public Interactable targetInteractable;
    public GameObject newPanel;

    [Header("Note Button Reference")]
    public Button noteButton;
    public GameObject objectToDestroy;

    [Header("Diary Reference")]
    public Diary diary;

    private bool redirected = false;

    private void Start()
    {
        if (noteButton != null && objectToDestroy != null)
        {
            noteButton.onClick.AddListener(() =>
            {
                Destroy(objectToDestroy);

                if (diary != null)
                    diary.AddNextPreparedPage();
            });
        }
    }

    private void Update()
    {
        if (!redirected && lampPuzzle != null && targetInteractable != null && newPanel != null)
        {
            if (LampPuzzle.hasLampPart1 && LampPuzzle.hasLampPart2 && LampPuzzle.hasLampPart3)
            {
                redirected = true;

                targetInteractable.switchablePanels = new GameObject[] { newPanel };
                targetInteractable.textGroups = new TextGroupElement[0];
                targetInteractable.panelToOpen = newPanel;

                Debug.Log("BedPuzzle: Redirected to Panel B with no text group.");
            }
        }
    }
}