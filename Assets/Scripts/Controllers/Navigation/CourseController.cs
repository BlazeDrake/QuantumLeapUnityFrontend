using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CourseController : MonoBehaviour
{
    private INavigationDAO navigationDAO;

    [SerializeField]
    private TMP_Dropdown courseDropdown;
    [SerializeField]
    private string defaultCourseName = "Select Course";

    [SerializeField]
    private Transform playerRep;

    [SerializeField]
    private TextMeshProUGUI etaText;

    [SerializeField]
    private string offCourseString;

    [SerializeField]
    private string notMovingString;

    [SerializeField]
    private string arrivalString;

    [SerializeField]
    private string etaFormat;

    public UnityEvent OnTargetSet;
    public UnityEvent OnTargetRemoved;
    public UnityEvent OnBearingUpdate;

    private List<INavigationDAO.Target> targets;
    // Start is called before the first frame update
    void Start()
    {
        navigationDAO=GetComponent<INavigationDAO>();
        RefreshTargets();
    }

    private void Update()
    {
        RecalculateBearing();
        if((navigationDAO.GetTargetLoc() - navigationDAO.GetShipPos()).magnitude < navigationDAO.GetArrivalDist())
        {
            etaText.text = arrivalString;
        }
        else if (VectorUtil.VectorApproximatelyEq(navigationDAO.GetShipHeading(), navigationDAO.GetShipBearing()))
        {
            if (Mathf.Approximately(0f, navigationDAO.GetFusionSpeed()))
            {
                etaText.text = notMovingString;
            }
            else
            {
                float seconds = Vector3.Distance(navigationDAO.GetShipPos(), navigationDAO.GetTargetLoc()) / navigationDAO.GetFusionSpeed();

                var time = TimeSpan.FromSeconds(seconds);
                //eta
                etaText.text = string.Format(etaFormat, Math.Truncate(time.TotalHours), time.Minutes, time.Seconds);
            }

        }
        else
        {
            etaText.text = offCourseString;
        }
    }

    public void RefreshTargets()
    {
        targets = navigationDAO.GetTargets();
        courseDropdown.ClearOptions();
        courseDropdown.options.Add(new TMP_Dropdown.OptionData(defaultCourseName));
        foreach (var target in targets)
        {
            courseDropdown.options.Add(new TMP_Dropdown.OptionData(target.name));
        }
        courseDropdown.value = 0; // Reset to default course
        courseDropdown.RefreshShownValue();
        SetTarget(0);
    }

    public void SetTarget(int index)
    {
        if (index == 0) {
            navigationDAO.SetTargetLoc(Vector3.zero);
            OnTargetRemoved.Invoke();
        }
        else
        {
            var target = targets[index - 1];
            navigationDAO.SetTargetLoc(target.position);
            OnTargetSet.Invoke();

            RecalculateBearing();
        }

    }

    public void RecalculateBearing()
    {
        playerRep.position = navigationDAO.GetShipPos();
        playerRep.LookAt(navigationDAO.GetTargetLoc());

        var bearing = playerRep.eulerAngles;

        navigationDAO.SetShipBearing(VectorUtil.RoundVector(bearing));
        OnBearingUpdate.Invoke();
    }
}
