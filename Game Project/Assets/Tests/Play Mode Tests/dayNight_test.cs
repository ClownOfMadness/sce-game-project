using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class dayNight_test : MonoBehaviour
{
    [OneTimeSetUp]
    public void LoadGame()
    {
        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }

    [UnityTest]
    public IEnumerator dayNight_Light()
    {
        //This function makes sure that the lighting changes by its current time and also makes sure that when the time
        // is over the night treshold then it will switch to night mode
        
        yield return new WaitForSeconds(1);
        System_DayNight dayNight = GameObject.Find("Day/Night Cycle").GetComponent<System_DayNight>();
        dayNight.currentTime = 0;
        yield return new WaitForSeconds(1);
        dayNight.currentTime = 1;
        yield return new WaitForSeconds(1);
        dayNight.currentTime = 2;
        yield return new WaitForSeconds(1);
        dayNight.currentTime = 3;
        yield return new WaitForSeconds(1);
        dayNight.currentTime = 4;
        yield return new WaitForSeconds(1);
        dayNight.currentTime = 5;
        yield return new WaitForSeconds(1);
        dayNight.currentTime = 6;
        yield return new WaitForSeconds(1);
        dayNight.currentTime = 7;
        yield return new WaitForSeconds(1);
        dayNight.currentTime = 8;
        yield return new WaitForSeconds(1);
        Assert.AreEqual(dayNight.isDay, false);
        yield return null;
    }
}
