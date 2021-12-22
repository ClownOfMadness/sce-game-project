using System.Collections.Generic;
using UnityEngine;

//responsible for creating Craft zone
public class Zone_Craft : MonoBehaviour
{
    public GameObject CardPrefab;                   //type of prefab for Card (attached via Inspector)
    [HideInInspector] public int Size;              //Zone size
    [HideInInspector] public int CombosTotal = 0;   //for statistics
    public GameObject craftMenu;
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
                    CombosTotal++;
                    cardObjects[0].screen.TopMessage(string.Format("Craft succeseful! Created {0}", cardObjects[0].card.name));

                    Destroy(cardObjects[1].gameObject);
                    cardObjects[0].AddCard(combination);    
                    string newName = combination.name;                                  //add cards to objects + save the new card name (for displaying in Scene)
                    cardObjects[0].name = string.Format("{0} (Card)", newName);         //updates name in scene
                }
                else if (Pool.IsMenuCombination(combination)) //if it's a combination that you pick via a menu
                {
                    List<Data_Card> combinations = Pool.FindMenuCombo(first, second);   //find all cards that first+second make
                    for (int i = 0; i < combinations.Count; i++)
                    {
                        GameObject newCard = Instantiate(CardPrefab, craftMenu.transform);     //create and instantiate object in scene
                        newCard.GetComponent<Card_Drag>().AddCard(combinations[i]);            //add cards to objects 
                        newCard.name = string.Format("{0} (Card)", combinations[i].name);      //update new card name (for displaying in Scene)
                    }
                    //zMenu.Size = combinations.Count;   //add them to menu

                    CombosTotal++;
                    cardObjects[0].screen.TopMessage(string.Format("Craft succeseful! Pick one card to add to your deck"));

                    this.gameObject.SetActive(false);
                    cardObjects[0].screen.visibleMap = false;
                    craftMenu.SetActive(true);

                    Destroy(cardObjects[0].gameObject);
                    Destroy(cardObjects[1].gameObject);
                   
                    Time.timeScale = 0f;    //pause game
                }
            }
            else //nothing was crafted
            {
                cardObjects[0].SwitchCardPlace();
                cardObjects[1].SwitchCardPlace();
            }
        }
    }
    public void ClearMenu()
    {
        foreach (Transform cardObject in craftMenu.transform)
        {
            GameObject.Destroy(cardObject.gameObject);
        }
        craftMenu.gameObject.SetActive(false);
        Time.timeScale = 1f;    //resume game
    }
}