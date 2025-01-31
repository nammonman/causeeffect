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
        history = new List<string>();
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
                DistinctHistoryAdd("TimeBomb", true);
                break;
            case "RECIPE_AlienInvasion":
                DistinctHistoryAdd("AlienInvasion", true);
                break;
            case "RECIPE_StabilizerI":
                DistinctHistoryAdd("StabilizerI", true);
                break;
            case "RECIPE_StabilizerII":
                DistinctHistoryAdd("StabilizerII", true);
                break;
            case "RECIPE_EnhancedVision":
                DistinctHistoryAdd("EnhancedVision", true);
                break;
            case "RECIPE_Overheat":
                DistinctHistoryAdd("Overheat");
                break;
            case "RECIPE_Explosive":
                DistinctHistoryAdd("Explosive");
                break;
            case "RECIPE_Corrosion":
                DistinctHistoryAdd("Corrosion");
                break;
            case "RECIPE_RadiationPoisoning":
                DistinctHistoryAdd("RadiationPoisoning");
                break;
        }
    }

    private void DistinctHistoryAdd(string s, bool important = false)
    {
        if (!history.Contains(s))
        {
            if (important)
            {
                history.Insert(0,s);
            }
            else
            {
                history.Add(s);
            }
            

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
