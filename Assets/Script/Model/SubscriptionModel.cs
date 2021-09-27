using System;


[Serializable]
public class SubscriptionModel
{
    public string type;
    public TypeSubsription id;
    public PayloadSubscriptionModel payload;
}

[Serializable]
public class PayloadSubscriptionModel
{
    public DataSubscriptionModel data;
}
[Serializable]
public class DataSubscriptionModel
{
    public NotificationModel notificationPushed;
    public RoomModel onAllReady;
    public RoomModel onReadyTimeout;
    public RoomModel onWaitForReady;
    public bool online;
    public PrepareTurnEventModel prepareTurnEvent;
    public EndTurnEventModel endTurnEvent;
    public MatchModel matchStarted;
    public MatchModel matchEnded;
    public ConfirmTurnEventModel confirmTurnEvent;
    public RewardModel rewardReceived;

}