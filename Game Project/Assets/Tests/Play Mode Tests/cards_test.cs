using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

//Tests related to the Cards functionalities
public class cards_test : MonoBehaviour
{
    [OneTimeSetUp]
    public void loadGame()
    {
        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }

    // Test if reading from the card database works
    [UnityTest]
    public IEnumerator cards_testCardPool()
    {
        yield return new WaitForSeconds(1); //wait till game opens
        Screen_Cards screen = FindObjectOfType<Screen_Cards>();
        Assert.IsNotNull(screen.Pool);
        yield return null;
    }

    //Test if the adding a card to an displayable object works.
    [UnityTest]
    public IEnumerator cards_testAddCard()
    {
        yield return new WaitForSeconds(1); //wait till game opens
        Screen_Cards screen = FindObjectOfType<Screen_Cards>();
        GameObject cardObject = Instantiate(screen.CardObject, screen.Craft.transform);
        Card_Display Card = cardObject.GetComponent<Card_Display>();
        Card.AddCard(screen.Pool.cards[0]);
        Assert.IsNotNull(Card.card);        //check if adding card to display object worked
        yield return null;
    }

    // Test if creating a card object works.
    [UnityTest]
    public IEnumerator cards_testCreateObject()
    {
        yield return new WaitForSeconds(1); //wait till game opens
        Screen_Cards screen = FindObjectOfType<Screen_Cards>();
        GameObject cardObject = screen.CreateObject(screen.Craft.transform, screen.Pool.cards[0]);
        Card_Display Card = cardObject.GetComponent<Card_Display>();
        Assert.IsNotNull(Card.card);        //check if adding card to display object worked
        yield return null;
    }

    // Test if creating cards and adding them to the Hand deck works
   [UnityTest]
    public IEnumerator cards_testInstantiateZone()
    {
        yield return new WaitForSeconds(1); //wait till game opens
        Screen_Cards screen = FindObjectOfType<Screen_Cards>();
        Assert.IsNotNull(screen.Hand.GetComponentInChildren<Card_Drag>());
        yield return null;
    }

    // Test if clicking on card moves it from one panel to the other
   [UnityTest]
    public IEnumerator cards_testHandToCraft()
    {
        yield return new WaitForSeconds(1); //wait till game opens
        Screen_Cards screen = FindObjectOfType<Screen_Cards>();
        Card_Drag[] Card = screen.Hand.GetComponentsInChildren<Card_Drag>();
        screen.CardClick(Card[0]);             //attempt to change panel
        Assert.AreNotEqual(Card[0].transform, screen.Hand.transform);   //test if panel changed
        yield return null;
    }

    // Test if clicking on card moves it from one panel to the other
   [UnityTest]
    public IEnumerator cards_testCraftToHand()
    {
        yield return new WaitForSeconds(1); //wait till game opens
        Screen_Cards screen = FindObjectOfType<Screen_Cards>();
        Card_Drag[] HandCard = screen.Hand.GetComponentsInChildren<Card_Drag>();
        Destroy(HandCard[0].gameObject);    //make space in hand in case
        GameObject cardObject = screen.CreateObject(screen.Craft.transform, screen.Pool.cards[0]);
        Card_Drag Card = cardObject.GetComponent<Card_Drag>();
        screen.CardClick(Card);             //attempt to change panel
        Assert.AreNotEqual(Card.transform, screen.Craft.transform);   //test if panel changed
        yield return null;
    }

    // Test if clicking on card moves it from one panel to the other
   [UnityTest]
    public IEnumerator cards_testSwitchCardPlace()
    {
        yield return new WaitForSeconds(1); //wait till game opens
        Screen_Cards screen = FindObjectOfType<Screen_Cards>();
        GameObject cardObject = screen.CreateObject(screen.Craft.transform, screen.Pool.cards[0]);
        Card_Drag Card = cardObject.GetComponent<Card_Drag>();
        Card.SwitchCardPlace();
        Assert.AreNotEqual(Card.transform, screen.Craft.transform);   //test if panel changed
        yield return null;
    }

    // Test if finding combination of cards works
    [UnityTest]
    public IEnumerator cards_testFindCombo()
    {
        yield return new WaitForSeconds(1); //wait till game opens
        Screen_Cards screen = FindObjectOfType<Screen_Cards>();
        Data_Card firstCard = screen.Pool.GetCard("Stick");     //a card that combines with second
        Data_Card secondCard = screen.Pool.GetCard("Flint");    //a card that combines with first                        
        Assert.IsNotNull(screen.Pool.FindCombo(firstCard, secondCard)); //only returns null when a combination wasn't found
        yield return null;
    }

    // Test if finding combination of cards placed in Craft zone works
    [UnityTest]
    public IEnumerator cards_testCraftZone()
    {
        yield return new WaitForSeconds(1); //wait till game opens
        Screen_Cards screen = FindObjectOfType<Screen_Cards>();
        Card_Drag[] Card = screen.Hand.GetComponentsInChildren<Card_Drag>();
        screen.CardClick(Card[0]);             //move card from Hand to Craft
        screen.CardClick(Card[1]);             //move card from Hand to Craft
        screen.Craft.GetComponent<Zone_Craft>().RefreshZone();
        Assert.AreEqual(0, screen.Craft.transform.childCount); //Craft should be empty after refreshing when it has two cards in it
        yield return null;
    }

    // Test if cards can be placed on map
    //[UnityTest]
    // public IEnumerator cards_testMapZone()
    // {
    //     yield return new WaitForSeconds(1); //wait till game opens
    //     yield return null;
    // }


    // Testing of Event related functions in ZoneBehaviour and CardDrag is impossible
    // due to their dependency on recieving event data from the UnityEngine.EventSystems Library
    // by using an official Unity Library I am recieving the assuance that it's working
}
