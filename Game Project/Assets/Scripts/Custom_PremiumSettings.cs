using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Custom_PremiumSettings : MonoBehaviour
{
    public Menu_Main menu;
    //public Game_Master master;
    public Dropdown DropD; //difficulty dropdown
    public Dropdown EnemiesDrop; //enemies on off drop
    public Dropdown DropLook;
    public Dropdown DropMap;

    public void Start()
    {
        if (menu.enemiesOff)
        {
            EnemiesDrop.value = 1;
        }
        else
        {
            EnemiesDrop.value = 0;
        }

        if (menu.difficulty == 1 || menu.difficulty == 0)
        {
            DropD.value = 0;
        }
        else
        {
            DropD.value = 1;
        }

        if (menu.fogOff)
        {
            DropMap.value = 1;
        }
        else
        {
            DropMap.value = 0;
        }

        if (menu.charLook == 0)
        {
            DropLook.value = 0;
        }
        else
        {
            DropLook.value = 1;
        }
    }

    //-------------------Parent's Custom Settings-------------------------------
    public void PremiumSetDifficulty()
    {
        if (DropD.value == 1)
        {
            menu.difficulty = 2;
        }
        else
        {
            menu.difficulty = 0; //normal
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

    public void SetCharacterSkin()
    {
        if (DropLook.value == 1)
        {
            menu.charLook = 1;
        }
        else
        {
            menu.charLook = 0;
        }
    }

    public void SetRevealMap()
    {
        if (DropMap.value == 1)
        {
            menu.fogOff = true;
        }
        else
        {
            menu.fogOff = false;
        }
    }
    //-------------------Premium's Custom Settings-------------------------------


}
