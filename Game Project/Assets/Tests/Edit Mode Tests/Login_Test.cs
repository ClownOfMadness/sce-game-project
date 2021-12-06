using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

public class Login_Test
{/*
    // A Test behaves as an ordinary method
    [Test]
    public void IsPremium() //checks if the code appears in the database
    {
        Screen_Behaviour code = new Screen_Behaviour();
        Assert.AreEqual(expected: "premium", actual: code.Pcode[1]);
    }

    [Test]
    public void IsPremiumVariant() //checks if the code variant appears in the database
    {
        Screen_Behaviour code = new Screen_Behaviour();
        Assert.AreEqual(expected: "premium2", actual: code.Pcode[5]);
    }

    [Test]
    public void IsCodeLength() //tests for the correct code database length
    {
        Screen_Behaviour code = new Screen_Behaviour();
        Assert.AreEqual(expected: 6, actual: code.Pcode.Length);
    }
    [Test]
    public void IsLoginButton() //tests the login button
    {
        Screen_Behaviour code = new Screen_Behaviour();
        Screen_Login loginField = new Screen_Login();
        code.loginButton.SetActive(true);
        loginField.GetComponentInChildren<InputField>().text = "Premium";
        code.TryLogin();
        Assert.IsFalse(code.loginButton.activeSelf);
    }
    [Test]
    public void IsSkipButton() //tests the skip button
    {
        Screen_Behaviour code = new Screen_Behaviour();
        code.Login.SetActive(true);
        code.CloseLoginOpenGame();
        Assert.IsFalse(code.Login.activeSelf);
    }
    [Test]
    public void IsOpenLogin() //tests for if buttons can close the game and open login
    {
        Screen_Behaviour code = new Screen_Behaviour();
        code.Login.SetActive(false);
        code.CloseGameOpenLogin();
        Assert.IsTrue(code.Login.activeSelf);
    }
    [Test]
    public void IsCreative() //tests if the access to the book is granted for the premium
    {

        Screen_Behaviour code = new Screen_Behaviour();
        Screen_Login loginField = new Screen_Login();
        Screen_Cards ZoneC = new Screen_Cards();
        code.loginButton.SetActive(true);
        loginField.GetComponentInChildren<InputField>().text = "Premium";
        code.TryLogin();
        Assert.IsTrue(ZoneC.creativeButton.activeSelf);
    }

    */

}
