using UnityEngine;
using UnityEngine.UI;

//responsible for the display only cards
public class Card_Display : MonoBehaviour
{
    //public fields:
    public Data_Card card;
    public Text nameText;
    public Image artwork;

    //internal fields:
    private bool clickeable;    //blocks empty cards from being clicked

    //external access:
    [HideInInspector] public Screen_Cards screen;   //needed for Card_Drag & clicking on cards functions

    public void AddCard(Data_Card _card) //fill fields in card on screen according to card data
    {
        card = _card;
        nameText.text = card.name;
        artwork.sprite = card.artwork;
        clickeable = true;
    }
    public void ClearCard() //fill fields in card on screen according to card data
    {
        card = null;
        nameText.text = "";
        artwork.sprite = null;
        clickeable = false;
    }
    public void PickedCard() //Adds card to hand from book
    {
        if(clickeable)
            screen.DisplayCardClick(this);
    }
}
