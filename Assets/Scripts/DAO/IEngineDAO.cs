using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Interface for accessing and controlling engine data.
/// </summary>
public interface IEngineDAO
{
    /// <summary>
    /// Gets the current fusion speed.
    /// </summary>
    public abstract int GetFusionSpeed();
    /// <summary>
    /// Sets the fusion speed asynchronously.
    /// </summary>
    public abstract Task SetFusionSpeed(int speed);

    /// <summary>
    /// Gets the highest fusion speed that the ship can currently get to
    /// </summary>
    public abstract int GetMaxFusionSpeed();
}
