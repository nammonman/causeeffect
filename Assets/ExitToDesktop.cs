using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitToDesktop : MonoBehaviour
{
    public void QuitGame()
    {
        // Logs a message when testing in the Unity Editor
#if UNITY_EDITOR
        Debug.Log("Exit to Desktop called. Application.Quit() does not work in the Editor.");
#else
        // Quits the application when running as a built application
        Application.Quit();
#endif
    }
}
