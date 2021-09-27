using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
[Serializable]
public class EndTurnEventModel
{
    public MatchModel currentMatchStatus;
    public string playerId;
}

