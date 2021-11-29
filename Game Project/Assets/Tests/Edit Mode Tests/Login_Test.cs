using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

public class Login_Test
{
    // A Test behaves as an ordinary method
    [Test]
    public void IsPremium() //checks if the code appears in the database
    {
        GameBehaviour code = new GameBehaviour();
        Assert.AreEqual(expected: "premium", actual: code.Pcode[1]);
    }

    [Test]
    public void IsPremiumVariant() //checks if the code variant appears in the database
    {
        GameBehaviour code = new GameBehaviour();
        Assert.AreEqual(expected: "premium2", actual: code.Pcode[5]);
    }

    [Test]
    public void IsCodeLength() //tests for the correct code database length
    {
        GameBehaviour code = new GameBehaviour();
        Assert.AreEqual(expected: 6, actual: code.Pcode.Length);
    }
    [Test]
    public void IsLoginButton() //tests the login button
    {
        GameBehaviour code = new GameBehaviour();
        LoginScreen loginField = new LoginScreen();
        code.loginButton.SetActive(true);
        loginField.GetComponentInChildren<InputField>().text = "Premium";
        code.TryLogin();
        Assert.IsFalse(code.loginButton.activeSelf);
    }
    [Test]
    public void IsSkipButton() //tests the skip button
    {
        GameBehaviour code = new GameBehaviour();
        code.Login.SetActive(true);
        code.CloseLoginOpenGame();
        Assert.IsFalse(code.Login.activeSelf);
    }
    [Test]
    public void IsOpenLogin() //tests for if buttons can close the game and open login
    {
        GameBehaviour code = new GameBehaviour();
        code.Login.SetActive(false);
        code.CloseGameOpenLogin();
        Assert.IsTrue(code.Login.activeSelf);
    }
    [Test]
    public void IsCreative() //tests if the access to the book is granted for the premium
    {
        GameBehaviour code = new GameBehaviour();
        LoginScreen loginField = new LoginScreen();
        ZoneCards ZoneC = new ZoneCards();
        code.loginButton.SetActive(true);
        loginField.GetComponentInChildren<InputField>().text = "Premium";
        code.TryLogin();
        Assert.IsTrue(ZoneC.creativeButton.activeSelf);
    }



}
