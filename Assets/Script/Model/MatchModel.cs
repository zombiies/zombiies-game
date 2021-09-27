using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class MatchModel
{
    public string id;
    public PlayerStatusModel[] playerStatuses;
    public UserModel winner;
}
[Serializable]

public class PlayerStatusModel
{
    public BoardCardModel[] onBoard;
    public ZombieModel[] onHand;
    public string playerId;
    public int crystal;
    public bool inTurn;
    public bool confirmTurn;

    public int GetAmountCardByType(TypeCard type)
    {
        int amount = 0;
        for (int i = 0; i < (onHand?.Length ?? 0); i++)
        {
            if (onHand[i].type == type)
                amount++;
        }
        for (int i = 0; i < (onBoard?.Length ?? 0); i++)
        {
            if (onBoard[i].type == type)
                amount++;
            if (onBoard[i].equipment != null && type == TypeCard.EQUIPMENT)
                amount++;
        }
        return amount;
    }
}
[Serializable]
public class BoardCardModel : ZombieModel
{
    public ZombieModel equipment;
}
