using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class BookPuzzleManager : MonoBehaviour
{
    [Header("Book Puzzle Settings")]
    public Transform bookParent;
    public GameObject bookPrefab;
    public int totalBooks = 9;
    public List<int> correctOrder;

    private List<BookSlot> bookSlots = new List<BookSlot>();
    private bool extraBookCollected = false;

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

    public void AddExtraBook()
    {
        if (extraBookCollected) return;
        extraBookCollected = true;
        CreateBook(totalBooks - 1);
    }

    void CreateBook(int index)
    {
        GameObject newBook = Instantiate(bookPrefab, bookParent);
        BookSlot slot = newBook.GetComponent<BookSlot>();

        if (slot != null)
        {
            slot.bookNumber = index + 1;
            slot.manager = this;
            bookSlots.Add(slot);
        }
    }

    public void SwapBooks(BookSlot a, BookSlot b)
    {
        int indexA = a.transform.GetSiblingIndex();
        int indexB = b.transform.GetSiblingIndex();

        a.transform.SetSiblingIndex(indexB);
        b.transform.SetSiblingIndex(indexA);

        CheckPuzzleComplete();
    }

    void CheckPuzzleComplete()
    {
        bool isCorrect = true;
        for (int i = 0; i < correctOrder.Count && i < bookParent.childCount; i++)
        {
            var slot = bookParent.GetChild(i).GetComponent<BookSlot>();
            if (slot == null || slot.bookNumber != correctOrder[i])
            {
                isCorrect = false;
                break;
            }
        }

        if (isCorrect)
        {
            Debug.Log("? Puzzle Complete!");
        }
    }
}