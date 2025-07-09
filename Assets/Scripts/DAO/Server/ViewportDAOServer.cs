using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewportDAOServer : ServerDAOBase<ViewportSystemState>
{
    protected override void Start()
    {
        base.Start();
        stationName = "json-plugin-viewscreen";
    }

    public string GetCurrentImage()
    {
        return curState?.JsonState?.CurrentImage ?? "None";
    }
}
