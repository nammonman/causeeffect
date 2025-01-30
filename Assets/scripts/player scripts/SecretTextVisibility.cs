using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SecretTextVisibility : MonoBehaviour
{
    [SerializeField] Slider debugSlider;
    [SerializeField] Rigidbody playerRigidbody;
    public static float allSecretTextMovementMultiplier;
    private bool isToggled = false;

    private void Start()
    {
        allSecretTextMovementMultiplier = 0;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T) && GameStateManager.gameStates.globalFlags.Contains("MIX_EnhancedVision"))
        {
            isToggled = !isToggled;
            GameStateManager.setSecretText(isToggled);
        }

        if (isToggled)
        {
            // Get the velocity magnitude of the player and clamp it to the range 0 - 2
            float velocityMagnitude = playerRigidbody.velocity.magnitude;
            float clampedValue = Mathf.Clamp(velocityMagnitude/2, 0f, 2f);

            ChangeSecretTextVisibility(clampedValue);
        }
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
