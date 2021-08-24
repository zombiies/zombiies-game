public class ZombieModel 
{
    public string name;
    public int cost;
    public TypeRarity rareLevel;
    public TypeFaction faction;
    public LevelZombie[] levels;


}
public class SkillZombie
{
    public TypeCoreSkill type;
    public int value;
}
public class LevelZombie
{
    public int level;
    public SkillZombie[] skills;
}
public enum TypeFaction
{
    CHAOS,
    NATURE,
    BALANCE,
    FORTUNE,
    WAR
}
public enum TypeCoreSkill
{
    MELEE,
    RANGED,
    MAGIC,
    RANDOM,
    HEAL,
    GROUPHEAL,
    HP,
}
public enum TypeRarity
{
    COMMON,
    RARE,
    EPIC,
    LEGENDARY,
    ELITE

}
