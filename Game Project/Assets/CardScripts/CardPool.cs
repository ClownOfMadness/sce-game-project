using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;  //needed for ToList function

//collection of all existing cards
public class CardPool : ScriptableObject
{
    public List<Card> cards;    //list of all existing cards

    public void Awake()         //fills list from Resources folder
    {
        cards = Resources.LoadAll<Card>("").ToList();
    }

    public string FillObject(GameObject cardObject) //add card to object + return the new card name (for displaying in Scene)
    {
        Card newCard = cards[Random.Range(0, cards.Count)];
        cardObject.GetComponent<CardDisplay>().AddCard(newCard);
        return newCard.name;
    }
}
