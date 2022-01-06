using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class PauseMenu_Test : MonoBehaviour
{
    [OneTimeSetUp]
    public void loadGame()  //loading game to run play-time testing
    {
        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }

    //Test if the game paused.
    [UnityTest]
    public IEnumerator A_Pause_Test()
    {
        yield return new WaitForSeconds(1); //wait till game opens

        Menu_Pause pause = FindObjectOfType<Menu_Pause>();
        pause.Pause();
        Assert.AreEqual(Menu_Pause.IsPaused, true);

        Time.timeScale = 1f;

            yield return null;
    }

    //Test if the game resumes.
    [UnityTest]
    public IEnumerator B_Resume_Test()
    {
        yield return new WaitForSeconds(1); //wait till game opens

        Menu_Pause pause = FindObjectOfType<Menu_Pause>();
        pause.Resume();
        Assert.AreEqual(Menu_Pause.IsPaused, false);

        yield return null;
    }
    //Test if the game Changes design.
    [UnityTest]
    public IEnumerator C_ChangeDesign_Test()
    {
        yield return new WaitForSeconds(1); //wait till game opens

        Menu_Pause pause = FindObjectOfType<Menu_Pause>();
        pause.UIDesignDrop.value = 0;
        pause.ChangeDesign();
        Assert.AreEqual(pause.UIDesignDrop.value, 0);

        yield return null;
    }
    //Test if the game moves to main menu.
    [UnityTest]
    public IEnumerator D_LoadMenu_test()
    {
        yield return new WaitForSeconds(1); //wait till game opens

        Menu_Pause pause = FindObjectOfType<Menu_Pause>();
        pause.LoadMenu();

        yield return new WaitForSeconds(2);

        Assert.True(SceneManager.GetActiveScene().name == "Menu");

        yield return null;
    }
}
