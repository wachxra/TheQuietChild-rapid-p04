using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class Diary : MonoBehaviour
{
    [Header("UI References")]
    public GameObject notebookPanel;
    public TextMeshProUGUI textPageContent;
    public TextMeshProUGUI textPageCount;
    public Button nextButton;
    public Button prevButton;

    [Header("Notebook Pages")]
    [TextArea(3, 6)]
    public List<string> pages = new List<string>();

    [Header("Prepared Notes (Add in Inspector)")]
    [TextArea(3, 6)]
    public List<string> preparedPages = new List<string>();

    private int currentPage = 0;
    private int nextPreparedIndex = 0;

    void Start()
    {
        notebookPanel.SetActive(false);

        nextButton.onClick.AddListener(NextPage);
        prevButton.onClick.AddListener(PrevPage);

        if (pages.Count == 0 && preparedPages.Count > 0)
        {
            AddNewPage(preparedPages[nextPreparedIndex]);
            nextPreparedIndex++;
        }

        UpdateNotebookUI();
    }

    public void OpenNotebook()
    {
        notebookPanel.SetActive(true);
        UpdateNotebookUI();
    }

    public void CloseNotebook()
    {
        notebookPanel.SetActive(false);
    }

    public void AddNewPage(string newText)
    {
        pages.Add(newText);
        currentPage = pages.Count - 1;
        UpdateNotebookUI();
    }

    public void AddNextPreparedPage()
    {
        if (nextPreparedIndex < preparedPages.Count)
        {
            AddNewPage(preparedPages[nextPreparedIndex]);
            nextPreparedIndex++;
        }
        else
        {
            Debug.LogWarning("No more prepared pages left!");
        }
    }

    public void NextPage()
    {
        if (currentPage < pages.Count - 1)
        {
            currentPage++;
            UpdateNotebookUI();
        }
    }

    public void PrevPage()
    {
        if (currentPage > 0)
        {
            currentPage--;
            UpdateNotebookUI();
        }
    }

    void UpdateNotebookUI()
    {
        if (pages.Count == 0)
        {
            textPageContent.text = "Nothing...";
            textPageCount.text = "- / -";
        }
        else
        {
            textPageContent.text = pages[currentPage];
            textPageCount.text = $"{currentPage + 1}/{pages.Count}";
        }

        nextButton.interactable = currentPage < pages.Count - 1;
        prevButton.interactable = currentPage > 0;
    }

    public bool IsAllPreparedPagesCollected()
    {
        return nextPreparedIndex >= preparedPages.Count;
    }
}