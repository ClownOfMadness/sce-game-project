using NUnit.Framework;
using UnityEngine;

//Tests related to the Cards functionalities
public class cards_test
{
    /*
    CardPool pool = ScriptableObject.CreateInstance<CardPool>();        //open CardPool connection to use its functions

    // Test if reading from the card database works
    [Test]
    public void cards_testCardPool()
    {
        Assert.IsNotNull(CardPool.cards);
    }

    // Test if the adding a card to an displayable object works.
    [Test]
    public void cards_testAddCard()
    {
        CardDisplay Card = new CardDisplay();
        Card.AddCard(CardPool.cards[0]);        //attempting to add a card 
        Assert.IsNotNull(Card.card);        //check if adding card to display object worked
    }

    // Test if the adding a card to an object works.
    [Test]
    public void cards_testFillObject()
    {
        Assert.IsNotEmpty(pool.FillObject(new GameObject()));
    }

    // Test if creating cards and adding them to the Hand deck works
    [Test]
    public void cards_testInstantiateZone()
    {
        ZoneBehaviour Zone = new ZoneBehaviour();
        ZoneHand Hand = new ZoneHand();
        Hand.Size = 1;
        Hand.InstantiateZone(Zone);         //attempt to fill the Hand deck with cards
        Assert.IsNotNull(Hand.GetComponentInChildren<CardDrag>());
    }

    // Test if finding combination of cards works
    [Test]
    public void cards_testFindCombo()
    {
        ZoneCraft Craft = new ZoneCraft();
        Card firstCard = CardPool.cards[2];     //a card that combines with second
        Card secondCard = CardPool.cards[3];    //a card that combines with first                        
        Assert.IsNotNull(pool.FindCombo(firstCard, secondCard)); //only returns null when a combination wasn't found
    }

    // Test if finding combination of cards placed in Craft zone works
    [Test]
    public void cards_testCraftZone()
    {
        ZoneCraft Craft = new ZoneCraft();
        CardDrag firstCard = new CardDrag();
        CardDrag secondCard = new CardDrag();
        secondCard.AddCard(CardPool.cards[3]);
        firstCard.transform.SetParent(Craft.transform);     //a card that combines with second
        secondCard.transform.SetParent(Craft.transform);    //a card that combines with first
        Assert.AreEqual(Craft.transform.childCount, 0); //CraftZone autoruns when it has two objects in it, then moves them back to hand => should be empty again
    }

    // Test if clicking on card moves it from one panel to the other
    [Test]
    public void cards_testHandToCraft()
    {
        ScreenCards MainScreen = new ScreenCards();
        CardDrag Card = new CardDrag();
        Card.transform.SetParent(null);             //original panel (null)
        MainScreen.HandToCraft(Card);               //attempt to chane panel
        Assert.AreNotEqual(Card.transform, null);    //test if panel changed
    }

    // Test if clicking on card moves it from one panel to the other
    [Test]
    public void cards_testCraftToHand()
    {
        ScreenCards MainScreen = new ScreenCards();
        CardDrag Card = new CardDrag();
        Card.transform.SetParent(null);             //original panel (null)
        MainScreen.CraftToHand(Card);               //attempt to chane panel
        Assert.AreNotEqual(Card.transform, null);   //test if panel changed
    }

    // Test if clicking on card moves it from one panel to the other
    [Test]
    public void cards_testReturnToHand()
    {
        CardDrag Card = new CardDrag();
        Card.transform.SetParent(null);             //original panel (null)
        Card.ReturnToHand();
        Assert.AreNotEqual(Card.transform, null);   //test if panel changed
    }

    // Test if clicking on card moves it from one panel to the other
    [Test]
    public void cards_testSwitchCardPlace()
    {
        CardDrag Card = new CardDrag();
        Card.transform.SetParent(null);             //original panel (null)
        Card.SwitchCardPlace();
        Assert.AreNotEqual(Card.transform, null);   //test if panel changed
    }

    // Test if cards can be placed on map
    [Test]
    public void cards_testMapZone()
    {
        ZoneMap Map = new ZoneMap();
        CardDrag Card = new CardDrag();
        Card.AddCard(CardPool.cards[8]);                //create a building card
        Card.transform.SetParent(Map.transform);
        Assert.IsNotNull(Map.transform.GetComponentInChildren<GameObject>());//MapZone autoruns when it has an objects in it, then turns it into a Tile
    }
    */

    // Testing of Event related functions in ZoneBehaviour and CardDrag is impossible
    // due to their dependency on recieving event data from the UnityEngine.EventSystems Library
    // by using an official Unity Library I am recieving the assuance that it's working
}
