using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//responsible for the Card Drag commands, extension of CardDisplay
public class CardDrag : CardDisplay, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [HideInInspector] public Transform parentReturnTo = null;         //helps snapping the card back to place
    [HideInInspector] public GameObject placeholder = null;           //saves the dragged card's spot (for changing card order)
    [HideInInspector] public Transform placeholderParent = null;      //saves the dragged card's parent

    void Start()
    {
        canvas = this.transform.parent.parent.GetComponent<Canvas>();
    }
    public void ReturnToHand()          //return to hand after craft attempt
    {
        canvas.GetComponent<ZoneCards>().CraftToHand(this);
    }
    public void SwitchCardPlace()       //move card between hand-craft on click
    {
        canvas.GetComponent<ZoneCards>().HandToCraft(this);
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
        this.transform.SetParent(this.transform.parent.parent);                     //changes parent once the card is picked
        GetComponent<CanvasGroup>().blocksRaycasts = false;
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
        this.transform.SetParent(parentReturnTo);
        this.transform.SetSiblingIndex(placeholder.transform.GetSiblingIndex());
        GetComponent<CanvasGroup>().alpha = 1f;                     //reset effect for card (can be changed)
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        Destroy(placeholder);
    }
}
