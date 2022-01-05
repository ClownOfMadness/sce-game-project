using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class Difficulty_Test
{
    [Test]
    public void IsParent()
    {
        Screen_Parent screen_Parent = new Screen_Parent();
        Assert.IsTrue(!screen_Parent.ParentOptions);
    }

    [Test]
    public void DifficultyDefault()
    {
        Game_Master game_Master = new Game_Master();
        Assert.AreEqual(expected: 1, actual: game_Master.difficulty);
    }

    [Test]
    public void DifficultySave()
    {
        Menu_Main menu_Main = new Menu_Main();
        Assert.AreEqual(expected: 1, actual: menu_Main.difficulty);
    }


    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    /*[UnityTest]
    public IEnumerator Difficulty_TestWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }*/
}
