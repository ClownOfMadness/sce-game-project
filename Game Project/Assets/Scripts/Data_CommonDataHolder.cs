using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data_CommonDataHolder : MonoBehaviour
{
    public Sprite workSiteSprite;
    public Sprite buildLocationTrue;
    public Sprite buildLocationFalse;
    public Sprite workPlace;
    public Sprite patrolPlace;
    public Sprite pointer;
    public Data_Card peasantCard;
    public Shader shaderGUItext;
    public Shader shaderSpritesDefault;
    private void Awake()
    {
        if (!workSiteSprite)
        {
            Debug.LogError("Worksite sprite is missing from the Data_CommonDataHolder");
        }
        if (!buildLocationTrue)
        {
            Debug.LogError("BuildLocation True sprite is missing from the Data_CommonDataHolder");
        }
        if (!buildLocationFalse)
        {
            Debug.LogError("BuildLocation False sprite is missing from the Data_CommonDataHolder");
        }
        if (!workPlace)
        {
            Debug.LogError("Workplace sprite is missing from the Data_CommonDataHolder");
        }
        if (!patrolPlace)
        {
            Debug.LogError("Patrolplace sprite is missing from the Data_CommonDataHolder");
        }
        if (!pointer)
        {
            Debug.LogError("Pointer sprite is missing from the Data_CommonDataHolder");
        }
        if (!peasantCard)
        {
            Debug.LogError("Peasant Card is missing from the Data_CommonDataHolder");
        }
        if (!(shaderGUItext = Shader.Find("GUI/Text Shader")))
        {
            Debug.LogError("GUI Shader is missing from the Data_CommonDataHolder");
        }
        if (!(shaderSpritesDefault = Shader.Find("Sprites/Default")))
        {
            Debug.LogError("Default Sprite Shader is missing from the Data_CommonDataHolder");
        }
    }
}
