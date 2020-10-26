using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//PURPOSE: Used to keep track of all the possible floor tiles
//USAGE: attached to the manager object

public class TileTracker : MonoBehaviour
{
    public Sprite[] floorTileSprite;
    //Sets of Sprites (with probabilities):
    //Elements: 0, 1 - Base : 40% and 0%, at the moment
    //Elements: 2, 3, 4, (5-8), (9-10) - Common: 10% each
}
