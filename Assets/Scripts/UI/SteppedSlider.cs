using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// Class that constrains a unity slider to specific steps
/// </summary>
public class SteppedSlider : MonoBehaviour
{
    public Slider slider;
    public float minValue=0f;
    public float maxValue=1f;
    public float stepRate;

    public UnityEvent<float> OnValueChanged;

    void Start()
    {
        int stepCount = Mathf.RoundToInt((maxValue-minValue) / stepRate);    

        // Ensure slider is properly set up
        slider.wholeNumbers = true;
        slider.minValue = 0;
        slider.maxValue = stepCount;
        slider.onValueChanged.AddListener(SliderValueListener);
    }

    private void SliderValueListener(float value)
    {
        float returnVal = (value * stepRate) + minValue;
        if (value == slider.maxValue)
        {
            returnVal = maxValue;
        }
        OnValueChanged.Invoke(returnVal);
    }
}

