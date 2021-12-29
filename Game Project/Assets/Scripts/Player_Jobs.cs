using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Jobs : MonoBehaviour
{
    public GameObject Job; //
    public GameObject JobButton; //
    private JobsHandler zJob; //
    [HideInInspector] public bool JobUp;
    [HideInInspector] public JobsHandler jobsHandler;

    //external access:
    public Screen_Cards screen;


    private void Awake()
    {
        //keydown part is handled by Game_Master.NewGame()
    }

    private void Update()
    {
        //keydown part is handled by Game_Master.Update()
    }
    public void SwitchJob() //
    {
        if (screen.visibleMap)
        {
            OpenJob();
        }
        else
        {
            CloseJob();
        }
    }
    private void OpenJob() //
    {
        screen.visibleMap = false;
        Job.SetActive(true);
        JobUp = true;
        jobsHandler.CheckUnitList();
        Time.timeScale = 0f;
    }
    public void CloseJob() //
    {
        screen.visibleMap = true;
        Job.SetActive(false);
        JobUp = false;
        Time.timeScale = 1f;    //unpause game
    }
}
