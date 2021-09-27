using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class ConfirmTurnEventModel
{
    public AttackEventModel[] attacks;
    public MatchModel currentMatchStatus;
    public string playerId;
}
[Serializable]
public class AttackEventModel
{
    public SkillZombie skill;
    public string tokenId;
    public int toPosition;
    public int damage;
    public bool destroy;
    public BoardCardModel[] targetOnBoard;
}
[Serializable]
public class RewardModel
{
    public ZombieModel token;
    public string playerId;
}
