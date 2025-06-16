using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.ComponentModel;
using UnityEngine.Events;

/// <summary>
/// Handles HTTP communication with the server, including command posting and polling for results.
/// </summary>
public class HttpController : MonoBehaviour
{
    private class PostCommandRequest
    {
        public string ClientSecret { get; set; }
        public string Type { get; set; }
        public object payload { get; set; }
        public PostCommandRequest(string clientSecret, string type, object payload)
        {
            this.ClientSecret = clientSecret;
            this.Type = type;
            this.payload = payload;
        }
    }


    private class GetCommandsResult
    {
        public List<CommandResult> results { get; set; }
        public long nextCursor { get; set; }
    }


    public RegisterClientRequest registerRequest;
    public long Cursor { get => cursor; }
    public bool IsReady { get; private set; } = false;

    public UnityEvent OnPoll;

    [SerializeField]
    private float updateRate = 0.5f;

    [SerializeField]
    [Description("Whether to update to the latest cursor on start. If false, will start at cursor 0.")]
    private bool startAtLatestCursor = true;

    private string secret;
    private long cursor = 0;
    private List<CommandResult> commands;

    private static HttpClient httpClient = new()
    {
        BaseAddress = new Uri("http://localhost:5002"),
    };
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(UpdateRoutine());
    }


    //Registering

    /// <summary>
    /// Registers the client with the server asynchronously.
    /// </summary>
    public async Task Register()
    {
        StringContent request = new StringContent(
            JsonUtility.ToJson(registerRequest),
            System.Text.Encoding.UTF8,
            "application/json"
        );
        using HttpResponseMessage response = await httpClient.PostAsync("/register", request);

        if (response.IsSuccessStatusCode)
        {
            var jsonResponse = await response.Content.ReadAsStringAsync();
            //   Debug.Log("Response: " + jsonResponse);

            RegisterClientResult result = JsonSerializer.Deserialize<RegisterClientResult>(jsonResponse, new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            });

            secret = result.ClientSecret;

        }
        else
        {
            Debug.Log("Error: " + response.StatusCode + "(" + response.ReasonPhrase + ")");
        }
    }

    /// <summary>
    /// Posts a command to the server. Returns true if successful, otherwise returns false
    /// </summary>
    /// <param name="type"></param>
    /// <param name="payload"></param>
    /// <returns></returns>
    public async Task<bool> PostCommand(string type, object payload)
    {

        var request = new PostCommandRequest(secret, type, payload);

        string json = JsonSerializer.Serialize(request);

        StringContent content = new StringContent(
            json,
            System.Text.Encoding.UTF8,
            "application/json"
        );
        using HttpResponseMessage response = await httpClient.PostAsync("/command", content);
        if (!response.IsSuccessStatusCode)
        {
            Debug.Log("Error: " + response.StatusCode + "(" + response.ReasonPhrase + ")");
            return false;
        }

        return true;
    }

    /// <summary>
    /// Polls the server for new commmands, updating the internal collection of commands.
    /// </summary>
    public async Task Poll()
    {
        string requestUri = $"/results?cursor={cursor}";
        using HttpResponseMessage response = await httpClient.GetAsync(requestUri);
        if (response.IsSuccessStatusCode)
        {
            var jsonResponse = await response.Content.ReadAsStringAsync();
            GetCommandsResult result = JsonSerializer.Deserialize<GetCommandsResult>(jsonResponse, new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
                MaxDepth = 10
            });
            cursor = result.nextCursor;
            commands.AddRange(result.results);
            if (IsReady)
            {
                OnPoll.Invoke();
            }
        }
        else
        {
            Debug.Log("Error: " + response.StatusCode + "(" + response.ReasonPhrase + ")");
        }
    }

    /// <summary>
    /// Gets commands from the internal command list, filtered by cursor and system.
    /// </summary>
    /// <param name="startCursor">The starting cursor position.</param>
    /// <param name="system">The system name to filter by.</param>
    /// <param name="onlyGetUpdateCommands">Whether to only get update commands.</param>
    /// <returns>A queue of matching command results.</returns>
    public Queue<CommandResult> GetCommands(long startCursor, string system = null, bool onlyGetUpdateCommands = true)
    {
        var queue = new Queue<CommandResult>();

        for (long i = startCursor; i < commands.Count; i++)
        {
            var command = commands[(int)i];
            if ((system == null || command.system == system)
                && (!onlyGetUpdateCommands || command.type == CommandResult.StateUpdatedType))
            {
                queue.Enqueue(command);
            }
        }

        return queue;
    }

    IEnumerator UpdateRoutine()
    {
        yield return CoroutineUtil.WaitForTask(Register());
        commands = new List<CommandResult>();


        if (startAtLatestCursor)
        {
            long lastCursor = -1;
            while (lastCursor != cursor)
            {
                lastCursor = cursor;
                yield return CoroutineUtil.WaitForTask(Poll());
            }
        }


        IsReady = true;
        while (true)
        {
            CoroutineUtil.WaitForTask(Poll());
            yield return new WaitForSeconds(updateRate);
        }
    }
}
