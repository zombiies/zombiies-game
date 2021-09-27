using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MutationModel
{
    public AuthModel signup;
    public AuthModel login;
    public ZombieModel[] ownedCardTokens;
    public DeckModel[] ownedDecks;
    public DeckModel addCardToDeck;
    public DeckModel removeCardFromDeck;
    public ZombieModel levelUpCard;
    public string getPackFee;
    public AuctionModel[] allAuctions;
    public AuctionModel bid;
    public UserWalletModel ownedWallet;
    public ZombieModel buyToken;
    public List<NotificationModel> ownedNotifications;
    public bool putCardToBoard;
    public bool denyCard;
    public bool confirmTurn;
    public bool endTurn;
    public bool surrender;
}
