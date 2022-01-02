using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Data_PlayerConfig
{
    //--------------------------------[Configuration]------------------------------------

    public bool isFirstGame = true; //First game on this save

    //[Premium]//
    //12. main character appearance:
    //public Game_Master.windowList windowLook; //needs defenition
    //14. fog:
    public bool fogOff = false;
    //17. main character appearance: 
    //public Game_Master.charList charLook; //needs defenition

    //[Parent]//
    //22. bedtime:
    private bool bedtimeSet = false;
    private float bedtime = 0f;
    private float realTime; //nead to save?
    //23. play time limit:
    public bool timeLimitSet = false;
    public float timeLimit = 0f;
    public float timeLeft = 0f;
    //27. font:
    public Game_Master.fontList fontSize = Game_Master.fontList.Normal;
    //28. hints:
    public bool hintsOn = false;
    //29. game speed:
    public Game_Master.speedList gameSpeed = Game_Master.speedList.Normal;

    //[Premium+Parent]//
    //15+24. enemies:
    public bool enemiesOff = false;
    //18+30. difficulty:
    //public int difficulty = Game_Master.difficultyList.Normal;
    //20. key mapping:
    //public static KeyCode creativeK;
    //public static KeyCode storageK;
    //public static KeyCode Hintsk;
    //public static KeyCode Jobsk;

    //-----------------------------------------------------------------------------------

    //-----------------------------------[Statistics]------------------------------------

    public float totalGameTime;
    public float gameDays;
    public int buildings;
    public int cardsFound;

    //-----------------------------------------------------------------------------------

}
