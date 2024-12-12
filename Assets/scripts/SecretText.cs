using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

[ExecuteAlways]
public class SecretText : MonoBehaviour
{
    public TMP_Text textMeshPro; // Reference to the TextMeshPro component
    public Transform originPoint; // Origin point for movement calculation
    public float movementMultiplier = 1.0f; // Controlled by a slider
    //public float alphaValue = 1.0f; // Controlled by another slider

    private List<GameObject> textChunks = new List<GameObject>();
    private string lastText; // To track changes in text
    private bool playerSaw = false;

    void Reset()
    {
        // Automatically get the TMP_Text component and set the origin point
        textMeshPro = GetComponent<TMP_Text>();
        if (textMeshPro == null)
        {
            Debug.LogWarning("No TMP_Text component found! Please add one to this GameObject.");
        }

        originPoint = transform;
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
        if (originPoint == null)
        {
            originPoint = transform;

        }

        UpdateTextChunks();
    }

    private void Update()
    {
        movementMultiplier = Mathf.Clamp(movementMultiplier, 0, 2);
    }

    void FixedUpdate()
    {
        
        
        UpdateTextChunks(true);
        
    }

    void UpdateTextChunks(bool fromOnVal = false)
    {
        if (textMeshPro == null || string.IsNullOrEmpty(textMeshPro.text)) return;

        // Check if the text has changed to avoid unnecessary updates
        if (textMeshPro.text != lastText && !fromOnVal)
        {
            ClearTextChunks();
            SplitTextIntoChunks();
            lastText = textMeshPro.text;
        }

        if (movementMultiplier > 0.01f)
        {
            if (textMeshPro.enabled)
            {
                textMeshPro.enabled = false;
            }
            foreach (var chunkObject in textChunks)
            {
                chunkObject.SetActive(true);
                Vector3 randomDirection = Random.insideUnitSphere;
                chunkObject.transform.localPosition = randomDirection * movementMultiplier;

                TMP_Text chunkText = chunkObject.GetComponent<TMP_Text>();
                // Calculate alpha range based on movementMultiplier
                float minAlpha = Mathf.Clamp01(1 - movementMultiplier);
                float maxAlpha = Mathf.Clamp01(2 - movementMultiplier);

                // Set a random opacity within the calculated range
                Color randomColor = textMeshPro.color;
                randomColor.a = Random.Range(minAlpha, maxAlpha);
                chunkText.color = randomColor;
            }
        }
        else
        {
            if (!textMeshPro.enabled)
            {
                textMeshPro.enabled = true;
            }
            foreach (var chunkObject in textChunks)
            {
                chunkObject.SetActive(false);
            }
        }

    }

    void SplitTextIntoChunks()
    {
        string originalText = textMeshPro.text;
        List<string> chunks = SplitIntoRandomChunks(originalText);

        foreach (var chunk in chunks)
        {
            GameObject chunkObject = new GameObject("Chunk");
            chunkObject.tag = "secret text chunk";
            chunkObject.transform.SetParent(transform);
            chunkObject.transform.localPosition = Vector3.zero;

            TMP_Text chunkText = chunkObject.AddComponent<TextMeshPro>();
            chunkText.text = chunk;
            chunkText.font = textMeshPro.font;
            chunkText.fontSize = textMeshPro.fontSize;
            chunkText.alignment = TextAlignmentOptions.Center;

            // Calculate alpha range based on movementMultiplier
            float minAlpha = Mathf.Clamp01(1 - movementMultiplier);
            float maxAlpha = Mathf.Clamp01(2 - movementMultiplier);

            // Set a random opacity within the calculated range
            Color randomColor = textMeshPro.color;
            randomColor.a = Random.Range(minAlpha, maxAlpha);
            chunkText.color = randomColor;

            textChunks.Add(chunkObject);
        }
    }

    List<string> SplitIntoRandomChunks(string text)
    {
        List<string> chunks = new List<string>();
        int index = 0;

        while (index < text.Length)
        {
            int chunkSize = Random.Range(1, Mathf.Min(4, text.Length - index + 1));
            string chunk = new string(' ', index*2) + text.Substring(index, chunkSize); // Add spaces before the chunk
            chunks.Add(chunk);
            index += chunkSize;
        }

        return chunks;
    }

    void ClearTextChunks()
    {
        foreach (var chunk in textChunks)
        {
            if (chunk != null)
            {
                if (Application.isPlaying)
                {
                    Destroy(chunk);
                }
                else
                {
                    DestroyImmediate(chunk);
                }


            }
        }
        textChunks.Clear();
    }

    private void OnEnable()
    {
        ClearTextChunks();
        SplitTextIntoChunks();
        UpdateTextChunks();
    }

    void OnDisable()
    {
        // Clear chunks when the script is disabled
        ClearTextChunks();
    }

    void OnDestroy()
    {
        // Clear chunks when the script is removed
        ClearTextChunks();
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
