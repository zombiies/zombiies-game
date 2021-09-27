using System;
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
    [SerializeField] Button btnBuyMonster = default;
    [SerializeField] Button btnBuyEquipment = default;
    [SerializeField] Button btnNotification = default;
    [SerializeField] Text txtEth = default;
    [SerializeField] GameObject objWarningNoti = default;
    public GameObject packPopup = default;
    public GameObject deckPopup = default;
    public GameObject SelectCardPopUp = default;
    public GameObject UpgradePopup = default;
    public GameObject AuctionPopup = default;
    public GameObject ShopPopUp = default;
    public GameObject BIDPopUp = default;
    public GameObject BuyPackPopup = default;
    public GameObject NotificationPopup = default;
    public GameObject WaitForReady = default;
    public GameObject objFindMatchPopup = default;
    [Header("FindMatchPopup")]
    [SerializeField] Button btnCloseFindMatch = default;
    [SerializeField] Text txtTime = default;
    void Start()
    {
        ShowInformation();
        OnButtonClicked();
        UpdateETH();
    }
    public void SetReadNotification()
    {
        if (DataManager.Instance.isReadAllNotification())
            objWarningNoti.SetActive(false);
        else
            objWarningNoti.SetActive(true);
    }
    public void UpdateETH()
    {
        GraphQLManager.Instance.GetWalletUser(() =>
        {
            txtEth.text = DataManager.Instance.balanceEthUser.ToString();
            GraphQLManager.Instance.GetOwnedNotification(() =>
            {
                SetReadNotification();
            });
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
        btnBuyEquipment.onClick.AddListener(OnBuyEquipmentPackButtonClicked);
        btnBuyMonster.onClick.AddListener(OnBuyMonsterPackButtonClicked);
        btnNotification.onClick.AddListener(OnNotificationButtonClicked);
        btnCloseFindMatch.onClick.AddListener(OnCloseFindMatchButtonClicked);
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
    private void OnBuyMonsterPackButtonClicked()
    {
        GraphQLManager.Instance.getPackFee(() =>
            {
                if (BuyPackPopup.activeSelf)
                    BuyPackPopup.SetActive(false);
                BuyPackPopup.SetActive(true);
                BuyCardPopupController.Instance.InitPopup(TypeCard.MONSTER);
            });
    }
    private void OnBuyEquipmentPackButtonClicked()
    {
        GraphQLManager.Instance.getPackFee(() =>
        {
            if (BuyPackPopup.activeSelf)
                BuyPackPopup.SetActive(false);
            BuyPackPopup.SetActive(true);
            BuyCardPopupController.Instance.InitPopup(TypeCard.EQUIPMENT);
        });
    }
    private void OnNotificationButtonClicked()
    {
        NotificationPopup.SetActive(true);

    }
    private Coroutine _coroutine;
    private int _time = 0;
    private TimeSpan t;
    private IEnumerator IETime()
    {
        t = TimeSpan.FromSeconds(_time);
        txtTime.text = t.ToString(@"mm\:ss");
        yield return new WaitForSeconds(1f);
        _time++;
        _coroutine = StartCoroutine(IETime());
    }
    public void OpenFindMatchPopup()
    {
        _time = 0;
        _coroutine = StartCoroutine(IETime());
    }
    public void OnCloseFindMatchButtonClicked()
    {
        GraphQLManager.Instance.StopFindMatch(() =>
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
                _coroutine = null;
            }
            objFindMatchPopup.SetActive(false);
        });
    }

    private void OnArenaButtonClicked()
    {
        if (DataManager.Instance.listDeckUser[0].cards.Length < 16) return;
        GraphQLManager.Instance.FindMatch(() =>
        {
            objFindMatchPopup.SetActive(true);
            OpenFindMatchPopup();
        });
    }
}
