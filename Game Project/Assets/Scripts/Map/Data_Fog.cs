using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data_Fog : MonoBehaviour
{
    public Sprite fullFog;
    public Sprite halfFog;
    public SpriteRenderer sprite;
    public TileData tileData;

    private void Awake()
    {
        sprite.sprite = fullFog;
        tileData = this.transform.parent.GetComponent<TileData>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8 || other.gameObject.layer == 9)
        {
            sprite.sprite = halfFog;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 8 || other.gameObject.layer == 9)
        {
            sprite.enabled = false;
            tileData.revealed = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 8 || other.gameObject.layer == 9)
        {
            sprite.enabled = true;
            tileData.revealed = false;
        }
    }
}
