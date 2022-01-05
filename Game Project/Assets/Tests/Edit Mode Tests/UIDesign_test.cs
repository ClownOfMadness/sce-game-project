using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class UIDesign_test
{
    // A Test behaves as an ordinary method
    [Test]
    public void IsPremium()
    {
        Game_Master game_Master = new Game_Master();
        Assert.IsTrue(!game_Master.premiumUser);
    }
    [Test]
    public void UIDesignCount()
    {
        Menu_Pause menu_Pause = new Menu_Pause();
        Assert.AreEqual(expected: 2, actual: menu_Pause.UIDesignDrop.options.Count);

    }

    [Test]
    public void UIDesignOptionDefault()
    {
        Menu_Pause menu_Pause = new Menu_Pause();
        Assert.AreEqual(expected: "Default", actual: menu_Pause.UIDesignDrop.options[0].text);
       
    }

    [Test]
    public void UIDesignOptionGolden()
    {
        Menu_Pause menu_Pause = new Menu_Pause();
        Assert.AreEqual(expected: "Golden", actual: menu_Pause.UIDesignDrop.options[1].text);

    }

    [Test]
    public void UIDesignValue()
    {
        Menu_Pause menu_Pause = new Menu_Pause();
        Assert.AreEqual(expected: 0, actual: menu_Pause.UIDesignDrop.value);

    }

    [Test]
    public void UIDesignHDefaultSprites()
    {
        Menu_Pause menu_Pause = new Menu_Pause();
        Assert.IsNotNull(menu_Pause.Hint1);

    }

    [Test]
    public void UIDesignHGoldenSprites()
    {
        Menu_Pause menu_Pause = new Menu_Pause();
        Assert.IsNotNull(menu_Pause.Hint2);

    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    /*[UnityTest]
    public IEnumerator UIDesign_testWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }*/
}
