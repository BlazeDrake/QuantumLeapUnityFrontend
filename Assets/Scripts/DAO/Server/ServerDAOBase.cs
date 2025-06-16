using System.Collections;
using System.Collections.Generic;
using System.Text.Json;
using UnityEngine;


/// <summary>
/// Base class for server data access objects, providing command handling and state management.
/// </summary>
public class ServerDAOBase<T> : MonoBehaviour
{
    protected T curState;

    protected string stationName;
    protected Queue<CommandResult> commands = new Queue<CommandResult>();
    protected HttpController httpController;
    private long cursor = 0;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        httpController = FindObjectOfType<HttpController>();
        httpController.OnPoll.AddListener(HandleCommands);
    }

    /// <summary>
    /// Handles incoming commands and updates the current state.
    /// </summary>
    protected virtual void HandleCommands()
    {
        foreach (var command in httpController.GetCommands(cursor, stationName))
        {
            commands.Enqueue(command);
        }
        cursor = httpController.Cursor;
        while (commands.Count > 0)
        {
            var command = commands.Dequeue();
            string payload = command.ExtensionData["payload"].GetRawText();
            curState = JsonSerializer.Deserialize<T>(payload, new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
                MaxDepth = 10
            });

        }
    }

    /// <summary>
    /// Generates a random identifier.
    /// </summary>
    protected int GenerateId()
    {
        return Random.Range(100000, 999999);
    }
    


}
