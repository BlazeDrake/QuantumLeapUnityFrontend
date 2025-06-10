using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ManualQueryController : MonoBehaviour
{
    private ISensorsDAO sensorsDAO;

    [SerializeField]
    private TextMeshProUGUI responseText;

    [SerializeField]
    private GameObject resultParent;

    [SerializeField]
    private GameObject waitingParent;

    private string lastResponse;

    // Start is called before the first frame update
    void Start()
    {
        sensorsDAO = GetComponent<ISensorsDAO>();
        lastResponse = sensorsDAO.CheckForQueryResponse();
    }

    // Update is called once per frame
    void Update()
    {
        var response = sensorsDAO.CheckForQueryResponse();
        if (response != lastResponse)
        {
            resultParent.SetActive(true);
            waitingParent.SetActive(false);
            responseText.text = response;
            lastResponse = response;
        }
    }

    public void SendQuery(string query)
    {
        sensorsDAO.SendCustomScanQuery(query);
        resultParent.SetActive(false);
        waitingParent.SetActive(true);
    }
}
