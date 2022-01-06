using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class GameSpeed_Test
{
    [Test]
    public void IsParent() //game speed is under parent custom settings
    {
        Screen_Parent screen_Parent = new Screen_Parent();
        Assert.IsTrue(!screen_Parent.ParentOptions);
    }

    [Test]
    public void GameSpeedSDNTest() //testing the day night cycle game speed
    {
        System_DayNight system_DayNight = new System_DayNight();
        Assert.AreEqual(expected: 1f, actual: system_DayNight.cycleSpeed);
    }

    [Test]
    public void GameSpeedDefaultTest() //testing that the game starts with the correct game speed
    {
        Menu_Main menu_Main = new Menu_Main();
        Assert.AreEqual(expected: 0, actual: menu_Main.gameSpeed);
    }

    [Test]
    public void SetGameSpeed() //testing that the game sets the correct speed
    {
        System_DayNight system_DayNight = new System_DayNight();
        Game_Master game_Master = new Game_Master();
        game_Master.SetGameSpeed();
        Assert.AreEqual(expected:0f ,actual: system_DayNight.cycleSpeed);
    }


    /*[UnityTest]
    public IEnumerator GameSpeed_TestWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }*/
}
