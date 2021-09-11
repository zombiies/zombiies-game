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

    [HideInInspector]public ZombieModel[] zombies;

    private void Awake()
    {
        zombies = JsonConvert.DeserializeObject<ZombieModel[]>(jsonZombie);
    }
}
