using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class UIDesign_test
{

    [Test]
    public void IsPremium() //ui design option should appear only for premium users 
    {
        Game_Master game_Master = new Game_Master();
        Assert.IsTrue(!game_Master.premiumUser);
    }

    [Test]
    public void UIDesignCount() //testing how many design options are there
    {
        Menu_Pause menu_Pause = new Menu_Pause();
        Assert.AreEqual(expected: 2, actual: menu_Pause.UIDesignDrop.options.Count);

    }

    [Test]
    public void UIDesignOptionDefault() //testing the default design
    {
        Menu_Pause menu_Pause = new Menu_Pause();
        Assert.AreEqual(expected: "Default", actual: menu_Pause.UIDesignDrop.options[0].text);
       
    }

    [Test]
    public void UIDesignOptionGolden() //testing the golden design
    {
        Menu_Pause menu_Pause = new Menu_Pause();
        Assert.AreEqual(expected: "Golden", actual: menu_Pause.UIDesignDrop.options[1].text);

    }

    [Test]
    public void UIDesignValue() //testing the design dropdown value
    {
        Menu_Pause menu_Pause = new Menu_Pause();
        Assert.AreEqual(expected: 0, actual: menu_Pause.UIDesignDrop.value);

    }

    [Test]
    public void UIDesignHDefaultSprites() //testing that the sprites are not null 
    {
        Menu_Pause menu_Pause = new Menu_Pause();
        Assert.IsNotNull(menu_Pause.Hint1);
       
    }

    [Test]
    public void UIDesignHGoldenSprites() //testing that the golden designs are fine
    {
        Menu_Pause menu_Pause = new Menu_Pause();
        Assert.IsNotNull(menu_Pause.Hint2);

    }

    /*[UnityTest]
    public IEnumerator UIDesign_testWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }*/
}
