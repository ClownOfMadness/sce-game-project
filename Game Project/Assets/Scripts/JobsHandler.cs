using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JobsHandler : MonoBehaviour
{
    public Player_Control PControl; //to access the units
    public Unit_List unit_List; //to access the unit count
    public Screen_Cards screen_Cards; //to close the job menu after selecting a unit
    public GameObject[] Jobs; // an array of available jobs
    int i; //an index for the jobs array
    public Image unit; //for changing the button to the image of the chosen unit
    public Image bg; //for changing the button to the image of the chosen unit's background

    public void CheckUnitList()
    {
        for (int i = 0; i < Jobs.Length; i++)
        {
            if(unit_List.units[i].unitGroup.transform.childCount > 0)
            {
                Jobs[i].SetActive(true);
            }
            else
            {
                Jobs[i].SetActive(false);
            }
        }
    }

    public void SelectUnit(GameObject clicked)
    {
        if (clicked != null)
        {
            for (int i = 0; i < Jobs.Length; i++)
            {
                if (Jobs[i] == clicked)
                {
                    Debug.Log("Selected a unit num " + i);
                    PControl.selectedJob = i;
                    unit.sprite = Jobs[i].transform.GetChild(1).GetComponent<Image>().sprite;
                    bg.color = Jobs[i].GetComponent<Image>().color;
                    screen_Cards.CloseJob();
                }
            }
        }
    }


}
