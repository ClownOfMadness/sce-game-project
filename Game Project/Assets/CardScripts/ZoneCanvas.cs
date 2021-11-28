using UnityEngine;
using UnityEngine.UI;

//responsible for switching panels and button related functions
public class ZoneCanvas : MonoBehaviour
{
    public GameObject LoginButton;
    public GameObject Login;
    public GameObject Hand;
    public GameObject Craft;
    public GameObject Book;
    public GameObject creativeButton;
    public GameObject destroyButton;
    private bool CraftEnabled;
    private bool UIEnabled;
    private bool LogEnabled;

    void Start()    //initilizing in case something was off
    {
        Hand.SetActive(true);
        Book.SetActive(false);
        Craft.SetActive(true);
        creativeButton.SetActive(true);
        destroyButton.SetActive(true);
        CraftEnabled = true;
        UIEnabled = true;
        Login.SetActive(false);
        LogEnabled = false;
    }
    public void LoginUI()    //open card UI on click
    {
        if (LogEnabled)
        {
            SkipLogIn();
        }
        else
        {
            Hand.SetActive(false);
            Book.SetActive(false);
            Craft.SetActive(false);
            creativeButton.SetActive(false);
            destroyButton.SetActive(false);
            CraftEnabled = false;
            UIEnabled = false;
            LogEnabled = true;
            Login.SetActive(true);
        }
    }

    public void SkipLogIn()
    {
        Hand.SetActive(true);
        Book.SetActive(false);
        Craft.SetActive(true);
        creativeButton.SetActive(true);
        destroyButton.SetActive(true);
        CraftEnabled = true;
        UIEnabled = true;
        LogEnabled = false;
        Login.SetActive(false);
        Debug.Log("Log in skipped"); //to delete later
    }
    public void TryLogin()
    {
        string code = Login.GetComponentInChildren<InputField>().text;
        if (code == Login.GetComponent<LoginScreen>().Pcode)
        {
            Debug.Log("Log in succefull");
            Login.GetComponent<LoginScreen>().IsLogin = true;
            SkipLogIn();
            LoginButton.SetActive(false);
        }
        else
        {
            Debug.Log("Incorrect Code");
        }
    }

    public void CardUI()    //open card UI on click
    {
        if (UIEnabled)
        {
            Hand.SetActive(false);
            Book.SetActive(false);
            Craft.SetActive(false);
            creativeButton.SetActive(false);
            destroyButton.SetActive(false);
            CraftEnabled = false;
            UIEnabled = false;
        }
        else
        {
            Hand.SetActive(true);
            Book.SetActive(false);
            Craft.SetActive(true);
            creativeButton.SetActive(true);
            destroyButton.SetActive(true);
            CraftEnabled = true;
            UIEnabled = true;
        }
    }

    public void SwitchCreative()
    {
        if (CraftEnabled)           //open book, close craft
        {
            Craft.SetActive(false);
            Book.SetActive(true);
            CraftEnabled = false;
        }
        else
        {
            Book.GetComponent<ZoneBook>().FirstPage();
            Book.SetActive(false);  //open craft, close book
            Craft.SetActive(true);
            CraftEnabled = true;
        }
    }
    public void HandToCraft(CardDrag card)  //move card from Hand zone to Hand on click
    {
        if (CraftEnabled)
        {
            if (card.transform.parent == Hand.transform)
            {
                card.transform.SetParent(Craft.transform);
            }
            else
            {
                CraftToHand(card);
            }
        }
    }
    public void CraftToHand(CardDrag card)  //move card from Craft zone to Hand on click
    {
        card.gameObject.transform.SetParent(Hand.transform);
    }
    public void BookToHand(CardDisplay pickedCard)  //adds new card from hand based on what was picked in Book
    {
        if (Hand.GetComponentsInChildren<CardDrag>().Length < Hand.GetComponent<ZoneHand>().Size)
        {
            GameObject newCard = Instantiate(Hand.GetComponent<ZoneHand>().CardPrefab, Hand.transform);     //create and instantiate object in scene
            newCard.GetComponent<CardDrag>().AddCard(pickedCard.card);                                      //add cards to objects 
            newCard.name = string.Format("{0}", pickedCard.name);                                           //update new card name (for displaying in Scene)
        }
    }
}
