using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class GlobalConfig : Singleton<GlobalConfig>
{
    public Sprite[] sprBorderCards;
    public Sprite[] sprRaritys;
    public Sprite[] sprCoreSkill;

    public string jsonZombie;
    public CardInformation cardInformation;
    public CardInformationUI cardInformationUI;

    [HideInInspector]public ZombieModel[] zombies;

    private void Awake()
    {
        zombies = JsonConvert.DeserializeObject<ZombieModel[]>(jsonZombie);

        cardInformation.nameCard = zombies[0].name;
        cardInformation.skillZombies = zombies[0].levels[cardInformation.level].skills;
        cardInformation.rarity = zombies[0].rareLevel;
        cardInformation.faction = zombies[0].faction;
        cardInformation.crytals = zombies[0].cost;

        cardInformationUI.SetUpCard();
    }
}
