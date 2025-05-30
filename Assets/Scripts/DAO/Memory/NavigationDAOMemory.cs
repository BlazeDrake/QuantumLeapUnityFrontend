using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationDAOMemory : MonoBehaviour, INavigationDAO
{
    [SerializeField]
    private float arrivalDist;
    [SerializeField]
    private float fusionSpeed = 0f;
    [SerializeField]
    private float maxFusionSpeed;
    [SerializeField]
    private Vector3 shipPos;
    [SerializeField]
    private Vector3 shipHeading;
    [SerializeField]
    private Vector3 shipBearing;
    [SerializeField]
    private Vector3 targetLocation;
    [SerializeField]
    private List<INavigationDAO.Target> targets;

    public float GetArrivalDist()
    {
        return arrivalDist;
    }
    public float GetFusionSpeed()
    {
        return fusionSpeed;
    }

    public float GetMaxFusionSpeed()
    {
       return maxFusionSpeed;
    }

    public Vector3 GetShipBearing()
    {
        return shipBearing;
    }

    public Vector3 GetShipHeading()
    {
        return shipHeading;
    }

    public Vector3 GetShipPos()
    {
        return shipPos;
    }

    public Vector3 GetTargetLoc()
    {
        return targetLocation;
    }

    public List<INavigationDAO.Target> GetTargets()
    {
        INavigationDAO.Target[] targetArray = new INavigationDAO.Target[targets.Count];
        targets.CopyTo(targetArray);
        return new List<INavigationDAO.Target>(targetArray);
    }   

    public void SetFusionSpeed(float speed)
    {
        fusionSpeed = Mathf.Clamp(speed,0,maxFusionSpeed);
    }

    public void SetMaxFusionSpeed(float maxSpeed)
    {
        float curSpeedPercent = Mathf.InverseLerp(0,maxFusionSpeed, fusionSpeed);
        maxFusionSpeed = maxSpeed;
        fusionSpeed = maxFusionSpeed * curSpeedPercent;
    }

    public void SetShipBearing(Vector3 bearing)
    {
        shipBearing = bearing;
    }

    public void SetShipHeading(Vector3 heading)
    {
        shipHeading = heading;
    }

    public void SetShipPos(Vector3 shipPos)
    {
        this.shipPos = shipPos;
    }

    public void SetTargetLoc(Vector3 targetLoc)
    {
        targetLocation = targetLoc;
    }

}
