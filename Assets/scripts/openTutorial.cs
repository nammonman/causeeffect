using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public GameObject tutorialPanel;

    private void Start()
    {
        tutorialPanel.SetActive(false);
    }

    public void OpenTutorial()
    {
        tutorialPanel.SetActive(true);
        tutorialPanel.transform.SetAsLastSibling();
    }

    public void CloseTutorial()
    {
        tutorialPanel.SetActive(false);
    }
}