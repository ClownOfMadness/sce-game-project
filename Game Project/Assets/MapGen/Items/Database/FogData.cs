using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogData : MonoBehaviour
{
    public Sprite fullFog;
    public Sprite halfFog;
    public SpriteRenderer sprite;

    private void Awake()
    {
        sprite.sprite = fullFog;
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
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 8 || other.gameObject.layer == 9)
        {
            sprite.enabled = true;
        }
    }
}
