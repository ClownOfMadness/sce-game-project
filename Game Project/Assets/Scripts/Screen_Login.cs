using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//responsible for Premium user GUI
public class Screen_Login : MonoBehaviour
{
    [HideInInspector] public static bool IsLogin = false; 
    [HideInInspector] public string [] Pcode = new string[] { "Premium", "premium", "Premium1", "premium1","Premium2","premium2" };
                                                               //the premium access codes
    [HideInInspector] public string Pcancel = "cancel"; //to be able to cancel premium access
    public InputField code;
    public Button submitButton; //to be able to invoke it with the "Return" key
    public GameObject Menu;
    public GameObject LoginPremium;
    public GameObject Canvas;
    public GameObject ErrorMessage; //display an error message on a wrong code submission
    //public Menu_Pause menu_Pause;

    public void Update() 
    {
        if (Input.GetKeyDown(KeyCode.Return)||(Input.GetKey("enter")))
        {
            //to add the option to "submit" by also pressing "Enter" on the keyboard 
            submitButton.onClick.Invoke();
            Debug.Log("login button clicked");
        }

    }
    public void TryLogin()  //testing if the login info is correct
    {
        int i;
        Debug.Log(Pcode.Length);
        for (i = 0; i < Pcode.Length; i++)
        {
            if (code.text == Pcode[i]) //if the given code is in the code list
            {
                Debug.Log("Log in succefull");
                ErrorMessage.SetActive(false);
                //IsLogin = true;
                PlayerPrefs.SetInt("premium", 1);   //save premium=true;
                LoginPremium.SetActive(false);
                Menu.SetActive(true);
                Canvas.GetComponent<Menu_Main>().Achievments.SetActive(true);
                //menu_Pause.Resume();
                //replace with premium windows in future
                ClearInputField(); //to clear the input field
                break;
            }
        }
        if (code.text == Pcancel)
        {
            PlayerPrefs.SetInt("premium", 0);   //save premium=false, for testing;
        }
        if (Pcode.Length == i)
        {
            if (code.text != "cancel")
            {
                ErrorMessage.SetActive(true);
                Debug.Log("Incorrect Code");
            }
            ClearInputField(); //clears the input field after submission
        }
    }

    public void ClearInputField()
    {
         //to clear the input field
        code.Select();
        code.text = "";
    }
}
