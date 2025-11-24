using UnityEngine;
using UnityEngine.UI;

public class LampPuzzle : MonoBehaviour
{
    [Header("Lamp Parts UI")]
    public GameObject lampPart1UI;
    public GameObject lampPart2UI;
    public GameObject lampPart3UI;

    [Header("Panel to Change Color")]
    public Image panelToChangeColor;
    public Color collectedColor = Color.yellow;

    public static bool hasLampPart1 = false;
    public static bool hasLampPart2 = false;
    public static bool hasLampPart3 = false;

    private void Awake()
    {
        if (lampPart1UI != null) lampPart1UI.SetActive(false);
        if (lampPart2UI != null) lampPart2UI.SetActive(false);
        if (lampPart3UI != null) lampPart3UI.SetActive(false);
    }

    public void ReceiveLampPart(int partIndex)
    {
        switch (partIndex)
        {
            case 1:
                if (!hasLampPart1)
                {
                    hasLampPart1 = true;
                    if (lampPart1UI != null) lampPart1UI.SetActive(true);
                    Debug.Log("Lamp Part 1 collected!");
                }
                break;
            case 2:
                if (!hasLampPart2)
                {
                    hasLampPart2 = true;
                    if (lampPart2UI != null) lampPart2UI.SetActive(true);
                    Debug.Log("Lamp Part 2 collected!");
                }
                break;
            case 3:
                if (!hasLampPart3)
                {
                    hasLampPart3 = true;
                    if (lampPart3UI != null) lampPart3UI.SetActive(true);
                    Debug.Log("Lamp Part 3 collected!");
                }
                break;
        }

        CheckAllPartsCollected();
    }

    private void CheckAllPartsCollected()
    {
        if (hasLampPart1 && hasLampPart2 && hasLampPart3)
        {
            if (panelToChangeColor != null)
            {
                panelToChangeColor.color = collectedColor;
                Debug.Log("All parts collected! Panel color changed.");
            }
        }
    }
}