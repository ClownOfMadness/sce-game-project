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
        //if save file wasnt picked, run new game
        NewGame();

        //[testing time limit functionality]
        //timeLimit = TimeToFloat("00:01:10");
        //Debug.Log(FloatToTime(timeLimit));   
        //timeLeft = timeLimit;
        //timeLimitSet = true;
        //[testing time limit functionality]
    }
    void Update() //main Update, handles constant checking of parameters for the rest of the game
    {
        //[General]
        if(gameLost)
        {
            //this is where a lose screen will be called
        }
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

        //[Parent]
        //time limit
        if (timeLimitSet && timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
        }
        if (timeLimitSet && timeLeft <= 0)
        {
            Debug.Log(string.Format("{0} ran out", FloatToTime(timeLimit)));
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

        //[Premium]
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
    public float TimeToFloat(string time)   //converts time in mm:ss format into float
    {
        string[] times = time.Split(':');

        float hours = float.Parse(times[0]);
        float minutes = float.Parse(times[1]);
        float seconds = float.Parse(times[2]);
        return (hours * 60 + minutes) * 60 + seconds;
    }
    public string FloatToTime(float time)   //converts time floats into a string
    {
        double hours = Math.Floor(time / (60 * 60));
        double minutes = Math.Floor((time - hours * 60) / 60);
        double seconds = time - minutes * 60;
        return string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
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