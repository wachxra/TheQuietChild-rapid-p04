using UnityEngine;
using UnityEngine.UI;

public class PlantKeyPuzzle : MonoBehaviour
{
    [Header("Water Button")]
    public Button waterButton;
    public GameObject gameObjectA;

    [Header("Key Button in A")]
    public Button keyButton;
    public Sprite keyCollectedSprite;

    public static bool hasKey = false;

    private void Awake()
    {
        hasKey = false;

        if (waterButton != null)
            waterButton.onClick.AddListener(WaterTree);

        if (keyButton != null)
            keyButton.onClick.AddListener(PickupKey);

        if (gameObjectA != null)
            gameObjectA.SetActive(false);
    }

    private void WaterTree()
    {
        if (!RainPuzzleFlow.hasRainWater)
        {
            Debug.Log("Cannot water tree without rain water!");
            return;
        }

        RainPuzzleFlow.hasRainWater = false;
        Debug.Log("Tree watered! hasRainWater = " + RainPuzzleFlow.hasRainWater);

        if (gameObjectA != null)
            gameObjectA.SetActive(true);
    }

    private void PickupKey()
    {
        hasKey = true;
        Debug.Log("Key collected! hasKey = " + hasKey);

        if (keyButton != null && keyCollectedSprite != null)
        {
            Image btnImage = keyButton.GetComponent<Image>();
            if (btnImage != null)
            {
                btnImage.sprite = keyCollectedSprite;
            }
        }
    }
}