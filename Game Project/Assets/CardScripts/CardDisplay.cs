using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{ 
    public Card card;
    public Text nameText;
    public Image artwok;
    void Start()
    {   //allows you to connect the card data to the ui
        nameText.text = card.name;
        artwok.sprite = card.artwork;
        
    }
}
