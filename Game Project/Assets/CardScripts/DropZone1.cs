using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;
//responsible for creating a zone to drop the cards to
public class DropZone1 : MonoBehaviour,IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public CardDisplay[] Deck;  //objects to display
    //[HideInInspector]
    public List<Card> cards;    //directory of all existing cards
    public int cardsCount;

    public void Awake()
    {
        //attempt to add the CardDisplays to scene via code
        /*
        for (int i = 0; i < handSize; i++)
        {
            Debug.Log("Card display " + i + " placed.");
            Deck[i] = new CardDisplay(null);
            Deck[i].transform.parent = gameObject.transform;
            Deck[i].addCard(cards[i]);
            //GameObject handCard = Instantiate(Deck[i], Deck[i].transform);    //taken from map script... applicable here?
        }*/

        FillCards();    //load cards from folder Resources


        //print cards from directory (for debugging)
        /*
        for (int i = 0; i <cardsCount; i++)
            Debug.Log("Card " + cards[i].code);
        */


        //fill CardDisplays on screen with random cards from directory
        for (int i = 0; i < Deck.Length; i++)
        {
            int random = Random.Range(0, cardsCount);
            //print cards from deck (for debugging)
            /*
                Debug.Log("i " + i + " random " + random + " picked " + cards[random].name);
            */
            Deck[i].addCard(cards[random]);
        }
    }
public void FillCards() //Loading cards from folder, must be in "Resources"
{
cards = Resources.LoadAll<Card>("").ToList();
cardsCount = cards.Count;
}
public void OnPointerEnter(PointerEventData eventData)
{

}
public void OnPointerExit(PointerEventData eventData)
{

}
public void OnDrop(PointerEventData eventData)
{
Debug.Log("OnDrop to" + gameObject.name);
DragCard d = eventData.pointerDrag.GetComponent<DragCard>();
if (d != null)
{
    d.parentReturnTo = this.transform;
}
}
}
