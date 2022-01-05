using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class GameSpeed_Test
{
    [Test]
    public void IsParent()
    {
        Screen_Parent screen_Parent = new Screen_Parent();
        Assert.IsTrue(!screen_Parent.ParentOptions);
    }

    [Test]
    public void GameSpeedSDNTest()
    {
        System_DayNight system_DayNight = new System_DayNight();
        Assert.AreEqual(expected: 1f, actual: system_DayNight.cycleSpeed);
    }

    [Test]
    public void GameSpeedDefaultTest()
    {
        Menu_Main menu_Main = new Menu_Main();
        Assert.AreEqual(expected: 0, actual: menu_Main.gameSpeed);
    }

    /*[Test]
    public void SetGameSpeed()
    {
        System_DayNight system_DayNight = new System_DayNight();
        Game_Master game_Master = new Game_Master();
        game_Master.SetGameSpeed();
        Assert.AreEqual(expected:0f ,actual: system_DayNight.cycleSpeed);
    }*/


    /*[UnityTest]
    public IEnumerator GameSpeed_TestWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }*/
}
