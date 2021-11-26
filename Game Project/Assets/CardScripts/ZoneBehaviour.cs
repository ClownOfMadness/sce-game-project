using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//responsible for creating a Zone to drop the cards to, Parent of other Zones
public class ZoneBehaviour : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    [HideInInspector]
    public int Size;                //used by other Zones
    public GameObject CardPrefab;   //type of prefab for Card (attached via Inspector)

    public void InstantiateZone(ZoneBehaviour Zone)
    {
        CardPool pool = ScriptableObject.CreateInstance<CardPool>();        //open CardPool connection to use its functions
        for (int i = Zone.transform.childCount; i < Zone.Size; i++)  
        {
            GameObject newCard = Instantiate(CardPrefab, Zone.transform);   //create and instantiate objects in scene in runtime
            string newName=pool.FillObject(newCard);                        //add cards to objects + save the new card name (for displaying in Scene)
            newCard.name = string.Format("{0} (Card)", newName);            //updates name in scene
        }
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
        CardDrag d = eventData.pointerDrag.GetComponent<CardDrag>();
        if (d != null)
        {
            if (this.transform.childCount < this.Size)
                d.parentReturnTo = this.transform;  //switch parents only if there's extra space

        }
    }
}
