using UnityEngine;
using UnityEngine.EventSystems;

//responsible for creating a Zone to drop the cards to, Parent of other Zones that support dropping cards into them
public class Zone_Behaviour : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    [HideInInspector] public int Size;  //used by other Zones

    public virtual void EventDrop(Card_Drag cardObject) { } //lets other Zones override it
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
        {
            return;
        }
        Card_Drag d = eventData.pointerDrag.GetComponent<Card_Drag>();
        if (d != null)
        {
            if (this.transform.childCount < this.Size && this.transform == d.hand) 
            {
                d.placeholderParent = this.transform;       //only make a placeholder in Hand
            }
            bool zones = this.transform == d.hand || this.transform == d.destroy || this.transform == d.storagept2;
            if (zones)
            {
                d.cGroup.alpha = 1f; //reset effect for card (can be changed)
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
            d.positionReturnTo = d.transform.position;
            bool zones = this.transform == d.hand || this.transform == d.destroy || this.transform == d.storagept2;
            if (zones && d.screen.visibleMap && (d.card.buildingPrefab || int.TryParse(d.card.unitIndex, out int index)))
            {
                d.cGroup.alpha = 0f; //reset effect for card (can be changed)
            }
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
                if (this.transform == d.hand)
                {
                    d.transform.position = new Vector3(d.placeholder.transform.position.x, d.placeholder.transform.position.y - d.zHand.handShift, 0);  //original position
                }
                //move card
                d.parentReturnTo = this.transform;
                d.gameObject.transform.SetParent(this.transform);

                //close Building/Unit placement attempt
                d.screen.draggedBuilding = false;  //close selectedTile updates
                d.screen.draggedUnit = false;      //close selectedTile updates

                //reset for onHover
                d.dragged = false;
                d.clicked = false;
                d.screen.draggingCard = false;
               
                //run Zones
                this.EventDrop(d);  //run response function to dropping a card (of the fitting Zone)
                d.screen.CheckUnit();
                if (d.card != d.screen.Creation) 
                    d.zHand.RefreshZone();
            }
            else if (d.card == d.screen.Creation)   //doesn't snap on its own
            {
                d.transform.position = d.placeholder.transform.position;
            }
        }
    }
}
