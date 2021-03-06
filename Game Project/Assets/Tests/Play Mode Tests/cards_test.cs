using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

//Tests related to the Cards functionalities
public class cards_test : MonoBehaviour
{
    [OneTimeSetUp]
    public void loadGame()  //loading game to run play-time testing
    {
        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }

    //[Database Tests: Card_Pool.cs]//
    [UnityTest] //Test if reading from the card database works
    public IEnumerator A_cards_testCardPool()
    {
        yield return new WaitForSeconds(1); //wait till game opens
        Screen_Cards screen = FindObjectOfType<Screen_Cards>();
        Assert.AreNotEqual(screen.Pool.count, 0);
        yield return null;
    }
    [UnityTest] //Test if program manages to get card from database by name
    public IEnumerator B_cards_testGetCardByName()
    {
        yield return new WaitForSeconds(1); //wait till game opens
        Screen_Cards screen = FindObjectOfType<Screen_Cards>();
        Data_Card card = screen.Pool.GetCard("Stick");
        Assert.AreEqual(card.name, "Stick");   //check that the card we got is the same card we wanted
        yield return null;
    }
    [UnityTest] //Test if program manages to get card from database by code
    public IEnumerator C_cards_testGetCardByCode()
    {
        yield return new WaitForSeconds(1); //wait till game opens
        Screen_Cards screen = FindObjectOfType<Screen_Cards>();
        Data_Card card = screen.Pool.GetCard(1001);
        Assert.AreEqual(card.code, 1001);   //check that the card we got is the same card we wanted
        yield return null;
    }
    [UnityTest] //Test if program manages to get card name from database by code
    public IEnumerator D_cards_testCodeToName()
    {
        yield return new WaitForSeconds(1); //wait till game opens
        Screen_Cards screen = FindObjectOfType<Screen_Cards>();
        string name = screen.Pool.CodeToName(1001);
        Assert.AreEqual(screen.Pool.GetCard(name), screen.Pool.GetCard(1001));   //check that the card we got is the same card we wanted
        yield return null;
    }
    [UnityTest] //Test if program manages to find index of card from database by name
    public IEnumerator E_cards_testFindCardIndexByName()
    {
        yield return new WaitForSeconds(1); //wait till game opens
        Screen_Cards screen = FindObjectOfType<Screen_Cards>();
        int index = screen.Pool.FindCard("Stick");
        Assert.AreEqual(screen.Pool.cards[index].name, "Stick");   //check that the card we got is the same card we wanted
        yield return null;
    }
    [UnityTest] //Test if program manages to find index of card from database by code
    public IEnumerator F_cards_testFindCardIndexByCode()
    {
        yield return new WaitForSeconds(1); //wait till game opens
        Screen_Cards screen = FindObjectOfType<Screen_Cards>();
        int index = screen.Pool.FindCard(1001);
        Assert.AreEqual(screen.Pool.cards[index].code, 1001);   //check that the card we got is the same card we wanted
        yield return null;
    }
    [UnityTest] //Test if program manages to recognize the card's fields
    public IEnumerator G_cards_testCardFields()
    {
        yield return new WaitForSeconds(1); //wait till game opens
        Screen_Cards screen = FindObjectOfType<Screen_Cards>();
        Data_Card card = screen.Pool.GetCard("Stick");
        Assert.IsFalse(screen.Pool.IsCombination(card));   //sending card that isn't a combination, should be false
        yield return null;
    }
    [UnityTest] //Test if program manages to recognize the card's fields
    public IEnumerator H_cards_testCardFields2()
    {
        yield return new WaitForSeconds(1); //wait till game opens
        Screen_Cards screen = FindObjectOfType<Screen_Cards>();
        Data_Card card = screen.Pool.GetCard("Iron Axe");
        Assert.IsTrue(screen.Pool.IsMenuCombination(card));   //sending card that is a menu combination, should be true
        yield return null;
    }
    [UnityTest] //Test if program manages to generate loot(resources) for the Ruins
    public IEnumerator I_cards_testGetLoot()
    {
        yield return new WaitForSeconds(1); //wait till game opens
        Screen_Cards screen = FindObjectOfType<Screen_Cards>();
        Data_Card card = screen.Pool.GetLoot(3);
        Assert.IsNotNull(card);   //check that card was returned succesefully
        yield return null;
    }
    [UnityTest] //Test if finding combination of cards works
    public IEnumerator J_cards_testFindCombo()
    {
        yield return new WaitForSeconds(1); //wait till game opens
        Screen_Cards screen = FindObjectOfType<Screen_Cards>();
        Data_Card firstCard = screen.Pool.GetCard("Stick");     //a card that combines with second
        Data_Card secondCard = screen.Pool.GetCard("Stick");    //a card that combines with first                        
        Assert.IsNotNull(screen.Pool.FindCombo(firstCard, secondCard)); //only returns null when a combination wasn't found
        yield return null;
    }
    [UnityTest] //Test if finding menu type combination of cards works
    public IEnumerator K_cards_testFindMenuCombo()
    {
        yield return new WaitForSeconds(1); //wait till game opens
        Screen_Cards screen = FindObjectOfType<Screen_Cards>();
        Data_Card firstCard = screen.Pool.GetCard("Stick");     //a card that combines with second
        Data_Card secondCard = screen.Pool.GetCard("Flint");    //a card that combines with first                        
        Assert.IsNotNull(screen.Pool.FindCombo(firstCard, secondCard)); //only returns null when a combination wasn't found
        yield return null;
    }
    [UnityTest] //Test if getting all possible combination works
    public IEnumerator L_cards_testGetAllCombos()
    {
        yield return new WaitForSeconds(1); //wait till game opens
        Screen_Cards screen = FindObjectOfType<Screen_Cards>();
        Assert.IsNotNull(screen.Pool.GetAllCombos()); //check that database managed to return data
        yield return null;
    }


