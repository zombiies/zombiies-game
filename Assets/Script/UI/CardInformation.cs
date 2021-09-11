using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardInformation : MonoBehaviour
{
    [HideInInspector] public int crytals;
    [HideInInspector] public string nameCard;
    [HideInInspector] public int level = 1;
    [HideInInspector] public TypeFaction faction;
    [HideInInspector] public TypeRarity rarity;
    [HideInInspector] public SkillZombie[] skillZombies;
    [HideInInspector] public LevelZombie[] levelZombies;
    [HideInInspector] public TypeCard type;

    public void SetData(ZombieModel _data)
    {
        crytals = _data.cost;
        nameCard = _data.name;
        level = _data.level;
        faction = _data.faction;
        rarity = _data.rareLevel;
        skillZombies = _data.skills;
        type = _data.type;
    }
}
