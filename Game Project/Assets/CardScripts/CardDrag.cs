using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//responsible for the card drag command, extension of CardDisplay
public class CardDrag : CardDisplay, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Transform parentReturnTo = null;
    public Transform oldParent;
    public GameObject placeholder = null;    //needed to hold the dragged card's spot
    public Transform placeholderParent = null;      //needed to hold the dragged card's spot
    public Canvas canvas;

    public void Awake()
    {
        canvas = this.transform.parent.parent.GetComponent<Canvas>();
    }
    public void ReturnToHand()  //return to hand after craft attempt
    {
        Debug.Log("Trying to change Zone");
        this.transform.SetParent(this.oldParent);
    }
    public void SavePlaceholder()   //enables card order
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
        oldParent = this.transform.parent;
        Debug.Log("OldParent: " + oldParent.name);
        Debug.Log("OnBeginDrag");
        parentReturnTo = this.transform.parent;
        SavePlaceholder();
        this.transform.SetParent(this.transform.parent.parent); //changes parent once the card is picked
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }
    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("OnDrag");
        //this.transform.position = eventData.position; //moves by the middle of the card
        GetComponent<RectTransform>().anchoredPosition += eventData.delta / canvas.scaleFactor; //moves by wherever we picked the card
        if (placeholder.transform.parent != placeholderParent)
        {
            placeholder.transform.SetParent(placeholderParent);
        }
        int newSiblingIndex = placeholderParent.childCount;    //a workaround to make it also work from right to left
        for (int i = 0; i < placeholderParent.childCount; i++) //loops to move the placeholder to the left
        {
            if (this.transform.position.x < placeholderParent.GetChild(i).position.x)
            {
                newSiblingIndex = i;
                if (placeholder.transform.GetSiblingIndex() < newSiblingIndex)
                    newSiblingIndex--;
                break;
            }
        }
        placeholder.transform.SetSiblingIndex(newSiblingIndex); //The abillity to swap places with cards in hand
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag");
        this.transform.SetParent(parentReturnTo);
        this.transform.SetSiblingIndex(placeholder.transform.GetSiblingIndex());
        GetComponent<CanvasGroup>().alpha = 1f;         //reset effect for card, can be changed
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        Destroy(placeholder);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {

    }
    public void OnPointerExit(PointerEventData eventData) 
    {

    }
}
