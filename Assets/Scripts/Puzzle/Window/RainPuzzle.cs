using UnityEngine;
using UnityEngine.UI;

public class RainPuzzleFlow : MonoBehaviour
{
    [Header("Start Button")]
    public Button startButton;
    public GameObject gameObjectA;

    [Header("Glass Button")]
    public Button glassButton;

    [Header("Mirror Button")]
    public Button mirrorButton;
    public Sprite mirrorRainSprite;

    [Header("GameObject B")]
    public GameObject gameObjectB;
    public Button rainWaterButton;

    [Header("Back Button")]
    public Button backButton;

    public static bool hasGlass = false;
    public static bool isRaining = false;
    public static bool hasRainWater = false;

    private Image mirrorButtonImage;

    [Header("Glass Pickup Sprite Change")]
    public GameObject targetObjectToChangeSprite;
    public Sprite newSpriteAfterPickupGlass;

    private void Awake()
    {
        if (startButton != null)
            startButton.onClick.AddListener(OpenGameObjectA);

        if (glassButton != null)
            glassButton.onClick.AddListener(PickupGlass);

        if (mirrorButton != null)
        {
            mirrorButton.onClick.AddListener(TriggerRain);
            mirrorButtonImage = mirrorButton.GetComponent<Image>();
        }

        if (rainWaterButton != null)
            rainWaterButton.onClick.AddListener(PickupRainWater);

        if (backButton != null)
        {
            backButton.onClick.AddListener(CloseCurrentPanel);
            backButton.gameObject.SetActive(false);
        }

        if (gameObjectA != null)
            gameObjectA.SetActive(false);
        if (gameObjectB != null)
            gameObjectB.SetActive(false);
    }

    private void OpenGameObjectA()
    {
        if (isRaining)
        {
            if (gameObjectB != null)
            {
                gameObjectB.SetActive(true);

                AudioManager.Instance.PlayRainLoop();

                if (backButton != null)
                    backButton.gameObject.SetActive(true);
            }

            if (gameObjectA != null)
                gameObjectA.SetActive(false);

            Debug.Log("Rain has started! GameObject B opens instead of A.");
        }
        else
        {
            if (gameObjectA != null)
            {
                gameObjectA.SetActive(true);
                if (backButton != null)
                    backButton.gameObject.SetActive(true);
            }

            Debug.Log("GameObject A opened!");
        }
    }

    private void TriggerRain()
    {
        isRaining = true;
        AudioManager.Instance.PlaySFX("Raining");

        Debug.Log("Rain triggered! isRaining = " + isRaining);

        if (mirrorButtonImage != null && mirrorRainSprite != null)
            mirrorButtonImage.sprite = mirrorRainSprite;

        if (gameObjectA != null)
            gameObjectA.SetActive(false);
    }

    private void PickupGlass()
    {
        hasGlass = true;
        AudioManager.Instance.PlaySFX("PickUp");

        if (glassButton != null)
            Destroy(glassButton.gameObject);

        Debug.Log("Glass picked up! hasGlass = " + hasGlass);

        if (targetObjectToChangeSprite != null && newSpriteAfterPickupGlass != null)
        {
            var img = targetObjectToChangeSprite.GetComponent<Image>();
            if (img != null)
                img.sprite = newSpriteAfterPickupGlass;

            var sr = targetObjectToChangeSprite.GetComponent<SpriteRenderer>();
            if (sr != null)
                sr.sprite = newSpriteAfterPickupGlass;
        }
    }

    private int rainWaterClickCount = 0;

    private void PickupRainWater()
    {
        rainWaterClickCount++;

        if (rainWaterClickCount == 1)
        {
            return;
        }

        if (rainWaterClickCount >= 2)
        {
            if (!hasGlass)
            {
                return;
            }

            if (!isRaining)
            {
                return;
            }

            hasRainWater = true;
            AudioManager.Instance.PlaySFX("WaterPlant");
            Debug.Log("Rain water collected! hasRainWater = " + hasRainWater);

            if (rainWaterButton != null)
                Destroy(rainWaterButton.gameObject);
        }
    }

    private void CloseCurrentPanel()
    {
        if (gameObjectA != null && gameObjectA.activeSelf)
            gameObjectA.SetActive(false);

        if (gameObjectB != null && gameObjectB.activeSelf)
            gameObjectB.SetActive(false);

        AudioManager.Instance.StopRainLoop();

        if (backButton != null)
            backButton.gameObject.SetActive(false);
    }
}