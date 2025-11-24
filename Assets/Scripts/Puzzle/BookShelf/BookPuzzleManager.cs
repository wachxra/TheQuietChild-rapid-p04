using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class SlotBookPair
{
    public BookSlot slot;
    public int correctBookIndex;
}

public class BookPuzzleManager : MonoBehaviour
{
    [Header("Book Slots (Drag & Drop in Inspector)")]
    public List<BookSlot> bookSlots;

    [Header("Slot-Book Pairs for Puzzle Completion")]
    public List<SlotBookPair> correctPairs;

    [Header("Initially Disabled Books")]
    public List<BookSlot> disabledBooks;

    [Header("Diary Reference")]
    public Diary diary;

    private HashSet<BookSlot> collectedBooks = new HashSet<BookSlot>();
    private bool puzzleCompleted = false;

    void Start()
    {
        for (int i = 0; i < bookSlots.Count; i++)
        {
            bookSlots[i].manager = this;
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

        slot.transform.SetAsLastSibling();

        bookSlots.Sort((a, b) => a.transform.GetSiblingIndex().CompareTo(b.transform.GetSiblingIndex()));

        for (int i = 0; i < bookSlots.Count; i++)
            bookSlots[i].currentSlotIndex = i;
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

        bookSlots.Sort((a, b) => a.transform.GetSiblingIndex().CompareTo(b.transform.GetSiblingIndex()));

        for (int i = 0; i < bookSlots.Count; i++)
            bookSlots[i].currentSlotIndex = i;

        CheckPuzzleComplete();
    }

    void CheckPuzzleComplete()
    {
        foreach (var pair in correctPairs)
        {
            BookSlot slot = pair.slot;
            int correctIndex = pair.correctBookIndex;

            Debug.Log($"Checking Slot: {slot.name}. Current Index: {slot.currentSlotIndex}, Required Index: {correctIndex}. Is Active: {slot.gameObject.activeSelf}");

            if (!slot.gameObject.activeSelf || slot.currentSlotIndex != correctIndex)
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

        if (diary != null)
            diary.AddNextPreparedPage();
    }
}