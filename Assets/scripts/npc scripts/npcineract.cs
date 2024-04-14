using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class npcineract : MonoBehaviour
{

    [SerializeField] public TextMeshProUGUI dialogueText;
    public bool isDisplayingText = false;

    /*void Start()
    {
        // Ensure that the textMeshProUGUI field is assigned
        if (dialogueText == null)
        {
            Debug.LogError("TextMeshProUGUI component not assigned!");
            return;
        }

        // Change the text of the TextMeshProUGUI component
        dialogueText.text = "Initial Text";
    }*/

    IEnumerator waitClearText(float delay)
    {
        yield return new WaitForSeconds(delay);
        isDisplayingText = true;
    }

    public void changeText()
    {
        dialogueText.text = "hi";
        StartCoroutine(waitClearText(1));
        
    }
    
    public void clearText()
    {
        dialogueText.text = "";
        isDisplayingText = false;
    }

}
