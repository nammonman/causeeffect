using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ScreenGlitch : MonoBehaviour
{
    [SerializeField] Volume glitchVolume;
    private void OnEnable()
    {
        GameStateManager.OnStartGlitch += StartScreenGlitch;
        GameStateManager.OnStopGlitch += StopScreenGlitch;
    }

    private void OnDisable()
    {
        GameStateManager.OnStartGlitch -= StartScreenGlitch;
        GameStateManager.OnStopGlitch -= StopScreenGlitch;
    }

    private void Start()
    {
        glitchVolume.enabled = false;
    }

    public void StartScreenGlitch()
    {
        glitchVolume.enabled = true;
    }

    public void StopScreenGlitch()
    {
        glitchVolume.enabled = false;
    }
}
