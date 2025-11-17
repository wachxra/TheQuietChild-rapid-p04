using UnityEngine;
using System.Collections.Generic;

public class BookPuzzleManager : MonoBehaviour
{
    [Header("Book Slots (Drag & Drop in Inspector)")]
    public List<BookSlot> bookSlots;

    [Header("Correct Order by Index")]
    public List<int> correctOrder;

    [Header("Initially Disabled Books")]
    public List<BookSlot> disabledBooks;

    [Header("Diary Reference")]
    public Diary diary;
    public string diaryNote = "New diary entry from book puzzle.";

    private HashSet<BookSlot> collectedBooks = new HashSet<BookSlot>();
    private bool puzzleCompleted = false;

    void Start()
    {
        for (int i = 0; i < bookSlots.Count; i++)
        {
            bookSlots[i].manager = this;
            bookSlots[i].bookIndex = i;
            bookSlots[i].canDrag = true;
        }

        foreach (var slot in disabledBooks)
        {
            if (slot != null)
                slot.gameObject.SetActive(false);
        }
    }

    public void AddExtraBook(BookSlot slot)
    {
        if (collectedBooks.Contains(slot)) return;

        collectedBooks.Add(slot);

        slot.gameObject.SetActive(true);
        slot.transform.SetParent(bookSlots[0].transform.parent);

        if (!bookSlots.Contains(slot))
            bookSlots.Add(slot);

        for (int i = 0; i < bookSlots.Count; i++)
        {
            bookSlots[i].transform.SetSiblingIndex(i);
        }

        CheckPuzzleComplete();
    }

    public void SwapBooks(BookSlot draggedSlot, int targetIndex)
    {
        if (puzzleCompleted) return;

        int originalIndex = draggedSlot.transform.GetSiblingIndex();

        for (int i = 0; i < bookSlots.Count; i++)
        {
            var slot = bookSlots[i];
            if (slot == draggedSlot) continue;
            int sibling = slot.transform.GetSiblingIndex();
            if (originalIndex < targetIndex)
            {
                if (sibling > originalIndex && sibling <= targetIndex)
                    slot.transform.SetSiblingIndex(sibling - 1);
            }
            else if (originalIndex > targetIndex)
            {
                if (sibling >= targetIndex && sibling < originalIndex)
                    slot.transform.SetSiblingIndex(sibling + 1);
            }
        }

        draggedSlot.transform.SetSiblingIndex(targetIndex);

        CheckPuzzleComplete();
    }

    void CheckPuzzleComplete()
    {
        for (int i = 0; i < correctOrder.Count && i < bookSlots.Count; i++)
        {
            BookSlot slot = bookSlots[i];
            if (!slot.gameObject.activeSelf || slot.bookIndex != correctOrder[i])
                return;
        }

        PuzzleCompleted();
    }

    void PuzzleCompleted()
    {
        puzzleCompleted = true;

        foreach (var slot in bookSlots)
            slot.canDrag = false;

        Debug.Log("Puzzle Complete!");

        Diary diary = FindFirstObjectByType<Diary>();
        diary.AddNextPreparedPage();
    }
}