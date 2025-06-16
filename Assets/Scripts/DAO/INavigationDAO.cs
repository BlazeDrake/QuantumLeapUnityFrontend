using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface for accessing navigation data
/// </summary>
public interface INavigationDAO
{
    /// <summary>
    /// Sends a course request to the flight director for the specified destination.
    /// </summary>
    /// <param name="destination">The destination for the course.</param>
    public abstract void RequestCourse(string destination);
    /// <summary>
    /// Gets the target location for the current course.
    /// </summary>
    /// <returns>The current target locatoin</returns>
    public abstract Vector3 GetTargetLoc();
    /// <summary>
    /// Gets the estimated time of arrival in milliseconds for a given engine speed.
    /// </summary>
    /// <param name="engineSpeed">The engine speed to calculate ETA for.</param>
    /// <returns>ETA in milliseconds, or -1 iif there is an error.</returns>
    public abstract float GetETAInMilliseconds(int engineSpeed = 0);  

}
