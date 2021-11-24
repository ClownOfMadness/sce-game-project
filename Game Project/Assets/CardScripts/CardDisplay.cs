using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//stores the CardDisplays that appear in the Zones
public class CardDisplay : MonoBehaviour
{ 
    public Card card;
    public Text nameText;
    public Image artwork;

    public CardDisplay(Card _card)
    {
        card = _card;
        nameText.text = card.name;
        artwork.sprite = card.artwork;
    }
    public void addCard(Card _card)
    {
        card = _card;
        nameText.text = card.name;
        artwork.sprite = card.artwork;
    }
}
