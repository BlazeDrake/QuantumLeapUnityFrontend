using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Representation of a contact on the sensor map
/// </summary>
public class MapTarget : MonoBehaviour
{
    public ISensorsDAO.Target target;

    private SensorMapController sensorMapController;
    // Start is called before the first frame update
    void Start()
    {
        sensorMapController = FindObjectOfType<SensorMapController>();
    }

    public void ShowInfo()
    {
        sensorMapController.DisplayTargetInfo(target);  
    }
}
