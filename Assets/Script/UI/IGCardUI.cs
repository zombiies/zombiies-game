using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class IGCardUI : MonoBehaviour
{
    [SerializeField] Image imgZombie = default;
    [SerializeField] Image imgEquipment = default;
    [SerializeField] Image imgBorder = default;
    [SerializeField] SkillCardUI skillMelee = default;
    [SerializeField] SkillCardUI skillRanged = default;
    [SerializeField] SkillCardUI skillMagic = default;
    [SerializeField] SkillCardUI skillRandom = default;
    [SerializeField] SkillCardUI skillHp = default;
    [SerializeField] SkillCardUI skillArmor = default;
    [SerializeField] Animator animator;
    [SerializeField] GameObject DameMelee = default;
    [SerializeField] GameObject DameMagic = default;
    [SerializeField] GameObject DameRanged = default;

    [HideInInspector] public int positionCard = 1;
    [HideInInspector] public string tokenId;
    public void SetUpCard(BoardCardModel boardCardModel)
    {
        positionCard = boardCardModel.position;
        tokenId = boardCardModel.tokenId;
        gameObject.SetActive(true);
        SetSkillCard(boardCardModel);
        SetBorderCard(boardCardModel);
        imgZombie.sprite = Resources.Load<Sprite>(string.Format("Zombie/{0}", boardCardModel.name));
        if (boardCardModel.equipment != null)
        {
            imgEquipment.gameObject.SetActive(true);
            imgEquipment.sprite = Resources.Load<Sprite>(string.Format("Item/{0}", boardCardModel.equipment.name));
        }
        else
            imgEquipment.gameObject.SetActive(false);

    }
    private void SetSkillCard(BoardCardModel boardCardModel)
    {
        SkillZombie[] skills = boardCardModel.skills;
        for (int i = 0; i < skills.Length; i++)
        {

            switch (skills[i].type)
            {
                case TypeCoreSkill.HP:
                    SetDataSkill(skills[i], skillHp);
                    break;
                case TypeCoreSkill.ARMOR:
                    SetDataSkill(skills[i], skillArmor);
                    break;
                case TypeCoreSkill.MELEE:
                    SetDataSkill(skills[i], skillMelee, boardCardModel.position != 1);
                    break;
                case TypeCoreSkill.MAGIC:
                    SetDataSkill(skills[i], skillMagic, boardCardModel.position == 1);
                    break;
                case TypeCoreSkill.RANGED:
                    SetDataSkill(skills[i], skillRanged, boardCardModel.position == 1);
                    break;
                case TypeCoreSkill.RANDOM:
                    SetDataSkill(skills[i], skillRandom);
                    break;
            }
        }
    }
    private void SetDataSkill(SkillZombie skill, SkillCardUI skillUI, bool isProhibit = false)
    {
        skillUI.SetActive(true);
        skillUI.SetText(skill.value);
        skillUI.SetProhibit(isProhibit);
    }
    private void SetBorderCard(BoardCardModel boardCard)
    {
        switch (boardCard.faction)
        {
            case TypeFaction.BALANCE:
                imgBorder.sprite = GlobalConfig.Instance.sprBorderCardIGs[0];
                break;
            case TypeFaction.NATURE:
                imgBorder.sprite = GlobalConfig.Instance.sprBorderCardIGs[1];
                break;
            case TypeFaction.FORTUNE:
                imgBorder.sprite = GlobalConfig.Instance.sprBorderCardIGs[2];
                break;
            case TypeFaction.CHAOS:
                imgBorder.sprite = GlobalConfig.Instance.sprBorderCardIGs[3];
                break;
            case TypeFaction.WAR:
                imgBorder.sprite = GlobalConfig.Instance.sprBorderCardIGs[4];
                break;
        }
    }
    public void PlayAnimAttack()
    {
        PlayAnimIdle();
        animator.SetBool("isAttack", true);
    }
    public void PlayAnimHit(TypeCoreSkill type)
    {
        PlayAnimIdle();
        animator.SetBool("isHit", true);
        switch (type)
        {
            case TypeCoreSkill.MELEE:
                DameMelee.SetActive(true);
                break;
            case TypeCoreSkill.RANDOM:
                DameMelee.SetActive(true);
                break;
            case TypeCoreSkill.MAGIC:
                DameMagic.SetActive(true);
                break;
            case TypeCoreSkill.RANGED:
                DameRanged.SetActive(true);
                break;
            default:
                break;
        }
    }
    public void PlayAnimIdle()
    {
        animator.SetBool("isAttack", false);
        animator.SetBool("isHit", false);
    }

}
