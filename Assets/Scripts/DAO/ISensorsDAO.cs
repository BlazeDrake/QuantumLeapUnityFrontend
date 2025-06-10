using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISensorsDAO
{
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
    public abstract List<Target> GetTargets();
    public abstract string CheckForQueryResponse();
    public abstract void SendCustomScanQuery(string query);
    public abstract float SensorRange { get; }
}
