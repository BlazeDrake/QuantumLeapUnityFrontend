using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.Json;
using System.Text.Json.Serialization;

[System.Serializable]
public class CommandResult
{
    public static string UnrecognizedCommandType = "unrecognized-command";
    public static string StateUpdatedType = "state-updated";
    public static string NoChangeType = "no-change";
    public static string ErrorType = "error";

    public long rowId { get; set; }
    public string commandResultId { get; set; }
    public string type { get; set; }
    public string commandId { get; set; }
    public string clientId { get; set; }
    public string system { get; set; }
    public DateTimeOffset timestamp { get; set; }

    [JsonExtensionData]
    [Description("includes the payload")]
    public Dictionary<string, JsonElement> ExtensionData { get; set; }

}