using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//responsible for creating a Zone to drop the cards to, Parent of Zone1&Zone2
public class ZoneBehaviour : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject CardPrefab;   //type of prefab for Card (attached via GUI)
    public int ZoneSize;            //default
    public GameObject[] Deck;       //objects in Zone
    public void InstantiateZone(int start, int zoneSize, GameObject[] deck, ZoneBehaviour Zone)
    {
        deck = new GameObject[zoneSize];
        for (int i = start; i < zoneSize; i++)  //create and instantiate objects in scene in runtime
        {
            deck[i] = Instantiate(CardPrefab, Zone.transform);
            deck[i].name = string.Format("Card ({0})", i);
        }

        CardPool cards = new CardPool();        //open CardPool connection to use its functions
        cards.FillDeck(zoneSize, deck);
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
        //GameObject c = eventData.pointerDrag.GetComponent<GameObject>();
        if (d != null)
        {
            //remove card from old Deck
            //ZoneSize-- of old Zone;

            d.parentReturnTo = this.transform;

            //add card to new Deck
            //ZoneSize-- to new Zone;
        }
    }
}
