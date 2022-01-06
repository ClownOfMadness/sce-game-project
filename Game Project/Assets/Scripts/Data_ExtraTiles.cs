using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data_ExtraTiles : MonoBehaviour
{
    public string tileName = "None";
    private SpriteRenderer spriteRenderer = null;
    
    private void Awake()
    {
        if (!(spriteRenderer = GetComponent<SpriteRenderer>()))
        {
            Debug.LogError("Sprite Renderer is missing from the " + tileName + " Data_ExtraTiles");
        }
    }

    private void Start()
    {
        spriteRenderer.sortingOrder = -(int)Mathf.Abs(this.transform.position.z);
    }
}
