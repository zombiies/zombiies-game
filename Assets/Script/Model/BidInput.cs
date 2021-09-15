using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BidInput
{
    public string auctionId;
    public string bid;

    public BidInput(string _auctionId, string _bid)
    {
        auctionId = _auctionId;
        bid = _bid;
    }
}
