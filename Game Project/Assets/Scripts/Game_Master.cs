using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//responsible for game states and monitoring the entire game
public class Game_Master : MonoBehaviour
{
    //general:
    [Header("---[Systems]---")]
    public Player_Jobs Jobs;
    public Screen_Cards Cards;
    public System_DayNight Cycle;
    public Map_Gen MapGen;
    public Map_Display MapDisplay;
    public Player_SpawnBuilding Buildings;
    public Map_SpawnControl MapSpawn;
    public Data_CommonDataHolder MapDataHolder;
    public Player_Control Control;
    public Menu_Pause Pause;

    [Header("---[Objects]---")]
    public Unit_List Units;
    public Enemy_List Enemies;

    [HideInInspector] public bool gameLost;

    //parent:
    [HideInInspector] public bool hintsOn;
    [HideInInspector] public bool timeLimitSet;
    [HideInInspector] public float timeLimit;
    private float timeLeft;
    [HideInInspector] public bool bedTimeSet;
    [HideInInspector] public float bedTime;
    private float realTime;

    //premium:
    [HideInInspector] public bool premiumUser;

    //key mapping:
    [HideInInspector] public static KeyCode creativeK;
    [HideInInspector] public static KeyCode storageK;
    [HideInInspector] public static KeyCode Hintsk;
    [HideInInspector] public static KeyCode Jobsk;

    void Awake()
    {
        gameLost = false;

        //giving access to other scripts: (enable whaterver is relevant)
        //Jobs.Game = this;
        Cards.Game = this;
        //Cycle.Game = this;
        //MapGen.Game = this;
        //Control.Game = this;
        //Pause.Game = this;
        //Units.Game = this;
        //Enemies.Game = this;

        //when save file is picked, everything should be loaded from save file
        timeLimit = TimeToFloat("2:00");
        Debug.Log(string.Format("{0:00}:{1:00}", TimeSpan.FromSeconds(timeLimit).Minutes, TimeSpan.FromMilliseconds(timeLimit).Seconds));
        timeLeft = timeLimit;

        //if save file wasnt picked, run new game
        NewGame();
    }
    void Update() //main Update, handles constant checking of parameters for the rest of the game
    {
        //general:
        if(gameLost)
        {
            //this is where a lose screen will be called
        }
        //parent:
        //time limit
        if (timeLimitSet && timeLeft > 0) 
            timeLeft -= Time.deltaTime;
        if (timeLeft < 0)
        {
            //save game and exit
        }
        //bedtime
        if (bedTimeSet) 
        {
            realTime = (float)DateTime.Now.Hour + ((float)DateTime.Now.Minute * 0.01f);
            if (realTime > bedTime)
            {
                //save game and exit
            }
        }

        //premium:
        //keymapping
        if (Input.GetKeyDown(creativeK) && (!Menu_Pause.IsPaused)) //to close and open "Creative" with keyboard as well
        {
            Cards.SwitchCreative();
        }
        else if (Input.GetKeyDown(storageK) && (!Menu_Pause.IsPaused)) //to close and open "Storage" with keyboard as well
        {
            Cards.SwitchStorage();
        }
        else if (Input.GetKeyDown(Hintsk) && (!Menu_Pause.IsPaused)) //to display hints 
        {
            Cards.SwitchHints();
        }
        if (Input.GetKeyDown(Jobsk) && (!Menu_Pause.IsPaused)) //close and open job menu
        {
            Jobs.SwitchJob();
        }
        //move other key listeners here
    }
    public void NewGame()   //run when new game is started
    {
        // the default keycodes -> will later be implemented in saves
        storageK = KeyCode.I;
        creativeK = KeyCode.C;
        Hintsk = KeyCode.H;
        Jobsk = KeyCode.J;

        // defaults for a new game:
        hintsOn = false;
        timeLimitSet = false;
        timeLimit = 0;
        bedTimeSet = false;
        bedTime = 0;
    }
    public float TimeToFloat(string time)
    {
        string[] times = time.Split(":"[0]);

        float minutes = float.Parse(times[0]);
        float seconds = float.Parse(times[1]);

        return minutes * 60 + seconds;
    }
    public Game_Info ExportGame()             //will be used to save the game
    {
        Game_Info export = new Game_Info();
        //fill class
        return export;
    }
    public void ImportGame(Game_Info import)  //will be used to load the game
    {
        //fill script fields
    }
    public class Game_Info //used for saves
    {

    }
}