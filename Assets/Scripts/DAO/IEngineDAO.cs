using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public interface IEngineDAO
{
    public abstract int GetFusionSpeed();
    public abstract Task SetFusionSpeed(int speed);
    public abstract int GetMaxFusionSpeed();
}
