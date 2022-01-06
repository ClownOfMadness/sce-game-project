using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

public class Keybinding_test
{
    [Test]
    public void IsPremium() //keybinds is also active for premium users and is deactivated for default users
    {
        Game_Master game_Master = new Game_Master();
        Assert.IsTrue(!game_Master.premiumUser);
    }

    [Test]
    public void KeybindingCreativeDText() //testing that the game shows the correct button for the keybinds
    {
        KeyBinding keyBinding = new KeyBinding();
        Assert.AreEqual(expected: "C", actual: keyBinding.Keys["Creative"].ToString());
       
    }

    [Test]
    public void KeybindingCreativeDButton() //testing that the keycode for the button is correct
    {
        KeyBinding keyBinding = new KeyBinding();
        Assert.AreEqual(expected: KeyCode.C, actual: keyBinding.Keys["Creative"]);

    }

    [Test]
    public void KeybindingGetKeyTest() //testing if it returns the correct key
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
