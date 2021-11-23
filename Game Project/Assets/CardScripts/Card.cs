using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Card",menuName ="Card")] //allows you to create cards
public class Card : ScriptableObject
{
    //The basic card stuff
    public new string name;
    public Sprite artwork;
}
