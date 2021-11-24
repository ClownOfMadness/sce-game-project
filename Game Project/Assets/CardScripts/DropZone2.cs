using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//responsible for creating and filling Zone2
public class DropZone2 : ZoneBehaviour
{
    public int ZoneSize = 2;    //default
    public CardDisplay[] Deck;  //objects to display

    public void Awake()
    {
        CardPool cards = new CardPool();
        cards.FillDeck(ZoneSize, Deck);
    }
}
