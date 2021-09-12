using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpCardInput 
{
    public string[] sacrificeTokenIds;

    public LevelUpCardInput(string cardId1, string cardId2)
    {
        this.sacrificeTokenIds = new string[] { cardId1, cardId2 };
    }
}
