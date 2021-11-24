using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//responsible for creating a zone to drop the cards to
public class DropZone2 : DropZone1
{
    public CardDisplay[] DeckPool;  //objects to display

    public new void Awake()
    {
        FillCards();    //load cards from folder Resources

        for (int i = 0; i < DeckPool.Length; i++)
        {
            int random = Random.Range(0, cardsCount);
            Debug.Log("random " + random);
            Debug.Log("i " + i + " random " + random + " picked " + cards[random].name);
            DeckPool[i].addCard(cards[random]);
        }
        
    }
}
