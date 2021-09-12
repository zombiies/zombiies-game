using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TokenInDeckInput 
{
    public string tokenId;
    public string deckId;

    public TokenInDeckInput(string _tokenId, string _deckId)
    {
        this.tokenId = _tokenId;
        this.deckId = _deckId;
    }
}
