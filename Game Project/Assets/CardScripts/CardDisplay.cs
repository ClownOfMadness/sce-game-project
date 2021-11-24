using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{ 
    public Card card;
    public Text nameText;
    public Image artwork;

    public void addCard(Card _card)
    {
        card = _card;
        nameText.text = _card.name;
        artwork.sprite = _card.artwork;

    }
}
