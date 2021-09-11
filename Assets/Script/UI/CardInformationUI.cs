using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardInformationUI : MonoBehaviour
{
    [SerializeField] CardInformation cardInformation;
    [Header("TEXT")]
    [SerializeField] Text txtHp = default;
    [SerializeField] Text txtName = default;
    [SerializeField] Text txtMagic = default;
    [SerializeField] Text txtMelee = default;
    [SerializeField] Text txtCrytal = default;
    [SerializeField] Text txtRanged = default;
    [SerializeField] Text txtRandom = default;
    [Header("IMAGE")]
    [SerializeField] Image imgElite = default;
    [SerializeField] Image imgBorder;
    [SerializeField] Image imgZombie = default;
    [SerializeField] Image imgRarity = default;
    [SerializeField] Material materialFade = default;

    private void Awake()
    {
        imgBorder.material = new Material(materialFade);
        imgZombie.material = new Material(materialFade);
        imgElite.material = new Material(materialFade);
        imgRarity.material = new Material(materialFade);
    }
    #region Public Method
    public void SetFactionBorderCard()
    {
        switch (cardInformation.faction)
        {
            case TypeFaction.BALANCE:
                imgBorder.sprite = GlobalConfig.Instance.sprBorderCards[0];
                break;
            case TypeFaction.NATURE:
                imgBorder.sprite = GlobalConfig.Instance.sprBorderCards[1];
                break;
            case TypeFaction.FORTUNE:
                imgBorder.sprite = GlobalConfig.Instance.sprBorderCards[2];
                break;
            case TypeFaction.CHAOS:
                imgBorder.sprite = GlobalConfig.Instance.sprBorderCards[3];
                break;
            case TypeFaction.WAR:
                imgBorder.sprite = GlobalConfig.Instance.sprBorderCards[4];
                break;
        }
    }
    public void SetRarityCard()
    {
        switch (cardInformation.rarity)
        {
            case TypeRarity.COMMON:
                SetActiveRarity(false);
                imgRarity.sprite = GlobalConfig.Instance.sprRaritys[0];
                break;
            case TypeRarity.RARE:
                SetActiveRarity(false);
                imgRarity.sprite = GlobalConfig.Instance.sprRaritys[1];
                break;
            case TypeRarity.EPIC:
                SetActiveRarity(false);
                imgRarity.sprite = GlobalConfig.Instance.sprRaritys[2];
                break;
            case TypeRarity.LEGENDARY:
                SetActiveRarity(false);
                imgRarity.sprite = GlobalConfig.Instance.sprRaritys[3];
                break;
            case TypeRarity.ELITE:
                SetActiveRarity(true);
                break;
        }
    }
    public void SetImageZombie()
    {
        switch (cardInformation.type)
        {
            case TypeCard.MONSTER:
                imgZombie.sprite = Resources.Load<Sprite>(string.Format("Zombie/{0}", cardInformation.nameCard));
                break;
            case TypeCard.EQUIPMENT:
                imgZombie.sprite = Resources.Load<Sprite>(string.Format("Item/{0}", cardInformation.nameCard));
                break;
        }
    }
    public void SetCostZombie()
    {
        txtCrytal.text = cardInformation.crytals.ToString();
    }
    public void SetSkill()
    {
        for (int i = 0; i < cardInformation.skillZombies.Length; i++)
        {
            switch (cardInformation.skillZombies[i].type)
            {
                case TypeCoreSkill.HP:
                    SetTextSkill(txtHp, i);
                    break;
                case TypeCoreSkill.MELEE:
                    SetTextSkill(txtMelee, i);
                    break;
                case TypeCoreSkill.RANGED:
                    SetTextSkill(txtRanged, i);
                    break;
                case TypeCoreSkill.MAGIC:
                    SetTextSkill(txtMagic, i);
                    break;
                case TypeCoreSkill.RANDOM:
                    SetTextSkill(txtRandom, i);
                    break;
            }
        }
    }
    public void SetNameCard()
    {
        txtName.text = cardInformation.nameCard + "★" + cardInformation.level;
    }
    public void SetUpCard()
    {
        SetFactionBorderCard();
        SetRarityCard();
        SetImageZombie();
        SetCostZombie();
        SetSkill();
        SetNameCard();
    }

    //public IEnumerator SetAnimationFade()
    //{

    //}    
    #endregion

    #region Private Method
    private void SetTextSkill(Text skill, int index)
    {
        skill.transform.parent.gameObject.SetActive(true);
        skill.text = cardInformation.skillZombies[index].value.ToString();
    }
    private void SetActiveRarity(bool isElite)
    {
        imgElite.gameObject.SetActive(isElite);
        imgRarity.gameObject.SetActive(!isElite);
    }
    public void Click()
    {
        Debug.Log("Checlk");
    }
    #endregion
}

