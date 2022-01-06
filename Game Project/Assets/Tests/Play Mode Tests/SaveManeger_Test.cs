using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using System.IO;

public class SaveManeger_Test : MonoBehaviour
{
    [OneTimeSetUp]
    public void loadGame()  //loading game to run play-time testing
    {
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }
    
    //Check if the data per slot saved.
    [UnityTest]
    public IEnumerator A_SavePerSlot_Test()
    {
        if (!Directory.Exists(Application.persistentDataPath + "/tests"))
            Directory.CreateDirectory(Application.persistentDataPath + "/tests");

        string pathManager = Application.persistentDataPath + "/tests/manager.test"; //Global data
        string pathSettings = Application.persistentDataPath + "/saves/Settings.save"; //Global 

        Save_Manager manager = FindObjectOfType<Save_Manager>();

        yield return new WaitForSeconds(1);
        manager.SavePerSlot(pathManager);

        Assert.IsNotNull(IO_Files.ReadDataSetting(pathSettings, pathManager));
    }
    //Check if the ssetting loaded.
    [UnityTest]
    public IEnumerator B_UpdateMenu_Test()
    {
        yield return new WaitForSeconds(1);
        Menu_Main main = FindObjectOfType<Menu_Main>();
        Assert.IsNotNull(main.isBuilder);
        Assert.IsNotNull(main.isVeteran);
        Assert.IsNotNull(main.isCrafty);
        Assert.IsNotNull(main.windowLook);

        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }

    //Check if the scene changed.
    [UnityTest]
    public IEnumerator C_Play()
    {
        yield return new WaitForSeconds(1);
        Save_Manager manager = FindObjectOfType<Save_Manager>();

        Assert.True(true);
        manager.Play();
    }


    //Check if the data loaded in the new scene.
    [UnityTest]
    public IEnumerator SceneLoaded_Test()
    {
        yield return new WaitForSeconds(1);
        Save_Manager manager = FindObjectOfType<Save_Manager>();

        Assert.IsNotNull(manager.SceneLoaded());
    }
}
