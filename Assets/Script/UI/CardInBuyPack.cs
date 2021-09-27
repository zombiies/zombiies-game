using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardInBuyPack : MonoBehaviour
{
    [SerializeField] CardInformation cardInformation = default;
    [SerializeField] CardInformationUI cardInformationUI = default;
    private void Awake()
    {
        SetUp();
    }
    private void OnEnable()
    {
        SetUp();
    }
    public void SetUp()
    {
        if (BuyCardPopupController.Instance.zombieModel == null) return;
        cardInformation.SetData(BuyCardPopupController.Instance.zombieModel);
        cardInformationUI.SetUpCard();
        cardInformationUI.SetUpCard();
    }
}
