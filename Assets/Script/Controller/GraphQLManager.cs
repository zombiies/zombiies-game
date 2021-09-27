using GraphQlClient.Core;
using GraphQlClient.EventCallbacks;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using UnityEngine.Networking;
using Newtonsoft.Json;
using UnityEngine.UI;
using System.Linq;
using UnityEngine;

public class GraphQLManager : Singleton<GraphQLManager>
{
    #region SerializeField

    #endregion

    #region Public Variable
    public GraphApi graphApi;
    public string jsonZombie;
    #endregion

    #region Private Variable
    private ClientWebSocket cwsNotification;
    private ClientWebSocket cwsAllReady;
    private ClientWebSocket cwsOnline;
    private ClientWebSocket cwsReadyTimeout;
    private ClientWebSocket cwsWaitForReady;
    private ClientWebSocket cwsMatchStarted;
    private ClientWebSocket cwsPrepareTurnEvent;
    private ClientWebSocket cwsEndTurnEvent;
    private ClientWebSocket cwsMatchEnded;
    private ClientWebSocket cwsConfirmTurnEvent;
    private ClientWebSocket cwsRewardReceived;
    private string _error;
    #endregion


    private void ReceivedData(OnSubscriptionDataReceived info)
    {
        SubscriptionModel subscription = JsonConvert.DeserializeObject<SubscriptionModel>(info.data);
        Debug.Log(info.data);
        switch (subscription.id)
        {
            case TypeSubsription.NOTIFICATION:
                DataManager.Instance.AddNotification(subscription.payload.data.notificationPushed);
                HomeController.Instance?.SetReadNotification();
                break;
            case TypeSubsription.ONLINE:
                break;
            case TypeSubsription.WAITFORREADY:
                HomeController.Instance.WaitForReady?.SetActive(true);
                break;
            case TypeSubsription.READYTIMEOUT:
                HomeController.Instance.WaitForReady?.SetActive(false);
                HomeController.Instance.objFindMatchPopup?.SetActive(false);
                break;
            case TypeSubsription.READYALL:
                SceneController.Instance.OpenScene(SCENE_NAME.Game);
                break;
            case TypeSubsription.MATCHSTARTED:
                DataManager.Instance.matchStarted = subscription.payload.data.matchStarted;
                if (GameController.Instance)
                    GameController.Instance.UpdateGame(DataManager.Instance.matchStarted);
                break;
            case TypeSubsription.PREPARETURNEVENT:
                PrepareTurnEventModel prepareTurnEvent = subscription.payload.data.prepareTurnEvent;
                GameController.Instance?.UpdateGame(prepareTurnEvent.currentMatchStatus);
                break;
            case TypeSubsription.ENDTURNEVENT:
                EndTurnEventModel endTurnEvent = subscription.payload.data.endTurnEvent;
                GameController.Instance?.UpdateGame(endTurnEvent.currentMatchStatus);
                break;
            case TypeSubsription.CONFIRMTURNEVENT:
                ConfirmTurnEventModel confirmTurnEvent = subscription.payload.data.confirmTurnEvent;

                foreach (var item in confirmTurnEvent.currentMatchStatus.playerStatuses)
                    GameController.Instance?.SetTextTime(item);

                if (confirmTurnEvent.attacks.Length <= 0)
                    EndTurn();
                else
                    GameController.Instance?.HandleAttack(confirmTurnEvent.playerId, confirmTurnEvent.attacks);

                break;
            case TypeSubsription.MATCHENDED:
                MatchModel matchEnded = subscription.payload.data.matchEnded;
                if (matchEnded.winner.id.Equals(DataManager.Instance.userInfo.id))
                    GameController.Instance?.objVictoryPopup.SetActive(true);
                else
                    GameController.Instance?.objLosePopup.SetActive(true);
                break;
            case TypeSubsription.REWARDRECEIVED:
                RewardModel reward = subscription.payload.data.rewardReceived;
                GameController.Instance?.SetRewardCard(reward.token);
                break;

        }

    }
    private void OnEnable()
    {
        OnSubscriptionDataReceived.RegisterListener(ReceivedData);
    }
    private void OnDisable()
    {
        OnSubscriptionDataReceived.UnregisterListener(ReceivedData
);

    }
    private void Awake()
    {
    }
    public async void SendDataWaitLoading(
        string queryName,
        GraphApi.Query.Type queryType,
        object inputObject = null,
        Action<DataModel> onSuccess = null,
        Action<DataModel> onError = null,
        bool isIncludeToken = true,
        bool isWaitLoading = true
        )
    {
        if (isWaitLoading)
            LoadingPopUp.Instance.SetActive(true);
        GraphApi.Query query = graphApi.GetQueryByName(queryName, queryType);
        if (inputObject != null)
            query.SetArgs(inputObject);
        if (isIncludeToken)
            graphApi.SetAuthToken(DataManager.Instance.userInfo.token);
        UnityWebRequest request = await graphApi.Post(query);
        DataModel dataModel = JsonConvert.DeserializeObject<DataModel>(request.downloadHandler.text);
        if (isWaitLoading)
            LoadingPopUp.Instance.SetActive(false);
        if (dataModel.data != null)
        {
            onSuccess?.Invoke(dataModel);
        }
        if (dataModel.errors != null)
        {
            onError?.Invoke(dataModel);
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
        GetOwnedDecks();
    }
    #region QUERY
    public void GetUserCards(Action _success)
    {
        SendDataWaitLoading(
          "OwnedCardTokens",
           GraphApi.Query.Type.Query,
           null,
           data =>
           {
               DataManager.Instance.listCard = data.data.ownedCardTokens;
               DataManager.Instance.ClassifyCardUser();
               if (_success != null) _success();
           });
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
            CreateDeck(_success);
        }
        else
        {
            _success?.Invoke();
        }
    }
    public void getAllAuctopn(Action _success = null)
    {
        SendDataWaitLoading(
           "AllAuctions",
            GraphApi.Query.Type.Query,
            null,
            data =>
            {
                DataManager.Instance.listAllAuction = data.data.allAuctions;
                _success?.Invoke();
            });
    }
    public void getPackFee(Action _success = null)
    {
        SendDataWaitLoading(
           "GetPackFee",
            GraphApi.Query.Type.Query,
            null,
            data =>
            {
                DataManager.Instance.PackFee = double.Parse(data.data.getPackFee);
                _success?.Invoke();
            });
    }
    public void GetWalletUser(Action _success = null)
    {
        SendDataWaitLoading(
           "GetWallet",
            GraphApi.Query.Type.Query,
            null,
            data =>
            {
                DataManager.Instance.balanceEthUser = double.Parse(data.data.ownedWallet.balance);
                if (_success != null) _success();
            });
    }
    public void GetOwnedNotification(Action _success = null)
    {
        SendDataWaitLoading(
           "OwnedNotifications",
            GraphApi.Query.Type.Query,
            null,
            data =>
            {
                DataManager.Instance.listNotificationUser = data.data.ownedNotifications;
                _success?.Invoke();
            });
    }
    #endregion
    #region MUTATION
    public void Register(string _username, string _email, string _password, Text _txtError = default)
    {
        object postData = new { input = new RegisterModel(_username, _email, _password) };
        SendDataWaitLoading(
          "Register",
           GraphApi.Query.Type.Mutation,
           postData,
           data =>
           {
               _txtError.gameObject.SetActive(false);
               SetUpInfoUser(data.data.signup);
               SubscriptionSceneHome();
           },
           data => ShowError(data, _txtError), false);
    }
    public void Login(string _email, string _password, Text _txtError = default)
    {
        object postData = new { input = new LoginModel(_email, _password) };
        SendDataWaitLoading(
          "Login",
           GraphApi.Query.Type.Mutation,
           postData,
           data =>
           {
               _txtError.gameObject.SetActive(false);
               SetUpInfoUser(data.data.login);
               SubscriptionSceneHome();
           },
           data => ShowError(data, _txtError), false);
    }
    public void CreateDeck(Action _onSuccess = null)
    {

        object postdata = new { input = new DeckInput("Deck 1") };
        SendDataWaitLoading(
           "CreateDeck",
            GraphApi.Query.Type.Mutation,
            postdata,
            data =>
            {
                DataManager.Instance.listDeckUser = data.data.ownedDecks;
                _onSuccess?.Invoke();
            },
            null,
            true,
            false);
    }
    public void AddCardToDeck(string tokenId, string deckId, Action _success = null)
    {
        object postData = new { input = new TokenInDeckInput(tokenId, deckId) };
        SendDataWaitLoading(
           "AddCardToDeck",
            GraphApi.Query.Type.Mutation,
            postData,
            data =>
            {
                DataManager.Instance.listDeckUser = new DeckModel[] { data.data.addCardToDeck };
                _success?.Invoke();
            });
    }
    public void RemoveCardFromDeck(string tokenId, string deckId, Action _success = null)
    {
        object postData = new { input = new TokenInDeckInput(tokenId, deckId) };
        SendDataWaitLoading(
           "RemoveCardFromDeck",
            GraphApi.Query.Type.Mutation,
            postData,
            data =>
            {
                DataManager.Instance.listDeckUser = new DeckModel[] { data.data.removeCardFromDeck };
                _success?.Invoke();
            });
    }
    public void UpgradeCard(ZombieModel zombie1, ZombieModel zombie2, Action _success = null)
    {
        object postData = new { input = new LevelUpCardInput(zombie1.tokenId, zombie2.tokenId) };
        SendDataWaitLoading(
          "UpgradeCard",
           GraphApi.Query.Type.Mutation,
           postData,
           data =>
           {
               DataManager.Instance.AddCard(data.data.levelUpCard);
               DataManager.Instance.RemoveCard(zombie1);
               DataManager.Instance.RemoveCard(zombie2);
               ZombieModel _zombie = data.data.levelUpCard;
               List<ZombieModel> _listCard = _zombie.type == TypeCard.MONSTER ? DataManager.Instance.listCardMosterUser[_zombie.name] : DataManager.Instance.listCardEquipmentUser[_zombie.name];
               UpgradePopupController.Instance?.Init(_zombie, _listCard);
               SelecCardPopupController.Instance?.Init(_listCard);
               PackPopupController.Instance?.ShowPage();
               _success?.Invoke();
           });
    }
    public void AuctionCard(ZombieModel zombie1, string startBid, Action _success = null)
    {
        object postData = new { input = new StartAuctionInput(zombie1.tokenId, startBid) };
        SendDataWaitLoading(
           "StartAuction",
            GraphApi.Query.Type.Mutation,
            postData,
            data =>
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
                    SelecCardPopupController.Instance?.Init(_listCard);
                else
                    SelecCardPopupController.Instance?.gameObject.SetActive(false);
                PackPopupController.Instance?.ShowPage();
                _success?.Invoke();
            });
    }
    public void BidToken(string _auctionId, string _bid, Action _success = null)
    {
        object postData = new { input = new BidInput(_auctionId, _bid) };
        SendDataWaitLoading(
           "Bid",
            GraphApi.Query.Type.Mutation,
            postData,
            data =>
            {
                _success?.Invoke();
            });
    }
    public void BuyToken(TypeCard _typeCard, Action _success = null)
    {
        object postData = new { type = _typeCard };
        SendDataWaitLoading(
            "BuyToken",
            GraphApi.Query.Type.Mutation,
            postData,
            data =>
            {
                if (BuyCardPopupController.Instance)
                    BuyCardPopupController.Instance.zombieModel = data.data.buyToken;
                _success?.Invoke();
            });
    }
    public void FindMatch(Action _success = null)
    {
        SendDataWaitLoading(
            "FindMatch",
            GraphApi.Query.Type.Mutation,
            null,
            data =>
            {
                _success?.Invoke();
            },
            null,
            true,
            false);
    }
    public void StopFindMatch(Action _success = null)
    {
        SendDataWaitLoading(
            "StopFindMatch",
            GraphApi.Query.Type.Mutation,
            null,
            data =>
            {
                _success?.Invoke();
            },
            null,
            true,
            false);
    }
    public void ReadyMatch(Action _success = null)
    {
        SendDataWaitLoading(
            "ReadyMatch",
            GraphApi.Query.Type.Mutation,
            null,
            data =>
            {
                _success?.Invoke();
            },
            null,
            true,
            false);
    }
    public void MarkAllNotificationsAsRead(Action _success = null)
    {
        SendDataWaitLoading(
            "MarkAllNotificationsAsRead",
            GraphApi.Query.Type.Mutation,
            null,
            data =>
            {
                _success?.Invoke();
            },
            null,
            true,
            false);
    }
    public void PutCardToBoard(int _position, string _tokenId, Action _success = null, Action _failed = null)
    {
        object postData = new { position = _position, tokenId = _tokenId };
        SendDataWaitLoading(
            "PutCardToBoard",
            GraphApi.Query.Type.Mutation,
            postData,
            data =>
            {
                if (data.data.putCardToBoard)
                    _success?.Invoke();
                else
                    _failed?.Invoke();
            },
            error => _failed?.Invoke(),
            true,
            false);
    }
    public void DenyCard(string _tokenId, Action _success = null)
    {
        object postData = new { tokenId = _tokenId };
        SendDataWaitLoading(
            "DenyCard",
            GraphApi.Query.Type.Mutation,
            postData,
            data =>
            {
                if (data.data.denyCard)
                    _success?.Invoke();
            },
            null,
            true,
            false);
    }
    public void ConfirmTurn(Action _success = null)
    {
        SendDataWaitLoading(
            "ConfirmTurn",
            GraphApi.Query.Type.Mutation,
            null,
            data =>
            {
                if (data.data.confirmTurn)
                    _success?.Invoke();
            },
            null,
            true,
            false);
    }
    public void EndTurn(Action _success = null)
    {
        SendDataWaitLoading(
            "EndTurn",
            GraphApi.Query.Type.Mutation,
            null,
            data =>
            {
                if (data.data.endTurn)
                    _success?.Invoke();
            },
            null,
            true,
            false);
    }
    public void Surrender(Action _success = null)
    {
        SendDataWaitLoading(
            "Surrender",
            GraphApi.Query.Type.Mutation,
            null,
            data =>
            {
                if (data.data.surrender)
                    _success?.Invoke();
            },
            null,
            true,
            false);
    }

    #endregion
    #region SUBSCRIPTION
    public async void SubscriptionSceneHome()
    {
        LoadingPopUp.Instance.SetActive(true);
        cwsNotification = await graphApi.Subscribe("NotificationPushed", GraphApi.Query.Type.Subscription, TypeSubsription.NOTIFICATION.ToString());
        cwsWaitForReady = await graphApi.Subscribe("WaitForReady", GraphApi.Query.Type.Subscription, TypeSubsription.WAITFORREADY.ToString());
        cwsReadyTimeout = await graphApi.Subscribe("ReadyTimeout", GraphApi.Query.Type.Subscription, TypeSubsription.READYTIMEOUT.ToString());
        cwsAllReady = await graphApi.Subscribe("AllReady", GraphApi.Query.Type.Subscription, TypeSubsription.READYALL.ToString());
        cwsOnline = await graphApi.Subscribe("Online", GraphApi.Query.Type.Subscription, TypeSubsription.ONLINE.ToString());
        LoadingPopUp.Instance.SetActive(false);
        SubscriptionSceneGame();
        SceneController.Instance.OpenScene(SCENE_NAME.Home);
    }
    public async void SubscriptionSceneGame()
    {
        LoadingPopUp.Instance.SetActive(true);
        cwsMatchStarted = await graphApi.Subscribe("MatchStarted", GraphApi.Query.Type.Subscription, TypeSubsription.MATCHSTARTED.ToString());
        cwsPrepareTurnEvent = await graphApi.Subscribe("PrepareTurnEvent", GraphApi.Query.Type.Subscription, TypeSubsription.PREPARETURNEVENT.ToString());
        cwsEndTurnEvent = await graphApi.Subscribe("EndTurnEvent", GraphApi.Query.Type.Subscription, TypeSubsription.ENDTURNEVENT.ToString());
        cwsRewardReceived = await graphApi.Subscribe("RewardReceived", GraphApi.Query.Type.Subscription, TypeSubsription.REWARDRECEIVED.ToString());
        cwsMatchEnded = await graphApi.Subscribe("MatchEnded", GraphApi.Query.Type.Subscription, TypeSubsription.MATCHENDED.ToString());
        cwsConfirmTurnEvent = await graphApi.Subscribe("ConfirmTurnEvent", GraphApi.Query.Type.Subscription, TypeSubsription.CONFIRMTURNEVENT.ToString());
        LoadingPopUp.Instance.SetActive(false);
    }
    #endregion

}
public enum TypeSubsription
{
    NOTIFICATION,
    ONLINE,
    WAITFORREADY,
    READYALL,
    READYTIMEOUT,
    MATCHSTARTED,
    PREPARETURNEVENT,
    ENDTURNEVENT,
    MATCHENDED,
    CONFIRMTURNEVENT,
    REWARDRECEIVED
}