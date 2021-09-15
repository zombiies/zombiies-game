using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BIDPopUpController : Singleton<BIDPopUpController>
{
    [SerializeField] CardInformation cardInformation = default;
    [SerializeField] CardInformationUI cardInformationUI = default;
    [SerializeField] Button btnBid = default;
    [SerializeField] Text txtLastestBid = default;
    [SerializeField] Text txtTime = default;
    [SerializeField] Text txtNameBidder = default;
    [SerializeField] Button btnClose = default;
    [SerializeField] InputField inputBID = default;
    [SerializeField] GameObject objTxtName = default;

    private AuctionModel auctionModel = new AuctionModel();
    private string _price;
    void Awake()
    {
        OnButtonClicked();
        inputBID.onValueChanged.AddListener(delegate { OnChangeValueInput(); });

    }
    private void OnEnable()
    {
        inputBID.text = "";
    }

    public void Init(AuctionModel _auctionModel)
    {
        auctionModel = _auctionModel;
        ZombieModel zombieModel = auctionModel.token;
        cardInformation.SetData(zombieModel);
        cardInformationUI.SetUpCard();
        _price = auctionModel.latestBid != null ? auctionModel.latestBid : auctionModel.startBid;
        txtLastestBid.text = _price + "ETH";
        txtTime.text = auctionModel.endAt;
        if (auctionModel.latestBidder != null)
        {
            txtNameBidder.gameObject.SetActive(true);
            objTxtName.SetActive(true);
            txtNameBidder.text = auctionModel.latestBidder.username;
        }
        else
        {
            txtNameBidder.gameObject.SetActive(false);
            objTxtName.SetActive(false);
        }
        OnChangeValueInput();
    }
    private void OnButtonClicked()
    {
        btnBid.onClick.AddListener(OnBidButtonClicked);
        btnClose.onClick.AddListener(OnCloseButtonClicked);
    }
    private void OnCloseButtonClicked()
    {
        gameObject.SetActive(false);
    }
    private void OnBidButtonClicked()
    {
        GraphQLManager.Instance.BidToken(auctionModel.ID, inputBID.text, () =>
         {
             HomeController.Instance.UpdateETH();
             gameObject.SetActive(false);
             GraphQLManager.Instance.getAllAuctopn(() =>
             {
                 if (HomeController.Instance.ShopPopUp.activeSelf)
                     HomeController.Instance.ShopPopUp.SetActive(false);
                 HomeController.Instance.ShopPopUp.SetActive(true);
             });
         });
    }
    private void OnChangeValueInput()
    {
        if (auctionModel.latestBidder != null && auctionModel.latestBidder.id.Equals(DataManager.Instance.userInfo.id))
        {
            SetActiveButton(false);
            return;
        }
        if (inputBID.text == "")
        {
            SetActiveButton(false);
            return;
        }
        if (double.Parse(inputBID.text) < double.Parse(_price))
        {
            SetActiveButton(false);
            return;
        }
        if (DataManager.Instance.balanceEthUser < double.Parse(inputBID.text))
        {
            SetActiveButton(false);
            return;
        }
        SetActiveButton();
    }
    private void SetActiveButton(bool isActive = true)
    {
        if (!isActive)
        {
            btnBid.interactable = false;
            btnBid.gameObject.GetComponent<Image>().color = new Color32(255, 255, 255, 70);
            return;
        }
        btnBid.interactable = true;
        btnBid.gameObject.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
    }
}
