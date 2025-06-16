using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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


    public async void ModifyFusionSpeed(int val)
    {
        int newSpeed = engineDAO.GetFusionSpeed() + val;
        newSpeed = Mathf.Clamp(newSpeed, 0, engineDAO.GetMaxFusionSpeed());

        await engineDAO.SetFusionSpeed(newSpeed);
        UpdateUI();
    }

    public void SetFusonSpeed(int value)
    {
        engineDAO.SetFusionSpeed(value);
    }

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
