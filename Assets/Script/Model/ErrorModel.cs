using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErrorModel
{
    public string message;
    public ExtenstionErrorModel extensions;

}
public class ExtenstionErrorModel
{
    public string code;
    public ResponseErrorModel response;
}

public class ResponseErrorModel
{
    public int statusCode;
    public string[] message;
    public string error;
}
