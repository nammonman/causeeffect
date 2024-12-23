using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class IObjProperties : MonoBehaviour
{
    public bool interactable;
    public bool onetime;
    public List<string> funcs = new List<string>();


    public void RunFuncs()
    {
        if (funcs.Count > 0)
        {
            foreach (var item in funcs)
            {
                string[] f = item.Split('_');
                // funcname_arg1_arg2_arg3_...

                if (f[0] == "FadeBlack")
                {
                    StartCoroutine(FadeBlackForSeconds(int.Parse(f[1])));
                }
                else if (f[0] == "Glitch")
                {
                    StartCoroutine(GlitchForSeconds(int.Parse(f[1])));
                }
                else if (f[0] == "ChaneScene")
                {
                    GameStateManager.setLoadNewScene(f[1]);
                }
                else if (f[0] == "ChaneSceneSetPos")
                {
                    GameStateManager.setLoadNewSceneWithPos(f[1], new Vector3(int.Parse(f[2]), int.Parse(f[3]), int.Parse(f[4])));
                }
                else if (f[0] == "ChaneSetting")
                {
                    GameStateManager.setLoadSceneSetting(f[1]);
                }
            }
        }
    }

    IEnumerator FadeBlackForSeconds(float delay)
    {
        GameStateManager.setPausedState(true);
        GameStateManager.setScreenFadeIn();
        yield return new WaitForSeconds(delay);
        GameStateManager.setScreenFadeOut();
        GameStateManager.setPausedState(false);
    }

    IEnumerator GlitchForSeconds(float delay)
    {
        GameStateManager.setStartGlitch();
        yield return new WaitForSeconds(delay);
        GameStateManager.setStopGlitch();
    } 


}
