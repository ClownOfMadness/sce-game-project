using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

public class FontSize_Test
{
    [Test]
    public void IsParent() //font size is under parent access
    {
        Screen_Parent screen_Parent = new Screen_Parent();
        Assert.IsTrue(!screen_Parent.ParentOptions);
    }

    [Test]
    public void FontSizeDefault() //testing that the font starts from the intended value
    {
        Assert.AreEqual(expected: 0, actual: PlayerPrefs.GetInt("ChangeFont",0));
    }

    [Test]
    public void DefaultFontSizeTest() //load default font test
    {
        Game_Master game_Master = new Game_Master();
        game_Master.LoadDefaultFont();
        Screen_Cards screen_Cards = new Screen_Cards();
        int a = screen_Cards.Desc_Text.fontSize;
        Assert.AreEqual(expected: 18, actual: a);
    }

    [Test]
    public void BigFontSizeTest() //load the big font test
    {
        Game_Master game_Master = new Game_Master();
        game_Master.LoadBigFont();
        Screen_Cards screen_Cards = new Screen_Cards();
        int a = screen_Cards.Desc_Text.fontSize;
        Assert.AreEqual(expected: 23, actual: a);
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    /*[UnityTest]
    public IEnumerator FontSize_TestWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }*/
}
