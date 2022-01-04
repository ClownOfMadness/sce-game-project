using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Data_PerSaveSlot
{
    //--------------------------------[Configuration]------------------------------------

    public int charLook;
    Game_Master.windowList windowLook;
    public bool fogOff;
    //Game_Master.fontList fontSize;

    //22. bedtime:
    public bool bedtimeSet;
    public float bedtime;
    //23. play time limit:
    public bool timeLimitSet;
    public float timeLimit;
    public float timeLeft;
    //27. font:
    //public int fontSize;    //0=Normal, 1=Big
    //28. hints:
    public bool hintsOn;
    //29. game speed:
    public int gameSpeed;   //0=Normal, 1=Slow
    //15+24. enemies:
    public bool enemiesOff;
    //18+30. difficulty:
    public int difficulty;  //0=Normal, 1=Easy, 2=Hardcore

    //-----------------------------------------------------------------------------------


    //-----------------------------------[Statistics]------------------------------------

    public bool isFirstGame; //First game on this save

    //26. game statistics:
    public float TotalGameTime;
    public int CardsCombined;
    public int CardsDiscovered;
    //Achievements
    public float gameDays;
    public int buildings;

    //-----------------------------------------------------------------------------------

    public Data_PerSaveSlot()
    {
        //isFirstGame = true;
        //TotalGameTime = 0;
        //windowLook = (Game_Master.windowList)1;
        //fogOff = false;
        //charLook = 0;
        //bedtimeSet = false;
        //bedtime = 0;
        //timeLimitSet = false;
        //timeLimit = 0;
        //fontSize = Game_Master.fontList.Normal;
        //hintsOn = false;
        //SetGameSpeed(PlayerPrefs.GetInt("GameSpeed")); //Change this to just take the value from the 
                                                       //SetGameSpeed() in Screen_Parent
                                                       //gameSpeed = speedList.Normal;

        //[Premium+Parent]//
        enemiesOff = false;
        //difficulty = difficultyList.Normal;
    }

    //Game update
    public Data_PerSaveSlot(Game_Parent data)
    {
        ////22. bedtime:
        //bedtimeSet = data.bedtimeSet;
        //bedtime = data.bedtime;
        ////23. play time limit:
        //timeLimitSet = data.timeLimitSet;
        //timeLimit = data.timeLimit;
        //timeLeft = data.timeLeft;
        ////26. game statistics:
        //TotalGameTime = data.TotalGameTime;
        //CardsCombined = data.CardsCombined;
        //CardsDiscovered = data.CardsDiscovered;
        ////27. font:
        //fontSize = data.fontSize;
        ////28. hints:
        //hintsOn = data.hintsOn;
        ////29. game speed:
        //gameSpeed = data.gameSpeed;
        ////15+24. enemies:
        //enemiesOff = data.enemiesOff;
        ////18+30. difficulty:
        //difficulty = data.difficulty;
    }
    //Menu update
    public Data_PerSaveSlot(Menu_Main data)
    {
        ////22. bedtime:
        //bedtimeSet = data.bedtimeSet;
        //bedtime = data.bedtime;
        ////23. play time limit:
        //timeLimitSet = data.timeLimitSet;
        //timeLimit = data.timeLimit;
        //timeLeft = data.timeLeft;
        ////26. game statistics:
        //TotalGameTime = data.TotalGameTime;
        //CardsCombined = data.CardsCombined;
        //CardsDiscovered = data.CardsDiscovered;
        ////27. font:
        //fontSize = data.fontSize;
        ////28. hints:
        //hintsOn = data.hintsOn;
        ////29. game speed:
        //gameSpeed = data.gameSpeed;
        ////15+24. enemies:
        //enemiesOff = data.enemiesOff;
        ////18+30. difficulty:
        //difficulty = data.difficulty;
    }
}
