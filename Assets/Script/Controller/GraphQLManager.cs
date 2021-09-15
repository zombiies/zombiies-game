using GraphQlClient.Core;
using GraphQlClient.EventCallbacks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.WebSockets;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using UnityEngine.UI;
using System.Linq;

public class GraphQLManager : Singleton<GraphQLManager>
{
    #region SerializeField

    #endregion

    #region Public Variable
    public GraphApi graphApi;
    public string jsonZombie;
    #endregion

    #region Private Variable
    private ClientWebSocket cws;
    private ZombieModel[] arrZombie;
    private string _error;
    #endregion

    private void OnEnable()
    {
        OnSubscriptionDataReceived.RegisterListener(DisplayData);
    }

    private void DisplayData(OnSubscriptionDataReceived info)
    {
    }

    private void OnDisable()
    {
        OnSubscriptionDataReceived.UnregisterListener(DisplayData);

    }
    private void Awake()
    {
        arrZombie = JsonConvert.DeserializeObject<ZombieModel[]>(jsonZombie);
    }

    public async void Register(string _username, string _email, string _password, Text _txtError = default)
    {
        LoadingPopUp.Instance.SetActive(true);
        GraphApi.Query query = graphApi.GetQueryByName("Register", GraphApi.Query.Type.Mutation);
        query.SetArgs(new { input = new RegisterModel(_username, _email, _password) });
        UnityWebRequest request = await graphApi.Post(query);
        DataModel dataModel = JsonConvert.DeserializeObject<DataModel>(request.downloadHandler.text);
        LoadingPopUp.Instance.SetActive(false);
        if (dataModel.data != null)
        {
            _txtError.gameObject.SetActive(false);
            SetUpInfoUser(dataModel.data.signup);
        }
        else
            ShowError(dataModel, _txtError);
    }
    public async void Login(string _email, string _password, Text _txtError = default)
    {
        LoadingPopUp.Instance.SetActive(true);
        GraphApi.Query query = graphApi.GetQueryByName("Login", GraphApi.Query.Type.Mutation);
        query.SetArgs(new { input = new LoginModel(_email, _password) });
        UnityWebRequest request = await graphApi.Post(query);
        DataModel dataModel = JsonConvert.DeserializeObject<DataModel>(request.downloadHandler.text);
        LoadingPopUp.Instance.SetActive(false);
        if (dataModel.data != null)
        {
            _txtError.gameObject.SetActive(false);
            SetUpInfoUser(dataModel.data.login);
        }
        else
            ShowError(dataModel, _txtError);
    }
    public async void BuyStarterPack()
    {
        LoadingPopUp.Instance.SetActive(true);
        GraphApi.Query query = graphApi.GetQueryByName("BuyStarterPack", GraphApi.Query.Type.Mutation);
        graphApi.SetAuthToken(DataManager.Instance.userInfo.token);
        UnityWebRequest request = await graphApi.Post(query);
        LoadingPopUp.Instance.SetActive(false);
    }
    public async void GetUserCards(Action _success)
    {
        LoadingPopUp.Instance.SetActive(true);
        GraphApi.Query query = graphApi.GetQueryByName("OwnedCardTokens", GraphApi.Query.Type.Query);
        graphApi.SetAuthToken(DataManager.Instance.userInfo.token);
        UnityWebRequest request = await graphApi.Post(query);
        DataModel dataModel = JsonConvert.DeserializeObject<DataModel>(request.downloadHandler.text);
        LoadingPopUp.Instance.SetActive(false);
        if (dataModel.data != null)
        {
            DataManager.Instance.listCard = dataModel.data.ownedCardTokens;
            DataManager.Instance.ClassifyCardUser();
            if (_success != null) _success();
        }
    }
    public async void CanBuyStarterPack()
    {
        GraphApi.Query query = graphApi.GetQueryByName("CanBuyStarterPack", GraphApi.Query.Type.Query);
        graphApi.SetAuthToken(DataManager.Instance.userInfo.token);
        UnityWebRequest request = await graphApi.Post(query);
        Debug.LogError(request.downloadHandler.text);
    }
    public async void GetOwnedDecks(Action _success = null)
    {
        GraphApi.Query query = graphApi.GetQueryByName("OwnedDecks", GraphApi.Query.Type.Query);
        graphApi.SetAuthToken(DataManager.Instance.userInfo.token);
        UnityWebRequest request = await graphApi.Post(query);
        DataModel dataModel = JsonConvert.DeserializeObject<DataModel>(request.downloadHandler.text);
        DataManager.Instance.listDeckUser = dataModel.data.ownedDecks;
        if (DataManager.Instance.listDeckUser.Length == 0)
        {
            CreateDeck();
        }
        if (_success != null) _success();
    }
    public async void CreateDeck()
    {
        GraphApi.Query query = graphApi.GetQueryByName("OwnedDecks", GraphApi.Query.Type.Mutation);
        graphApi.SetAuthToken(DataManager.Instance.userInfo.token);
        query.SetArgs(new { input = new DeckInput("Deck 1") });
        UnityWebRequest request = await graphApi.Post(query);
        DataModel dataModel = JsonConvert.DeserializeObject<DataModel>(request.downloadHandler.text);
        DataManager.Instance.listDeckUser = dataModel.data.ownedDecks;
    }
    public async void AddCardToDeck(string tokenId, string deckId, Action _success = null)
    {
        LoadingPopUp.Instance.SetActive(true);
        GraphApi.Query query = graphApi.GetQueryByName("AddCardToDeck", GraphApi.Query.Type.Mutation);
        graphApi.SetAuthToken(DataManager.Instance.userInfo.token);
        query.SetArgs(new { input = new TokenInDeckInput(tokenId, deckId) });
        UnityWebRequest request = await graphApi.Post(query);
        DataModel dataModel = JsonConvert.DeserializeObject<DataModel>(request.downloadHandler.text);
        LoadingPopUp.Instance.SetActive(false);
        if (dataModel.data != null)
        {
            DataManager.Instance.listDeckUser = new DeckModel[] { dataModel.data.addCardToDeck };
            if (_success != null) _success();
        }
    }
    public async void RemoveCardFromDeck(string tokenId, string deckId, Action _success = null)
    {
        LoadingPopUp.Instance.SetActive(true);
        GraphApi.Query query = graphApi.GetQueryByName("RemoveCardFromDeck", GraphApi.Query.Type.Mutation);
        graphApi.SetAuthToken(DataManager.Instance.userInfo.token);
        query.SetArgs(new { input = new TokenInDeckInput(tokenId, deckId) });
        UnityWebRequest request = await graphApi.Post(query);
        DataModel dataModel = JsonConvert.DeserializeObject<DataModel>(request.downloadHandler.text);
        LoadingPopUp.Instance.SetActive(false);
        if (dataModel.data != null)
        {
            DataManager.Instance.listDeckUser = new DeckModel[] { dataModel.data.removeCardFromDeck };
            if (_success != null) _success();
        }
    }
    public async void UpgradeCard(ZombieModel zombie1, ZombieModel zombie2, Action _success = null)
    {
        LoadingPopUp.Instance.SetActive(true);
        GraphApi.Query query = graphApi.GetQueryByName("UpgradeCard", GraphApi.Query.Type.Mutation);
        graphApi.SetAuthToken(DataManager.Instance.userInfo.token);
        query.SetArgs(new { input = new LevelUpCardInput(zombie1.tokenId, zombie2.tokenId) });
        UnityWebRequest request = await graphApi.Post(query);
        DataModel dataModel = JsonConvert.DeserializeObject<DataModel>(request.downloadHandler.text);
        LoadingPopUp.Instance.SetActive(false);
        if (dataModel.data != null)
        {
            DataManager.Instance.AddCard(dataModel.data.levelUpCard);
            DataManager.Instance.RemoveCard(zombie1);
            DataManager.Instance.RemoveCard(zombie2);
            if (UpgradePopupController.Instance)
            {
                ZombieModel _zombie = dataModel.data.levelUpCard;
                List<ZombieModel> _listCard = _zombie.type == TypeCard.MONSTER ? DataManager.Instance.listCardMosterUser[_zombie.name] : DataManager.Instance.listCardEquipmentUser[_zombie.name];
                UpgradePopupController.Instance.Init(_zombie, _listCard);
                if (SelecCardPopupController.Instance)
                    SelecCardPopupController.Instance.Init(_listCard);
            }
            if (PackPopupController.Instance)
                PackPopupController.Instance.ShowPage();
            if (_success != null) _success();
        }
    }
    public async void AuctionCard(ZombieModel zombie1, string startBid, Action _success = null)
    {
        LoadingPopUp.Instance.SetActive(true);
        GraphApi.Query query = graphApi.GetQueryByName("StartAuction", GraphApi.Query.Type.Mutation);
        graphApi.SetAuthToken(DataManager.Instance.userInfo.token);
        query.SetArgs(new { input = new StartAuctionInput(zombie1.tokenId, startBid) });
        UnityWebRequest request = await graphApi.Post(query);
        DataModel dataModel = JsonConvert.DeserializeObject<DataModel>(request.downloadHandler.text);
        LoadingPopUp.Instance.SetActive(false);
        if (dataModel.data != null)
        {
            DataManager.Instance.RemoveCard(zombie1);
            List<ZombieModel> _listCard = new List<ZombieModel>();
            switch (zombie1.type)
            {
                case TypeCard.MONSTER:
                    if (DataManager.Instance.listCardMosterUser.ContainsKey(zombie1.name))
                        _listCard = DataManager.Instance.listCardMosterUser[zombie1.name];
                    break;
                case TypeCard.EQUIPMENT:
                    if (DataManager.Instance.listCardEquipmentUser.ContainsKey(zombie1.name))
                        _listCard = DataManager.Instance.listCardEquipmentUser[zombie1.name];
                    break;
            }
            if (_listCard.Count > 0)
            {
                if (SelecCardPopupController.Instance)
                    SelecCardPopupController.Instance.Init(_listCard);
            }
            else
                if (SelecCardPopupController.Instance)
                SelecCardPopupController.Instance.gameObject.SetActive(false);
            if (PackPopupController.Instance)
                PackPopupController.Instance.ShowPage();
            if (_success != null) _success();
        }
    }
    public async void getAllAuctopn(Action _success = null)
    {
        LoadingPopUp.Instance.SetActive(true);
        GraphApi.Query query = graphApi.GetQueryByName("AllAuctions", GraphApi.Query.Type.Query);
        graphApi.SetAuthToken(DataManager.Instance.userInfo.token);
        UnityWebRequest request = await graphApi.Post(query);
        DataModel dataModel = JsonConvert.DeserializeObject<DataModel>(request.downloadHandler.text);
        LoadingPopUp.Instance.SetActive(false);
        if (dataModel.data != null)
        {
            DataManager.Instance.listAllAuction = dataModel.data.allAuctions;
            if (_success != null) _success();
        }
    }
    public async void BidToken(string _auctionId, string _bid, Action _success = null)
    {
        LoadingPopUp.Instance.SetActive(true);
        GraphApi.Query query = graphApi.GetQueryByName("Bid", GraphApi.Query.Type.Mutation);
        query.SetArgs(new { input = new BidInput(_auctionId, _bid) });
        graphApi.SetAuthToken(DataManager.Instance.userInfo.token);
        UnityWebRequest request = await graphApi.Post(query);
        DataModel dataModel = JsonConvert.DeserializeObject<DataModel>(request.downloadHandler.text);
        LoadingPopUp.Instance.SetActive(false);
        if (dataModel.data != null)
        {
            if (_success != null) _success();
        }
    }
    public async void GetWalletUser(Action _success = null)
    {
        LoadingPopUp.Instance.SetActive(true);
        GraphApi.Query query = graphApi.GetQueryByName("GetWallet", GraphApi.Query.Type.Query);
        graphApi.SetAuthToken(DataManager.Instance.userInfo.token);
        UnityWebRequest request = await graphApi.Post(query);
        DataModel dataModel = JsonConvert.DeserializeObject<DataModel>(request.downloadHandler.text);
        LoadingPopUp.Instance.SetActive(false);
        if (dataModel.data != null)
        {
            DataManager.Instance.balanceEthUser = double.Parse(dataModel.data.ownedWallet.balance); 
            if (_success != null) _success();
        }
    }
    private void ShowError(DataModel _dataModel, Text _textError)
    {
        _textError.gameObject.SetActive(true);
        _error = _dataModel.errors[0].extensions.response.message[0];
        _textError.text = DATA_USER.TextStar + _error.First().ToString().ToUpper() + _error.Substring(1);
    }

    private void SetUpInfoUser(AuthModel authModel)
    {
        DataManager.Instance.userInfo = authModel.user;
        DataManager.Instance.userInfo.token = authModel.token.accessToken;
        SceneController.Instance.OpenScene(SCENE_NAME.Home);
    }
}
