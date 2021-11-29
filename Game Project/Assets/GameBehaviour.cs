using UnityEngine;
using UnityEngine.UI;

public class GameBehaviour : MonoBehaviour
{
    public GameObject Login;
    public GameObject Cards;
    public GameObject loginButton;
    public GameObject cardsButton;
    [HideInInspector] public string[] Pcode = new string[] { "Premium", "premium", "Premium1", "premium1", "Premium2", "premium2" };

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
        int i;
        string code = Login.GetComponentInChildren<InputField>().text;
        Debug.Log(Pcode.Length);
        for (i = 0; i < Pcode.Length; i++) {
            if (code == Pcode[i])
            {
                Debug.Log("Log in succefull");
                Login.GetComponent<LoginScreen>().IsLogin = true;
                CloseLoginOpenGame();
                loginButton.SetActive(false);
                Cards.GetComponent<ZoneCards>().creativeButton.SetActive(true);
                break;
            }
        }
        if (Pcode.Length==i)
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
