using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndingChooser : MonoBehaviour
{
    [SerializeField] Button sleepButton;

    private void OnEnable()
    {
        if (GameStateManager.gameStates.currentDay >= 6)
        {
            sleepButton.onClick.RemoveAllListeners();
            sleepButton.onClick.AddListener(ChooseEnding);
        }
    }

    public void ChooseEnding()
    {
        // check for conditions for each ending

        /// for later: after each ending sequence is played, load timeline with id 2 (the first time player gets LATE FOR WORK)

        // ending 0 (joke ending): lash out at The President on the first day
        // flags: handled in dialogue

        // ending 1 (bad ending): made Zeph's mixture but fail to hack
        // flags: "zephMixture", "FAIL HACK DOCUMENT" 

        // ending 2 (bad ending): made Zeph's mixture and hacked sucessfully
        // flags: "zephMixture", "HACKED DOCUMENT" 

        // ending 3 (good ending): made the mixture from the notes but did not made Zeph's mixture
        // flags: to be determined

        // ending 4 (true ending): made the mixture from the notes but and made Zeph's mixture
        // flags: to be determined
    }

    public void GoToSleep()
    {
        GameStateManager.setDateTime(7, 2);
        List<string> funcs = new List<string>
        {
            "RandomDreamText",
            "FadeBlack_4",
            //"ChangeScene_WorkHallway",
            //$"ChangeSetting_WorkHallwayNPC{GameStateManager.gameStates.currentDay.ToString()}",
            //"IncrementFix",
            "Wait_2",
            $"NewTLTitle_BEGIN DAY {GameStateManager.gameStates.currentDay.ToString()}"
        };

        TriggerRunner.RunFuncsCaller(funcs);
    }
}
