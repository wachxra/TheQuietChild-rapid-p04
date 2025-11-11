using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class BookPuzzleManager : MonoBehaviour
{
    [Header("Book Puzzle Settings")]
    public Transform bookParent;
    public GameObject bookPrefab;
    public int totalBooks = 9;
    public List<int> correctOrder;

    private List<BookSlot> bookSlots = new List<BookSlot>();
    private HashSet<int> collectedBooks = new HashSet<int>();

    void Start()
    {
        SpawnInitialBooks();
    }

    void SpawnInitialBooks()
    {
        for (int i = 0; i < totalBooks - 1; i++)
        {
            CreateBook(i);
        }
    }

    public void AddExtraBook(int bookIndex)
    {
        if (collectedBooks.Contains(bookIndex)) return;

        collectedBooks.Add(bookIndex);
        CreateBook(bookIndex);
    }

    void CreateBook(int index)
    {
        GameObject newBook = Instantiate(bookPrefab, bookParent);
        BookSlot slot = newBook.GetComponent<BookSlot>();
        if (slot != null)
        {
            slot.manager = this;
            bookSlots.Add(slot);
        }
    }

    public int GetClosestIndex(Vector3 worldPos, BookSlot dragged)
    {
        int closestIndex = 0;
        float closestDistance = float.MaxValue;

        for (int i = 0; i < bookParent.childCount; i++)
        {
            Transform child = bookParent.GetChild(i);
            if (child == dragged.transform) continue;

            float distance = Vector3.Distance(worldPos, child.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestIndex = i;
            }
        }

        return closestIndex;
    }

    public void ReorderBook(BookSlot dragged, int targetIndex)
    {
        int currentIndex = dragged.transform.GetSiblingIndex();
        dragged.transform.SetSiblingIndex(targetIndex);

        for (int i = 0; i < bookParent.childCount; i++)
        {
            if (bookParent.GetChild(i) == dragged.transform) continue;
            bookParent.GetChild(i).SetSiblingIndex(i < targetIndex ? i : i + 1);
        }
    }

    /*public void CheckPuzzleComplete()
    {
        bool isCorrect = true;
        for (int i = 0; i < correctOrder.Count && i < bookParent.childCount; i++)
        {
            BookSlot slot = bookParent.GetChild(i).GetComponent<BookSlot>();
            if (slot == null || slot.GetSiblingIndex() != correctOrder[i])
            {
                isCorrect = false;
                break;
            }
        }

        if (isCorrect) Debug.Log("Puzzle Complete!");
    }*/
}