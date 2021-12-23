using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//responsible for creating and storing Book zone
public class Zone_Book : MonoBehaviour
{
    //attached via Inspector:
    public GameObject PagePrefab;   //type of prefab for Page
    public GameObject CardPrefab;   //type of prefab for Card
    public Text backButton;
    public Text nextButton;

    //public fields:
    [HideInInspector] public int pageSize;  //amount of cards allowed per page

    //internal fields:
    private List<GameObject> Pages;
    private int pageIndex;

    //external access:
    [HideInInspector] public Screen_Cards screen;
    private Card_Pool Pool; //open Card_Pool connection to use its functions

    public void InstantiateZone()
    {
        pageSize = 8;   //max Zone size

        Pool = screen.Pool;

        Pages = new List<GameObject>();
        int cardIndex = 0;  //keep track of what card we're adding

        while (cardIndex < Pool.count)
        {
            GameObject newPage = Instantiate(PagePrefab, this.transform);           //create and instantiate Page objects in scene
            newPage.name = string.Format("Page {0}", Pages.Count + 1);              //new Page name (for displaying in Scene)
            
            for (int inPage = 0; inPage < pageSize && cardIndex < Pool.count; cardIndex++) 
            {
                if (Pool.cards[cardIndex].source[0].ToString()!="None") //don't show non real cards (Cardmaster, Creation, TownHall)
                {
                    screen.CreateDisplay(newPage.transform, Pool.cards[cardIndex]);
                    inPage++;
                }
            }
            if (Pages.Count > 0)
                newPage.SetActive(false);
            Pages.Add(newPage);
        }
        backButton.gameObject.SetActive(false);
        nextButton.gameObject.SetActive(true);
        nextButton.gameObject.transform.SetAsLastSibling(); //move button to the far right
        pageIndex = 0;  //stores what Page we're on
    }
    public void NextPage()  //move to next Page on click
    {
        if (pageIndex < Pages.Count - 1) 
        {
            if (pageIndex == 0)
                backButton.gameObject.SetActive(true);
            Pages[pageIndex].SetActive(false);  //hide last page
            pageIndex++;
            Pages[pageIndex].SetActive(true);   //show new page
            if (pageIndex == Pages.Count - 1) 
                nextButton.gameObject.SetActive(false);
        }
    }
    public void BackPage()  //move a Page back on click
    {
        if (pageIndex > 0)
        {
            if (pageIndex == Pages.Count - 1) 
                nextButton.gameObject.SetActive(true);
            Pages[pageIndex].SetActive(false);  //hide last page
            pageIndex--;
            Pages[pageIndex].SetActive(true);   //show new page
            if (pageIndex == 0)
                backButton.gameObject.SetActive(false);
        }
    }
    public void RefreshZone()   //switch back to first page (used when Book is closed)
    {
        nextButton.gameObject.SetActive(true);
        Pages[pageIndex].SetActive(false);  //hide last page
        pageIndex = 0;
        Pages[pageIndex].SetActive(true);   //show new page
        backButton.gameObject.SetActive(false);
    }
}


