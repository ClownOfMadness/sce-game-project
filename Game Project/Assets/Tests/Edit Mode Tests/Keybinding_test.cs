using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

public class Keybinding_test
{
    [Test]
    public void IsPremium()
    {
        Game_Master game_Master = new Game_Master();
        Assert.IsTrue(!game_Master.premiumUser);
    }

    [Test]
    public void KeybindingCreativeDText()
    {
        KeyBinding keyBinding = new KeyBinding();
        Assert.AreEqual(expected: "C", actual: keyBinding.Keys["Creative"].ToString());
       
    }

    [Test]
    public void KeybindingCreativeDButton()
    {
        KeyBinding keyBinding = new KeyBinding();
        Assert.AreEqual(expected: KeyCode.C, actual: keyBinding.Keys["Creative"]);

    }

    [Test]
    public void KeybindingGetKeyTest()
    {
        KeyBinding keyBinding = new KeyBinding();
        string k=keyBinding.GetKey("Jobs");
        Assert.AreEqual(expected: "J", actual: k);

    }


    /* [UnityTest]
     public IEnumerator Keybinding_testWithEnumeratorPasses()
     {
         // Use the Assert class to test conditions.
         // Use yield to skip a frame.
         yield return null;
     }*/
}