    //[Object Tests: Screen_Cards.cs]//
    [UnityTest] //Test if creating a card object works.
    public IEnumerator M_cards_testCreateObject()
    {
        yield return new WaitForSeconds(1); //wait till game opens
        Screen_Cards screen = FindObjectOfType<Screen_Cards>();
        GameObject cardObject = screen.CreateObject(screen.Hand.transform, screen.Pool.cards[0]);
        Card_Display Card = cardObject.GetComponent<Card_Display>();
        Assert.IsNotNull(Card.card);        //check if adding card to display object worked
        yield return null;
    }


    //[Visual Tests: Card_Display.cs]//
    [UnityTest] //Test if the adding a card to an displayable object works
    public IEnumerator N_cards_testAddCard()
    {
        yield return new WaitForSeconds(1); //wait till game opens
        Screen_Cards screen = FindObjectOfType<Screen_Cards>();
        GameObject cardObject = Instantiate(screen.CardObject, screen.Hand.transform);
        Card_Display Card = cardObject.GetComponent<Card_Display>();
        Card.AddCard(screen.Pool.cards[0]);
        Assert.IsNotNull(Card.card);        //check if adding card to display object worked
        Destroy(cardObject);
        yield return null;
    }
    [UnityTest] //Test if clearing data from a displayable object works
    public IEnumerator O_cards_testClearCard()
    {
        yield return new WaitForSeconds(1); //wait till game opens
        Screen_Cards screen = FindObjectOfType<Screen_Cards>();
        GameObject cardObject = Instantiate(screen.CardObject, screen.Hand.transform);
        Card_Display Card = cardObject.GetComponent<Card_Display>();
        Card.AddCard(screen.Pool.cards[0]);
        Card.ClearCard();
        Assert.IsNull(Card.card);        //check if removing data from display object worked
        Destroy(cardObject);
        yield return null;
    }


