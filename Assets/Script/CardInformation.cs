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
}
