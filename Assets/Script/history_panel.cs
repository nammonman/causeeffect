using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class historyPanel : MonoBehaviour
{
    public static List<string> history = new List<string>();
    public GameObject object1;
    public GameObject object2;
    public Image image1;
    public Image image2;
    public Sprite[] resultsSprites;
    private int index = 0;
    private void Start()
    {
        foreach (var item in GameStateManager.gameStates.globalFlags)
        {
            AddHistory(item);
        }
        object1.SetActive(false);
        object2.SetActive(false);
        if (history.Count > 0)
        {
            object1.SetActive(true);
            SetImage(image1, history[0]);
            index = 0;
            // Debug.Log(index);
        }
        if (history.Count > 1)
        {
            object2.SetActive(true);
            SetImage(image2, history[1]);
            index = 1;
            // Debug.Log(index);
        }
    }

    private void SetImage(Image img, string condition)
    {
        switch (condition)
        {
            case "TimeBomb":
                img.sprite = resultsSprites[0];
                Debug.Log("img now" + img.sprite.name);
                break;
            case "AlienInvasion":
                img.sprite = resultsSprites[1];
                Debug.Log("img now" + img.sprite.name);
                break;
            case "StabilizerI":
                img.sprite = resultsSprites[2];
                Debug.Log("img now" + img.sprite.name);
                break;
            case "StabilizerII":
                img.sprite = resultsSprites[3];
                Debug.Log("img now" + img.sprite.name);
                break;
            case "EnhancedVision":
                img.sprite = resultsSprites[4];
                Debug.Log("img now" + img.sprite.name);
                break;
            case "Overheat":
                img.sprite = resultsSprites[5];
                Debug.Log("img now" + img.sprite.name);
                break;
            case "Explosive":
                img.sprite = resultsSprites[6];
                Debug.Log("img now" + img.sprite.name);
                break;
            case "Corrosion":
                img.sprite = resultsSprites[7];
                Debug.Log("img now" + img.sprite.name);
                break;
            case "RadiationPoisoning":
                img.sprite = resultsSprites[8];
                Debug.Log("img now" + img.sprite.name);
                break;
        }
    }

    private void AddHistory( string condition)
    {
        switch (condition)
        {
            case "RECIPE_TimeBomb":
                history.Add("TimeBomb");
                break;
            case "RECIPE_AlienInvasion":
                history.Add("AlienInvasion");
                break;
            case "RECIPE_StabilizerI":
                history.Add("StabilizerI");
                break;
            case "RECIPE_StabilizerII":
                history.Add("StabilizerII");
                break;
            case "RECIPE_EnhancedVision":
                history.Add("EnhancedVision");
                break;
            case "RECIPE_Overheat":
                history.Add("Overheat");
                break;
            case "RECIPE_Explosive":
                history.Add("Explosive");
                break;
            case "RECIPE_Corrosion":
                history.Add("Corrosion");
                break;
            case "RECIPE_RadiationPoisoning":
                history.Add("RadiationPoisoning");
                break;
        }
    }
    public void forwardButton()
    {
        if (history.Count > 2)
        {
            if (history.Count - (index + 1) == 1)
            {
                object2.SetActive(false);
                index++;
                Debug.Log(index);
                SetImage(image1, history[index]);
                Debug.Log("forward for 1");
            }
            else if (history.Count - (index + 1) > 1)
            {
                index++;
                SetImage(image1, history[index]);
                Debug.Log(index);
                index++;
                SetImage(image2, history[index]);
                Debug.Log("forward for 2");
                Debug.Log(index);
            }
        }
    }

    public void backwardButton()
    {
        if (history.Count > 2 && index > 1)
        {
            object2.SetActive(true);
            if (index % 2 == 1)
            {
                index--;
            }
                index--;
                Debug.Log(index);
                SetImage(image2, history[index]);
                index--;
                SetImage(image1, history[index]);
                index++;
                Debug.Log(index);
                Debug.Log("backward");
        }
    }
}
