using System.Collections.Generic;
using UnityEngine;
using System.Linq;  //needed for ToList function

//collection of all existing cards
public class Card_Pool : ScriptableObject
{
    public static List<Data_Card> cards;    //list of all existing cards, can be called from any part of the program
    public static int count;
    public int discoveredTotal = 0;         //for statistics

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
    public bool IsCombination(Data_Card card)     //find if card is listed as combination
    {
        for (int i = 0; i < card.source.Count; i++) 
            if (card.source[i].ToString() == "Combination")
                return true;
        return false;
    }
    public bool IsMenuCombination(Data_Card card)  //find if card is listed as menu combination
    {
        for (int i = 0; i < card.source.Count; i++)
            if (card.source[i].ToString() == "Menu")
                return true;
        return false;
    }
    public int FindCard(string cardName)    //gets card index from name
    {
        int i=0;
        for (; i < count && cards[i].name != cardName; i++) ;
        return i;
    }
    public int FindCard(int cardCode)       //gets card index from code
    {
        int i=0;
        for (; i < count && cards[i].code != cardCode; i++) ;
        return i;
    }
    public Data_Card GetCard(string cardName)   //gets card from name
    {
        int i = 0;
        for (; i < count && cards[i].name != cardName; i++) ;
        return cards[i];
    }
    public Data_Card GetCard(int cardCode)      //gets card from code
    {
        int i=0;
        for (; i < count && cards[i].code != cardCode; i++) ;
        return cards[i];
    }
    private List<int> TotalComplexities(int threshold)      //returns list of all complexities above threshold
    {
        List<int> complexities = new List<int>();
        complexities.Add(threshold);
        for (int i = 0; i < count; i++)
            if (cards[i].complexity > complexities[complexities.Count - 1])
            {
                complexities.Add(cards[i].complexity);
                complexities.OrderBy(complexities => complexities);
            }
        return complexities;
    }
    private List<Data_Card> PickedComplexity(int complexity)//returns list of all cards with set complexity
    {
        List<Data_Card> cardList = new List<Data_Card>();
        for (int i = 0; i < count; i++)
            if (cards[i].complexity == complexity)
                cardList.Add(cards[i]);
        return cardList;
    }
    public Data_Card GetLoot(int threshold)                 //gets random card above complexity, higher complexity => smaller probability
    {
        List<Data_Card> possibleCards = new List<Data_Card>(PickedComplexity(threshold));//fill list with a default - list with complexity=threshold
        List<int> complexities = TotalComplexities(threshold);
        int random = Random.Range(1, 101);
        for (int i = 1; i < complexities.Count; i++)
        {
            if (random <= ((float)(complexities.Count - i) / complexities.Count) * 100)  //smaller random => higher complexity that can get added
                for (int j = 0; j < count; j++)
                {
                    if (cards[j].complexity == complexities[i])
                        possibleCards.Add(cards[j]);
                }
        }
        return possibleCards[Random.Range(0, possibleCards.Count)];
    }
    public string FillObject(GameObject cardObject)                 //add random card to object + return the new card name (for displaying in Scene)
    {
        Data_Card newCard = cards[Random.Range(0, count)];
        cardObject.GetComponent<Card_Display>().AddCard(newCard);
        return newCard.name;
    }
    public string FillObject(GameObject cardObject, int cardCode)   //add card of given code to object + return the new card name (for displaying in Scene)
    {
        Data_Card newCard = cards[FindCard(cardCode)];
        cardObject.GetComponent<Card_Display>().AddCard(newCard);
        return newCard.name;
    }
    public string FillObject(GameObject cardObject, string cardName)//add card of given name to object + return the new card name (for displaying in Scene)
    {
        Data_Card newCard = cards[FindCard(cardName)];
        cardObject.GetComponent<Card_Display>().AddCard(newCard);
        return newCard.name;
    }
    public Data_Card FindCombo(Data_Card first, Data_Card second)               //returns card created from first+second
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
    public List<Data_Card> FindMenuCombo(Data_Card first, Data_Card second)     //returns list of all cards created from first+second
    {
        List<Data_Card> cardList = new List<Data_Card>();
        Data_Card CombineAttempt;
        for (int startCard = 0; startCard < count; startCard++)
        {
            CombineAttempt = cards[startCard];
            List<Data_Card.Combinations> Combos = CombineAttempt.combinationOf;
            if (Combos.Count > 0) //failsafe - will catch bugged out cards that don't have all their fields filled out
            {
                if (IsMenuCombination(CombineAttempt))          //if card can be created via a menu
                {
                    for (int startCombo = 0; startCombo < Combos.Count; startCombo++)   //check all possible combinations of current CombineAttempt
                    {
                        if (Combos[startCombo].card1 == first && Combos[startCombo].card2 == second)
                            cardList.Add(CombineAttempt);
                        else if (Combos[startCombo].card2 == first && Combos[startCombo].card1 == second)
                            cardList.Add(CombineAttempt);
                    }
                }
            }
        }
        return cardList;
    }
}
