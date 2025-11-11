using UnityEngine;

public class CollectBook : MonoBehaviour
{
    public BookPuzzleManager puzzleManager;

    void OnMouseDown()
    {
        if (puzzleManager != null)
        {
            puzzleManager.AddExtraBook();
        }

        Destroy(gameObject);
    }
}