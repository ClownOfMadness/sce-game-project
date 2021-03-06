using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Jobs : MonoBehaviour
{
    public GameObject Job; //the game object to setActive if the corresponding button should appear or not
    public GameObject JobButton; //the reference to the button
    [HideInInspector] public bool JobUp; //updating if the the unit with the specific job exists on the map
    public JobsHandler jobsHandler; //reference to the jobs handler object (held by "Job")
    bool OpenJ=false; //if the jobs menu is open

    //external access:
    public Screen_Cards screen; //to check if the map is visible for the switch job function

    public void SwitchJob() //responsible for opening or closing the jobs menu
    {
        if (screen.visibleMap && !OpenJ)
        {
            OpenJob();
        }
    else
        if(OpenJ)
        {
            CloseJob();
        }
    }
    private void OpenJob() //opening the jobs menu
    {
        screen.visibleMap = false;
        Job.SetActive(true);
        JobUp = true;
        OpenJ = true;
        jobsHandler.CheckUnitList();
        Time.timeScale = 0f; //pause the game
    }
    public void CloseJob() //closing the jobs menu
    {
        screen.visibleMap = true;
        Job.SetActive(false);
        JobUp = false;
        OpenJ = false;
        Time.timeScale = 1f;    //unpause game
    }
}
