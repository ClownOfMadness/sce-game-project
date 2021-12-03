using UnityEngine;
using UnityEngine.UI;

//stores the CardDisplays that appear in the Zones
public class CardDisplay : MonoBehaviour
{
    public Card card;
    public Text nameText;
    public Image artwork;
    private bool clickeable;
    [HideInInspector] public Canvas canvas;   //needed for DragCard & clicking on cards functions
    [HideInInspector] public ScreenCards screen;   //needed for DragCard & clicking on cards functions

    void Start()
    {
        canvas = this.transform.parent.parent.parent.GetComponent<Canvas>();
        screen = canvas.GetComponent<ScreenCards>();
    }
    public void AddCard(Card _card) //fill fields in card on screen according to card data
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
