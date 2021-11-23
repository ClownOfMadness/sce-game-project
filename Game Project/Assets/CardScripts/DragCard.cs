using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
 //Responsible for the drag command
public class DragCard : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Transform parentReturnTo = null;
    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log ("OnBeginDrag");
        parentReturnTo = this.transform.parent;
        this.transform.SetParent(this.transform.root);
        //GetComponent<CanvasGroup>().blocksRaycasts = false; ->need to find a way to fix this
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
        //GetComponent<CanvasGroup>().blocksRaycasts = true;
    }
}
