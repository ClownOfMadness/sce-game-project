using System;
using UnityEngine;
using UnityEngine.UI;

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
    public GameObject gameOver;

    [Header("---[Objects]---")]
    public Unit_List Units;
    public Enemy_List Enemies;

    [Header("---[Text Objects]---")]
    public Text TopMessage; //message text
    public Text PeasantT; //jobs peasant 
    public Text WoodcutterT; //jobs woodcutter
    public Text SpearmanT; //jobs spearman
    //Later add here all the other jobs text objects
    public Text Desc; //cards' descriptions
    public TMPro.TextMeshProUGUI resume; //options menu -> resume
    public TMPro.TextMeshProUGUI save; //pause menu -> save
    public TMPro.TextMeshProUGUI load; //pause menu -> load
    public TMPro.TextMeshProUGUI optionsP; //pause menu -> options
    public TMPro.TextMeshProUGUI exit; //pause menu -> exit to menu
    public TMPro.TextMeshProUGUI back1; //pause menu -> back
    public TMPro.TextMeshProUGUI play; //game menu -> play
    public TMPro.TextMeshProUGUI nameInputP; //game menu -> nameInput Placeholder
    public TMPro.TextMeshProUGUI nameInputT; //game menu -> nameInput Text
    public TMPro.TextMeshProUGUI delete; //game menu -> delete
    public TMPro.TextMeshProUGUI optionsM; //options menu -> options
    public TMPro.TextMeshProUGUI volume; //options menu -> volume
    public TMPro.TextMeshProUGUI resolution; //options menu -> resolution
    public Text resolutionL; //options menu -> resolution label
    public TMPro.TextMeshProUGUI keybinding; //options menu -> keybinding
    public TMPro.TextMeshProUGUI back2; //options menu -> back
    public TMPro.TextMeshProUGUI KeybindingMenu; //keybinding menu
    public Text CreativeKeyT; //keybinding -> creative
    public Text StorageKeyT; //keybinding -> storage
    public Text HintsKeyT; //keybinding -> Hints
    public Text JobsKeyT; //keybinding -> jobs
    public Text MoveUpKeyT; //keybinding -> moveUp
    public Text MoveDownKeyT; //keybinding -> moveDown
    public Text MoveRightKeyT; //keybinding -> moveRight
    public Text MoveLeftKeyT; //keybinding -> moveLeft
    public Text SprintKeyT; //keybinding -> sprint
    public Text CKeyT; //keybinding -> C text
    public Text IKeyT; //keybinding -> S text
    public Text HKeyT; //keybinding -> H text
    public Text JKeyT; //keybinding -> J text
    public Text WUpKeyT; //keybinding -> W text
    public Text SDownKeyT; //keybinding -> S text
    public Text DRightKeyT; //keybinding -> D text
    public Text ALeftKeyT; //keybinding -> A text
    public Text LShiftKeyT; //keybinding -> LeftShift text
    public TMPro.TextMeshProUGUI back3; //keybinding -> back
    public Text ErrorMessage; //keybindings ErrorMessage
    //------------ End of Text Objects---------------

    [HideInInspector] public float totalGameTime;
    [HideInInspector] public bool isFirst;

    [System.Serializable]
    public enum fontList        //used by fontSize
    {
        Normal, //default
        Big,
    }
    [System.Serializable]
    public enum speedList       //used by gameSpeed
    {
        Normal, //default
        Slow,
    }     
    
    [System.Serializable]
    public enum windowList      //used by window appearance (remove if not needed)
    {
        //fill if needed
    }
    

    //[Premium]//
    [HideInInspector] public bool premiumUser;
    //12. main character appearance:
    [HideInInspector] public windowList windowLook;
    //14. fog:
    [HideInInspector] public bool fogOff;
    //17. main character appearance:
    [HideInInspector] public int charLook;

    //[Parent]//
    //22. bedtime:
    private bool bedtimeSet;
    private float bedtime;
    private float realTime;
    //23. play time limit:
    private bool timeLimitSet;
    private float timeLimit;
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
    [HideInInspector] public int difficulty = 1;
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
        Control.game = this;
        //Pause.Game = this;
        //Units.Game = this;
        //Enemies.Game = this;

        //set default
        NewGame();

        //when save file is picked, everything should be loaded from save file
        //[Premium]//
        if (PlayerPrefs.GetInt("premium", 0) == 1)
            premiumUser = true;
        else
            premiumUser = false;

        //[testing bedtime functionality]//
        //bedtime = TimeToFloat("22:17");
        //bedtimeSet = true;
        //[testing bedtime functionality]

        //[testing time limit functionality]//
        //timeLimit = TimeToFloat("00:01:10");
        //Debug.Log(FloatToTime(timeLimit));
        //timeLimitSet = true;
        //[testing time limit functionality]

        if (timeLimitSet)
        {
            //string currentDate = DateTime.Today.Date.ToString("d");
            //if ([dateOfLastSave]!=currentDate)
                //timeLeft = timeLimit;
        }
        SetFontSize(PlayerPrefs.GetInt("ChangeFont")); //sets the correct font size
    }
    void Update() //main Update, handles constant checking of parameters for the rest of the game
    {
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
        if (timeLimitSet)
        {
            if (timeLeft > 0)
            {
                timeLeft -= Time.deltaTime;
            }
            if (timeLeft <= 0)
            {
                Debug.Log(string.Format("{0} ran out", FloatToTime(timeLimit)));
                //save game and exit
            }
        }
    }
    public void NewGame()   //run when new game is started
    {
        // Keycodes:each one draws a value from playerprefs and has a default value for backup
        creativeK = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Creative", "C"));
        storageK = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Storage", "I"));
        Hintsk = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Hints", "H"));
        Jobsk = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Jobs", "J"));

        //defaults for a new game:
        //[General]//
        totalGameTime = 0;

        //[Premium]//
        premiumUser = false;
        windowLook = (windowList)1;
        fogOff = false;
        //charLook = (charList)1;

        //[Parent]//
        bedtimeSet = false;
        bedtime = 0;
        timeLimitSet = false;
        timeLimit = 0;
        fontSize = fontList.Normal;
        hintsOn = false;
        SetGameSpeed(PlayerPrefs.GetInt("GameSpeed")); //Change this to just take the value from the 
                                                       //SetGameSpeed() in Screen_Parent
        //gameSpeed = speedList.Normal;

        //[Premium+Parent]//
        enemiesOff = false;
        //difficulty = difficultyList.Normal;
    }

    public void GameLost()
    {
        Debug.Log("you're dead");
        gameOver.SetActive(true);
    }

    public void SetFontSize(int c) //recieves the 0 or 1 from the save and loads the corresponding font
    {
        if (c == 1)  //1 for big
        {
            LoadBigFont(); 
        }
        else
            if(c==0)//0 for default
            {
                LoadDefaultFont();
            }
    }

    public void SetGameSpeed(int c) //in construction atm
    {
        if (c == 0)  //0 for default
        {
            Cycle.cycleSpeed = 1f;
        }
        else
            if (c == 1) //1 for slow
        {
            Cycle.cycleSpeed = 0.5f;
        }
    }
    public void LoadDefaultFont() //sets all text objects to their default size
    {
        TopMessage.fontSize = 40;
        PeasantT.fontSize = 20;
        WoodcutterT.fontSize = 18;
        SpearmanT.fontSize = 19;
        Desc.fontSize = 18;
        resume.fontSize = 80;
        save.fontSize = 80;
        load.fontSize = 80;
        optionsP.fontSize = 80;
        exit.fontSize = 80;
        back1.fontSize = 60;
        play.fontSize = 20;
        nameInputT.fontSize = 14;
        nameInputP.fontSize = 14;
        delete.fontSize = 20;
        optionsM.fontSize = 80;
        volume.fontSize = 60;
        resolution.fontSize = 60;
        resolutionL.fontSize = 32;
        keybinding.fontSize = 60;
        back2.fontSize = 60;
        KeybindingMenu.fontSize = 80;
        CreativeKeyT.fontSize = 25;
        StorageKeyT.fontSize = 25;
        HintsKeyT.fontSize = 25;
        JobsKeyT.fontSize = 25;
        MoveUpKeyT.fontSize = 25;
        MoveDownKeyT.fontSize = 25;
        MoveRightKeyT.fontSize = 25;
        MoveLeftKeyT.fontSize = 25;
        SprintKeyT.fontSize = 25;
        CKeyT.fontSize = 18;
        IKeyT.fontSize = 18;
        HKeyT.fontSize = 18;
        JKeyT.fontSize = 18;
        WUpKeyT.fontSize = 18;
        SDownKeyT.fontSize = 18;
        DRightKeyT.fontSize = 18;
        ALeftKeyT.fontSize = 18;
        LShiftKeyT.fontSize = 18;
        back3.fontSize = 60;
        ErrorMessage.fontSize = 40;

    }
    public void LoadBigFont() //sets all text objects to their big size
    {
        TopMessage.fontSize = 50;
        PeasantT.fontSize = 25;
        WoodcutterT.fontSize = 21;
        SpearmanT.fontSize = 22;
        Desc.fontSize = 23;
        resume.fontSize = 90;
        save.fontSize = 90;
        load.fontSize = 90;
        optionsP.fontSize = 90;
        exit.fontSize = 90;
        back1.fontSize = 70;
        play.fontSize = 30;
        nameInputT.fontSize = 20;
        nameInputP.fontSize = 20;
        delete.fontSize = 30;
        optionsM.fontSize = 90;
        volume.fontSize = 80;
        resolution.fontSize = 70;
        resolutionL.fontSize = 45;
        keybinding.fontSize = 80;
        back2.fontSize = 80;
        KeybindingMenu.fontSize = 90;
        CreativeKeyT.fontSize = 28;
        StorageKeyT.fontSize = 28;
        HintsKeyT.fontSize = 28;
        JobsKeyT.fontSize = 28;
        MoveUpKeyT.fontSize = 28;
        MoveDownKeyT.fontSize = 28;
        MoveRightKeyT.fontSize = 28;
        MoveLeftKeyT.fontSize = 28;
        SprintKeyT.fontSize = 28;
        CKeyT.fontSize = 25;
        IKeyT.fontSize = 25;
        HKeyT.fontSize = 25;
        JKeyT.fontSize = 25;
        WUpKeyT.fontSize = 25;
        SDownKeyT.fontSize = 25;
        DRightKeyT.fontSize = 25;
        ALeftKeyT.fontSize = 25;
        LShiftKeyT.fontSize = 25;
        back3.fontSize = 70;
        ErrorMessage.fontSize = 55;
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

    //[General+Parent]//
    public Game_Parent ExportGame()             //will be used to save the game
    {
        Game_Parent export = new Game_Parent();
        //22. bedtime:
        export.bedtimeSet = bedtimeSet;
        export.bedtime = bedtime;
        //23. play time limit:
        export.timeLimitSet = timeLimitSet;
        export.timeLimit = timeLimit;
        export.timeLeft = timeLeft;
        //26. game statistics:
        export.TotalGameTime = totalGameTime;
        export.CardsCombined = Cards.CardsCombined;
        export.CardsDiscovered = Cards.CardsDiscovered;
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
    public void ImportGame(Game_Parent import)  //will be used to load the game
    {
        //22. bedtime:
        bedtimeSet = import.bedtimeSet;
        bedtime = import.bedtime;
        //23. play time limit:
        timeLimitSet = import.timeLimitSet;
        timeLimit = import.timeLimit;
        timeLeft = import.timeLeft;
        //26. game statistics:
        totalGameTime = import.TotalGameTime;
        Cards.CardsCombined = import.CardsCombined;
        Cards.CardsDiscovered = import.CardsDiscovered;
        //27. font:
        fontSize = (fontList)import.fontSize;
        //28. hints:
        hintsOn = import.hintsOn;
        //29. game speed:
        gameSpeed = (speedList)import.gameSpeed;
        //15+24. enemies:
        enemiesOff = import.enemiesOff;
        //18+30. difficulty:
        //difficulty = (difficultyList)import.difficulty;
    }
}
public class Game_Parent
{
    //22. bedtime:
    public bool bedtimeSet;
    public float bedtime;
    //23. play time limit:
    public bool timeLimitSet;
    public float timeLimit;
    public float timeLeft;
    //26. game statistics:
    public float TotalGameTime;
    public int CardsCombined;
    public int CardsDiscovered;
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
