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
    private IEngineDAO engineDAO;


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

    private bool hasCourse = false;

    private Vector3 prevTargetLoc = Vector3.zero;

    public UnityEvent OnTargetSet;
    public UnityEvent OnTargetRemoved;
    public UnityEvent OnBearingUpdate;

    // Start is called before the first frame update
    void Start()
    {
        navigationDAO=GetComponent<INavigationDAO>();
        engineDAO=GetComponent<IEngineDAO>();
        prevTargetLoc = Vector3.zero;
    }

    private void Update()
    {
        var targetLoc = navigationDAO.GetTargetLoc();

        if (hasCourse)
        {
            if (targetLoc == Vector3.zero)
            {
                OnTargetRemoved.Invoke();
                hasCourse = false;
            }
            else
            {
                if (navigationDAO.GetETAInMilliseconds(1) <= 0)
                {
                    etaText.text = arrivalString;
                    hasCourse = false;
                    OnTargetRemoved.Invoke();
                }
                else if (true/*VectorUtil.VectorApproximatelyEq(navigationDAO.GetShipHeading(), navigationDAO.GetShipBearing())*/)
                {
                    if (Mathf.Approximately(0f, engineDAO.GetFusionSpeed()))
                    {
                        etaText.text = notMovingString;
                    }
                    else
                    {
                        var time = TimeSpan.FromMilliseconds(navigationDAO.GetETAInMilliseconds(engineDAO.GetFusionSpeed()));
                        //eta
                        etaText.text = string.Format(etaFormat, Math.Truncate(time.TotalHours), time.Minutes, time.Seconds);
                    }

                }
                else
                {
                    etaText.text = offCourseString;
                }
            }
        }

        if (targetLoc != prevTargetLoc)
        {
            OnTargetSet.Invoke();
            hasCourse = true;
            RecalculateBearing();
        }

        prevTargetLoc = targetLoc;

    }

    /// <summary>
    /// Sends a course request to the flight director for the specified destination.
    /// </summary>
    /// <param name="destination">The destination for the course.</param>
    public void RequestCourse(string destination)
    {

        navigationDAO.RequestCourse(destination);
    }

    public void RecalculateBearing()
    {
        /*playerRep.LookAt(navigationDAO.GetTargetLoc());

        var bearing = playerRep.eulerAngles;

        navigationDAO.SetShipBearing(VectorUtil.RoundVector(bearing));
        OnBearingUpdate.Invoke();*/
    }
}
