using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyCardPopupController : Singleton<BuyCardPopupController>
{
    [SerializeField] ChestAnim chestAnim = default;
    [SerializeField] Button btnClose = default;
    [SerializeField] Button btnBuy = default;
    [SerializeField] Text txtType = default;
    [SerializeField] Text txtPrice = default;
    [SerializeField] CardInBuyPack cardInBuyPack = default;
    [SerializeField] GameObject objButton = default;

    public ZombieModel zombieModel = new ZombieModel();
    private TypeCard _typeCard;

    private void Awake()
    {
        OnButtonClicked();   
    }
    private void OnEnable()
    {
        cardInBuyPack.gameObject.SetActive(false);
        SetActiveButton();
        chestAnim.SetAnimation(false);
    }
    public void InitPopup(TypeCard typeCard)
    {
        _typeCard = typeCard;
        if (_typeCard == TypeCard.EQUIPMENT)
            txtType.text = "Equipment Pack";
        else
            txtType.text = "Monster Pack";
        txtPrice.text = DataManager.Instance.PackFee.ToString() + " ETH";
    }
    private void OnButtonClicked()
    {
        btnClose.onClick.AddListener(OnCloseButtonClicked);
        btnBuy.onClick.AddListener(OnBuyButtonClicked);

    }
    private void OnCloseButtonClicked()
    {
        gameObject.SetActive(false);
    }
    private void OnBuyButtonClicked()
    {
        cardInBuyPack.gameObject.SetActive(false);
        chestAnim.SetAnimation(false);
        GraphQLManager.Instance.BuyToken(_typeCard,() => 
        {
            cardInBuyPack.SetUp();
            chestAnim.SetAnimation(true);
            SetActiveButton();
        });

    }
    private void SetActiveButton()
    {
        if (DataManager.Instance.balanceEthUser < DataManager.Instance.PackFee)
        {
            objButton.SetActive(true);
            btnBuy.interactable = false;
            return;
        }
        objButton.SetActive(false);
        btnBuy.interactable = true;
    }

}
