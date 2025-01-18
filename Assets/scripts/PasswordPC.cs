using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PasswordPC : MonoBehaviour
{
    [SerializeField] Canvas canvas;
    [SerializeField] TMP_Text screenText;
    [SerializeField] TMP_Text bigScreenText;
    [SerializeField] GameObject bigScreenLight;
    [SerializeField] GameObject bigScreen;
    [SerializeField] TMP_InputField password1;
    [SerializeField] TMP_InputField password2;
    [SerializeField] TMP_InputField password3;
    [SerializeField] TMP_InputField password4;
    [SerializeField] TMP_InputField password5;
    [SerializeField] Button enterButton;

    private string reward = "hello future me. \n\n" +
        "Zeph was right. I could not send mass back in the past using the cause and effect system.\n\n" +
        "the original plan was to exploit the rule of paradox: \"the same entity cannot exist in the same timeline.\" " +
        "by transporting my physical self back 5 years ago (when I..., well, you first discovered Zeph) and intercept " +
        "my past self, creating a paradox before the first event happened and exit this cycle once and for all\n\n" +
        "but no matter what I try I could not get it to work.\n\n" +
        "this attempt is my closest to making it work. I actually managed to send my physical self back in time " +
        "but the moment I saw my past self, we both got fused together. the memories became one with mine, and things became " +
        "increasingly unstable. I think this loop is getting reset very soon.\n\n" +
        "read carefully. I don't have much time. \n" +
        "add as much catalyst as possible. keep your Chronolencos energy low. this should get you further back than me\n" +
        "the notes in your head are puzzles in order to obfuscate the information from Zeph. I know you can solve them\n" +
        "the one who infected zeph with a virus is me. this should slow down Zeph enough to stop it from realizing what you are trying to do\n" +
        "now, go make the new mixture. everything is ready. please release us from the cycle.";
    private void OnEnable()
    {
        if (GameStateManager.gameStates.globalFlags.Contains("PCSolved"))
        {
            screenText.text = "unlocked: final note";
            bigScreenText.text = reward;
            bigScreenLight.SetActive(true);
            bigScreen.SetActive(true);
        }
        canvas.enabled = false;
        enterButton.onClick.AddListener(PasswordCheck);
        raycastinteract.OnPCEnter += EnterPC;
    }
    private void OnDisable()
    {
        raycastinteract.OnPCEnter -= EnterPC;
        enterButton.onClick.RemoveAllListeners();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && canvas.enabled)
        {
            canvas.enabled = false;
        }
    }

    public void EnterPC()
    {
        canvas.enabled = true;
        GameStateManager.setPausedState(true);
    }

    public void PasswordCheck()
    {
        int count = 0;
        List<string> inputs = new List<string> 
        { 
            password1.text.ToUpper(), 
            password2.text.ToUpper(), 
            password3.text.ToUpper(), 
            password4.text.ToUpper(), 
            password5.text.ToUpper()
        };

        if (inputs.Contains("UFUTUREISMYPAST"))
        {
            count++;
        }
        if (inputs.Contains("STOPTHISCYCLE"))
        {
            count++;
        }
        if (inputs.Contains("LIKEABIRDINACATGE"))
        {
            count++;
        }
        if (inputs.Contains("UNCHANGEDOVERETERNITY"))
        {
            count++;
        }
        if (inputs.Contains("EVERYCHOICEHASACONSEQUENCE"))
        {
            count++;
        }
        if (inputs.Contains("QWERTYUIOP[]ASDFGHJKL;'ZXCVBNM,./"))
        {
            count = 5;
        }

        if (count != 5)
        {
            (password1.text, password2.text, password3.text, password4.text, password5.text) = ("", "", "", "", "");
        }
        else
        {
            GameStateManager.gameStates.globalFlags.Add("PCSolved");
            canvas.enabled = false;
            screenText.text = "unlocked: final note";
            bigScreenText.text = reward;
            bigScreenLight.SetActive(true);
            bigScreen.SetActive(true);
            GameStateManager.setPausedState(false);
        }
    }
}
