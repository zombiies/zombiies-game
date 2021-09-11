using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomeController : MonoBehaviour
{

    [SerializeField] Text txtNameUser = default;
    [SerializeField] Button btnShop = default;
    [SerializeField] Button btnPack = default;
    [SerializeField] Button btnArena = default;
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

    }
    private void OnArenaButtonClicked()
    {

    }
}
