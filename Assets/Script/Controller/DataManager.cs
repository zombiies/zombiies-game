using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataManager : Singleton<DataManager>
{
    public UserModel userInfo = new UserModel();
    public ZombieModel[] listCard;
    public DeckModel[] listDeckUser;
    public List<NotificationModel> listNotificationUser;
    public double balanceEthUser;
    public double PackFee;
    public Dictionary<string, List<ZombieModel>> listCardMosterUser = new Dictionary<string, List<ZombieModel>>(); 
    public Dictionary<string, List<ZombieModel>> listCardEquipmentUser = new Dictionary<string, List<ZombieModel>>();
    public AuctionModel[] listAllAuction;
    public MatchModel matchStarted;
    public void AddNotification(NotificationModel notification)
    {
        if(listNotificationUser == null)
        {
            listNotificationUser = new List<NotificationModel>();
        }
        listNotificationUser.Insert(0, notification);

    }    
    public void ClassifyCardUser()
    {
        ZombieModel[] arrCard = listCard;
        listCardMosterUser = new Dictionary<string, List<ZombieModel>>();
        listCardEquipmentUser = new Dictionary<string, List<ZombieModel>>();
        for (int i = 0; i < arrCard.Length; i++)
        {
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
    public void RemoveCard(ZombieModel zombie)
    {
        switch (zombie.type)
        {
            case TypeCard.MONSTER:
                RemoveCardFromDictionary(listCardMosterUser, zombie);
                break;
            case TypeCard.EQUIPMENT:
                RemoveCardFromDictionary(listCardEquipmentUser, zombie);
                break;
        }
    }
    public void AddCard(ZombieModel zombie)
    {
        switch (zombie.type)
        {
            case TypeCard.MONSTER:
                AddCardToDictionary(listCardMosterUser, zombie);
                break;
            case TypeCard.EQUIPMENT:
                AddCardToDictionary(listCardEquipmentUser, zombie);
                break;
        }
    }
    private void RemoveCardFromDictionary(Dictionary<string, List<ZombieModel>> listCard, ZombieModel zombie)
    {
        if (!listCard.ContainsKey(zombie.name)) return;
        listCard[zombie.name].RemoveAll(x=>x.tokenId.Equals(zombie.tokenId));
        if (listCard[zombie.name]!= null && listCard[zombie.name].Count == 0)
            listCard.Remove(zombie.name);
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
    public bool isReadAllNotification()
    {

        for (int i = 0; i < listNotificationUser.Count; i++)
        {
            if (!listNotificationUser[i].isRead)
                return false;
        }
        return true;
    }

}
public class UserWalletModel
{
    public string address;
    public string balance;
}
public struct DATA_USER
{
    public const string Remember = "Remember";
    public const string InputEmail = "InputEmail";
    public const string InputPassword = "InputPassword";
    public const string TextStar = "** ";

}
