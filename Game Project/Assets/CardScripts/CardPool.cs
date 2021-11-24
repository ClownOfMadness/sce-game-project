using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CardPool : ScriptableObject
{
    public List<Card> cards;//list of all existing cards
    public int cardsCount;

    public CardPool()       //fills list from Resources folder
    {
        cards = Resources.LoadAll<Card>("").ToList();
        cardsCount = cards.Count;
    }
    public Card GetRandom() //pull random card out of card list
    {
        int random = Random.Range(1, cardsCount);   //starts at 1 to skip 0000 card
        return cards[random];
    }
    public void FillDeck(int ZoneSize, CardDisplay[] Deck)
    {
        for (int i = 0; i < ZoneSize; i++)          //fill deck of a Zone with random cards
        {
            Deck[i].addCard(GetRandom());
        }
    }
}
