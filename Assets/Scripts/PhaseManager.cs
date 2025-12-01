using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class PhaseData
{
    [Header("Phase Type Settings")]
    public bool changeColor = false;
    public bool changeSprite = false;
    public bool changePanel = false;

    [Header("Color Change (SpriteRenderer)")]
    public SpriteRenderer targetColorRenderer;
    public Color sceneColor = Color.white;

    [Header("Sprite Change (SpriteRenderer)")]
    public SpriteRenderer targetSpriteRenderer;
    public Sprite sceneSprite;

    [Header("Panel Change (UI Image) - Primary")]
    public Image targetPanelImage;
    public Sprite panelSprite;

    [Header("Panel Change (UI Image) - Secondary")]
    public Image targetPanelImage2;
    public Sprite panelSprite2;
}

public class PhaseManager : MonoBehaviour
{
    [Header("All Phases Settings")]
    public PhaseData[] phases;

    private int currentPhase = -1;

    void Start()
    {
        UpdatePhase();
    }

    public void UpdatePhase()
    {
        Diary diary = Object.FindFirstObjectByType<Diary>();
        if (diary == null) return;

        int collectedNotes = diary.pages.Count;

        int newPhase = Mathf.Clamp(collectedNotes, 0, phases.Length);

        if (newPhase != currentPhase)
        {
            currentPhase = newPhase;
            ApplyPhase();
        }
    }

    private void ApplyPhase()
    {
        if (currentPhase <= 0 || currentPhase > phases.Length) return;

        PhaseData phase = phases[currentPhase - 1];

        if (phase.changeColor && phase.targetColorRenderer != null)
        {
            phase.targetColorRenderer.color = phase.sceneColor;
        }

        if (phase.changeSprite && phase.targetSpriteRenderer != null && phase.sceneSprite != null)
        {
            phase.targetSpriteRenderer.sprite = phase.sceneSprite;
        }

        if (phase.changePanel && phase.targetPanelImage != null && phase.panelSprite != null)
        {
            phase.targetPanelImage.sprite = phase.panelSprite;
        }

        if (phase.changePanel && phase.targetPanelImage2 != null && phase.panelSprite2 != null)
        {
            phase.targetPanelImage2.sprite = phase.panelSprite2;
        }

        Debug.Log($"Phase {currentPhase} applied!");
    }

    public bool CurrentPhaseIsLast()
    {
        return currentPhase == phases.Length;
    }

    public GameObject GetFinalColorObject()
    {
        if (phases.Length == 0) return null;
        return phases[phases.Length - 1].targetColorRenderer?.gameObject;
    }
}