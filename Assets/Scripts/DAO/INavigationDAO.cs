using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INavigationDAO
{
    public abstract void RequestCourse(string destination);
    public abstract Vector3 GetTargetLoc();
    public abstract float GetETAInMilliseconds(int engineSpeed = 0);  

    public abstract Vector3 GetShipHeading();
    public abstract void SetShipHeading(Vector3 heading);
    public abstract Vector3 GetShipBearing();

    public abstract void SetShipBearing(Vector3 bearing);

}
