using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//responsible for the card drag command, extension of CardDisplay
public class DragCard : CardDisplay, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Transform parentReturnTo = null;
    public static GameObject placeholder = null;//a placeholder to hold the dragged card's spot
    public Transform placeholderParent = null;

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log ("OnBeginDrag");

        placeholder = new GameObject(); //initalizing the temp object to save the card's spot
        placeholder.transform.SetParent(this.transform.parent);
        LayoutElement le = placeholder.AddComponent<LayoutElement>();
        le.preferredWidth = this.GetComponent<LayoutElement>().preferredWidth;
        le.preferredHeight = this.GetComponent<LayoutElement>().preferredHeight;
        le.flexibleWidth = 0;
        le.flexibleHeight = 0;
        placeholder.transform.SetSiblingIndex(this.transform.GetSiblingIndex());
        //basically makes sure it stays the same size                                                                    

        parentReturnTo = this.transform.parent;
        placeholderParent = parentReturnTo;
        this.transform.SetParent(this.transform.parent.parent); //changes parent once the card is picked
        GetComponent<CanvasGroup>().blocksRaycasts = false;
       
    }
    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("OnDrag");
        this.transform.position = eventData.position;

        if (placeholder.transform.parent!=placeholderParent) {
            placeholder.transform.parent.SetParent(placeholderParent);
        }
        int newSiblingIndex = placeholderParent.childCount;    //a workaround to make it also work from right to left

        for(int i = 0; i < placeholderParent.childCount; i++) //loops to move the placeholder to the left
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
        this.transform.SetSiblingIndex(placeholder.transform.GetSiblingIndex()); //
        GetComponent<CanvasGroup>().blocksRaycasts = true;

        Destroy(placeholder);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        //effect for when mouse is on card
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        //go back to normal effect
    }
}
