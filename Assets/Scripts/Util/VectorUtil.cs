using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VectorUtil
{
    public static Vector3 RoundVector(Vector3 inputVec)
    {
        return new Vector3(Mathf.Round(inputVec.x), Mathf.Round(inputVec.y), Mathf.Round(inputVec.z));
    }

    public static bool VectorApproximatelyEq(Vector3 a, Vector3 b)
    {
        return Mathf.Approximately(a.x, b.x) &&
               Mathf.Approximately(a.y, b.y) &&
               Mathf.Approximately(a.z, b.z);
    }
}
