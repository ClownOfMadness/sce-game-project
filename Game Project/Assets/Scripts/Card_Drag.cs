using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

//responsible for the Card Drag commands, extension of CardDisplay
public class Card_Drag : Card_Display, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [HideInInspector] public Transform parentReturnTo = null;         //helps snapping the card back to place
    [HideInInspector] public GameObject placeholder = null;           //saves the dragged card's spot (for changing card order)
    [HideInInspector] public Transform placeholderParent = null;      //saves the dragged card's parent
    [HideInInspector] public Transform hand;
    [HideInInspector] public Transform unit;
    [HideInInspector] public Transform destroy;
    private Zone_Hand zHand;
    private Player_SpawnBuilding Tiles;
    private Unit_List Units;

    void Start()
    {
        canvas = this.transform.parent.parent.GetComponent<Canvas>();
        screen = canvas.GetComponent<Screen_Cards>();
        hand = screen.Hand.transform;
        unit = screen.Unit.transform;
        destroy = screen.destroyButton.transform;
        zHand = hand.GetComponent<Zone_Hand>();
        Tiles = FindObjectOfType<Player_SpawnBuilding>();   //connection to use SpawnBuilding functions
        Units = FindObjectOfType<Unit_List>();              //connection to use UnitList functions
    }
    public void ReturnToHand()          //return to hand after craft attempt
    {
        screen.CraftToHand(this);       //needs to happen here to get the "this" of the object
    }
    public void SwitchCardPlace()       //move card between hand-craft on click
    {
        screen.CardClick(this);         //needs to happen here to get the "this" of the object
    }
    private void SavePlaceholder()      //enables card order
    {
        placeholder = new GameObject(); //initalizing the temp object to save the card's spot
        placeholder.transform.SetParent(this.transform.parent);
        placeholder.name = "Placeholder";
        LayoutElement le = placeholder.AddComponent<LayoutElement>();
        le.preferredWidth = this.GetComponent<LayoutElement>().preferredWidth;
        le.preferredHeight = this.GetComponent<LayoutElement>().preferredHeight;
        le.flexibleWidth = 0;
        le.flexibleHeight = 0;
        placeholder.transform.SetSiblingIndex(this.transform.GetSiblingIndex());    //saves size and position of card
        placeholderParent = parentReturnTo;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        parentReturnTo = this.transform.parent;
        SavePlaceholder();
        this.transform.SetParent(this.transform.parent.parent); //changes parent once the card is picked
        GetComponent<CanvasGroup>().blocksRaycasts = false;
        if (card.buildingPrefab && placeholderParent == hand)                           //if building, start recieving selectedTile updates and hide card
        {
            screen.draggedBuilding = true;
            screen.draggedSprite = card.artwork;
            GetComponent<CanvasGroup>().alpha = 0f; //hide building until placed/returned to hand
        }
        if (int.TryParse(card.unitIndex, out int index) && placeholderParent == hand)   //if unit, start recieving selectedTile updates and hide card
        {
            screen.draggedUnit = true;
            screen.draggedSprite = card.artwork;
            GetComponent<CanvasGroup>().alpha = 0f; //hide unit until placed/returned to hand
        }
    }
    public void OnDrag(PointerEventData eventData)
    {
        GetComponent<RectTransform>().anchoredPosition += eventData.delta / canvas.scaleFactor; //moves by wherever we picked the card instead of by the middle
        if (placeholder.transform.parent != placeholderParent)
        {
            placeholder.transform.SetParent(placeholderParent);
        }
        int newSiblingIndex = placeholderParent.childCount;         //a workaround to make it also work from right to left
        for (int i = 0; i < placeholderParent.childCount; i++)      //loops to move the placeholder to the left
        {
            if (this.transform.position.x < placeholderParent.GetChild(i).position.x)
            {
                newSiblingIndex = i;
                if (placeholder.transform.GetSiblingIndex() < newSiblingIndex)
                    newSiblingIndex--;
                break;
            }
        }
        placeholder.transform.SetSiblingIndex(newSiblingIndex);     //the abillity to swap places with cards in hand
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (this.transform.parent != hand && this.transform.parent != destroy && this.transform.parent != unit) //needed to allow building placement only outside of UI, don't change
        {
            if (placeholderParent == hand)
                if (card.buildingPrefab)                    //if building, try to place
                {
                    if (Tiles.Spawn(card.buildingPrefab, screen.selectedTile))     //spawn the card's building on the tile that's under the pointer
                    {
                        //Debug.Log("Card_Drag: placing " + card.buildingPrefab.name + " at " + screen.selectedTile.name);
                        Destroy(placeholder);
                        Destroy(this.gameObject);           //destroy card that was placed successfully
                    }
                }
                else if (int.TryParse(card.unitIndex, out int index))             //if unit, try to place
                {
                    if (Units.AddUnit(index, screen.selectedTile))     //spawn the card's unit on the tile that's under the pointer
                    {
                        Debug.Log("Card_Drag: placing unit #" + card.unitIndex + " at " + screen.selectedTile.name);
                        Destroy(placeholder);
                        Destroy(this.gameObject);           //destroy card that was placed successfully
                    }
                }
        }
        screen.draggedBuilding = false; //close selectedTile updates
        screen.draggedUnit = false;     //close selectedTile updates
        if (this.gameObject)     //if object still exists, snap back to Hand
            SnapToParent();
    }
    private void SnapToParent()
    {
        this.transform.SetParent(parentReturnTo);
        this.transform.SetSiblingIndex(placeholder.transform.GetSiblingIndex());
        GetComponent<CanvasGroup>().alpha = 1f; //reset effect for card (can be changed)
        this.transform.localScale = new Vector3(zHand.CardPrefab.transform.localScale.x, zHand.CardPrefab.transform.localScale.y, 0);
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        Destroy(placeholder);
    }
}
