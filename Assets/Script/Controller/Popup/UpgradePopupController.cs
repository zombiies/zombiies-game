using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradePopupController : Singleton<UpgradePopupController>
{
    [SerializeField] CardInformation cardInformationA = default;
    [SerializeField] CardInformation cardInformationB = default;
    [SerializeField] CardInformation cardInformationR = default;
    [SerializeField] CardInformationUI cardInformationUIA = default;
    [SerializeField] CardInformationUI cardInformationUIB = default;
    [SerializeField] CardInformationUI cardInformationUIR = default;
    [SerializeField] Text txtStartA = default;
    [SerializeField] Text txtStartB = default;
    [SerializeField] Text txtStartR = default;
    [SerializeField] Text txtnumberRequire = default;
    [SerializeField] Button btnUpgrade = default;
    [SerializeField] Button btnExit = default;

    private ZombieModel _zombieCard;
    private List<ZombieModel> _listCard = new List<ZombieModel>();

    // Start is called before the first frame update
    void Awake()
    {
        OnButtonClicked();
    }
    private void OnEnable()
    {

    }
    public void Init(ZombieModel zombie, List<ZombieModel> listCard)
    {
        _zombieCard = zombie;
        _listCard = listCard;
        ShowPage();
    }

    private void OnButtonClicked()
    {
        btnUpgrade.onClick.AddListener(OnUpgradeButtonClicked);
        btnExit.onClick.AddListener(OnExitButtonClicked);
    }
    private void OnExitButtonClicked()
    {
        gameObject.SetActive(false);
    }
    private void OnUpgradeButtonClicked()
    {
        if (CountCard() >= 2)
        {
            ZombieModel zombie1 = new ZombieModel();
            for (int i = 0; i < _listCard.Count; i++)
            {
                if (_listCard[i].level == _zombieCard.level && !_listCard[i].tokenId.Equals(_zombieCard.tokenId))

                {
                    zombie1 = _listCard[i];
                    break;
                }
            }
            if (isCardInDeck(_zombieCard))
                GraphQLManager.Instance.RemoveCardFromDeck(_zombieCard.tokenId, DataManager.Instance.listDeckUser[0].ID, () =>
                 {
                     DeckPopupController.Instance.ShowPage();
                     SelecCardPopupController.Instance.ShowCard();
                     if (isCardInDeck(zombie1))
                     {
                         GraphQLManager.Instance.RemoveCardFromDeck(zombie1.tokenId, DataManager.Instance.listDeckUser[0].ID, () =>
                         {
                             DeckPopupController.Instance.ShowPage();
                             SelecCardPopupController.Instance.ShowCard();
                             GraphQLManager.Instance.UpgradeCard(_zombieCard, zombie1);
                         });
                     }
                     else
                         GraphQLManager.Instance.UpgradeCard(_zombieCard, zombie1);

                 });
            else
            {
                if (isCardInDeck(zombie1))
                {
                    GraphQLManager.Instance.RemoveCardFromDeck(zombie1.tokenId, DataManager.Instance.listDeckUser[0].ID, () =>
                    {
                        DeckPopupController.Instance.ShowPage();
                        SelecCardPopupController.Instance.ShowCard();
                        GraphQLManager.Instance.UpgradeCard(_zombieCard, zombie1);
                    });
                }
                else
                    GraphQLManager.Instance.UpgradeCard(_zombieCard, zombie1);
            }
        }
    }
    public void ShowPage()
    {
        cardInformationB.SetData(_zombieCard);
        cardInformationA.SetData(_zombieCard);
        cardInformationR.SetData(_zombieCard);
        cardInformationUIA.SetUpCard();
        cardInformationUIB.SetUpCard();
        cardInformationUIR.SetUpCard();
        txtStartA.text = "★" + (cardInformationB.level + 1).ToString();
        txtStartB.text = "★" + cardInformationB.level;
        txtStartR.text = "★" + cardInformationB.level;
        int count = CountCard();
        if (count >= 2)
            txtnumberRequire.color = Color.green;
        else
            txtnumberRequire.color = Color.red;
        txtnumberRequire.text = count + "/2";
    }
    private bool isCardInDeck(ZombieModel zombieModel)
    {
        ZombieModel[] arrZombie = DataManager.Instance.listDeckUser[0].cards;
        for (int i = 0; i < arrZombie.Length; i++)
        {
            if (zombieModel.tokenId.Equals(arrZombie[i].tokenId))
                return true;
        }
        return false;
    }
    private int CountCard()
    {
        int count = 0;
        for (int i = 0; i < _listCard.Count; i++)
        {
            if (_listCard[i].level == _zombieCard.level)
                count++;
        }
        return count;
    }
}
