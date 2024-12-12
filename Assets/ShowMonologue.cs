using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowMonologue : MonoBehaviour
{
    [SerializeField] CanvasGroup canvasOpacity;
    [SerializeField] TextMeshProUGUI monologue;

    private void Start()
    {
        canvasOpacity.alpha = 0f;
    }
    private void OnEnable()
    {
        GameStateManager.OnMonologue += DisplayMonologue;
    }

    private void OnDisable()
    {
        GameStateManager.OnMonologue -= DisplayMonologue;
    }
    public void DisplayMonologue(string text)
    {
        monologue.text = text;
        StartCoroutine(FadeInAndOut());
    }

    private IEnumerator FadeInAndOut()
    {
        // Fade in
        yield return StartCoroutine(LerpCanvasAlpha(0f, 1f, 0.5f));

        // Wait for 3 seconds
        yield return new WaitForSeconds(3f);

        // Fade out
        yield return StartCoroutine(LerpCanvasAlpha(1f, 0f, 0.5f));
    }

    private IEnumerator LerpCanvasAlpha(float start, float end, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            canvasOpacity.alpha = Mathf.Lerp(start, end, elapsed / duration);
            yield return null;
        }

        canvasOpacity.alpha = end;

    }
}
