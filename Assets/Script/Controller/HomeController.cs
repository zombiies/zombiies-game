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
    public GameObject packPopup = default;
    public GameObject deckPopup = default;
    public GameObject SelectCardPopUp = default;
    public GameObject UpgradePopup = default;
    void Start()
    {
        ShowInformation();
        OnButtonClicked();
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
