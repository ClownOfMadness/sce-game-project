using UnityEngine;

//responsible for creating Map zone and changing building cards to buildings on contact, extension of ZoneBehaviour
public class Zone_Map : Zone_Behaviour
{
    [HideInInspector] public GameObject selectedTile;   //updated by PlayerControl
    private Player_SpawnBuilding Tiles;                

    void Awake()
    {
        Size = 1;       //max Zone size 
        Tiles = FindObjectOfType<Player_SpawnBuilding>();   //connection to use SpawnBuilding functions
    }
    public override void EventDrop(Card_Drag cardObject)
    {
        if (this.transform.childCount == this.Size)         //failsafe - verifying that there's an object in the Zone
        {
            if (Tiles.Spawn(cardObject.card.buildingPrefab, selectedTile))  //spawn the card's building on the tile that's under the pointer
            {  
                GameObject placeholder = cardObject.placeholder;            //get the card's placeholder in the Hand to delete it
                Destroy(placeholder);
                cardObject.gameObject.SetActive(false);                     //hide in scene (can be replaced with Destroy function)
                Size++;                                                     //needed because the cards get hidden
            }
            //return to hand??
        }
    }
}


