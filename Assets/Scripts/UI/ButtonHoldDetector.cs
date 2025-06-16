using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Detects and tracks button hold events for UI interactions.
/// </summary>
public class ButtonHoldDetector : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool buttonPressed = false;
    private Button attachedButton;

    /// <summary>
    /// Gets whether the button is currently pressed.
    /// </summary>

    public bool ButtonPressed
    {
        get { return buttonPressed; }
    }


    void Start()
    {
        attachedButton = GetComponent<Button>();
        if (attachedButton == null)
        {
            Debug.LogError("ButtonHoldDetector requires a Button component on the same GameObject.");
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (attachedButton.interactable)
        {
            buttonPressed = true;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        buttonPressed = false;
    }
}
