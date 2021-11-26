using UnityEngine;
using UnityEngine.EventSystems;

//responsible for the card drag command, extension of CardDisplay
public class CardDrag : CardDisplay, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Transform parentReturnTo = null;
    public Transform oldParent;
    public void ReturnToHand()  //return to hand after craft attempt
    {
        Debug.Log("Trying to change Zone");
        this.transform.SetParent(this.oldParent);
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        oldParent = this.transform.parent;
        Debug.Log("OldParent: " + oldParent.name);
        Debug.Log("OnBeginDrag");
        parentReturnTo = this.transform.parent;
        this.transform.SetParent(this.transform.parent.parent); //changes parent once the card is picked
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }
    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("OnDrag");
        this.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag");
        this.transform.SetParent(parentReturnTo);
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }
    public void OnPointerEnter(PointerEventData eventData)      //effect for when mouse is on card
    {

    }
    public void OnPointerExit(PointerEventData eventData)       //go back to normal effect
    {

    }
}
