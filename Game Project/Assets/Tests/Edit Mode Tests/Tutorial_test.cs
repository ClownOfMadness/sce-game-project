using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class Tutorial_test
{
    [Test]
    public void TutorialStartIndex() //testing that the tutorial starts in the first page
    {
        Tutorial tutorial = new Tutorial();
        Assert.AreEqual(expected: 0, actual: tutorial.index);
    }

    [Test]
    public void TutorialPause() //testing that the game is paused during the tutorial
    {
        /*Game_Master game_Master = new Game_Master();
        game_Master.Tutorial.SetActive(true);*/

        Tutorial tutorial = new Tutorial();
        tutorial.OpenTutorial();
        Assert.AreEqual(expected: 0f, actual: Time.timeScale);
    }

    [Test]
    public void TutorialResume() //testing that ending the tutorial resumes the game
    {
        /*Game_Master game_Master = new Game_Master();
        game_Master.Tutorial.SetActive(false);*/

        Tutorial tutorial = new Tutorial();
        tutorial.transform.gameObject.SetActive(false);
        //tutorial.OpenTutorial();
        Assert.AreEqual(expected: 1f, actual: Time.timeScale);
    }

    [Test]
    public void TutorialNextPage() //testing that the next button advances the tutorial
    {
        Tutorial tutorial = new Tutorial();
        tutorial.OpenTutorial();
        tutorial.Next();
        Assert.AreEqual(expected: 1, actual: tutorial.index);
    }

    [Test]
    public void TutorialPreviousPage() //tests that the previous button goes one page back properly
    {
        Tutorial tutorial = new Tutorial();
        tutorial.OpenTutorial();
        tutorial.index = 5;
        tutorial.Previous();
        Assert.AreEqual(expected: 4, actual: tutorial.index);
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
