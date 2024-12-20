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
                if (item == "FadeBlack_2")
                {
                    StartCoroutine(FadeBlackForSeconds(2));
                }
                else if (item == "Glitch_4")
                {
                    StartCoroutine(GlitchForSeconds(4));
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
