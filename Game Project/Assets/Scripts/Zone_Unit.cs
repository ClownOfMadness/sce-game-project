using System.Collections.Generic;
using UnityEngine;

//responsible for creating Unit prompt zone
public class Zone_Unit : MonoBehaviour
{
    //public fields:
    [HideInInspector] public int Size;          //Zone size
    [HideInInspector] public bool hasBacklog;   //used to avoid it clashing with the Craft Menu screen

    //internal fields:
    private class Backlog  //organizing the cards and units still waiting to be sorted out
    {
        public Data_Card card;
        public Data_Unit waiting;
    }
    private List<Backlog> queue;
    private Backlog current;

    //external access:
    [HideInInspector] public Screen_Cards screen;

    public void InstantiateZone()
    {
        Size = 1;   //max Zone size

        queue = new List<Backlog>();
        current = null;
        hasBacklog = false;
    }
    public void CardAdded(Data_Card pickedCard, Data_Unit unit)   //card was added to Zone
    {
        queue.Add(new Backlog { card = pickedCard, waiting = unit });
        hasBacklog = true;
    } 
    public void FreeUnit()      //let the Unit know that it can go back to work
    {
        if (current.waiting)
        {
            current.waiting.card = null;
        }
        queue.Remove(current);
        RefreshZone();
    }
    public void RefreshZone()   //display the next card in the queue
    {
        if (queue.Count > 0)     //failsafe - verifying that there's no objects displayed in the Zone
        {
            if (current != queue[0]) //only refresh displayed card if the current one is outdated
            {
                current = queue[0];
                screen.CreateObject(this.transform, current.card); //create and instantiate object in zone
                this.gameObject.SetActive(true);
            }
        }
        else
        {
            screen.UnitUp = false;
            screen.visibleMap = true;
            this.gameObject.SetActive(false);
            hasBacklog = false;
        }
    }
}


