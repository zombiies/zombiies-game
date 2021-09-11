public class ZombieModel 
{
    public string ID;
    public string name;
    public int cost;
    public int level;
    public TypeRarity rareLevel;
    public TypeFaction faction;
    public SkillZombie[] skills;
    public string cid;
    public string tokenUri;
    public TypeCard type;

}
public enum TypeCard
{
    MONSTER,
    EQUIPMENT
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
    ARMOR
}
public enum TypeRarity
{
    COMMON,
    RARE,
    EPIC,
    LEGENDARY,
    ELITE

}
