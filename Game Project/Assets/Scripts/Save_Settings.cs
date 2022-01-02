using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Save_Settings
{
    public bool premiumUser;
    public string parentPassword;

    //[Achievements]//
    //...
    public bool isVeteran;
    public float maxGameDays;
    public bool isBuilder;
    public int maxBuildings;
    public bool isCrafty;
    public int maxCardsFound;
    //...

    public Data_PlayerConfig data_playerConfig;

    public Save_Settings()
    {
        //premiumUser = false;
        //parentPassword = "0000";

        isVeteran = false;
        maxGameDays = 0;
        isBuilder = false;
        maxBuildings = 0;
        isCrafty = false;
        maxCardsFound = 0;

        data_playerConfig = null;
    }
}
