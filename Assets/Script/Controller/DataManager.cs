using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    public UserModel userInfo = new UserModel();
    public ZombieModel[] listCard;
    public Dictionary<string, List<ZombieModel>> listCardMosterUser = new Dictionary<string, List<ZombieModel>>(); 
    public Dictionary<string, List<ZombieModel>> listCardEquipmentUser = new Dictionary<string, List<ZombieModel>>(); 
    public void ClassifyCardUser()
    {
        ZombieModel[] arrCard = listCard;
        for (int i = 0; i < arrCard.Length; i++)
        {
            Debug.LogError(arrCard[i].type);
            switch (arrCard[i].type)
            {
                case TypeCard.MONSTER:
                    AddCardToDictionary(listCardMosterUser, arrCard[i]);
                    break;
                case TypeCard.EQUIPMENT:
                    AddCardToDictionary(listCardEquipmentUser, arrCard[i]);
                    break;
            }
        }
    }    
    private void AddCardToDictionary(Dictionary<string, List<ZombieModel>> listCard, ZombieModel zombie)
    {
        if (listCard.ContainsKey(zombie.name))
            listCard[zombie.name].Add(zombie);
        else
        {
            listCard.Add(zombie.name, new List<ZombieModel>());
            listCard[zombie.name].Add(zombie);
        }
    }
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
