using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//responsible for game states and monitoring the entire game
public class Game_Master : MonoBehaviour
{
    //general:
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

    void Awake()
    {
        //when save file is picked, everything should be loaded from save file
        timeLeft = timeLimit;
    }

    public void NewGame()   //run when new game is started
    {
        hintsOn = false;
        timeLimitSet = false;
        timeLimit = 0;
        bedTimeSet = false;
        bedTime = 0;
    }

    void Update()
    {
        if (timeLimitSet && timeLeft > 0)
            timeLeft -= Time.deltaTime;
        if (timeLeft < 0)
        {
            //save game and exit
        }
        if (bedTimeSet) {
            realTime = (float)DateTime.Now.Hour + ((float)DateTime.Now.Minute * 0.01f);
            if (realTime > bedTime)
            {
                //save game and exit
            }

        }
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
