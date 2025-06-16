using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls the ship's heading and bearing, updating UI elements and handling user input for rotation.
/// </summary>
public class HeadingController : MonoBehaviour
{
    [Serializable]
    public struct RotationButtonInfo
    {
        public ButtonHoldDetector button;
        public int index; // 0 for pitch, 1 for yaw, 2 for roll
        public int rotationChange;
    }


    [SerializeField]
    private List<RotationButtonInfo> rotationButtons;

    //replace with thrusters
    private INavigationDAO navigationDAO;
    [SerializeField]
    private List<TextMeshProUGUI> headingTexts;
    [SerializeField]
    private List<TextMeshProUGUI> bearingTexts;
    [SerializeField]
    private List<string> headingFormats;
    [SerializeField]
    private string bearingFormat;

    [SerializeField]
    private float updateDelay;


    private Coroutine updateLoop;

    private void Start()
    {
        // navigationDAO = GetComponent<INavigationDAO>();
        var heading = Vector3.zero;// navigationDAO.GetShipHeading();
        UpdateHeadingTexts(heading);
        UpdateBearingTexts();

        updateLoop = StartCoroutine(UpdateRoutine());
    }
    /// <summary>
    /// Sets the yaw (rotation around the Y axis) to the specified value.
    /// </summary>
    /// <param name="yaw">The yaw value to set.</param>
    public void SetYaw(float yaw)
    {
        SetHeading(1, yaw);
    }

    /// <summary>
    /// Sets the pitch (rotation around the X axis) to the specified value.
    /// </summary>
    /// <param name="pitch">The pitch value to set.</param>
    public void SetPitch(float pitch)
    {
        SetHeading(0, pitch);
    }

    /// <summary>
    /// Sets the roll (rotation around the Z axis) to the specified value.
    /// </summary>
    /// <param name="roll">The roll value to set.</param>
    public void SetRoll(float roll)
    {
        SetHeading(2, roll);
    }

    /// <summary>
    /// Updates the bearing text UI elements to reflect the current bearing.
    /// </summary>
    public void UpdateBearingTexts()
    {
        var bearing = Vector3.zero; //navigationDAO.GetShipBearing();
        for (int i = 0; i < 3; i++)
        {
            bearingTexts[i].text = string.Format(bearingFormat, bearing[i]);
        }
    }

    private void ChangeHeadnig(int index, float amount)
    {
        SetHeading(index, /*navigationDAO.GetShipHeading()[index]*/ 0 + amount);
    }

    private void SetHeading(int index, float val)
    {
        var heading = Vector3.zero;// navigationDAO.GetShipHeading();

        while (val < 0)
        {
            val += 360f;
        }
        while (val >= 360f)
        {
            val -= 360f;
        }

        heading[index] = val;
        UpdateHeadingTexts(heading);
        //navigationDAO.SetShipHeading(heading);
    }

    private void UpdateHeadingTexts(Vector3 heading)
    {
        for (int i = 0; i < 3; i++)
        {
            headingTexts[i].text = string.Format(headingFormats[i], heading[i]);
        }
    }

    private IEnumerator UpdateRoutine()
    {
        while (true)
        {
            foreach (var buttonInfo in rotationButtons)
            {
                if (buttonInfo.button.ButtonPressed)
                {
                    ChangeHeadnig(buttonInfo.index, buttonInfo.rotationChange);
                }
            }
            yield return new WaitForSeconds(updateDelay);
        }
    }
}

