using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

[System.Serializable]
public class PreparedPage
{
    [TextArea(2, 4)] public string text1;
    [TextArea(2, 4)] public string text2;
    [TextArea(2, 4)] public string text3;
    [TextArea(3, 10)] public string text4;
}

public class Diary : MonoBehaviour
{
    [Header("UI References")]
    public GameObject notebookPanel;
    public TextMeshProUGUI textPageContent;
    public TextMeshProUGUI textPageCount;
    public Button nextButton;
    public Button prevButton;

    [Header("Back Button")]
    public Button backButton;

    [Header("Notebook Pages")]
    [TextArea(3, 6)]
    public List<string> pages = new List<string>();

    [Header("Prepared Notes (Add in Inspector, 4 texts per page)")]
    public List<PreparedPage> preparedPages = new List<PreparedPage>();

    [Header("Prepared Page TMPs")]
    public TextMeshProUGUI textPageContent1;
    public TextMeshProUGUI textPageContent2;
    public TextMeshProUGUI textPageContent3;
    public TextMeshProUGUI textPageContent4;

    private int currentPage = 0;
    private int nextPreparedIndex = 0;

    void Start()
    {
        notebookPanel.SetActive(false);

        nextButton.onClick.AddListener(NextPage);
        prevButton.onClick.AddListener(PrevPage);

        if (backButton != null)
        {
            backButton.onClick.AddListener(ClosePanel);
            backButton.gameObject.SetActive(false);
        }

        if (pages.Count == 0 && preparedPages.Count > 0)
        {
            AddNextPreparedPage();
        }

        UpdateNotebookUI();
    }

    public void OpenNotebook()
    {
        notebookPanel.SetActive(true);
        if (backButton != null)
            backButton.gameObject.SetActive(true);
        UpdateNotebookUI();
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
            PreparedPage p = preparedPages[nextPreparedIndex];

            string combinedText = string.Join("\n", new string[] { p.text1, p.text2, p.text3, p.text4 });
            AddNewPage(combinedText);

            nextPreparedIndex++;

            PhaseManager phaseManager = Object.FindFirstObjectByType<PhaseManager>();
            if (phaseManager != null)
                phaseManager.UpdatePhase();
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
            AudioManager.Instance.PlaySFX("NextPage");
            currentPage++;
            UpdateNotebookUI();
        }
    }

    public void PrevPage()
    {
        if (currentPage > 0)
        {
            AudioManager.Instance.PlaySFX("NextPage");
            currentPage--;
            UpdateNotebookUI();
        }
    }

    void UpdateNotebookUI()
    {
        if (pages.Count == 0)
        {
            textPageContent.text = "Nothing...";
            textPageContent1.text = "";
            textPageContent2.text = "";
            textPageContent3.text = "";
            textPageContent4.text = "";
            textPageCount.text = "- / -";
        }
        else
        {
            string[] lines = pages[currentPage].Split(new string[] { "\n" }, System.StringSplitOptions.None);
            textPageContent1.text = lines.Length > 0 ? lines[0] : "";
            textPageContent2.text = lines.Length > 1 ? lines[1] : "";
            textPageContent3.text = lines.Length > 2 ? lines[2] : "";
            textPageContent4.text = lines.Length > 3 ? lines[3] : "";

            textPageCount.text = $"{currentPage + 1}/{pages.Count}";
        }

        nextButton.interactable = currentPage < pages.Count - 1;
        prevButton.interactable = currentPage > 0;
    }

    public bool IsAllPreparedPagesCollected()
    {
        return nextPreparedIndex >= preparedPages.Count;
    }

    private void ClosePanel()
    {
        notebookPanel.SetActive(false);
        backButton.gameObject.SetActive(false);
    }
}