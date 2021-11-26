using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//simple script just to destroy cards on contact
public class ZoneDestroy : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    private static GameObject placeholder;

    public static GameObject Placeholder { get => placeholder; set => placeholder = value; }

    public void OnPointerEnter(PointerEventData eventData)
    {

    }
    public void OnPointerExit(PointerEventData eventData)
    {

    }
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("Destroying " + gameObject.name);
        CardDrag d = eventData.pointerDrag.GetComponent<CardDrag>();
        // GameObject placeholder = GameObject.Find("placeholder"); //
        // Placeholder = DragCard.placeholder;
        if (d != null)
        {
            Destroy(d.gameObject);

            //destroys the card that was dropped on this zone
            // Destroy(Placeholder.gameObject);
            //make sure to also destroy the placeholder
        }
    }
}
