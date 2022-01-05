using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class Tutorial_test
{
    [Test]
    public void TutorialStartIndex()
    {
        Tutorial tutorial = new Tutorial();
        Assert.AreEqual(expected: 0, actual: tutorial.index);
    }

    [Test]
    public void TutorialPause()
    {
        /*Game_Master game_Master = new Game_Master();
        game_Master.Tutorial.SetActive(true);*/
        Tutorial tutorial = new Tutorial();
        tutorial.OpenTutorial();
        Assert.AreEqual(expected: 0f, actual: Time.timeScale);
    }

    [Test]
    public void TutorialResume()
    {
        /*Game_Master game_Master = new Game_Master();
        game_Master.Tutorial.SetActive(false);*/
        Tutorial tutorial = new Tutorial();
        tutorial.OpenTutorial();
        Assert.AreEqual(expected: 1f, actual: Time.timeScale);
    }


    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    /*[UnityTest]
    public IEnumerator Tutorial_testWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }*/
}
