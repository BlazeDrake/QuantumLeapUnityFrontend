using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISensorsDAO
{
    [System.Serializable]
    public class Target
    {
        public Vector3 position;
        public string name;
        public string scanInfo;
    }
    public abstract List<Target> GetTargets();
    public abstract string CheckForQueryResponse();
    public abstract void SendCustomScanQuery(string query);
}
