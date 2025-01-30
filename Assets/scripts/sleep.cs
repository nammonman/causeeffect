using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sleep : MonoBehaviour
{
    [SerializeField] GameObject secondFloorCameraClose;
    [SerializeField] GameObject secondFloorCameraFar;
    [SerializeField] GameObject firstFloorCamera;

    private void OnEnable()
    {
        secondFloorCameraClose.SetActive(false);
        secondFloorCameraFar.SetActive(true);
        firstFloorCamera.SetActive(false);
    }

    public void GoToSleep()
    {
        if (GameStateManager.gameStates.currentDay < 6)
        {
            firstFloorCamera.SetActive(true);
            secondFloorCameraClose.SetActive(false);
            secondFloorCameraFar.SetActive(false);
            GameStateManager.setDateTime(GameStateManager.gameStates.currentDay + 1, 0);
            List<string> funcs = new List<string>
            {
                "RandomDreamText",
                "FadeBlack_4",
                "ChangeScene_WorkHallway",
                $"ChangeSetting_WorkHallwayNPC{GameStateManager.gameStates.currentDay.ToString()}",
                "IncrementFix",
                "Wait_2",
                $"NewTLTitle_BEGIN DAY {GameStateManager.gameStates.currentDay.ToString()}"
            };
            if (GameStateManager.gameStates.fixLevel == 1)
            {
                funcs.Add("pause");
                funcs.Add("Glitch_2");
                funcs.Add("Wait_1");
                funcs.Add("unpause");
                funcs.Add("Monologue_you can now hold [TAB] to access something...");
            }
            TriggerRunner.RunFuncsCaller(funcs);
        }
        
    }

}
