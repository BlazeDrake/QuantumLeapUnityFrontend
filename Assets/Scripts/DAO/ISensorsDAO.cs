using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface for accessing sensor data and operations.
/// </summary>
public interface ISensorsDAO
{
    /// <summary>
    /// Represents a sensor target with position, orientation, and scan information.
    /// </summary>
    [System.Serializable]
    public struct Target
    {
        public Vector3 position;
        public float yaw;
        public string name;
        public string scanInfo;

        public Target(Vector3 position, float yaw, string name, string scanInfo)
        {
            this.position = position;
            this.yaw = yaw;
            this.name = name;
            this.scanInfo = scanInfo;
        }
    }
    /// <summary>
    /// Retrieves a list of all current sensor targets
    /// </summary>
    /// <returns>A List of all current sensor targets</returns>
    public abstract List<Target> GetTargets();
    /// <summary>
    /// Checks for a response to a sensor query.
    /// </summary>
    /// <returns>The query response string.</returns>
    public abstract string CheckForQueryResponse();
    /// <summary>
    /// Sends a custom scan query to the server and flight director.
    /// </summary>
    /// <param name="query">The query to send</param>
    public abstract void SendCustomScanQuery(string query);
    /// <summary>
    /// Gets the current sensor range.
    /// </summary>
    public abstract float SensorRange { get; }
}
