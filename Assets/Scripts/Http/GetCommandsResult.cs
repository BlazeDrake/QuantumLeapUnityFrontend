using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetCommandsResult
{
    public List<CommandResult> results { get; set; }
    public long nextCursor { get; set; }
}

