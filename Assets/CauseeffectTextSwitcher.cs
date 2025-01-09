using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class CauseeffectTextSwitcher : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI dreamText;
    [SerializeField] Image cover;
    [SerializeField] Button nextButton;
    [SerializeField] GameObject canvasGroup;
    public Dream currentDream;
    public int currentIndex = 0;
    public List<int> availableDreams = new List<int>();

    public static event Action OnStart;
    public static event Action OnEnd;
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
        GameStateManager.OnCauseeffectText += SwitchTextCaller;
    }

    private void OnDisable()
    {
        GameStateManager.OnCauseeffectText -= SwitchTextCaller;
    }

    public void SwitchTextCaller(string s)
    {
        OnStart?.Invoke();
        currentDream = null;
        currentIndex = 0;
        SetCoverAlpha(0f);
        StartCoroutine(SwitchText(null, s));
        nextButton.onClick.AddListener(() => StartCoroutine(SwitchText()));

    }

    public void SwitchTextCaller(int i)
    {
        OnStart?.Invoke();
        currentDream = null;
        currentIndex = 0;
        SetCoverAlpha(0f);
        StartCoroutine(SwitchText(i));
        nextButton.onClick.AddListener(() => StartCoroutine(SwitchText()));

    }
    IEnumerator SwitchText(int? i = null, string blackScreenText = "")
    {

        nextButton.enabled = false;
        canvasGroup.SetActive(true);
        if (currentDream == null && i == null && string.IsNullOrEmpty(blackScreenText)) // random dream from pool
        {
            currentIndex = 0;

            int ran;
            if (availableDreams.Count > 0)
            {
                ran = UnityEngine.Random.Range(0, availableDreams.Count);
                currentDream = DreamTexts.dreams[availableDreams[ran]];
                Debug.Log("removed dream " + currentDream.dreamName + " from list");
                availableDreams.RemoveAt(ran);

            }
            else
            {
                currentDream = DreamTexts.dreams[0];

            }


            Debug.Log("start dream " + currentDream.dreamName);
        }
        else if (i != null && string.IsNullOrEmpty(blackScreenText)) // select dream 
        {
            currentDream = DreamTexts.dreams[i.Value];
        }
        else if (i == null && !string.IsNullOrEmpty(blackScreenText)) // select black screen text
        {
            currentDream = DreamTexts.blackScreenTexts[blackScreenText];
        }

        if (currentIndex >= currentDream.dreamTexts.Count)
        {
            Debug.Log("end dream " + currentDream.dreamName);
            nextButton.onClick.RemoveAllListeners();
            
            dreamText.text = "Reboot in 3";
            yield return new WaitForSeconds(0.9f);
            dreamText.text = "Reboot in 2";
            
            GameStateManager.setScreenFadeIn();
            yield return new WaitForSeconds(0.9f);
            dreamText.text = "Reboot in 1";
            yield return new WaitForSeconds(0.2f);
            OnEnd?.Invoke();
            canvasGroup.SetActive(false);
            GameStateManager.setStartGlitch();
            GameStateManager.setScreenFadeOut();
            yield return new WaitForSeconds(3f);
            GameStateManager.setStopGlitch();
            yield break;
        }

        Debug.Log(currentIndex);
        dreamText.text = currentDream.dreamTexts[currentIndex].text;

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
