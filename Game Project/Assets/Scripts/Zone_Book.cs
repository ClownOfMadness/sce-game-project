using UnityEngine;
using UnityEngine.UI;

//responsible for creating and storing Book zone
public class Zone_Book : MonoBehaviour
{
    public GameObject PagePrefab;   //type of prefab for Page (attached via Inspector)
    public GameObject CardPrefab;   //type of prefab for Card (attached via Inspector)
    public Text backButton;
    public Text nextButton;
    private int pageIndex;
    private int pagesInBook;
    [HideInInspector] public int Size;  //Page size
    private Card_Pool Pool;

    void Start()
    {
        Size = 8;           //max Zone size
        Pool = ScriptableObject.CreateInstance<Card_Pool>();        //open Card_Pool connection to use its functions;
        InstantiateZone();
    }
    private void InstantiateZone()
    {
        int cardIndex = 0;  //keep track of what card we're adding
        pagesInBook = 0;    //will store maximum pages in book
        while (cardIndex < Card_Pool.count)
        {
            GameObject newPage = Instantiate(PagePrefab, this.transform);           //create and instantiate Page objects in scene
            newPage.name = string.Format("Page {0}", pagesInBook + 1);              //new Page name (for displaying in Scene)
            for (int inPage = 0; inPage < Size && cardIndex < Card_Pool.count; cardIndex++) 
            {
                if (Card_Pool.cards[cardIndex].source[0].ToString()!="None") //don't show non real cards (Cardmaster, Creation, TownHall)
                {
                    GameObject newCard = Instantiate(CardPrefab, newPage.transform);            //create and instantiate card objects in scene
                    newCard.GetComponent<Card_Display>().AddCard(Card_Pool.cards[cardIndex]);
                    string newName = Card_Pool.cards[cardIndex].name;                           //save the new card name (for displaying in Scene)
                    newCard.name = string.Format("{0} (Card)", newName);                        //updates name in scene
                    newCard.transform.localScale -= new Vector3((CardPrefab.transform.localScale.x) / 3, (CardPrefab.transform.localScale.y) / 3, 0);
                    inPage++;
                }
            }
            if (pagesInBook > 0)
                newPage.SetActive(false);
            pagesInBook++;
        }
        //backButton.CrossFadeAlpha(0.0f, 0.05f, false);
        backButton.gameObject.SetActive(false);
        nextButton.gameObject.SetActive(true);
        nextButton.gameObject.transform.SetAsLastSibling();             //move button to the far right
        pageIndex = 1;  //stores what Page we're on
    }
    public void NextPage()  //move to next Page on click
    {
        if (pageIndex < pagesInBook)
        {
            if (pageIndex == 1)
                backButton.gameObject.SetActive(true);
            this.gameObject.transform.GetChild(pageIndex).gameObject.SetActive(false);  //hide last page
            pageIndex++;
            this.gameObject.transform.GetChild(pageIndex).gameObject.SetActive(true);   //show new page
            if (pageIndex == pagesInBook)
                nextButton.gameObject.SetActive(false);
        }
    }
    public void BackPage()  //move a Page back on click
    {
        if (pageIndex > 1)
        {
            if (pageIndex == pagesInBook)
                nextButton.gameObject.SetActive(true);
            this.gameObject.transform.GetChild(pageIndex).gameObject.SetActive(false);  //hide last page
            pageIndex--;
            this.gameObject.transform.GetChild(pageIndex).gameObject.SetActive(true);   //show new page
            if (pageIndex == 1)
                backButton.gameObject.SetActive(false);
        }
    }
    public void FirstPage()     //switch back to first page (used when Book is closed)
    {
        nextButton.gameObject.SetActive(true);
        this.gameObject.transform.GetChild(pageIndex).gameObject.SetActive(false);  //hide last page
        pageIndex = 1;
        this.gameObject.transform.GetChild(pageIndex).gameObject.SetActive(true);   //show new page
        backButton.gameObject.SetActive(false);
    }
}


