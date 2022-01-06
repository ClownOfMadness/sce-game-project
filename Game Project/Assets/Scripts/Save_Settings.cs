using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Object of all the global data that saved.
[System.Serializable]
public class Save_Settings
{
    public int windowLook;

    //[Achievements]//
    //...
    public bool isVeteran;
    public bool isBuilder;
    public bool isCrafty;
    //...

    public Data_PerSaveSlot data_perSaveSlot;

    public Save_Settings() //Builder for Save_Settings
    {
        isVeteran = false;
        isBuilder = false; 
        isCrafty = false;
        windowLook = 0;

        data_perSaveSlot = null;
    }
}
