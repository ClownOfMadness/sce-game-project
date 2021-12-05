using UnityEngine;
using UnityEngine.UI;

public class Screen_Behaviour : MonoBehaviour
{
    public GameObject Login;
    public GameObject Cards;
    public GameObject loginButton;
    public GameObject cardsButton;
    [HideInInspector] public string[] Pcode = new string[] { "Premium", "premium", "Premium1", "premium1", "Premium2", "premium2" };
    private Screen_Login LoginS;
    private Screen_Cards CardsS;
    public Button submitButton; //

    void Start()    //initilizing screen
    {
        LoginS = Login.transform.GetComponent<Screen_Login>();
        CardsS = Cards.transform.GetComponent<Screen_Cards>();

        Login.SetActive(false);
        Cards.SetActive(true);
        loginButton.SetActive(true);
        cardsButton.SetActive(true);
    }
    public void TopMessage(string text)
    {
        //move things about Message from Screen_Cards here
    }
    public void CloseLoginOpenGame()
    {
        Login.SetActive(false);
        //Cards.SetActive(true); //is this needed?
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

    public void Update() //
    {
        if (Login.activeSelf)   //only check if login screen is open (feel free to remove if I misunderstood)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                submitButton.onClick.Invoke();
                Debug.Log("login button clicked");
            }
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
                LoginS.IsLogin = true;
                CloseLoginOpenGame();
                loginButton.SetActive(false);
                CardsS.creativeButton.SetActive(true); 
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
        Debug.Log("Clicked "+Cards.activeSelf);
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
