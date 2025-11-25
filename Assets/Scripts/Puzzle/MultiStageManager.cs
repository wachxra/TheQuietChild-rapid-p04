using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class StageSettings
{
    [Header("Object 1: Change Color")]
    public Renderer object1;
    public Color object1Color = Color.white;

    [Header("Object 2: Change Sprite")]
    public SpriteRenderer object2;
    public Sprite object2Sprite;

    [Header("Object 3: Change UI Image")]
    public Image object3;
    public Sprite object3Sprite;
}

public class MultiStageManager : MonoBehaviour
{
    [Header("Stages Settings (3 levels)")]
    public StageSettings[] stages = new StageSettings[3];

    private int currentStage = 0;

    private void Start()
    {
        UpdateVisuals();
    }

    public void AdvanceStage()
    {
        if (stages == null || stages.Length == 0)
            return;

        if (currentStage >= stages.Length)
            return;

        currentStage++;
        UpdateVisuals();
    }

    private void UpdateVisuals()
    {
        if (stages == null || stages.Length == 0)
            return;

        int index = Mathf.Min(currentStage, stages.Length - 1);
        var stage = stages[index];

        if (stage.object1 != null)
        {
            stage.object1.material.color = stage.object1Color;
        }

        if (stage.object2 != null && stage.object2Sprite != null)
        {
            stage.object2.sprite = stage.object2Sprite;
        }

        if (stage.object3 != null && stage.object3Sprite != null)
        {
            stage.object3.sprite = stage.object3Sprite;
            stage.object3.SetNativeSize();
        }
    }
}