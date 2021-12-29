using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//stores statistics and parent configuration on a save
[System.Serializable]
public class Data_Parent
{
    //--------------------------------[Configuration]------------------------------------

    //22. bedtime:
    public bool bedtimeSet;
    public float bedtime;
    //23. play time limit:
    public bool timeLimitSet;
    public float timeLimit;
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

    //--------------------------------[Statistics]------------------------------------

    //26. game statistics:
    public float TotalGameTime;
    public int CardsCombined;
    public int CardsDiscovered;

    public Data_Parent(Game_Parent data)
    {
        //22. bedtime:
        bedtimeSet = data.bedtimeSet;
        bedtime = data.bedtime;
        //23. play time limit:
        timeLimitSet = data.timeLimitSet;
        timeLimit = data.timeLimit;
        //26. game statistics:
        TotalGameTime = data.TotalGameTime;
        CardsCombined = data.CardsCombined;
        CardsDiscovered = data.CardsDiscovered;
        //27. font:
        fontSize = data.fontSize;
        //28. hints:
        hintsOn = data.hintsOn;
        //29. game speed:
        gameSpeed = data.gameSpeed;
        //15+24. enemies:
        enemiesOff = data.enemiesOff;
        //18+30. difficulty:
        difficulty = data.difficulty;
    }
}

