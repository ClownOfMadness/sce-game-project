using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//responsible for game states and monitoring the entire game
public class Game_Master : MonoBehaviour
{
    //[General]//
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

    [HideInInspector] public float totalGameTime;
    [HideInInspector] public bool gameLost;

    public enum fontList        //used by fontSize
    {
        Normal,
        Big,
    }
    public enum speedList       //used by gameSpeed
    {
        Normal,
        Slow,
    }
    public enum charList        //used by character appearance (remove if not needed)
    {
       //fill if needed
    }
    public enum windowList      //used by window appearance (remove if not needed)
    {
        //fill if needed
    }
    public enum difficultyList  //used by difficulty, both by parent and premium
    {
        Normal,
        Easy,       //for parent
        Hardcore    //for premium, make sure that it doesn't clash with Easy
    }

    //[Premium]//
    [HideInInspector] public bool premiumUser;
    //12. main character appearance:
    [HideInInspector] public windowList windowLook;
    //14. fog:
    [HideInInspector] public bool fogOff;
    //17. main character appearance:
    [HideInInspector] public charList charLook;

    //[Parent]//
    //22. bedtime:
    [HideInInspector] public bool bedtimeSet;
    [HideInInspector] public float bedtime;
    private float realTime;
    //23. play time limit:
    [HideInInspector] public bool timeLimitSet;
    [HideInInspector] public float timeLimit;
    private float timeLeft;
    //27. font:
    [HideInInspector] public fontList fontSize;
    //28. hints:
    [HideInInspector] public bool hintsOn;
    //29. game speed:
    [HideInInspector] public speedList gameSpeed;

    //[Premium+Parent]//
    //15+24. enemies:
    [HideInInspector] public bool enemiesOff;
    //18+30. difficulty:
    [HideInInspector] public difficultyList difficulty;
    //20. key mapping:
    [HideInInspector] public static KeyCode creativeK;
    [HideInInspector] public static KeyCode storageK;
    [HideInInspector] public static KeyCode Hintsk;
    [HideInInspector] public static KeyCode Jobsk;

    void Awake()
    {
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

        //[testing bedtime functionality]//
        //bedtime = TimeToFloat("22:17");
        //bedtimeSet = true;
        //[testing bedtime functionality]

        //[testing time limit functionality]//
        //timeLimit = TimeToFloat("00:01:10");
        //Debug.Log(FloatToTime(timeLimit));   
        //timeLeft = timeLimit;
        //timeLimitSet = true;
        //[testing time limit functionality]
    }
    void Update() //main Update, handles constant checking of parameters for the rest of the game
    {
        //[General]//
        if(gameLost)
        {
            //this is where a lose screen will be called
        }
        //keylisteners
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
        else if (Input.GetKeyDown(Jobsk) && (!Menu_Pause.IsPaused)) //close and open job menu
        {
            Jobs.SwitchJob();
        }
        //move other key listeners here


        //[Premium]//
        //

        //[Parent]//
        //22. bedtime:
        if (bedtimeSet)
        {
            realTime = (TimeToFloat(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second));
            if (realTime > bedtime)
            {
                Debug.Log("Reached bedtime");
                //save game and exit
            }
        }
        //23. time limit:
        if (timeLimitSet && timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
        }
        if (timeLimitSet && timeLeft <= 0)
        {
            Debug.Log(string.Format("{0} ran out", FloatToTime(timeLimit)));
            //save game and exit
        }
    }
    public void NewGame()   //run when new game is started
    {
        // the default keycodes -> will later be implemented in saves
        storageK = KeyCode.I;
        creativeK = KeyCode.C;
        Hintsk = KeyCode.H;
        Jobsk = KeyCode.J;

        //defaults for a new game:
        //[General]//
        totalGameTime = 0;
        gameLost = false;

        //[Premium]//
        premiumUser = false;
        windowLook = (windowList)1;
        fogOff = false;
        charLook = (charList)1;

        //[Parent]//
        bedtimeSet = false;
        bedtime = 0;
        timeLimitSet = false;
        timeLimit = 0;
        fontSize = fontList.Normal;
        hintsOn = false;
        gameSpeed = speedList.Normal;

        //[Premium+Parent]//
        enemiesOff = false;
        difficulty = difficultyList.Normal;
    }
    public float TimeToFloat(string time)   //converts time in hh:mm:ss or hh:mm format into float
    {
        string[] times = time.Split(':');
        float hours = float.Parse(times[0]);
        float minutes = float.Parse(times[1]);
        float seconds = 0;
        if (times.Length > 2) 
        {
            seconds = float.Parse(times[2]);
        }
        return (hours * 60 + minutes) * 60 + seconds;
    }
    public float TimeToFloat(int hours, int minutes, int seconds)   //converts time in hh:mm:ss format into float
    {
        return (hours * 60 + minutes) * 60 + seconds;
    }
    public string FloatToTime(float time)   //converts time floats into a string
    {
        double hours = Math.Floor(time / (60 * 60));
        double minutes = Math.Floor((time - hours * 60 * 60) / 60);
        double seconds = time - hours * 60 * 60 - minutes * 60;
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

    //[Parent]//
    public Game_Config ExportConfig()             //will be used to save the game
    {
        Game_Config export = new Game_Config();
        //22. bedtime:
        export.bedtimeSet = bedtimeSet;
        export.bedtime = bedtime;
        //23. play time limit:
        export.timeLimitSet = timeLimitSet;
        export.timeLimit = timeLimit;
        //26. game statistics:
        export.TotalGameTime = totalGameTime;
        //27. font:
        export.fontSize = (int)fontSize;
        //28. hints:
        export.hintsOn=hintsOn;
        //29. game speed:
        export.gameSpeed = (int)gameSpeed;
        //15+24. enemies:
        export.enemiesOff = enemiesOff;
        //18+30. difficulty:
        export.difficulty = (int)difficulty;
        return export;
    }
    public void ImportConfig(Game_Config import)  //will be used to load the game
    {
        //22. bedtime:
        bedtimeSet = import.bedtimeSet;
        bedtime = import.bedtime;
        //23. play time limit:
        timeLimitSet = import.timeLimitSet;
        timeLimit = import.timeLimit;
        //26. game statistics:
        totalGameTime = import.TotalGameTime;
        //27. font:
        fontSize = (fontList)import.fontSize;
        //28. hints:
        hintsOn = import.hintsOn;
        //29. game speed:
        gameSpeed = (speedList)import.gameSpeed;
        //15+24. enemies:
        enemiesOff = import.enemiesOff;
        //18+30. difficulty:
        difficulty = (difficultyList)import.difficulty;
    }
}
public class Game_Config
{
    //22. bedtime:
    public bool bedtimeSet;
    public float bedtime;
    //23. play time limit:
    public bool timeLimitSet;
    public float timeLimit;
    //26. game statistics:
    public float TotalGameTime;
    //27. font:
    public int fontSize;    //0=Normal, 1=Big
    //28. hints:
    public bool hintsOn;
    //29. game speed:
    public int gameSpeed;   //0=Normal, 1=Slow
    //15+24. enemies:
    public bool enemiesOff;
    //18+30. difficulty:
    public int difficulty;  //0=Normal, 1=Easy, 2=Hardcore
}