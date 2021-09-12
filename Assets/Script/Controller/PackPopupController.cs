using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public class PackPopupController : Singleton<PackPopupController>
{
    [SerializeField] ElementCard elementCard;
    [SerializeField] Transform elementParent;
    [SerializeField] Button btnNextPage;
    [SerializeField] Button btnBackPage;
    [SerializeField] Button btnMonster;
    [SerializeField] Button btnEquipment;
    [SerializeField] Button btnExit;
    [SerializeField] Image imgMonster;
    [SerializeField] Image imgEquipment;
    [SerializeField] Sprite spiteOnSelectBtn;
    [SerializeField] Sprite spriteOffSelectBtn;
    [SerializeField] Text txtPage;

    private Dictionary<string, List<ZombieModel>> listCardUser = new Dictionary<string, List<ZombieModel>>();

    private int _currentPage = 0;
    private int _currentType = 0;
    private int _totalPage;
    private bool _isMonsterClicked = true;
    private bool _isEquipmentClicked = false;

    void Awake()
    {
        GraphQLManager.Instance.GetUserCards(() =>
        {
            OnButtonClicked();
            SetUpSpriteWhenClicked(imgMonster, imgEquipment);
            ShowPage();
        });
    }
    private void OnEnable()
    {
        GraphQLManager.Instance.GetUserCards(() =>
        {
            OnButtonClicked();
            SetUpSpriteWhenClicked(imgMonster, imgEquipment);
            ShowPage();
        });
    }
    private void OnButtonClicked()
    {
        btnBackPage.onClick.AddListener(OnButtonBackPageClicked);
        btnNextPage.onClick.AddListener(OnButtonNextPageClicked);
        btnMonster.onClick.AddListener(OnButtonMonsterClicked);
        btnEquipment.onClick.AddListener(OnButtonEquipmentClicked);
        btnExit.onClick.AddListener(OnExitButtonClicked);
    }
    private void OnButtonMonsterClicked()
    {
        if (_isMonsterClicked) return;
        SetUpSpriteWhenClicked(imgMonster, imgEquipment);
        _isEquipmentClicked = false;
        _isMonsterClicked = true;
        _currentPage = 0;
        _currentType = 0;
        ShowPage();
    }
    private void OnButtonEquipmentClicked()
    {
        if (_isEquipmentClicked) return;
        SetUpSpriteWhenClicked(imgEquipment, imgMonster);
        _isEquipmentClicked = true;
        _isMonsterClicked = false;
        _currentPage = 0;
        _currentType = 1;
        ShowPage();
    }
    private void OnButtonNextPageClicked()
    {
        _currentPage++;
        if (_currentPage > _totalPage - 1)
            _currentPage = 0;
        ShowPage();
    }
    private void OnButtonBackPageClicked()
    {
        _currentPage--;
        if (_currentPage < 0)
            _currentPage = _totalPage;
        ShowPage();
    }
    private void OnExitButtonClicked()
    {
        if (!HomeController.Instance.gameObject.activeSelf) return;
        if (HomeController.Instance.packPopup.activeSelf)
            HomeController.Instance.packPopup.SetActive(false);
        if (HomeController.Instance.deckPopup.activeSelf)
            HomeController.Instance.deckPopup.SetActive(false);
    }
    public void ShowPage()
    {
        DeleteElement();
        switch (_currentType)
        {
            case 0:
                listCardUser = DataManager.Instance.listCardMosterUser;
                break;
            case 1:
                listCardUser = DataManager.Instance.listCardEquipmentUser;
                break;
        }
        if (listCardUser.Count <= 0) return;
        _totalPage = listCardUser.Count / 6 + 1;

        for (int i = _currentPage * 6; i < (listCardUser.Count <= (_currentPage * 6 + 6) ? listCardUser.Count : (_currentPage * 6 + 6)); i++)
        {
            ElementCard element = Instantiate<ElementCard>(elementCard, elementParent);
            element.cardInformation.SetData(listCardUser.ElementAt(i).Value[0]);
            element.cardInformationUI.SetUpCard();
            element.txt_Amount.text = "X" + listCardUser.ElementAt(i).Value.Count;
        }
        int _intCount = _currentPage + 1;
        txtPage.text = _intCount + "/" + _totalPage;
    }
    void DeleteElement()
    {
        int childs = elementParent.childCount;
        for (int i = childs - 1; i >= 0; i--)
        {
            GameObject.Destroy(elementParent.GetChild(i).gameObject);
        }
    }
    void SetUpSpriteWhenClicked(Image imgclick, Image imgUnclick)
    {
        imgclick.sprite = spiteOnSelectBtn;
        imgUnclick.sprite = spriteOffSelectBtn;
    }

}
