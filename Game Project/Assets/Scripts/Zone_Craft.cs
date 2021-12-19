using System.Collections.Generic;
using UnityEngine;

//responsible for creating Craft zone
public class Zone_Craft : MonoBehaviour
{
    public GameObject CardPrefab;                   //type of prefab for Card (attached via Inspector)
    [HideInInspector] public int Size;              //Zone size
    [HideInInspector] public bool success;          //only needed for TopMessage in Screen_Cards
    [HideInInspector] public int CombosTotal = 0;   //for statistics
    private Card_Pool Pool;


    void Awake()
    {
        Size = 2;   //max Zone size
        Pool = ScriptableObject.CreateInstance<Card_Pool>();        //open Card_Pool connection to use its functions
    }

    public void EventClick()
    {
        if (this.transform.childCount == this.Size)     //failsafe - verifying that there's two objects in the Zone
        {
            Card_Drag[] cardObjects = this.gameObject.transform.GetComponentsInChildren<Card_Drag>();
            Data_Card first = cardObjects[0].card;
            Data_Card second = cardObjects[1].card;
            Data_Card combination = Pool.FindCombo(first, second);         //find if cards can be combined
            if (combination)
            {
                if (Pool.IsCombination(combination))        //if it's a normal combination
                {
                    if (combination.neverDiscovered)
                    {
                        combination.neverDiscovered = false;
                        //Debug.Log("New card found: " + combination.name);
                        Pool.discoveredTotal++;
                    }

                    Destroy(cardObjects[1].gameObject);
                    cardObjects[0].AddCard(combination);    
                    string newName = combination.name;                                  //add cards to objects + save the new card name (for displaying in Scene)
                    cardObjects[0].name = string.Format("{0} (Card)", newName);         //updates name in scene

                    CombosTotal++;
                    success = true;
                }
                else if (Pool.IsMenuCombination(combination)) //if it's a combination that you pick via a menu
                {
                    List<Data_Card> combinations = Pool.FindMenuCombo(first, second);   //find all cards that first+second make
                    //instantiate Drag_Cards for list and place in a panel
                    //add part in Screen_cards that checks if card is in new panel, adds picked to hand, and clears+hides panel
                }
            }
            else //nothing was crafted
            {
                success = false;
                cardObjects[1].ReturnToHand();
            }
            cardObjects[0].ReturnToHand();  //returns at least something by default
        }
    }
}

