using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonHoldDetector : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool buttonPressed = false;
    private Button attachedButton;

    void Start()
    {
        attachedButton = GetComponent<Button>();
        if (attachedButton == null)
        {
            Debug.LogError("ButtonHoldDetector requires a Button component on the same GameObject.");
        }
    }

    public bool ButtonPressed
    {
        get { return buttonPressed; }
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
