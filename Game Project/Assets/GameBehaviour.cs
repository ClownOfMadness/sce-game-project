using UnityEngine;
using UnityEngine.UI;

public class GameBehaviour : MonoBehaviour
{
    public GameObject Login;
    public GameObject Cards;
    public GameObject loginButton;
    public GameObject cardsButton;

    void Start()    //initilizing screen
    {
        Login.SetActive(false);
        Cards.SetActive(true);
        loginButton.SetActive(true);
        cardsButton.SetActive(true);
    }
    public void CloseLoginOpenGame()
    {
        Login.SetActive(false);
        Cards.SetActive(true);
    }
    public void CloseGameOpenLogin()
    {
        Login.SetActive(true);
        Cards.SetActive(false);
    }
    public void LoginUI()    //open login UI on click
    {
        
        if (Login.activeSelf)
        {
            CloseLoginOpenGame();
        }
        else
        {
            CloseGameOpenLogin();
        }
    }
    public void TryLogin()  //try to login
    {
        string code = Login.GetComponentInChildren<InputField>().text;
        if (code == Login.GetComponent<LoginScreen>().Pcode)
        {
            Debug.Log("Log in succefull");
            Login.GetComponent<LoginScreen>().IsLogin = true;
            CloseLoginOpenGame();
            loginButton.SetActive(false);
            Cards.GetComponent<ZoneCards>().creativeButton.SetActive(true);
        }
        else
        {
            Debug.Log("Incorrect Code");
        }
    }
    public void CardUI()    //open card UI on click
    {
        if (Cards.activeSelf)
        {
            Cards.SetActive(false);
        }
        else
        {
            Cards.SetActive(true);
        }
    }
}
