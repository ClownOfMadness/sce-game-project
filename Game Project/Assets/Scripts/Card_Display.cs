using UnityEngine;
using UnityEngine.UI;

//stores the Card_Displays that appear in the Zones
public class Card_Display : MonoBehaviour
{
    public Data_Card card;
    public Text nameText;
    public Image artwork;
    private bool clickeable;
    [HideInInspector] public Canvas canvas;   //needed for DragCard & clicking on cards functions
    [HideInInspector] public Screen_Cards screen;   //needed for DragCard & clicking on cards functions

    void Start()
    {
        canvas = this.transform.parent.parent.parent.GetComponent<Canvas>();
        screen = canvas.GetComponent<Screen_Cards>();
    }

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
            screen.ClickToHand(this);
    }
}
