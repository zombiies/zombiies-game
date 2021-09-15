using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopElementUI : MonoBehaviour
{
    public CardInformation cardInformation = default;
    public CardInformationUI cardInformationUI = default;
    public Text txtLastestBid = default;
    public Text txtEndTime = default;
    public Button btnBid = default;
    public Text txtName = default;
    public Text txtStart = default;
    private AuctionModel auctionModel = new AuctionModel();
    void Start()
    {
        OnButtonClicked();
    }

    public void Init(AuctionModel _auctionModel)
    {
        auctionModel = _auctionModel;
        cardInformation.SetData(auctionModel.token);
        cardInformationUI.SetUpCard();
        txtName.text = cardInformation.nameCard;
        txtStart.text = "★" + cardInformation.level;
        txtLastestBid.text = auctionModel.latestBid != null ? auctionModel.latestBid : auctionModel.startBid + " ETH";
        txtEndTime.text = auctionModel.endAt;
    }
    private void OnButtonClicked()
    {
        btnBid.onClick.AddListener(OnBidButtonClicked);
    }

    private void OnBidButtonClicked()
    {
        if (HomeController.Instance.BIDPopUp.activeSelf)
            HomeController.Instance.BIDPopUp.SetActive(false);
        HomeController.Instance.BIDPopUp.SetActive(true);
        BIDPopUpController.Instance.Init(auctionModel);
    }
}
