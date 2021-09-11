using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckPopupController : MonoBehaviour
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
    // Start is called before the first frame update
    void Start()
    {
        SetUpSpriteWhenClicked(imgMonster, imgEquipment);
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
