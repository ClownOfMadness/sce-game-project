using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Data_PerSaveSlot
{
    //--------------------------------[configuration]-------------------------------------//

    public int charLook;
    public bool bedtimeSet;
    public float bedtime;
    public bool timeLimitSet;
    public float timeLimit;
    public float timeLeft;
    public int fontSize;
    public bool hintsOn;
    public int gameSpeed;
    public bool enemiesOff;
    public int difficulty;
    public bool fogOff;

    //-----------------------------------------------------------------------------------//


    //-----------------------------------[Statistics]------------------------------------//

    public bool isFirstGame; //First game on this save

    public float TotalGameTime;
    public int CardsCombined;
    public int CardsDiscovered;
    //Achievements
    public float gameDays;
    public int buildingsCount;

    //-----------------------------------------------------------------------------------

    public Data_PerSaveSlot()
    { 
        charLook = 0;
        bedtimeSet = false;
        bedtime = 0;
        timeLimitSet = false;
        timeLimit = 0;
        timeLeft = 0;
        fontSize = 0;
        hintsOn = false;
        gameSpeed = 0;
        enemiesOff = false;
        difficulty = 0;
        fogOff = false;

        isFirstGame = true;
        TotalGameTime = 0;
        CardsCombined = 0;
        CardsDiscovered = 0;
        gameDays = 0;
        buildingsCount = 0;
    }
    public Data_PerSaveSlot(Data_PerSaveSlot old)
    {
        charLook = 0;
        bedtimeSet = false;
        bedtime = 0;
        timeLimitSet = false;
        timeLimit = 0;
        timeLeft = 0;
        fontSize = 0;
        hintsOn = false;
        gameSpeed = 0;
        enemiesOff = false;
        difficulty = 0;
        fogOff = false;

        isFirstGame = true;
        TotalGameTime = 0;
        CardsCombined = 0;
        CardsDiscovered = 0;
        gameDays = 0;
        buildingsCount = 0;
    }
}
