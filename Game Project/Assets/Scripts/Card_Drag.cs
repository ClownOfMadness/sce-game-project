using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//responsible for the draggable cards, extension of Card_Display
public class Card_Drag : Card_Display, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    //public fields:
    [HideInInspector] public Transform parentReturnTo = null;       //saves the dragged card's parent
    [HideInInspector] public Transform placeholderParent = null;    //saves the dragged card's spot (for changing card order)
    [HideInInspector] public GameObject placeholder = null;         //saves the dragged card's spot (for changing card order)
    [HideInInspector] public Canvas canvas;   //needed for moving the card
    [HideInInspector] public CanvasGroup cGroup;
    [HideInInspector] public RectTransform rectT;

    //public Zones:
    [HideInInspector] public Transform hand;
    [HideInInspector] public Transform menu;
    [HideInInspector] public Transform craft;
    [HideInInspector] public Transform unit;
    [HideInInspector] public Transform storage;
    [HideInInspector] public Transform storagept2;
    [HideInInspector] public Transform destroy;

    //external access:
    [HideInInspector] public Zone_Hand zHand;
    [HideInInspector] public Zone_Craft zCraft;
    [HideInInspector] public Zone_Unit zUnit;
    [HideInInspector] public Zone_Storage zStorage;
    private Player_SpawnBuilding Tiles;
    private Unit_List Units;

    //ohHover:
    [HideInInspector] public Vector3 positionReturnTo;
    [HideInInspector] public bool dragged;
    [HideInInspector] public bool clicked;
    [HideInInspector] public bool automatic;
    private GameObject Desc;
    private Text Desc_Text;

    void Start()
    {
        cGroup = GetComponent<CanvasGroup>();
        rectT = GetComponent<RectTransform>();
        Tiles = screen.Game.Buildings;  //connection to use SpawnBuilding functions
        Units = screen.Game.Units;      //connection to use UnitList functions
        dragged = false;
        clicked = false;
        Desc = screen.Desc;
        Desc_Text = screen.Desc_Text;
    }
    public void SwitchCardPlace()   //move card between zones on click, used by button
    {
        screen.CardClick(this);         //needs to happen here to get the "this" of the object
    }
    public float HandShift()    //used to shift cards onHover
    {
        return Mathf.Abs(Screen.height / 20);
    }
    public void OnPointerEnter(PointerEventData eventData)   //onHover set
    {
        if (!dragged && !screen.draggingCard && transform.parent == hand)
        {
            positionReturnTo = new Vector3(transform.position.x, transform.position.y, 0);  //original position
            screen.overlapingCard = false;
            transform.position = new Vector3(transform.position.x, positionReturnTo.y + HandShift(), 0);

            Desc_Text.text = card.description;
            Desc.gameObject.SetActive(true);
            Desc.transform.position = new Vector3(transform.position.x, Desc.transform.position.y, 0);
        }
    }
    public void OnPointerExit(PointerEventData eventData)   //onHover reset
    {
        Card_Drag d = eventData.pointerEnter.GetComponent<Card_Drag>();
        if (d == null && screen.draggingCard) //prevent cards from acting weird when placed on top of each other
        {
            screen.overlapingCard = true;
        }
        if (clicked || automatic || screen.automaticCard)  //handles cards being added from other zones
        {
            positionReturnTo = transform.position;  //save position (used by cards that never entered the OnPointerEnter function)
            clicked = false;                //handles cards being added by click from Craft&Unit
            automatic = false;              //handles automatic addition of cards from failed Craft
            screen.automaticCard = false;   //handles automatic addition of cards from Units
        }
        else if (transform.parent == hand && !screen.overlapingCard)
        {
            transform.position = positionReturnTo;
        }
        Desc.gameObject.SetActive(false);
    }
    private void SavePlaceholder()  //enables card order
    {
        placeholder = new GameObject(); //initalizing the temp object to save the card's spot
        placeholder.transform.SetParent(this.transform.parent);
        placeholder.name = "Placeholder";
        LayoutElement lep = placeholder.AddComponent<LayoutElement>();
        LayoutElement led = this.GetComponent<LayoutElement>();
        lep.preferredWidth = led.preferredWidth;
        lep.preferredHeight = led.preferredHeight;
        lep.flexibleWidth = 0;
        lep.flexibleHeight = 0;
        placeholder.transform.SetSiblingIndex(this.transform.GetSiblingIndex());    //saves size and position of card
        placeholderParent = parentReturnTo;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        dragged = true;
        screen.draggingCard = true;
        parentReturnTo = this.transform.parent;
        SavePlaceholder();
        this.transform.SetParent(this.transform.parent.parent); //changes parent once the card is picked
        cGroup.blocksRaycasts = false;

        if (screen.visibleMap && placeholderParent == hand)     //enable pointer change only for cards leaving hand when UI is down
        {
            if (card.buildingPrefab)                                //if building, start recieving selectedTile updates and hide card
            {
                screen.draggedBuilding = true;
                screen.draggedSprite = card.artwork;
                cGroup.alpha = 0f; //hide building until placed/returned to hand
            }
            else if (int.TryParse(card.unitIndex, out int index))   //if unit, start recieving selectedTile updates and hide card
            {
                screen.draggedUnit = true;
                screen.draggedSprite = card.artwork;
                cGroup.alpha = 0f; //hide unit until placed/returned to hand
            }
        }
        if (this.card != screen.Creation)
            zHand.RefreshZone();
    }
    public void OnDrag(PointerEventData eventData)
    {
        rectT.anchoredPosition += eventData.delta / canvas.scaleFactor; //moves by wherever we picked the card instead of by the middle
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
        placeholder.transform.SetSiblingIndex(newSiblingIndex); //the abillity to swap places with cards in hand
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        //needed to allow building placement only outside of UI, don't change
        bool validPlacement = this.transform.parent != hand && this.transform.parent != destroy && this.transform.parent != storagept2;
        bool snap = true;
        if (validPlacement && screen.visibleMap)
        {
            if (placeholderParent == hand)
                if (card.buildingPrefab)    //if building, try to place
                {
                    if (Tiles.Spawn(card.buildingPrefab, screen.selectedTile))     //spawn the card's building on the tile that's under the pointer
                    {
                        //Debug.Log("Card_Drag: placing " + card.buildingPrefab.name + " at " + screen.selectedTile.name);
                        Destroy(placeholder);
                        Destroy(this.gameObject);   //destroy card that was placed successfully
                        screen.draggingCard = false;
                        zHand.RefreshZone();
                        //refresh cards?
                        snap = false;
                    }
                }
                else if (int.TryParse(card.unitIndex, out int index))   //if unit, try to place
                {
                    if (Units.SummonUnit(index, screen.selectedTile, null, false))  //spawn the card's unit on the tile that's under the pointer
                    {
                        //Debug.Log("Card_Drag: placing unit #" + card.unitIndex + " at " + screen.selectedTile.name);
                        Destroy(placeholder);
                        Destroy(this.gameObject);   //destroy card that was placed successfully
                        screen.draggingCard = false;
                        zHand.RefreshZone();
                        //refresh cards?
                        snap = false;
                    }
                }
        }
        screen.draggedBuilding = false; //close selectedTile updates
        screen.draggedUnit = false;     //close selectedTile updates

        if (snap)      //if object still exists, snap back to Hand
            SnapToParent();
    }
    public void SnapToParent()
    {
        if (card == screen.Creation)   //doesn't snap on its own
        {
            transform.position = placeholder.transform.position;
        }
        else if (transform.parent == hand)
        {
            transform.position = positionReturnTo;
        }
        this.transform.SetParent(parentReturnTo);
        this.transform.SetSiblingIndex(placeholder.transform.GetSiblingIndex());
        cGroup.alpha = 1f; //reset effect for card (can be changed)
        cGroup.blocksRaycasts = true;
        Destroy(placeholder);

        dragged = false;
        clicked = false;
        screen.draggingCard = false;
        zHand.RefreshZone();
    }
}
