using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndingChooser : MonoBehaviour
{
    [SerializeField] GameObject secondFloorCameraClose;
    [SerializeField] GameObject secondFloorCameraFar;
    [SerializeField] Button sleepButton;
    List<string> funcs = new List<string>
        {
            "RandomDreamText",
            "FadeBlack_4",

        };

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
        if (GameStateManager.gameStates.globalFlags.Contains("MIX_AlienInvasion") && GameStateManager.gameStates.globalFlags.Contains("FAIL HACK DOCUMENT"))
        {
            funcs.Add("ChangeScene_presentation");
            funcs.Add("ChangeSetting_presentationNPC ending 1");
        }


        // ending 2 (bad ending): made Zeph's mixture and hacked sucessfully
        // flags: "zephMixture", "HACKED DOCUMENT" 
        else if (GameStateManager.gameStates.globalFlags.Contains("MIX_AlienInvasion") && GameStateManager.gameStates.globalFlags.Contains("HACKED DOCUMENT"))
        {
            funcs.Add("ChangeScene_presentation");
            funcs.Add("ChangeSetting_presentationNPC ending 2");
        }


        // ending 3 (good ending): made the mixture from the notes but did not made Zeph's mixture
        // flags: to be determined
        else if (!GameStateManager.gameStates.globalFlags.Contains("MIX_AlienInvasion") && GameStateManager.gameStates.globalFlags.Contains("MIX_TimeBomb"))
        {
            funcs.Add("ChangeScene_Home good ending");
            funcs.Add("Monologue_darn it! did not go back far enough");
            funcs.Add("Monologue_Zeph is still there and thats...me");
            funcs.Add("one hell of a out of body experience");
            funcs.Add("I have to try to touch \"me\"");
        }


        // ending 4 (true ending): made the mixture from the notes and made Zeph's mixture
        // flags: to be determined
        else if (GameStateManager.gameStates.globalFlags.Contains("MIX_AlienInvasion") && GameStateManager.gameStates.globalFlags.Contains("MIX_TimeBomb"))
        {
            funcs.Add("ChangeScene_Home true ending");
            funcs.Add("Monologue_yes! I made it before Zeph existed");
            funcs.Add("Monologue_now the only thing left to do is...");
        }

        // default ending
        else 
        {
            funcs.Add("ChangeScene_presentation");
            funcs.Add("ChangeSetting_presentationNPC ending default");

        }

        // start tomorrow
        GoToSleep();
    }

    public void GoToSleep()
    {
        secondFloorCameraClose.SetActive(false);
        secondFloorCameraFar.SetActive(false);
        GameStateManager.setDateTime(7, 2);
        TriggerRunner.RunFuncsCaller(funcs);
    }
}
