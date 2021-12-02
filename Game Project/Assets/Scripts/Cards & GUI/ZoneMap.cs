using UnityEngine;

//responsible for creating Map zone and changing building cards to buildings on contact, extension of ZoneBehaviour
public class ZoneMap : ZoneBehaviour
{
    public GameObject selectedTile;
    public SpawnBuilding spawner;

    public GameObject building;
    
    void Awake()
    {
        Size = 1;       //max Zone size
        
    }
    void Update()
    {
    }
    public void EventDrop() {
        if (this.transform.childCount == this.Size)     //runs when card gets dropped in Zone
        {
            Debug.Log("Drop happened.");
            foreach (Transform child in this.transform) //finds the card (not needed since there's always one)
            {
                if (child.GetComponent<CardDrag>())     //checks if its card (not needed since there's always one card)
                {
                    //GameObject newBuilding =  Instantiate(child.GetComponent<CardDrag>().card.buildingPrefab, selectedTile.transform.position);
                    //newBuilding.name = string.Format("{0}", child.GetComponent<CardDrag>().card.name);            //updates name in scene
                    spawner.Spawn(child.GetComponent<CardDrag>().card.buildingPrefab, selectedTile);
                    Destroy(child.gameObject);          //deletes the card
                    Debug.Log("Card On Map Panel");
                }
            }
        }

    }

    }


