using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginModel
{
    public string email;
    public string password;
    
    public LoginModel(string _email, string _password)
    {
        email = _email;
        password = _password;
    }
}
