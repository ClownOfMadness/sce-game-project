using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//responsible for creating and filling Zone1
public class DropZone1 : ZoneBehaviour
{
    public int ZoneSize = 8;        //default
    public CardDisplay[] Deck;      //objects to display

    public void Awake()
    {
        CardPool cards = new CardPool();
        cards.FillDeck(ZoneSize, Deck);
    }
}
