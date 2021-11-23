using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
//responsible for creating a zone to drop the cards to
public class DropZone1 : MonoBehaviour,IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        
    }
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDrop to" + gameObject.name);
        DragCard d = eventData.pointerDrag.GetComponent<DragCard>();
        if (d != null)
        {
            d.parentReturnTo = this.transform;
        }
    }
}
