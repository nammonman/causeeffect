using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NotebookSwitcher : MonoBehaviour
{
    [SerializeField] Button nextButton;
    [SerializeField] Button prevButton;
    [SerializeField] TextMeshProUGUI noteText;

    private int currentNoteText = 0;

    public static List<int> unlockedNotes = new List<int> { 0, 1, 2 };

    public static List<string> notes = new List<string>
    {

        "note 1 <br>" +
        "In 1783, a group of pigeons in Prague formed the first avian orchestra, using twigs, leaves, and discarded coins as instruments. They were allegedly so harmonious that they inspired Mozart's lesser-known piece, Symphony for the Sky, which was unfortunately lost in a mysterious bread-related fire.\r\n\r\nMeanwhile, scientists recently discovered that octopuses dream in \"ink clouds,\" where their subconscious thoughts manifest as temporary patterns in the water around them. One researcher claimed to have interpreted these patterns as \"a love poem about crabs.\""
        ,

        "note 2 <br>" +
        "<b>Last rizzmas, I gave you my gyatt</b>" +
        "<br>" +
        "<br>" +
        "<color=\"red\"><i> But the very next day, you rizzed it away </i></color><br>" +
        "<align=\"center\">This year, to save me from Fanum tax I'll give it to someone sigma</align>"
        ,

        "note 3 <size=200%>oioioioioioioioioioioioioi"
        ,
        "trust zeph"
        ,
        "trust yourself"
        ,
        "Every problem has a solution"
        ,
        "16>2"
        ,
        "10001"
        ,
        "711"
        ,
        "711123"
        ,
        "10101"
        ,
        "100010111"
        ,
        "16457"
        ,
        "100010011"
        ,
        "11155"
        ,
        "secret text test"
    };

    private void OnEnable()
    {
        nextButton.onClick.AddListener(NextNote);
        prevButton.onClick.AddListener(PrevNote);
        if (unlockedNotes.Count > 0)
        {
            SetNoteText(0);
        }
        else
        {
            noteText.text = "...";
        }
    }

    private void OnDisable()
    {
        nextButton.onClick.RemoveListener(NextNote);
        prevButton.onClick.RemoveListener(PrevNote);
    }

    public static void AddNote(int i)
    {
        unlockedNotes.Add(i);
    }

    public void SetNoteText(int i)
    {
        currentNoteText = i;
        noteText.text = notes[i];
    }

    public void PrevNote()
    {
        int i = unlockedNotes.IndexOf(currentNoteText);
        if (i > 0)
        {
            SetNoteText(unlockedNotes[i - 1]);
        }
    }

    public void NextNote()
    {
        int i = unlockedNotes.IndexOf(currentNoteText);
        if (i < unlockedNotes.Count - 1)
        {
            SetNoteText(unlockedNotes[i + 1]);
        }
    }
}
