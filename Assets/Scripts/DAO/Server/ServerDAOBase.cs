using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerDAOBase : MonoBehaviour
{
    protected string stationName;
    protected Queue<CommandResult> commands = new Queue<CommandResult>();
    protected HttpController httpController;
    private long cursor = 0;
    public float updateRate = 0.5f;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        httpController = FindObjectOfType<HttpController>();
        httpController.OnPoll.AddListener(HandleCommands);
    }

    protected virtual void HandleCommands()
    {
        foreach (var command in httpController.GetCommands(cursor, stationName))
        {
            commands.Enqueue(command);
        }
        cursor = httpController.Cursor;
    }
    


}
