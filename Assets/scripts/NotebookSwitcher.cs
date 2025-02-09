﻿using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VolFx;

public class NotebookSwitcher : MonoBehaviour
{
    [SerializeField] Button nextButton;
    [SerializeField] Button prevButton;
    [SerializeField] TextMeshProUGUI noteText;
    [SerializeField] Slider caesarSlider;
    [SerializeField] TextMeshProUGUI caesarShift;

    private int currentNoteText = 0;

    public static List<int> unlockedNotes = new List<int> { 0, 1, 2 };

    public static List<string> notes = null;
    private string notes4original;
    private void InitializeNotesIfNeeded()
    {
        if (notes == null)
        {
            notes = new List<string>
            {
                "Recipes for Substance Mixes\n\n" +
                "E[]ha[]c[][] Vi[]i[][]\n22 -6 20 32\n\n" +
                "[][][][][][][][][][][][]\n[][][][][][][][]\n\n" +
                "[][][][][][][]\n[][][][][][][][]\n\n",

                "Caesar Salad\n\n" +
                "Don’t forget to buy vegetables for making Caesar salad.\n" +
                "You should eat Caesar salad first to nourish your eyes and brain.\n" +
                "It might even help you better understand what you’re writing!\n\n",

                "Caesar Salad\n\n" +
                "Ingredients: \n" +
                "5 Caesar Dressing\n" +
                "6 Lettuce\n" +
                "2 Parmesan Cheese\n" +
                "3 Croutons\n\n" +
                "Instructions:\n" +
                "- Mix the lettuce and croutons together\n" +
                "- Cut the flavor with Caesar dressing\n" +
                "- Multiply the flavor with Parmesan cheese\n" +
                "This shifts the flavor from boring to exciting!",

                "GOLDENKEY\n\n" +
                "The “GOLDENKEY” can be used to unlock the first seal of the forbidden one.",

                "Treasure Hunt!\n\n" +
                "\"Bpm nqzab tqma jmvmibp bpm apilm wn bpm bzmm.  \r\nBpm amkwvl pqlma eqbpqv bpm sqvo’a kpiujmz.  \r\nBpm bpqzl qa bcksml jmbemmv ntwwza wn bpm lizs bwemz.  \r\nKwttmkb itt bpzmm bw nwzu bpm nqzab smg bw bpm nqvit ivaemz.\"\r\n",

                "Symbols\n\n" +
                "Stay hydrated\n" +
                "The president's calling again\n" +
                "Be\"sides\" your front door\n" +
                "Happy birthday!\n\n" +
                "5 i 9 14 i 11",

                "Good Old Days\n\n" +
                "Man, I missed when things were simpler. When I was a wee lad, if I wanted to type something out on a phone " +
                "I had to go like\n" +
                "2228885574448886666555 9369999 6226999988877224 \n\n" +
                "Kids these days have it too easy!!!",

                "Dear Diary #1\n\n" +
                "I have been getting weird dreams lately. One of them had a voice trying to tell me something but " +
                "couldn't quite make out the meaning of it. At first, I thought it happened because of stress from work " +
                "and it would go away, but it keeps coming back, clearer and clearer. Now the voice sounds like myself. " +
                "\"Traitor?\" \"Aliens?\" \"Hopelessness?\" Why is this happening and what does it mean?",

                "Dear Diary #2\n\n" +
                "I asked Zeph to scan my body and found a lot of Chronolencos energy inside me. This amount of Chronolencos " +
                "is lethal to most lifeforms, but somehow I am able to contain it. Zeph then used the Chronolencos in my " +
                "body to install a special system that can transfer data across time.\n\n" +
                "The dreams tell me to be wary of Zeph. Can I really trust this alien computer?",

                "Dear Diary #3\n\n" +
                "I tried the time travel system for the first time. It actually works but gives me a massive headache. " +
                "Probably not good for my body and should not be used often.\n\n" +
                "This time travel system can be used to extend the time to complete my research!",

                "Dear Diary #4\n\n" +
                "I dreamt that the research project failed and Chronolencos had contaminated the earth. The earth is nothing " +
                "but a wasteland.\n\n" +
                "But Zeph told me that Chronolencos is now safe???\n" +
                "[][][][][][][][][][][][][][][][][]",

                "Dear Diary #55\n\n" +
                "So tired of this time traveling. No matter how many times I restarted the loop, something seems to go wrong.\n\n" +
                "How will I be able to account for everything\n\n" +
                "When will this end...\n\n" +
                "The cycle never ends.",

                "Dear Diary #666\n\n" +
                "When will this end...\n\n" +
                "I even tried to break out of this cycle by death, but it does not work. Zeph has a failsafe for everything. " +
                "It's like I am the \"main character\" of this story, carefully written by Zeph, forcing me back on the script " +
                "every time I stray away from it.\n\n" +
                "Why me? I don't know. It could have been anyone else, but Zeph chose me for some reason.",

                "Dear Diary #7777\n\n" +
                "It is not a dream. The \"dreams\" are showing me my last memories before restarting the cycle.",

                "Dear Diary #88888\n\n" +
                "My existence might be a very important variable to determine the future of this world.\n\n" +
                "Can I remove myself from the variables? And will it change anything?",

                "Dear Diary #999999\n\n" +
                "The plan to escape the cycle could be making a \"paradox event\".",

                "Dear Diary #NaN\n\n" +
                "How long have I been here? Don't know. Don't even want to think about it. But I want out of this insanity.\n\n" +
                "The preparation for creating the paradox is coming along but not quick enough.\n\n" +
                "Let me out.",

                "Dear Future Me\n\n" +
                "The plan failed.\n\n" +
                "I made a mistake.\n\n" +
                "Slowly losing memories.\n\n" +
                "If you are reading this, you probably have no recollection of what I am talking about.\n\n" +
                "But don't worry about it. Just decode all the passwords and release yourself from the cycle.",
            };

            notes4original = notes[4];
        }

        notes[4] = notes4original;

        if (GameStateManager.gameStates.globalFlags.Contains("FoundItem1") && !notes[4].EndsWith("<align=center><color=#000000><i>every</i></color>"))
        {
            notes[4] += "\n<align=center><color=#000000><i>every</i></color>";
        }
        if (GameStateManager.gameStates.globalFlags.Contains("FoundItem2") && !notes[4].EndsWith("<color=#000000><i>choice</i></color>"))
        {
            notes[4] += "\n<color=#000000><i>choice</i></color>";
        }
        if (GameStateManager.gameStates.globalFlags.Contains("FoundItem3") && !notes[4].EndsWith("<color=#000000><i>has a consequence</i></color>"))
        {
            notes[4] += "\n<color=#000000><i>has a consequence</i></color>";
        }
        Debug.Log("notes reinittialized");
    }

