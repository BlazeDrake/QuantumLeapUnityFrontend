using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INavigationDAO
{
    [System.Serializable]
    public struct Target
    {
        public Vector3 position;
        public string name;
    }
    public abstract float GetFusionSpeed();
    public abstract void SetFusionSpeed(float speed);
    public abstract float GetMaxFusionSpeed();
    public abstract void SetMaxFusionSpeed(float maxSpeed);
    public abstract Vector3 GetTargetLoc();

    public abstract void SetTargetLoc(Vector3 targetLoc);
    public abstract Vector3 GetShipPos();
    public abstract void SetShipPos(Vector3 shipPos);
    public abstract Vector3 GetShipHeading();
    public abstract void SetShipHeading(Vector3 heading);
    public abstract Vector3 GetShipBearing();

    public abstract void SetShipBearing(Vector3 bearing);

    public abstract List<Target> GetTargets();





}
