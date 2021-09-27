using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class PrepareTurnEventModel
{
    public PrepareTurnEventType type;
    public MatchModel currentMatchStatus;
    public string playerId;
    public string tokenId;
    public int toPosition;
}

public enum PrepareTurnEventType
{
    PUT_CARD,
    DENY_CARD
}
