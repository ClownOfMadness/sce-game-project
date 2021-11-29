using System.Collections.Generic;
using UnityEngine;
using System.Linq;  //needed for ToList function

//collection of all existing cards
public class CardPool : ScriptableObject
{
    public List<Card> cards;    //list of all existing cards

    void Awake()               
    {
        cards = Resources.LoadAll<Card>("").ToList();   //fills list from Resources folder
    }
    public string FillObject(GameObject cardObject)     //add card to object + return the new card name (for displaying in Scene)
    {
        Card newCard = cards[Random.Range(0, cards.Count)];
        cardObject.GetComponent<CardDisplay>().AddCard(newCard);
        return newCard.name;
    }
    public Card FindCombo(Card first, Card second)  //returns what first+second card create
    {
        Card CombineAttempt;
        for (int startCard = 0; startCard < cards.Count; startCard++)
        {
            CombineAttempt = cards[startCard];
            List<Card.Combinations> Combos = CombineAttempt.combinationOf;
            if (CombineAttempt.source[0].ToString() == "Combination")    //if card can be created
            {
                for (int startCombo = 0; startCombo < Combos.Count; startCombo++)   //check all possible combinations of current CombineAttempt
                {
                    if (Combos[startCombo].card1 == first && Combos[startCombo].card2 == second)
                        return CombineAttempt;
                    else if (Combos[startCombo].card2 == first && Combos[startCombo].card1 == second)
                        return CombineAttempt;
                }
            }
        }
        return null;
    }
}
