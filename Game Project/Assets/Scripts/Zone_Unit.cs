using System.Collections.Generic;
using UnityEngine;


//responsible for creating Unit prompt zone, extension of ZoneBehaviour
public class Zone_Unit : Zone_Behaviour
{
    public List<Data_Unit> queue;

    void Awake()
    {
        Size = 1;               //max Zone size
        queue = new List<Data_Unit>();
    }
    public void CardAdded(Data_Unit unit)
    {
        queue.Add(unit);
        RefreshZone();
    }
    public void FreeUnit()
    {
        Data_Unit unit = queue[0];
        unit.card = null;
        queue.Remove(unit);
        RefreshZone();
    }
    public void RefreshZone()
    {
        if (this.transform.childCount >= this.Size)     //failsafe - verifying that there's two objects in the Zone
        {
            GameObject card = this.transform.GetChild(0).gameObject;
            card.SetActive(true);
            this.gameObject.SetActive(true);
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }
}


