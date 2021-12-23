using UnityEngine;
using UnityEngine.EventSystems;

//responsible for creating a Zone to drop the cards to, Parent of other Zones that support dropping cards into them
public class Zone_Behaviour : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    [HideInInspector] public int Size;  //used by other Zones
    public GameObject CardPrefab;       //type of prefab for Card (attached via Inspector)

    public virtual void EventDrop(Card_Drag cardObject) { } //lets other Zones override it
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
        {
            return;
        }
        Card_Drag d = eventData.pointerDrag.GetComponent<Card_Drag>();
        if (d != null && this.transform.childCount < this.Size)
        {
            //d.GetComponent<CanvasGroup>().alpha = 1f;     //reenables itself in a loop for no reason
            if (this.transform == d.hand)
            {
                d.placeholderParent = this.transform;       //only make a placeholder in Hand
            }
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
            d.placeholderParent = d.parentReturnTo;
        }
    }
    public void OnDrop(PointerEventData eventData)
    {
        Card_Drag d = eventData.pointerDrag.GetComponent<Card_Drag>();
        if (d != null)
        {
            Card_Drag[] cardObjects = this.transform.GetComponentsInChildren<Card_Drag>(); //walkaround to ignore placeholders and only check cards

            //Creation can't be placed in Storage and Storagebutton, also Storage Size works differently from other Zones' Size
            bool storage = this.transform == d.storage || this.transform == d.storagept2;
            bool notStoringCreation = d.card != d.screen.Creation;
            bool spaceInStorage = cardObjects.Length < d.zStorage.Size;
            bool spaceInZones = cardObjects.Length < this.Size && !storage;
            if (spaceInZones || (storage && spaceInStorage && notStoringCreation)) 
            {
                if (d.parentReturnTo == d.menu && this.transform != d.menu)         //close menu prompt
                {
                    d.zCraft.ClearMenu();
                    d.screen.visibleMap = true;
                    d.screen.MenuUp = false;
                }
                else if (d.parentReturnTo == d.craft && this.transform != d.craft)  //close craft
                {
                    d.screen.Craft.SetActive(false);
                    d.screen.visibleMap = true;
                    d.screen.CraftUp = false;
                }
                else if (d.parentReturnTo == d.unit && this.transform != d.unit)    //close unit prompt
                {
                    d.zUnit.FreeUnit();
                }
                d.parentReturnTo = this.transform;
                d.gameObject.transform.SetParent(this.transform);
                d.screen.draggedBuilding = false;  //close selectedTile updates
                d.screen.draggedUnit = false;      //close selectedTile updates
                this.EventDrop(d);  //run response function to dropping a card (of the fitting Zone)
                d.screen.CheckUnit();
                d.zHand.RefreshZone();
            }
        }
    }
}
