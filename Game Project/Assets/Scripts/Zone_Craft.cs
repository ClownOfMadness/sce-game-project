using System.Collections.Generic;
using UnityEngine;

//responsible for creating Craft zone
public class Zone_Craft : MonoBehaviour
{
    //public fields:
    [HideInInspector] public int Size;              //Zone size
    //[HideInInspector] public int CombosTotal = 0;   //for statistics

    //internal fields:
    private GameObject CraftMenu;

    //external access:
    [HideInInspector] public Screen_Cards screen;
    private Card_Pool Pool; //open Card_Pool connection to use its functions

    public void InstantiateZone()
    {
        Size = 2;   //max Zone size

        Pool = screen.Pool;
        CraftMenu = screen.CraftMenu;
    }
    public void RefreshZone()    //card was added to Zone
    {
        if (this.transform.childCount == this.Size)     //failsafe - verifying that there's two objects in the Zone
        {
            Card_Drag[] cardObjects = this.gameObject.transform.GetComponentsInChildren<Card_Drag>();
            Data_Card first = cardObjects[0].card;
            Data_Card second = cardObjects[1].card;
            Data_Card combination = Pool.FindCombo(first, second);         //find if cards can be combined
            if (combination)
            {   //if it's a normal combination
                if (Pool.IsCombination(combination))
                {
                    screen.CardsCombined++;
                    screen.TopMessage(string.Format("Craft succeseful! Created {0}", combination.name));

                    Destroy(cardObjects[1].gameObject);
                    cardObjects[0].AddCard(combination);//add cards to objects + save the new card name (for displaying in Scene)
                    cardObjects[0].name = string.Format("{0} (Card)", combination.name);    //updates name in scene
                }
                //if it's a combination that you pick via a menu
                else if (Pool.IsMenuCombination(combination))
                {
                    List<Data_Card> combinations = Pool.FindMenuCombo(first, second);   //find all cards that first+second make
                    for (int i = 0; i < combinations.Count; i++)
                    {
                        screen.CreateObject(CraftMenu.transform, combinations[i]);
                    }

                    screen.CardsCombined++;
                    screen.TopMessage(string.Format("Craft succeseful! Pick one card to add to your deck"));

                    this.gameObject.SetActive(false);
                    CraftMenu.SetActive(true);

                    Destroy(cardObjects[0].gameObject);
                    Destroy(cardObjects[1].gameObject);
                }
            }
            else //nothing was crafted
            {
                screen.CraftFail(cardObjects[0], cardObjects[1]);
            }
        }
    }
    public void ClearMenu()
    {
        foreach (Transform cardObject in CraftMenu.transform)
        {
            GameObject.Destroy(cardObject.gameObject);
        }
        CraftMenu.SetActive(false);
    }

}