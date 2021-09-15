using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomeController : Singleton<HomeController>
{

    [SerializeField] Text txtNameUser = default;
    [SerializeField] Button btnShop = default;
    [SerializeField] Button btnPack = default;
    [SerializeField] Button btnArena = default;
    [SerializeField] Text txtEth = default;
    public GameObject packPopup = default;
    public GameObject deckPopup = default;
    public GameObject SelectCardPopUp = default;
    public GameObject UpgradePopup = default;
    public GameObject AuctionPopup = default;
    public GameObject ShopPopUp = default;
    public GameObject BIDPopUp = default;
    void Start()
    {
        ShowInformation();
        OnButtonClicked();
        UpdateETH();
    }
    public void UpdateETH()
    {
        GraphQLManager.Instance.GetWalletUser(()=> 
        {
            txtEth.text = DataManager.Instance.balanceEthUser.ToString();
        });
    }
    private void ShowInformation()
    {
        txtNameUser.text = DataManager.Instance.userInfo.username;
    }
    private void OnButtonClicked()
    {
        btnArena.onClick.AddListener(OnArenaButtonClicked);
        btnShop.onClick.AddListener(OnShopButtonClicked);
        btnPack.onClick.AddListener(OnPackButtonClicked);
    }
    private void OnShopButtonClicked()
    {
        GraphQLManager.Instance.getAllAuctopn(() => 
        {
            ShopPopUp.SetActive(true);
        });
    }
    private void OnPackButtonClicked()
    {
        if (!packPopup.activeSelf)
            packPopup.SetActive(true);
        if (!deckPopup.activeSelf)
            deckPopup.SetActive(true);
    }
    private void OnArenaButtonClicked()
    {

    }
}
