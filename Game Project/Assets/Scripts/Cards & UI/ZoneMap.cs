using UnityEngine;

//responsible for creating Map zone and changing building cards to buildings on contact, extension of ZoneBehaviour
public class ZoneMap : ZoneBehaviour
{
    [HideInInspector] public GameObject selectedTile;   //updated by PlayerControl
    private SpawnBuilding Tiles;                

    void Awake()
    {
        Size = 1;       //max Zone size 
        Tiles = FindObjectOfType<SpawnBuilding>();      //connection to use SpawnBuilding functions
    }
    public override void EventDrop(CardDrag cardObject)
    {
        if (this.transform.childCount == this.Size)     //failsafe - verifying that there's an object in the Zone
        {
            GameObject placeholder = cardObject.placeholder;            //get the card's placeholder in the Hand to delete it
            Destroy(placeholder);
            Tiles.Spawn(cardObject.card.buildingPrefab, selectedTile);  //spawn the card's building on the tile that's under the pointer
            cardObject.gameObject.SetActive(false);                     //hide in scene (can be replaced with Destroy function)
            Size++;                                                     //needed because the cards get hidden
        }
    }
}