    //[UI Tests: Screen_Cards.cs]//
    [UnityTest] //Test if opening storage works
    public IEnumerator P_cards_testStorageButton()
    {
        yield return new WaitForSeconds(1); //wait till game opens
        Screen_Cards screen = FindObjectOfType<Screen_Cards>();
        bool state = screen.BookUp;
        screen.SwitchStorage(); //open storage
        Time.timeScale = 1f;    //let the game keep running
        Assert.AreNotEqual(state, screen.StorageUp);   //test if book opened
        screen.SwitchStorage(); //close storage
        yield return null;
    }
    [UnityTest] //Test if opening book works
    public IEnumerator Q_cards_testBookButton()
    {
        yield return new WaitForSeconds(1); //wait till game opens
        Screen_Cards screen = FindObjectOfType<Screen_Cards>();
        bool state = screen.BookUp;
        screen.SwitchCreative(); //open book
        //Time.timeScale = 1f;    //let the game keep running
        Assert.AreNotEqual(state,screen.BookUp);   //test if book opened
        screen.SwitchCreative(); //close book
        yield return null;
    }
    [UnityTest] //Test if getting a hint works
    public IEnumerator R_cards_testHintButton()
    {
        yield return new WaitForSeconds(1); //wait till game opens
        Screen_Cards screen = FindObjectOfType<Screen_Cards>();
        bool state = screen.BookUp;
        screen.SwitchHints(); //get hint
        Assert.AreNotEqual(state, screen.HintUp);   //test if book opened
        screen.SwitchHints(); //close hint
        yield return null;
    }


    //[Interactive Tests: Screen_Cards.cs, Card_Drag.cs, Display_Card.cs]//
    [UnityTest] //Test if clicking on card moves it from one zone to the other
    public IEnumerator S_cards_testClickCard()
    {
        yield return new WaitForSeconds(1); //wait till game opens
        Screen_Cards screen = FindObjectOfType<Screen_Cards>();
        foreach (Transform cardObject in screen.Hand.transform)    //clear whatever is in Hand
        {
            GameObject.Destroy(cardObject.gameObject);
        }
        GameObject newCard = screen.CreateObject(screen.Hand.transform, screen.Pool.cards[0]);
        Card_Drag Card = newCard.GetComponent<Card_Drag>();
        screen.SwitchStorage(); //open storage
        Time.timeScale = 1f;    //let the game keep running
        screen.CardClick(Card);             //attempt to change panel
        Assert.AreNotEqual(Card.transform, screen.Hand.transform);   //test if card moved
        screen.SwitchStorage(); //close storage
        yield return null;
    }
    [UnityTest] //Test if clicking on card moves it from one zone to the other
    public IEnumerator T_cards_testSwitchCardPlace()
    {
        yield return new WaitForSeconds(1); //wait till game opens
        Screen_Cards screen = FindObjectOfType<Screen_Cards>();
        foreach (Transform cardObject in screen.Hand.transform)    //clear whatever is in Hand
        {
            GameObject.Destroy(cardObject.gameObject);
        }
        GameObject newCard = screen.CreateObject(screen.Hand.transform, screen.Pool.cards[0]);
        Card_Drag Card = newCard.GetComponent<Card_Drag>();
        screen.SwitchStorage(); //open storage
        Time.timeScale = 1f;    //let the game keep running
        Card.SwitchCardPlace();
        Assert.AreNotEqual(Card.transform, screen.Craft.transform);   //test if panel changed
        screen.SwitchStorage(); //close storage
        yield return null;
    }
    [UnityTest] //Test if clicking on display card creates an object in hand
    public IEnumerator U_cards_testDisplayCardClicked()
    {
        yield return new WaitForSeconds(1); //wait till game opens
        Screen_Cards screen = FindObjectOfType<Screen_Cards>();
        screen.TestSkipDay = true;
        Card_Display[] card = screen.Storage.transform.GetChild(0).GetComponentsInChildren<Card_Display>();
        screen.CreateObject(screen.Hand.transform, screen.Pool.cards[0]);
        int count = screen.Hand.transform.childCount;
        screen.SwitchStorage(); //open storage
        Time.timeScale = 1f;    //let the game keep running
        screen.DisplayCardClick(card[0]);
        int newCount = screen.Hand.transform.childCount;
        Assert.AreNotEqual(count, newCount);   //test if card was added
        screen.SwitchStorage(); //close storage
        yield return null;
    }
    [UnityTest] //Test if clicking on display card creates an object in hand
    public IEnumerator V_cards_testPickedCard()
    {
        yield return new WaitForSeconds(1); //wait till game opens
        Screen_Cards screen = FindObjectOfType<Screen_Cards>();
        screen.TestSkipDay = true;
        Card_Display[] card = screen.Storage.transform.GetChild(0).GetComponentsInChildren<Card_Display>();
        int count = screen.Hand.transform.childCount;
        screen.SwitchStorage(); //open storage
        Time.timeScale = 1f;    //let the game keep running
        card[0].PickedCard();
        int newCount = screen.Hand.transform.childCount;
        Assert.AreNotEqual(count, newCount);   //test if card was added
        screen.SwitchStorage(); //close storage
        yield return null;
    }


