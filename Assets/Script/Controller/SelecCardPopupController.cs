using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelecCardPopupController : Singleton<SelecCardPopupController>
{
    [SerializeField] Button btnBackPage = default;
    [SerializeField] Button btnNextPage = default;
    [SerializeField] Button btnExit = default;
    [SerializeField] Button btnUse = default;
    [SerializeField] Button btnUpgrade = default;
    [SerializeField] Button btnAuction = default;
    [SerializeField] CardInformation cardInformation = default;
    [SerializeField] CardInformationUI cardInformationUI = default;
    [SerializeField] Text txtPage = default;
    [SerializeField] Text txtButtonUse = default;

    private int _currentCard = 0;
    private List<ZombieModel> listCard = new List<ZombieModel>();

    private void Awake()
    {
        OnButtonClicked();
    }
    private void OnEnable()
    {
        _currentCard = 0;
    }
    public void Init(List<ZombieModel> _listCard)
    {
        _currentCard = 0;
        this.listCard = _listCard;
        ShowCard();
    }
    private void OnButtonClicked()
    {
        btnExit.onClick.AddListener(OnExitPageButtonClicked);
        btnNextPage.onClick.AddListener(OnNextPageButtonClicked);
        btnBackPage.onClick.AddListener(OnBackPageButtonClicked);
        btnUse.onClick.AddListener(OnUsePageButtonClicked);
        btnUpgrade.onClick.AddListener(OnUpgradePageButtonClicked);
        btnAuction.onClick.AddListener(OnAuctionButtonClicked);
    }
    private void OnNextPageButtonClicked()
    {
        _currentCard++;
        if (_currentCard == listCard.Count)
            _currentCard = 0;
        ShowCard();
    }
    private void OnBackPageButtonClicked()
    {
        _currentCard--;
        if (_currentCard < 0)
            _currentCard = listCard.Count - 1;
        ShowCard();
    }
    private void OnExitPageButtonClicked()
    {
        gameObject.SetActive(false);
    }
    private void OnUsePageButtonClicked()
    {
        string _tokenId = listCard[_currentCard].tokenId;
        string _deckId = DataManager.Instance.listDeckUser[0].ID;
        if (isCardInDeck(listCard[_currentCard]))
        {
            GraphQLManager.Instance.RemoveCardFromDeck(_tokenId, _deckId, () => 
            {
                DeckPopupController.Instance.ShowPage();
                if (isCardInDeck(listCard[_currentCard]))
                    txtButtonUse.text = "Disuse";
                else
                    txtButtonUse.text = "Use";
            });
        }
        else
        {
            GraphQLManager.Instance.AddCardToDeck(_tokenId, _deckId, () =>
            {
                DeckPopupController.Instance.ShowPage();
                if (isCardInDeck(listCard[_currentCard]))
                    txtButtonUse.text = "Disuse";
                else
                    txtButtonUse.text = "Use";
            });
        }

    }
    private void OnUpgradePageButtonClicked()
    {
        if (HomeController.Instance)
        {
            if (HomeController.Instance.UpgradePopup.activeSelf)
                HomeController.Instance.UpgradePopup.SetActive(false);
            HomeController.Instance.UpgradePopup.SetActive(true);
            UpgradePopupController.Instance.Init(listCard[_currentCard], listCard);
        }
    }
    private void OnAuctionButtonClicked()
    {
        if (HomeController.Instance)
        {
            if (HomeController.Instance.AuctionPopup.activeSelf)
                HomeController.Instance.AuctionPopup.SetActive(false);
            HomeController.Instance.AuctionPopup.SetActive(true);
            AuctionPopupController.Instance.Init(listCard[_currentCard]);
        }
    }

    public void ShowCard()
    {
        cardInformation.SetData(listCard[_currentCard]);
        cardInformationUI.SetUpCard();
        if (isCardInDeck(listCard[_currentCard]))
            txtButtonUse.text = "Disuse";
        else
            txtButtonUse.text = "Use";
        int page = _currentCard + 1;
        txtPage.text = page + "/" + listCard.Count;
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
}
