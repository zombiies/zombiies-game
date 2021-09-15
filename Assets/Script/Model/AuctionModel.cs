using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuctionModel 
{
    public string ID;
    public ZombieModel token;
    public UserModel seller;
    public UserModel latestBidder;
    public BidActionModel[] bidHistory;
    public string sellerAddress;
    public string startBid;
    public string startAt;
    public string endAt;
    public string startTransactionHash;
    public string latestBid;
    public string latestBidderAddress;
    public string latestBidAt;
    public string latestTransactionHash;
    public bool isEnded;
}
public class BidActionModel
{
    public string bid;
    public string bidderId;
    public string bidderAddress;
    public string bidAt;
    public string transactionHash;
}
