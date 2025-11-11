using UnityEngine;
using UnityEngine.UI;

public class CollectBook : MonoBehaviour
{
    public BookPuzzleManager puzzleManager;
    public int bookIndex;

    private Button button;

    void Awake()
    {
        button = GetComponent<Button>();
        if (button != null)
            button.onClick.AddListener(OnCollect);
    }

    public void OnCollect()
    {
        if (puzzleManager != null)
        {
            puzzleManager.AddExtraBook(bookIndex);
        }

        Destroy(gameObject);
    }
}