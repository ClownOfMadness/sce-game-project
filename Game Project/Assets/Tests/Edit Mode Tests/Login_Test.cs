using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

public class Login_Test
{ 
    [Test]
    public void IsPremiumAccess()
    {
        Game_Master game_Master = new Game_Master();
        Assert.IsTrue(!game_Master.premiumUser);
    }

    [Test]
    public void IsPremiumCode() //checks if the code appears in the database
    {
        Screen_Login code = new Screen_Login();
        Assert.AreEqual(expected: "premium", actual: code.Pcode[1]);
    }

    [Test]
    public void IsPremiumVariant() //checks if the code variant appears in the database
    {
        Screen_Login code = new Screen_Login();
        Assert.AreEqual(expected: "premium2", actual: code.Pcode[5]);
    }

    [Test]
    public void IsCodeLength() //tests for the correct code database length
    {
        Screen_Login code = new Screen_Login();
        Assert.AreEqual(expected: 6, actual: code.Pcode.Length);
    }
    [Test]
    public void IsLoginButton() //tests the login button
    {
        Screen_Login code = new Screen_Login();
        Screen_Login loginField = new Screen_Login();
        code.LoginPremium.SetActive(true);
        loginField.GetComponentInChildren<InputField>().text = "Premium";
        code.TryLogin();
        Assert.IsFalse(code.LoginPremium.activeSelf);
    }
    [Test]
    public void IsSkipButton() //tests the skip button
    {
       /* Screen_Login code = new Screen_Login();
        code.LoginPremium.SetActive(true);
        code.CloseLoginOpenGame();
        Assert.IsFalse(code.LoginPremium.activeSelf);*/
    }
    [Test]
    public void IsOpenLogin() //tests for if buttons can close the game and open login
    {
       /* Screen_Login code = new Screen_Login();
        code.LoginPremium.SetActive(false);
        code.CloseGameOpenLogin();
        Assert.IsTrue(code.LoginPremium.activeSelf);*/
    }
    [Test]
    public void IsCreative() //tests if the access to the book is granted for the premium
    {

        Screen_Login code = new Screen_Login();
        Screen_Login loginField = new Screen_Login();
        Screen_Cards ZoneC = new Screen_Cards();
        code.LoginPremium.SetActive(true);
        loginField.GetComponentInChildren<InputField>().text = "Premium";
        code.TryLogin();
        Assert.IsTrue(ZoneC.creativeButton.activeSelf);
    }

    

}
