using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartAuctionInput 
{
    public string tokenId;
    public string startBid;

    public StartAuctionInput(string _tokenId,string _startBid)
    {
        this.tokenId = _tokenId;
        this.startBid = _startBid;
    }
}