    //[Zone Tests: Zone_Hand.cs, Zone_Craft.cs, Zone_Storage.cs, Zone_Book.cs]//
    [UnityTest] //Test if Hand_Zone manages to create itself
    public IEnumerator W_cards_testHand()
    {
        yield return new WaitForSeconds(1); //wait till game opens
        Screen_Cards screen = FindObjectOfType<Screen_Cards>();
        Assert.AreNotEqual(screen.Hand.transform.childCount, 0); //will only be 0 if loading deck failed
        yield return null;
    }
    [UnityTest] //Test if Craft zone can detect failed craft
    public IEnumerator X_cards_testCraftZoneFail()
    {
        yield return new WaitForSeconds(1); //wait till game opens
        Screen_Cards screen = FindObjectOfType<Screen_Cards>();
        foreach (Transform cardObject in screen.Hand.transform)    //clear whatever is in Hand
        {
            GameObject.Destroy(cardObject.gameObject);
        }
        GameObject cardObject1 = screen.CreateObject(screen.Hand.transform, screen.Pool.GetCard("Flint"));
        Card_Drag card1 = cardObject1.GetComponent<Card_Drag>();
        GameObject cardObject2 = screen.CreateObject(screen.Hand.transform, screen.Pool.GetCard("Iron"));
        Card_Drag card2 = cardObject2.GetComponent<Card_Drag>();
        screen.CardClick(card1);             //move card from Hand to Craft
        screen.CardClick(card2);             //move card from Hand to Craft
        yield return new WaitForSeconds(1); //wait
        Assert.AreEqual(0, screen.Craft.transform.childCount); //Craft should be empty after refreshing when the craft failed
        yield return null;
    }
    [UnityTest] //Test if Craft zone can detect succeseful craft
    public IEnumerator Y_cards_testCraftZoneSuccess()
    {
        yield return new WaitForSeconds(1); //wait till game opens
        Screen_Cards screen = FindObjectOfType<Screen_Cards>();
        foreach (Transform cardObject in screen.Hand.transform)    //clear whatever is in Hand
        {
            GameObject.Destroy(cardObject.gameObject);
        }
        GameObject cardObject1 = screen.CreateObject(screen.Hand.transform, screen.Pool.GetCard("Stick"));
        Card_Drag card1 = cardObject1.GetComponent<Card_Drag>();
        GameObject cardObject2 = screen.CreateObject(screen.Hand.transform, screen.Pool.GetCard("Stick"));
        Card_Drag card2 = cardObject2.GetComponent<Card_Drag>();
        screen.CardClick(card1);             //move card from Hand to Craft
        screen.CardClick(card2);             //move card from Hand to Craft
        yield return new WaitForSeconds(1); //wait
        Assert.AreEqual(1, screen.Craft.transform.childCount); //Craft should have one card after refreshing when the craft suceeded
        Destroy(cardObject1);
        yield return null;
    }
    [UnityTest] //Test if Craft zone can detect succeseful tool craft
    public IEnumerator Z_cards_testMenuCraftZoneOpens()
    {
        yield return new WaitForSeconds(1); //wait till game opens
        Screen_Cards screen = FindObjectOfType<Screen_Cards>();
        foreach (Transform cardObject in screen.Hand.transform)    //clear whatever is in Hand
        {
            GameObject.Destroy(cardObject.gameObject);
        }
        GameObject cardObject1 = screen.CreateObject(screen.Hand.transform, screen.Pool.GetCard("Stick"));
        Card_Drag card1 = cardObject1.GetComponent<Card_Drag>();
        GameObject cardObject2 = screen.CreateObject(screen.Hand.transform, screen.Pool.GetCard("Flint"));
        Card_Drag card2 = cardObject2.GetComponent<Card_Drag>();
        screen.CardClick(card1);             //move card from Hand to Craft
        screen.CardClick(card2);             //move card from Hand to Craft
        screen.Craft.GetComponent<Zone_Craft>().RefreshZone();
        yield return new WaitForSeconds(1); //wait
        Assert.AreNotEqual(0, screen.CraftMenu.transform.childCount); //Craft menu should have cards after tool was crafted
        yield return null;
    }
    [UnityTest] //Test if Menu Craft zone can detect that it needs to close itself
    public IEnumerator ZA_cards_testMenuCraftZoneCloses()
    {
        yield return new WaitForSeconds(1); //wait till game opens
        Screen_Cards screen = FindObjectOfType<Screen_Cards>();
        Card_Drag[] card = screen.CraftMenu.GetComponentsInChildren<Card_Drag>();
        screen.CardClick(card[0]);
        screen.Craft.GetComponent<Zone_Craft>().ClearMenu();
        yield return new WaitForSeconds(1); //wait
        Assert.AreEqual(0, screen.CraftMenu.transform.childCount); //Craft menu should have no cards after it was closed
        yield return null;
    }
    [UnityTest] //Test if adding cards to storage works
    public IEnumerator ZB_cards_testStorage()
    {
        yield return new WaitForSeconds(1); //wait till game opens
        Screen_Cards screen = FindObjectOfType<Screen_Cards>();
        foreach (Transform cardObject in screen.Hand.transform)    //clear whatever is in Hand
        {
            GameObject.Destroy(cardObject.gameObject);
        }
        GameObject newCard = screen.CreateObject(screen.Hand.transform, screen.Pool.cards[0]);
        Card_Drag Card = newCard.GetComponent<Card_Drag>();
        GameObject newCard2 = screen.CreateObject(screen.Hand.transform, screen.Pool.cards[0]);
        Card_Drag Card2 = newCard.GetComponent<Card_Drag>();
        screen.SwitchStorage(); //open storage
        Time.timeScale = 1f;    //let the game keep running
        Card.SwitchCardPlace();
        Card2.SwitchCardPlace();
        int takenSlots = screen.Storage.GetComponent<Zone_Storage>().count;
        Assert.AreNotEqual(0, takenSlots);   //test if a slot got taken in storage
        yield return null;
    }
    [UnityTest] //Test if adding cards from book works
    public IEnumerator ZC_cards_testBook()
    {
        yield return new WaitForSeconds(1); //wait till game opens
        Screen_Cards screen = FindObjectOfType<Screen_Cards>();
        foreach (Transform cardObject in screen.Hand.transform)    //clear whatever is in Hand
        {
            GameObject.Destroy(cardObject.gameObject);
        }
        screen.SwitchCreative(); //open book
        Time.timeScale = 1f;    //let the game keep running
        Card_Display[] card = screen.Book.GetComponent<Zone_Book>().GetComponentsInChildren<Card_Display>();
        card[0].PickedCard();
        Assert.AreNotEqual(screen.Hand.transform.childCount, 0);   //test if card was added to hand
        screen.SwitchCreative(); //close book
        yield return null;
    }

    // Testing of Event related functions in Zone_Behaviour and Card_Drag is impossible
    // due to their dependency on recieving event data from the UnityEngine.EventSystems Library
    // by using an official Unity Library I am recieving the assuance that it's able to detect changes in pointer placement and so on
}
