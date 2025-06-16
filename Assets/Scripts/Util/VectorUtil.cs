using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Provides utility methods for vector operations.
/// </summary>
public static class VectorUtil
{
    /// <summary>
    /// Rounds each component of the given vector to the nearest integer.
    /// </summary>
    /// <param name="inputVec">The vector to round.</param>
    /// <returns>A new vector with each component rounded.</returns>
    public static Vector3 RoundVector(Vector3 inputVec)
    {
        return new Vector3(Mathf.Round(inputVec.x), Mathf.Round(inputVec.y), Mathf.Round(inputVec.z));
    }

    /// <summary>
    /// Determines whether two vectors are approximately equal, comparing each component using Mathf.Approximately.
    /// </summary>
    /// <param name="a">The first vector to compare.</param>
    /// <param name="b">The second vector to compare.</param>
    /// <returns>True if all components are approximately equal; otherwise, false.</returns>
    public static bool VectorApproximatelyEq(Vector3 a, Vector3 b)
    {
        return Mathf.Approximately(a.x, b.x) &&
               Mathf.Approximately(a.y, b.y) &&
               Mathf.Approximately(a.z, b.z);
    }
}
