using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MutationModel
{
    public AuthModel signup;
    public AuthModel login;
    public ZombieModel[] ownedCardTokens;
    public DeckModel[] ownedDecks;
    public DeckModel addCardToDeck;
    public DeckModel removeCardFromDeck;
    public ZombieModel levelUpCard;
}
