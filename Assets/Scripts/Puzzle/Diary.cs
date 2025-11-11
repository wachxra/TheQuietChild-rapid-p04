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

    private int currentPage = 0;

    void Start()
    {
        notebookPanel.SetActive(false);

        nextButton.onClick.AddListener(NextPage);
        prevButton.onClick.AddListener(PrevPage);

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
        UpdateNotebookUI();
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
}