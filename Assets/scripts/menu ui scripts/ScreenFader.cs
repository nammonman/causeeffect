using Subtegral.DialogueSystem.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFader : MonoBehaviour
{
    [SerializeField] Image screenFader;
    [SerializeField] Button testButton;

    private void Start()
    {
        SetCoverAlpha(0f);
        
    }

    private void OnEnable()
    {
        GameStateManager.OnFadeIn += FadeBlackInCaller;
        GameStateManager.OnFadeOut += FadeBlackOutCaller;
    }

    private void OnDisable()
    {
        GameStateManager.OnFadeIn -= FadeBlackInCaller;
        GameStateManager.OnFadeOut -= FadeBlackOutCaller;
    }


    public void FadeBlackInCaller()
    {
        StartCoroutine(FadeBlackIn());
    }

    public void FadeBlackOutCaller()
    {
        StartCoroutine(FadeBlackOut());
    }

    public IEnumerator FadeBlackIn()
    {
        yield return FadeTo(new Color(0, 0, 0), 1f, 1f, 0f);
    }

    public IEnumerator FadeBlackOut()
    {
        yield return FadeTo(new Color(0, 0, 0), 0f, 1f, 0f);
    }


    public IEnumerator FadeTo(Color col, float targetAlpha, float fadeDuration, float holdDuration)
    {
        screenFader.color = new Color(col.r, col.g, col.b, screenFader.color.a);
        Color currentColor = screenFader.color;
        float startAlpha = currentColor.a;

        float time = 0;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, time / fadeDuration);
            SetCoverAlpha(newAlpha);
            yield return null; // Wait for the next frame
        }

        SetCoverAlpha(targetAlpha);

        while (time < fadeDuration + holdDuration)
        {
            time += Time.deltaTime;
            yield return null;
        }

    }
    public void SetCoverAlpha(float alpha)
    {
        Color newColor = screenFader.color;
        newColor.a = alpha;
        screenFader.color = newColor;
    }
}
