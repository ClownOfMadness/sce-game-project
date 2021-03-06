using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Custom_ParentalSettings : MonoBehaviour
{
    public Menu_Main menu;
    //public Game_Master master;
    public Dropdown DropGS; //for the gameSpeed dropdown
    public Dropdown DropD; //difficulty dropdown
    public Dropdown EnemiesDrop; //enemies on off drop

    public void Start()
    {
        DropGS.value = menu.gameSpeed;

        if (menu.enemiesOff)
        {
            EnemiesDrop.value = 1;
        }
        else
        {
            EnemiesDrop.value = 0;
        }

        if (menu.difficulty==0 || menu.difficulty == 2)
        {
            DropD.value = 0;
        }
        else
        {
            DropD.value = 1;
        }
       
    }

    //-------------------Parent's Custom Settings-------------------------------
    public void SetGameSpeed()    //set game speed per save (0 - default, 1 - slow)
    {
        if (DropGS.value == 1)
        {
            menu.gameSpeed = 1;
        }
        else
        {
            menu.gameSpeed = 0;
        }
    }

    public void ParentSetDifficulty()
    {
        if (DropD.value == 1)
        {
            menu.difficulty = 1;
        }
        else
        {
            menu.difficulty = 0;
        }

    }

    public void SetEnemiesStatus()
    {
        if (EnemiesDrop.value == 1)
        {
            menu.enemiesOff = true;
        }
        else
        {
            menu.enemiesOff = false;
        }
    }

    //-------------------Premium's Custom Settings-------------------------------


}
