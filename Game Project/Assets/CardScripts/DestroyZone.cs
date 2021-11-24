using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
//simple script just to destroy cards on contact
public class DestroyZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {

    }
    public void OnPointerExit(PointerEventData eventData)
    {

    }
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("Destroying " + gameObject.name);
        DragCard d = eventData.pointerDrag.GetComponent<DragCard>();
        if (d != null)
        {

            Destroy(d.gameObject);
            //destroys the card that was dropped on this zone

        }
    }
}
