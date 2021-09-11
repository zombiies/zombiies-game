using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    public UserModel userInfo = new UserModel();
    public bool IsRemember
    {
        get
        {
            return PlayerPrefs.GetInt(DATA_USER.Remember, 0) == 1;
        }
        set
        {
            if (value)
                PlayerPrefs.SetInt(DATA_USER.Remember, 1);
            else
                PlayerPrefs.SetInt(DATA_USER.Remember, 0);

        }
    }
    public string InputEmail
    {
        get
        {
            return PlayerPrefs.GetString(DATA_USER.InputEmail, "");
        }
        set
        {
            PlayerPrefs.SetString(DATA_USER.InputEmail, value);
        }
    }
    public string InputPassword
    {
        get
        {
            return PlayerPrefs.GetString(DATA_USER.InputPassword, "");
        }
        set
        {
            PlayerPrefs.SetString(DATA_USER.InputPassword, value);
        }
    }


}
public struct DATA_USER
{
    public const string Remember = "Remember";
    public const string InputEmail = "InputEmail";
    public const string InputPassword = "InputPassword";
    public const string TextStar = "** ";

}
