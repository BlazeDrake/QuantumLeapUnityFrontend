using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public record ViewportJsonState
{
    public string CurrentImage { get; set; }
    public string[] Cards { get; set; }
}
public record ViewportSystemState : StandardSystemBaseState
{
    public ViewportJsonState JsonState { get; set; }
}