    private void OnEnable()
    {
        InitializeNotesIfNeeded();        
        nextButton.onClick.AddListener(NextNote);
        prevButton.onClick.AddListener(PrevNote);
        caesarSlider.onValueChanged.AddListener(SetSliderValue);
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
        if (!unlockedNotes.Contains(i))
        {
            unlockedNotes.Add(i);
        }
    }

    public void SetSliderValue(float f)
    {
        int i = (int)f;
        noteText.text = DecodeCaesarCipher(notes[currentNoteText], (int)caesarSlider.value);
        caesarShift.text = i.ToString();
    }

    public void SetNoteText(int i)
    {
        currentNoteText = i;
        noteText.text = DecodeCaesarCipher(notes[i], (int)caesarSlider.value);
    }
    public string DecodeCaesarCipher(string input, int shift)
    {
        if (shift == 0)
            return input;
        if (string.IsNullOrEmpty(input))
            return input;

        shift = Math.Abs(shift % 26); 
        StringBuilder decoded = new StringBuilder(input.Length);

        foreach (char c in input)
        {
            if (char.IsLetter(c))
            {
                char offset = char.IsUpper(c) ? 'A' : 'a';
                char newChar = (char)(((c - offset - shift + 26) % 26) + offset);
                decoded.Append(newChar);
            }
            else
            {
                decoded.Append(c); 
            }
        }

        return decoded.ToString();
    }

    public void PrevNote()
    {
        int i = currentNoteText;
        if (i > 0)
        {
            SetNoteText(i-1);
        }
    }

    public void NextNote()
    {
        int i = currentNoteText;
        if (i < notes.Count - 1)
        {
            SetNoteText(i+1);
        }
    }
}
