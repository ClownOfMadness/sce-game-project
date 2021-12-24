using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data_Fog : MonoBehaviour
{
    public Sprite fullFog;
    public Sprite halfFog;
    public SpriteRenderer sprite;
    public Data_Tile Data_Tile;
    private GameObject unit;

    private void Awake()
    {
        sprite.sprite = fullFog;
        Data_Tile = this.transform.parent.GetComponent<Data_Tile>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8 || other.gameObject.layer == 9 || other.gameObject.layer == 11)
        {
            sprite.sprite = halfFog;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 8 || other.gameObject.layer == 9 || other.gameObject.layer == 11)
        {
            sprite.enabled = false;
            Data_Tile.revealed = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 8 || other.gameObject.layer == 9 || other.gameObject.layer == 11)
        {
            sprite.enabled = true;
            Data_Tile.revealed = false;
        }
    }
}
