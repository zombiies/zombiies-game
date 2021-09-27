using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ElementCard : MonoBehaviour
{
    public Button btnElement = default;
    public CardInformation cardInformation = default;
    public CardInformationUI cardInformationUI = default;
    public Text txt_Amount = default;
    void Start()
    {
        btnElement.onClick.AddListener(OnButtonClicked);
    }

    void OnButtonClicked()
    {
        switch(cardInformation.type)
        {
            case TypeCard.MONSTER:
                if (DataManager.Instance.listCardMosterUser.ContainsKey(cardInformation.nameCard))
                {
                    if (HomeController.Instance)
                    {
                        if (HomeController.Instance.SelectCardPopUp.activeSelf)
                            HomeController.Instance.SelectCardPopUp.SetActive(false);
                        HomeController.Instance.SelectCardPopUp.SetActive(true);
                        SelecCardPopupController.Instance.Init(DataManager.Instance.listCardMosterUser[cardInformation.nameCard]);
                    }
                }
                break;
            case TypeCard.EQUIPMENT:
                if (DataManager.Instance.listCardEquipmentUser.ContainsKey(cardInformation.nameCard))
                {
                    if (HomeController.Instance)
                    {
                        if (HomeController.Instance.SelectCardPopUp.activeSelf)
                            HomeController.Instance.SelectCardPopUp.SetActive(false);
                        HomeController.Instance.SelectCardPopUp.SetActive(true);
                        SelecCardPopupController.Instance.Init(DataManager.Instance.listCardEquipmentUser[cardInformation.nameCard]);
                    }
                }
                break;
        }
    }
}
