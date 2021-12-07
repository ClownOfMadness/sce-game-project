using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//responsible for Premium user GUI
public class Screen_Login : MonoBehaviour
{
    [HideInInspector] public static bool IsLogin = false;
    [HideInInspector] public string [] Pcode = new string[] { "Premium", "premium", "Premium1", "premium1","Premium2","premium2" };
    public InputField code;
    public Button submitButton; //
    public GameObject Menu;
    public GameObject LoginPremium;
    public void Update() //
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            submitButton.onClick.Invoke();
            Debug.Log("login button clicked");
        }

    }
    public void TryLogin()  //try to login
    {
        int i;
        Debug.Log(Pcode.Length);
        for (i = 0; i < Pcode.Length; i++)
        {
            if (code.text == Pcode[i])
            {
                Debug.Log("Log in succefull");
                IsLogin = true;
                LoginPremium.SetActive(false);
                Menu.SetActive(true);
                //replace with premium windows in future
                break;
            }
        }
        if (Pcode.Length == i)
        {
            Debug.Log("Incorrect Code");
        }
    }
}
