using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class EngineDAOServer : ServerDAOBase<EnginesState>, IEngineDAO
{

    private class SetSpeedPayload
    {
        public int speed { get; set; }
        public SetSpeedPayload(int speed)
        {
            this.speed = speed;
        }
    }

    private Dictionary<int, int> powerByMaxSpeed;

    protected override void Start()
    {
        stationName = "sublight-engines";
        base.Start();
    }

    protected override void HandleCommands()
    {
        base.HandleCommands();
        if (curState != null)
        {
            powerByMaxSpeed = new Dictionary<int, int>();
            foreach (SpeedPowerRequirement speedReq in curState.SpeedPowerRequirements)
            {
                powerByMaxSpeed.Add(speedReq.Speed, speedReq.PowerNeeded);
            }
        }
    }

    public int GetFusionSpeed()
    {
        return curState?.CurrentSpeed ??0;
    }

    public int GetMaxFusionSpeed()
    {
        int maxSpeed = curState?.SpeedConfig?.MaxSpeed ?? 0;

        if (curState != null)
        {
            if (curState.CurrentPower < curState.RequiredPower)
            {
                maxSpeed = 0;
            }
            else
            {
                int powerReq;
                while (powerByMaxSpeed.TryGetValue(maxSpeed, out powerReq) && powerReq > curState.CurrentPower)
                {
                    maxSpeed--;
                }
            }
        }

        return maxSpeed;
    }

    public async Task SetFusionSpeed(int speed)
    {
        var payload = new SetSpeedPayload(speed);
        if(await httpController.PostCommand("set-sublight-engines-speed", payload))
        {
            curState.CurrentSpeed = speed;
        }
        
    }

}
