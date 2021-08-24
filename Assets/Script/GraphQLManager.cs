using GraphQlClient.Core;
using GraphQlClient.EventCallbacks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.WebSockets;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

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
    #endregion

    private void OnEnable()
    {
        OnSubscriptionDataReceived.RegisterListener(DisplayData);
    }

    private void DisplayData(OnSubscriptionDataReceived info)
    {
        Debug.LogError(info.data);
    }

    private void OnDisable()
    {
        OnSubscriptionDataReceived.UnregisterListener(DisplayData);
    }
    private void Awake()
    {
        arrZombie = JsonConvert.DeserializeObject<ZombieModel[]>(jsonZombie);
        Debug.LogError(JsonConvert.SerializeObject(arrZombie[0]));
        Subcriber();
    }

    public async void Subcriber()
    {
        cws = await graphApi.Subscribe("Subcriber", GraphApi.Query.Type.Subscription, "default");
    }
}
