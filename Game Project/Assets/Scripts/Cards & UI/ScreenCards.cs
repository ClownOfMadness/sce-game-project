using UnityEngine;

//responsible for switching panels and button related functions
public class ScreenCards : MonoBehaviour
{
    public GameObject Hand;
    public GameObject Craft;
    public GameObject Book;
    public GameObject Map;
    public GameObject creativeButton;
    public GameObject destroyButton;
    [Header("Game State:")]
    public bool SkipLogin;
    private ZoneHand zHand;
    private ZoneCraft zCraft;
    private ZoneBook zBook;

    void Start()    //initilizing in case something was off
    {
        zHand = Hand.transform.GetComponent<ZoneHand>();
        zCraft = Craft.transform.GetComponent<ZoneCraft>();
        zBook = Book.transform.GetComponent<ZoneBook>();

        Hand.SetActive(true);
        Book.SetActive(false);
        Craft.SetActive(true);
        destroyButton.SetActive(true);
        if (SkipLogin)  //for development testing
        {
            creativeButton.SetActive(true);
        }
        else
        {
            creativeButton.SetActive(false);
        }

    }
    public void SwitchCreative()
    {
        if (Craft.activeSelf)           //open book, close craft
        {
            Craft.SetActive(false);
            Map.SetActive(false);
            Book.SetActive(true);
        }
        else
        {
            zBook.FirstPage();
            Book.SetActive(false);  //open craft, close book
            Craft.SetActive(true);
            Map.SetActive(true);
        }
    }
    public void HandToCraft(CardDrag card)  //move card from Hand zone to Hand on click
    {
        if (Craft.activeSelf)
        {
            if (card.transform.parent == Hand.transform)
            {
                card.transform.SetParent(Craft.transform);
                if (Craft.transform.childCount == zCraft.Size)
                    zCraft.EventClick();
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
        if (Hand.transform.childCount < zHand.Size)
        {
            GameObject newCard = Instantiate(zHand.CardPrefab, Hand.transform);     //create and instantiate object in scene
            newCard.GetComponent<CardDrag>().AddCard(pickedCard.card);              //add cards to objects 
            newCard.name = string.Format("{0}", pickedCard.name);                   //update new card name (for displaying in Scene)
        }
    }
}
