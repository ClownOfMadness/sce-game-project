using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//responsible for creating and storing Book zone, extension of ZoneBehaviour
public class ZoneBook : ZoneBehaviour
{
    int PageSize = 8;
    public GameObject PagePrefab;

    public Text backButton;
    public Text nextButton;
    private int pageIndex;
    private int pagesInBook;

    public void Start()
    {
        FillBook();
        this.gameObject.SetActive(false);
    }
    private void FillBook()
    {
        CardPool pool = ScriptableObject.CreateInstance<CardPool>();    //open CardPool connection to use its functions
        Size = 4;           //max Zone size
        int cardIndex = 0;  //keep track of what card we're adding
        pagesInBook = 0;    //will store maximum pages in book
        while (cardIndex < pool.cards.Count)
        {
            GameObject newPage = Instantiate(PagePrefab, this.transform);           //create and instantiate Page objects in scene
            newPage.name = string.Format("Page {0}", pagesInBook + 1);              //new Page name (for displaying in Scene)
            for (int inPage = 0; inPage < PageSize && cardIndex < pool.cards.Count; inPage++, cardIndex++)
            {
                GameObject newCard = Instantiate(CardPrefab, newPage.transform);    //create and instantiate card objects in scene
                newCard.GetComponent<CardDrag>().AddCard(pool.cards[cardIndex]);
                string newName = pool.cards[cardIndex].name;                        //save the new card name (for displaying in Scene)
                Debug.Log("Card " + newName + " added to book.");
                newCard.name = string.Format("{0} (Card)", newName);                //updates name in scene
                newCard.transform.localScale -= new Vector3((CardPrefab.transform.localScale.x) / 3, (CardPrefab.transform.localScale.y) / 3, 0);
            }
            if (pagesInBook > 0)
                newPage.SetActive(false);
            pagesInBook++;
        }
        backButton.CrossFadeAlpha(0.0f, 0.05f, false);
        nextButton.gameObject.transform.SetAsLastSibling();     //move button to the far right
        pageIndex = 1;  //stores what page we're on
    }
    public void NextPage()
    {
        if (pageIndex < pagesInBook)
        {
            if (pageIndex == 1)
                backButton.CrossFadeAlpha(1.0f, 0.05f, false);
            this.gameObject.transform.GetChild(pageIndex).gameObject.SetActive(false);  //hide last page
            pageIndex++;
            this.gameObject.transform.GetChild(pageIndex).gameObject.SetActive(true);   //show new page
            if (pageIndex == pagesInBook)
                nextButton.CrossFadeAlpha(0.0f, 0.05f, false);
        }
    }
    public void BackPage()
    {
        if (pageIndex > 1)
        {
            if (pageIndex == pagesInBook)
                nextButton.CrossFadeAlpha(1.0f, 0.05f, false);
            this.gameObject.transform.GetChild(pageIndex).gameObject.SetActive(false);  //hide last page
            pageIndex--;
            this.gameObject.transform.GetChild(pageIndex).gameObject.SetActive(true);   //show new page
            if (pageIndex == 1)
                backButton.CrossFadeAlpha(0.0f, 0.05f, false);
        }
    }
    private void FirstPage()
    {
        nextButton.CrossFadeAlpha(1.0f, 0.05f, false);
        this.gameObject.transform.GetChild(pageIndex).gameObject.SetActive(false);  //hide last page
        pageIndex = 1;
        this.gameObject.transform.GetChild(pageIndex).gameObject.SetActive(true);   //show new page
        backButton.CrossFadeAlpha(0.0f, 0.05f, false);
    }
    public void CreativeButton()
    {
        if (!this.gameObject.activeSelf)         //if inactive turn on
            this.gameObject.SetActive(true);
        else
        {
            FirstPage();
            this.gameObject.SetActive(false);  //if active turn off
        }
    }
}


