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
    public TextMeshProUGUI etaText;

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

        navigationDAO.SetShipBearing(new Vector3(Mathf.Round(bearing.x), Mathf.Round(bearing.y), Mathf.Round(bearing.z)));
        OnBearingUpdate.Invoke();
    }
}
