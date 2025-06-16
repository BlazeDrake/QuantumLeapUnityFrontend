using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostCommandRequest
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
