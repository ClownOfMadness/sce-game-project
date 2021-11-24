using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

//collection of all existing cards
public class CardPool : ScriptableObject
{
    public List<Card> cards;    //list of all existing cards
    public int cardsCount;

    public void Awake()       //fills list from Resources folder
    {
        cards = Resources.LoadAll<Card>("").ToList();
        cardsCount = cards.Count;
    }
    public Card GetRandom() //pull random card out of card list
    {
        int random = Random.Range(1, cardsCount);   //starts at 1 to skip 0000 card
        return cards[random];
    }
    public void FillDeck(int ZoneSize, GameObject[] Deck)
    {
        for (int i = 0; i < ZoneSize; i++)          //fill Deck of a Zone with random cards
        {
            Deck[i].GetComponent<CardDisplay>().addCard(GetRandom());
        }
    }
    //Version for if Deck is a List
    /*
    public void FillDeck(int ZoneSize, List<GameObject> Deck)    
    {
        for (int i = 0; i < ZoneSize; i++)          //fill Deck of a Zone with random cards
        {
            Deck[i].GetComponent<CardDisplay>().addCard(GetRandom());
        }
    }
    */
    public Card getNull()   //might be needed eventually
    {
        return cards[0];
    }
}
