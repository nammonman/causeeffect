using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class SliderEasing : MonoBehaviour
{
    public Slider slider;            // Reference to the slider component
    public float smoothTime = 0.1f;  // Time for the slider to move to the clicked position
    private float oldSliderValue = 0f;
    private float velocity = 0f;     // Helper for SmoothDamp
    public static bool easeEnable = true;

    void Update()
    {
        if (easeEnable)
        {
            slider.value = Mathf.SmoothDamp(slider.value, (float)Math.Round(slider.value), ref velocity, smoothTime);
            
        }
        //oldSliderValue = slider.value;
    }
}
