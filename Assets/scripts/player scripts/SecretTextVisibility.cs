using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SecretTextVisibility : MonoBehaviour
{
    [SerializeField] Slider debugSlider;
    public static float allSecretTextMovementMultiplier;

    private void Start()
    {
        allSecretTextMovementMultiplier = 0;
    }

    public void ChangeSecretTextVisibility(float f)
    {
        allSecretTextMovementMultiplier = f;

        if (allSecretTextMovementMultiplier < 0.01f)
        {
            GameStateManager.gameStates.canReadSecretText = true;
            GameStateManager.gameStates.canSeeSecretText = true;
        }
        else
        {
            GameStateManager.gameStates.canReadSecretText = false;
            GameStateManager.gameStates.canSeeSecretText = false;
        }


        foreach (var item in ShowSecretText.currentSceneTexts) 
        { 
            item.GetComponent<SecretText>().movementMultiplier = allSecretTextMovementMultiplier; 
        }

        
    }

    public void DebugSliderValueChange()
    {
        ChangeSecretTextVisibility(debugSlider.value);
    }
}
