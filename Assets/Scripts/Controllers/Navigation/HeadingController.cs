using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeadingController : MonoBehaviour
{
    private INavigationDAO navigationDAO;
    [SerializeField]
    private List<Slider> headingSliders;
    [SerializeField]
    private List<TextMeshProUGUI> headingTexts;
    [SerializeField]
    private List<TextMeshProUGUI> bearingTexts;
    [SerializeField]
    private List<string> headingFormats;
    [SerializeField]
    private string bearingFormat;

    private void Start()
    {
        navigationDAO = GetComponent<INavigationDAO>();
        var heading = navigationDAO.GetShipHeading();
        UpdateHeadingTexts(heading);
        UpdateSliders(heading);
        UpdateBearingTexts();
    }

    public void SetYaw(float yaw)
    {
        ModifyHeading(1, yaw);
    }
    public void SetPitch(float pitch)
    {
        ModifyHeading(0, pitch);
    }
    public void SetRoll(float roll)
    {
        ModifyHeading(2, roll);
    }

    public void UpdateBearingTexts()
    {
        var bearing = navigationDAO.GetShipBearing();
        for (int i = 0; i < 3; i++)
        {
            bearingTexts[i].text = string.Format(bearingFormat, bearing[i]);
        }
    }

    private void ModifyHeading(int index,float val)
    {
        var heading = navigationDAO.GetShipHeading();
        heading[index] = val;
        UpdateHeadingTexts(heading);
        navigationDAO.SetShipHeading(heading);
    }

    private void UpdateHeadingTexts(Vector3 heading)
    {
        for (int i = 0; i < 3; i++)
        {
            headingTexts[i].text = string.Format(headingFormats[i], heading[i]);
        }
    }
    private void UpdateSliders(Vector3 heading)
    {
        for (int i = 0; i < 3; i++)
        {
            headingSliders[i].value = heading[i];
        }
    }
}
