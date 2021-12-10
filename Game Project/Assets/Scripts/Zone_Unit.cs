using System.Collections.Generic;
using UnityEngine;

//responsible for creating Unit prompt zone, extension of ZoneBehaviour
public class Zone_Unit : Zone_Behaviour
{
    private class Backlog  //organizing the cards and units still waiting to be sorted out
    {
        public Data_Card card;
        public Data_Unit waiting;
    }
    private List<Backlog> queue;
    private Backlog current;

    void Awake()
    {
        Size = 1;               //max Zone size
        queue = new List<Backlog>();
        current = null;
    }
    public void CardAdded(Data_Card pickedCard, Data_Unit unit)
    {
        queue.Add(new Backlog { card = pickedCard, waiting = unit });
        Debug.Log("Backlog: " + queue.Count);
        RefreshZone();
    } 
    public void FreeUnit()
    {
        if (current.waiting)
        {
            current.waiting.card = null;
        }
        queue.Remove(current);
        RefreshZone();
    }
    public void RefreshZone()
    {
        if (queue.Count > 0)     //failsafe - verifying that there's no objects displayed in the Zone
        {
            if (current != queue[0]) //only refresh displayed card if the current one is outdated
            {
                current = queue[0];

                GameObject newCard = Instantiate(this.CardPrefab, this.transform);     //create and instantiate object in scene
                newCard.GetComponent<Card_Drag>().AddCard(current.card);               //add cards to objects 
                newCard.name = string.Format("{0}", current.card.name);                //update new card name (for displaying in Scene)
                newCard.gameObject.transform.SetParent(this.transform);
                newCard.SetActive(true);
            }
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }
}


