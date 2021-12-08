using UnityEngine;
using UnityEngine.EventSystems;

//responsible for creating a Zone to drop the cards to, Parent of other Zones that support dragging
public class Zone_Behaviour : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    [HideInInspector] public int Size;  //used by other Zones
    public GameObject CardPrefab;       //type of prefab for Card (attached via Inspector)
    //public GameObject MapBlank;

    public virtual void EventDrop(Card_Drag cardObject)
    { Debug.LogError("Wrong EventDrop function called."); }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
        {
            return;
        }
        Card_Drag d = eventData.pointerDrag.GetComponent<Card_Drag>();
        if (d != null)
        {
            d.GetComponent<CanvasGroup>().alpha = 1f;        //reset effect for card (can be changed)
                d.placeholderParent = this.transform;
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
        {
            return;
        }
        Card_Drag d = eventData.pointerDrag.GetComponent<Card_Drag>();
        if ((d != null) && (d.placeholderParent == this.transform))
        {
            d.GetComponent<CanvasGroup>().alpha = .6f;      //effect for card when moved (can be changed)
                d.placeholderParent = d.parentReturnTo;
        }
    }
    public void OnDrop(PointerEventData eventData)
    {
        Card_Drag d = eventData.pointerDrag.GetComponent<Card_Drag>();
        if (d != null)
        {
                d.parentReturnTo = this.transform;
                //d.gameObject.transform.SetParent(this.transform);
                this.EventDrop(d);  //run response function to dropping a card (of the fitting Zone)
        }
    }
}
