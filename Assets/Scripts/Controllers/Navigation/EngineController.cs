using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

/// <summary>
/// Controls the engine UI, allowing speed adjustments and updating the display.
/// </summary>
public class EngineController : MonoBehaviour
{
    private IEngineDAO engineDAO;


    [SerializeField]
    private TextMeshProUGUI speedText;

    [SerializeField]
    private Button speedUp;
    [SerializeField] 
    private Button speedDown;

    [SerializeField]
    private float updateDelay;

    // Start is called before the first frame update
    void Start()
    {
        engineDAO=GetComponent<IEngineDAO>();
    }

    /// <summary>
    /// Modifies the fusion speed by the specified value and updates the UI.
    /// </summary>
    /// <param name="val">The value to add to the current speed.</param>
    public async void ModifyFusionSpeed(int val)
    {
        int newSpeed = engineDAO.GetFusionSpeed() + val;
        newSpeed = Mathf.Clamp(newSpeed, 0, engineDAO.GetMaxFusionSpeed());
        SetFusionSpeed(newSpeed);
    }

    /// <summary>
    /// Sets the fusion speed to the specified value and updates the ui
    /// </summary>
    /// <param name="value">The speed value to set.</param>

    public async void SetFusionSpeed(int value)
    {
        await engineDAO.SetFusionSpeed(value);
        UpdateUI();
    }

    /// <summary>
    /// Updates the engine speed UI and button interactability.
    /// </summary>
    public void UpdateUI()
    {
        int speed = engineDAO.GetFusionSpeed();
        speedText.text = speed.ToString();

        if (speed >= engineDAO.GetMaxFusionSpeed())
        {
            speedUp.interactable = false;
        }
        else
        {
            speedUp.interactable = true;    
        }

        if (speed <= 0)
        {
            speedDown.interactable = false;
        }
        else
        {
            speedDown.interactable = true;
        }
    }
}
