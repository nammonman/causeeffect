using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Subtegral.DialogueSystem.Runtime.DialogueParser;

public class DreamTextSwitcher : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI dreamText;
    [SerializeField] Image cover;
    [SerializeField] Button nextButton;
    [SerializeField] GameObject canvasGroup;
    private Dream currentDream;
    private int currentIndex = 0;
    private List<int> availableDreams = new List<int>();


    private void Start()
    {
        for (int i = 0; i < DreamTexts.dreams.Count; i++)
        {
            availableDreams.Add(i);
        }
        for (int i = 0; i < 2; i++) // no dreams
        {
            availableDreams.Add(0);
        }
        canvasGroup.SetActive(false);
    }

    private void OnEnable()
    {
        GameStateManager.OnDream += SwitchTextCaller;
    }

    private void OnDisable()
    {
        GameStateManager.OnDream -= SwitchTextCaller;
    }

    public void SwitchTextCaller(int i)
    {
        currentDream = null;
        currentIndex = 0;
        SetCoverAlpha(1f);
        StartCoroutine(SwitchText(i));
        nextButton.onClick.AddListener(() => StartCoroutine(SwitchText()));
        
    }
    IEnumerator SwitchText(int? i = null)
    {
        nextButton.enabled = false;
        canvasGroup.SetActive(true);
        if (currentDream == null && i == null)
        {
            currentIndex = 0;

            int ran;
            if (availableDreams.Count > 0)
            {
                ran = UnityEngine.Random.Range(0, availableDreams.Count);
                currentDream = DreamTexts.dreams[availableDreams[ran]];
                availableDreams.RemoveAt(ran);

            }
            else
            {
                currentDream = DreamTexts.dreams[0];
                
            }
            
            
            Debug.Log("start dream " + currentDream.dreamName);
        }
        else if (i != null)
        {
            currentDream = DreamTexts.dreams[i.Value];
        }

        if (currentIndex >= currentDream.dreamTexts.Count)
        {
            Debug.Log("end dream " + currentDream.dreamName);
            yield return new WaitForSeconds(2f);
            canvasGroup.SetActive(false);
            yield break;
        }

        // Fade in
        yield return StartCoroutine(FadeTo(1f, 0.4f)); // Fade to full opacity

        Debug.Log(currentIndex);
        dreamText.text = currentDream.dreamTexts[currentIndex].text;

        // Fade out
        yield return StartCoroutine(FadeTo(0f, 0.4f)); // Fade back to 0 opacity
        
        currentIndex++;
        nextButton.enabled = true;
    }

    IEnumerator FadeTo(float targetAlpha, float duration)
    {
        Color currentColor = cover.color;
        float startAlpha = currentColor.a;

        float time = 0;

        while (time < duration)
        {
            time += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, time / duration);
            SetCoverAlpha(newAlpha);
            yield return null; // Wait for the next frame
        }
        SetCoverAlpha(targetAlpha);

        while (time < duration + 0.4f)
        {
            time += Time.deltaTime;
            yield return null; 
        }

    }
    void SetCoverAlpha(float alpha)
    {
        Color newColor = cover.color;
        newColor.a = alpha;
        cover.color = newColor;
    }
    string ParseGlitchText(string text)
    {
        string glitchedTextOpen = "<glitch>";
        string glitchedTextClose = "</glitch>";
        string pattern = $"{glitchedTextOpen}(.*?){glitchedTextClose}";
        string result = Regex.Replace(text, pattern, match =>
        {
            string glitchedContent = match.Groups[1].Value;
            return ApplyGlitchEffect(glitchedContent);
        });

        return result;
    }

    string ApplyGlitchEffect(string text)
    {
        string glitchedText = "";
        foreach (char c in text)
        {
            glitchedText += c;
            glitchedText += GetRandomGlitch();  // Add random glitch after each character
        }
        return glitchedText;
    }

    string GetRandomGlitch()
    {
        // Randomly select some glitchy characters (combining diacritical marks)
        int[] glitchChars = new int[] { 0x0300, 0x0301, 0x0302, 0x0303, 0x0304, 0x0305, 0x0306, 0x0307, 0x0308, 0x0309, 0x030A, 0x030B, 0x030C, 0x030D, 0x030E, 0x030F, 0x0310, 0x0311, 0x0312, 0x0313, 0x0314 };
        int randomIndex = UnityEngine.Random.Range(0, glitchChars.Length);
        return char.ConvertFromUtf32(glitchChars[randomIndex]);
    }
}
