using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

[ExecuteAlways]
public class SecretText : MonoBehaviour
{
    public TMP_Text textMeshPro; // Reference to the TextMeshPro component
    public float movementMultiplier = 0f; // Controlled by a slider
    //public float alphaValue = 1.0f; // Controlled by another slider

    private string lastText; // To track changes in text
    private bool playerSaw = false;
    int frameCount = 0;
    float originalSize = 5;

    void Reset()
    {
        // Automatically get the TMP_Text component and set the origin point
        textMeshPro = GetComponent<TMP_Text>();
        if (textMeshPro == null)
        {
            Debug.LogWarning("No TMP_Text component found! Please add one to this GameObject.");
        }

        UpdateTextChunks();

    }

    void Start()
    {
        if (textMeshPro == null)
        {
            Debug.LogError("TMP_Text component is not assigned.");
            return;
        }

        UpdateTextChunks();
    }

    void OnValidate()
    {
        // Update text in the editor when properties change
        if (textMeshPro == null)
        {
            textMeshPro = GetComponent<TMP_Text>();
        }

        UpdateTextChunks();
    }

    private void Update()
    {
        movementMultiplier = Mathf.Clamp(movementMultiplier, 0, 2);
    }

    void FixedUpdate()
    {
        frameCount++;
        frameCount %= 2;
        if (frameCount % 2 == 1)
        {
            UpdateTextChunks(true);
        }

    }

    void UpdateTextChunks(bool fromOnVal = false)
    {
        if (textMeshPro == null || string.IsNullOrEmpty(textMeshPro.text)) return;

        // Check if the text has changed to avoid unnecessary updates
        if (textMeshPro.text != lastText && !fromOnVal)
        {
            lastText = textMeshPro.text;
        }

        TMP_Text chunkText = gameObject.GetComponent<TMP_Text>();
        if (movementMultiplier > 0.01f)
        {
            // Calculate alpha range based on movementMultiplier
            float minAlpha = Mathf.Clamp01(1 - movementMultiplier);
            float maxAlpha = Mathf.Clamp01(2 - movementMultiplier);
            float fontSize = Random.Range(1, (movementMultiplier + 1) * 10);
            // Set a random opacity within the calculated range
            Color randomColor = textMeshPro.color;
            randomColor.a = Random.Range(minAlpha, maxAlpha);
            chunkText.color = randomColor;
            chunkText.fontSize = fontSize;
        }
        else
        {
            if (!textMeshPro.enabled)
            {
                textMeshPro.enabled = true;
            }

            Color randomColor = textMeshPro.color;
            randomColor.a = 1;
            chunkText.color = randomColor;
            chunkText.fontSize = originalSize;
        }

    }



    public void SetMovementMultiplier(float value)
    {
        movementMultiplier = value;
        UpdateTextChunks();
    }

    /*public void SetAlphaValue(float value)
    {
        alphaValue = value;
        UpdateTextChunks();
    }*/
}
