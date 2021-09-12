using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckPopupController : Singleton<DeckPopupController>
{
    [SerializeField] Transform tranParentElement = default;
    [SerializeField] GameObject elementCard = default;
    [SerializeField] Button btnMonster = default;
    [SerializeField] Button btnEquipment = default;
    [SerializeField] Image imgMonster = default;
    [SerializeField] Image imgEquipment = default;
    [SerializeField] Sprite sprSelect = default;
    [SerializeField] Sprite spriteUnSelect = default;
    private int currentPage = 0;
    private bool _isMonsterClicked = true;
    private bool _isEquipmentClicked = false;
    // Start is called before the first frame update
    void Start()
    {
        GraphQLManager.Instance.GetOwnedDecks(() =>
        {
            OnButtonClicked();
            SetUpSpriteWhenClicked(imgMonster, imgEquipment);
            ShowPage();
        });
    }
    private void OnEnable()
    {
        GraphQLManager.Instance.GetOwnedDecks(() =>
        {
            OnButtonClicked();
            SetUpSpriteWhenClicked(imgMonster, imgEquipment);
            ShowPage();
        });
    }
    private void OnButtonClicked()
    {
        btnEquipment.onClick.AddListener(OnEquipmentButtonClicked);
        btnMonster.onClick.AddListener(OnMonsterButtonClicked);
    }
    private void OnMonsterButtonClicked()
    {
        if (_isMonsterClicked) return;
        SetUpSpriteWhenClicked(imgMonster, imgEquipment);
        _isEquipmentClicked = false;
        _isMonsterClicked = true;
        currentPage = 0;
        ShowPage();
    }
    private void OnEquipmentButtonClicked()
    {
        if (_isEquipmentClicked) return;
        SetUpSpriteWhenClicked(imgEquipment, imgMonster);
        _isEquipmentClicked = true;
        _isMonsterClicked = false;
        currentPage = 1;
        ShowPage();
    }
    public void ShowPage()
    {
        DeleteElement();
        ZombieModel[] _listCard = DataManager.Instance.listDeckUser[0].cards;
        switch (currentPage)
        {
            case 0:
                for (int i = 0; i < _listCard.Length; i++)
                {
                    if (_listCard[i].type == TypeCard.MONSTER)
                        InstanceElement(_listCard[i]);
                }
                break;
            case 1:
                for (int i = 0; i < _listCard.Length; i++)
                {
                    if (_listCard[i].type == TypeCard.EQUIPMENT)
                        InstanceElement(_listCard[i]);
                }
                break;

        }

    }
    public void InstanceElement(ZombieModel zombie)
    {
        GameObject element = Instantiate<GameObject>(elementCard, tranParentElement);
        element.transform.GetChild(0).GetComponent<CardInformation>().SetData(zombie);
        element.transform.GetChild(0).GetComponent<CardInformationUI>().SetUpCard();
        element.transform.GetChild(1).GetComponent<Text>().text = "★" + zombie.level;
    }
    void SetUpSpriteWhenClicked(Image imgSelect, Image imgUnSelect)
    {
        imgSelect.sprite = sprSelect;
        imgUnSelect.sprite = spriteUnSelect;
    }
    void DeleteElement()
    {
        int childs = tranParentElement.childCount;
        for (int i = childs - 1; i >= 0; i--)
        {
            GameObject.Destroy(tranParentElement.GetChild(i).gameObject);
        }
    }
}
