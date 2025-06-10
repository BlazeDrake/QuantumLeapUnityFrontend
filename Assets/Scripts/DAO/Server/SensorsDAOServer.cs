using System.Collections;
using System.Collections.Generic;
using System.Text.Json;
using UnityEngine;

public class SensorsDAOServer : ServerDAOBase<SensorsState>, ISensorsDAO
{
    private class CancelScanPayload
    {
        public string scanId { get; set; }

        public CancelScanPayload(string scanId)
        {
            this.scanId = scanId;
        }
    }
    private class NewScanPayload
    {
        public string scanId { get; set; }
        public string scanFor { get; set; }

        public NewScanPayload(string scanId, string scanFor)
        {
            this.scanId = scanId;
            this.scanFor = scanFor;
        }
    }

    [SerializeField]
    private float sensorsRange;
    public float SensorRange => sensorsRange;

    private int curScanId;

    public string CheckForQueryResponse()
    {
        return curState?.LastUpdatedScan?.Result ?? "";
    }

    public List<ISensorsDAO.Target> GetTargets()
    {
        var returnVal = new List<ISensorsDAO.Target>();
        if (curState == null)
        {
            return returnVal;
        }
        foreach(var contact in curState.Contacts)
        {
            var target = new ISensorsDAO.Target(contact.Position, 0f, contact.Name, "");
            if (contact.Destinations.Length > 0)
            {
                var pos = contact.Destinations[0].Position;
                target.scanInfo = $"Destination: {pos.X}, {pos.Y}, {pos.Z}";
            }
            else
            {
                target.scanInfo = "Not moving";
            }

            returnVal.Add(target);
        }

        return returnVal;
    }

    public async void SendCustomScanQuery(string query)
    {
        if (curScanId != 0)
        {
            var cancelPayload = new CancelScanPayload(curScanId.ToString());
            await httpController.PostCommand("cancel-sensor-scan", cancelPayload);
        }
        int scanId = Random.Range(100000, 999999);
        curScanId = scanId;
        var scanPayload = new NewScanPayload(scanId.ToString(), query);
        await httpController.PostCommand("new-sensor-scan", scanPayload);
    }

    // Start is called before the first frame update
    override protected void Start()
    {
        stationName = "sensors";
        base.Start();
    }

    private void PrintState()
    {
        if(curState == null)
        {
            Debug.Log("Null state!");
            return;
        }
        Debug.Log($"Current Power: {curState.CurrentPower}");
        Debug.Log($"Required Power: {curState.RequiredPower}");
        Debug.Log($"Disabled: {curState.Disabled}");
        Debug.Log($"Damaged: {curState.Damaged}");

        Debug.Log($"Contaacts: {curState.Contacts.Count}");
        foreach(var contact in curState.Contacts)
        {
            Debug.Log($"Contact ID: {contact.ContactId}, Name: {contact.Name}, Position: {contact.Position}");
        }
        Debug.Log("Last Updated Scan: " + curState.LastUpdatedScan);
        Debug.Log($"Scans: {curState.ActiveScans.Count}");
        foreach(var scan in curState.ActiveScans)
        {
            Debug.Log(scan);
        }

    }

}
