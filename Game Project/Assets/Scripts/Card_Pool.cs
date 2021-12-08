using System.Collections.Generic;
using UnityEngine;
using System.Linq;  //needed for ToList function

//collection of all existing cards
public class Card_Pool : ScriptableObject
{
    public static List<Data_Card> cards;    //list of all existing cards, can be called from any part of the program
    public static int count;

    void Awake()               
    {
        cards = Resources.LoadAll<Data_Card>("Cards").ToList();  //fills list from Resources folder
        count = cards.Count;
        CheckCards();
    }
    private void CheckCards()   //verifies card fields in case their fields are incorrect (can be removed at a later stage)
    {
        for (int i = 0; i < count; i++)
        {
            if (cards[i].source.Count == 0)                //cards without a source are invalid
                Debug.LogError("Error! Card " + cards[i].code + " is invalid, please set at least one source.");
            else if (IsCombination(cards[i]))
                if (cards[i].combinationOf.Count == 0)  //combination cards without a combination listed are invalid
                    Debug.LogError("Error! Combination of card " + cards[i].code + " is invalid, please add at least one set of cards that craft it.");
                else if (cards[i].combinationOf[0].card1 == null || cards[i].combinationOf[0].card2 == null)
                    Debug.LogError("Error! Combination of card " + cards[i].code + " is invalid, please add at least one set of cards that craft it.");
        }
    }
    private static bool IsCombination(Data_Card card)       //find if card is listed as combination
    {
        for (int i = 0; i < card.source.Count; i++) 
            if (card.source[i].ToString() == "Combination")
                return true;
        return false;
    }
    public int FindCard(string cardName)    //gets card index from name
    {
        int i;
        for (i = 0; i < count && cards[i].name != cardName; i++) ;
        return i;
    }
    public int FindCard(int cardCode)       //gets card index from code
    {
        int i;
        for (i = 0; i < count && cards[i].code != cardCode; i++) ;
        return i;
    }
    public string FillObject(GameObject cardObject)             //add random card to object + return the new card name (for displaying in Scene)
    {
        Data_Card newCard = cards[Random.Range(0, count)];
        cardObject.GetComponent<Card_Display>().AddCard(newCard);
        return newCard.name;
    }
    public string FillObject(GameObject cardObject, int cardCode)  //add card of given code to object + return the new card name (for displaying in Scene)
    {
        Data_Card newCard = cards[FindCard(cardCode)];
        cardObject.GetComponent<Card_Display>().AddCard(newCard);
        return newCard.name;
    }
    public string FillObject(GameObject cardObject, string cardName)  //add card of given name to object + return the new card name (for displaying in Scene)
    {
        Data_Card newCard = cards[FindCard(cardName)];
        cardObject.GetComponent<Card_Display>().AddCard(newCard);
        return newCard.name;
    }
    public static Data_Card FindCombo(Data_Card first, Data_Card second)   //returns what first+second card create
    {
        Data_Card CombineAttempt;
        for (int startCard = 0; startCard < count; startCard++)
        {
            CombineAttempt = cards[startCard];
            List<Data_Card.Combinations> Combos = CombineAttempt.combinationOf;
            if (Combos.Count > 0) //failsafe - will catch bugged out cards that don't have all their fields filled out
            {
                if (IsCombination(CombineAttempt))          //if card can be created
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
        }
        return null;
    }
}
