using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegisterModel
{
    public string username;
    public string email;
    public string password;
    public RegisterModel(string _username, string _email, string _password)
    {
        username = _username;
        email = _email;
        password = _password;
    }
    public RegisterModel(string _username, string _password)
    {
        username = _username;
        password = _password;
    }
}
