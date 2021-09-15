using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ShopPopupController : Singleton<ShopElementUI>
{
    [SerializeField] GameObject shopElementUI = default;
    [SerializeField] Transform transParent = default;
    [SerializeField] Button btnClose = default;
    [SerializeField] Button btnNextPage = default;
    [SerializeField] Button btnBackPage = default;
    [SerializeField] Text txtPage;

    private int _totalPage;
    private int _currentPage = 0;
    private List<AuctionModel> listAuction = new List<AuctionModel>();
    void Awake()
    {
        OnButtonClicked();
    }
    private void OnEnable()
    {
        _currentPage = 0;
        if (DataManager.Instance.listAllAuction.Length > 0)
            listAuction = DataManager.Instance.listAllAuction.ToList();
        ShowPage();
    }
    public void ShowPage()
    {
        DeleteElement();
        if (listAuction.Count <= 0) return;

        for (int i = _currentPage * 2; i < (listAuction.Count <= (_currentPage * 2 + 2) ? listAuction.Count : (_currentPage * 2 + 2)); i++)
        {
            AuctionModel auctionModel = listAuction[i];
            GameObject element = Instantiate<GameObject>(shopElementUI, transParent);
            element.GetComponent<ShopElementUI>().Init(auctionModel);
        }
        int _intCount = _currentPage + 1;
        txtPage.text = _intCount + "/" + _totalPage;
        _totalPage = listAuction.Count / 2 + 1;
    }
    private void OnButtonClicked()
    {
        btnBackPage.onClick.AddListener(OnBackPageButtonClicked);
        btnNextPage.onClick.AddListener(OnNextPageButtonClicked);
        btnClose.onClick.AddListener(OnCloseButtonClicked);
    }
    private void OnNextPageButtonClicked()
    {
        _currentPage++;
        if (_currentPage > _totalPage - 1)
            _currentPage = 0;
        ShowPage();
    }
    private void OnBackPageButtonClicked()
    {
        _currentPage--;
        if (_currentPage < 0)
            _currentPage = _totalPage - 1;
        ShowPage();
    }
    private void OnCloseButtonClicked()
    {
        gameObject.SetActive(false);
    }
    void DeleteElement()
    {
        int childs = transParent.childCount;
        for (int i = childs - 1; i >= 0; i--)
        {
            GameObject.Destroy(transParent.GetChild(i).gameObject);
        }
    }
}