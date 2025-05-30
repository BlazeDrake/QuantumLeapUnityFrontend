using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorsDAOMemory : MonoBehaviour, ISensorsDAO
{
    [SerializeField]
    private List<ISensorsDAO.Target> targets = new List<ISensorsDAO.Target>();
    [SerializeField]
    private string queryResponse = "";

    [SerializeField]
    private string curCustomQuery;
    public List<ISensorsDAO.Target> GetTargets()
    {
        return targets;
    }
    public string CheckForQueryResponse()
    {
        return queryResponse;
    }
    public void SendCustomScanQuery(string query)
    {
        curCustomQuery = query;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
