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
        DataManager.Instance.listCard = dataModel.data.ownedCardTokens;
        DataManager.Instance.ClassifyCardUser();
        if (_success != null) _success();
        LoadingPopUp.Instance.SetActive(false);
    }
    public async void CanBuyStarterPack()
    {
        GraphApi.Query query = graphApi.GetQueryByName("CanBuyStarterPack", GraphApi.Query.Type.Query);
        graphApi.SetAuthToken(DataManager.Instance.userInfo.token);
        UnityWebRequest request =  await graphApi.Post(query);
        Debug.LogError(request.downloadHandler.text);
        
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
