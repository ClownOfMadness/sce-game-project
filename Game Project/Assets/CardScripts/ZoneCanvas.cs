using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//responsible for switching panels and button related functions
public class ZoneCanvas : MonoBehaviour
{
    public GameObject Hand;
    public GameObject Craft;
    public GameObject Book;
    public GameObject creativeButton;
    public GameObject destroyButton;

    bool on = true;
    bool off = false;


    void Start()
    {
        Hand.SetActive(on);
        Book.SetActive(off);
        Craft.SetActive(on);
        creativeButton.SetActive(on);
        destroyButton.SetActive(on);
    }
    public void SwitchCreative()
    {
        if (!Book.activeSelf)        //open book, close craft
        {
            Craft.SetActive(off);
            Book.SetActive(on);
        }
        else
        {
            Book.GetComponent<ZoneBook>().FirstPage();
            Book.SetActive(off);  //open craft, close book
            Craft.SetActive(on);
        }
    }
    public void HandToCraft()
    {
        this.transform.SetParent(Craft.transform);
    }
    public void BookToHand()
    {
        if (Hand.transform.GetComponentsInChildren<CardDrag>().Length < Hand.GetComponent<ZoneHand>().Size)
        {
            GameObject newCard = Instantiate(Hand.GetComponent<ZoneHand>().CardPrefab, Hand.transform);   //create and instantiate object in scene
            Book.GetComponent<ZoneBook>().AddToHand(this.GetComponent<CardDisplay>(), newCard);
        }
    }
}
