using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AuctionPopupController : Singleton<AuctionPopupController>
{
    [SerializeField] Button btnClose = default;
    [SerializeField] Button btnAuction = default;
    [SerializeField] InputField inputAution = default;

    private ZombieModel zombieModel = new ZombieModel();
    void Start()
    {
        OnButtonClicked();
    }
    public void Init(ZombieModel _zombie)
    {
        zombieModel = _zombie;
    }
    private void OnButtonClicked()
    {
        btnClose.onClick.AddListener(OnExitButtonClicked);
        btnAuction.onClick.AddListener(OnAuctionButtonClicked);
    }
    private void OnExitButtonClicked()
    {
        gameObject.SetActive(false);
    }
    private void OnAuctionButtonClicked()
    {
        string _tokenId = zombieModel.tokenId;
        string _deckId = DataManager.Instance.listDeckUser[0].ID;
        if (isCardInDeck(zombieModel))
        {
            GraphQLManager.Instance.RemoveCardFromDeck(_tokenId, _deckId, () =>
            {
                GraphQLManager.Instance.AuctionCard(zombieModel, inputAution.text,()=>
                {
                    gameObject.SetActive(false);
                });
            });
        }
        else
            GraphQLManager.Instance.AuctionCard(zombieModel, inputAution.text,()=>
            {
                gameObject.SetActive(false);
            });


    }
    private bool isCardInDeck(ZombieModel zombieModel)
    {
        ZombieModel[] arrZombie = DataManager.Instance.listDeckUser[0].cards;
        for (int i = 0; i < arrZombie.Length; i++)
        {
            if (zombieModel.tokenId.Equals(arrZombie[i].tokenId))
                return true;
        }
        return false;
    }
}
