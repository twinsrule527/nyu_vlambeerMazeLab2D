using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//PURPOSE: Used by floor tiles to determine their sprite, and then it deletes itself
//USAGE: Attached to all floor tiles
public class FloorTileSpriter : MonoBehaviour
{
    public TileTracker myTile;
    public ManageRoom myManager;
    // Start is called before the first frame update
    void Start()
    {
        myTile = myManager.myTileTracker;
        SpriteRenderer mySprite = GetComponent<SpriteRenderer>();
        float rnd = Random.Range(0f, 1f);
        if(rnd < 0.4f) {
            mySprite.sprite = myTile.floorTileSprite[0];
        }
        else if(rnd < 0.4f) {
            mySprite.sprite = myTile.floorTileSprite[1];
        }
        else if(rnd < 0.5f) {
            mySprite.sprite = myTile.floorTileSprite[2];
        }
        else if(rnd < 0.6f) {
            mySprite.sprite = myTile.floorTileSprite[3];
        }
        else if(rnd < 0.7f) {
            mySprite.sprite = myTile.floorTileSprite[4];
        }
        else if(rnd < 0.8f) {
            float rnd2 = Random.Range(0f, 1f);
            if(rnd2 < 0.25f) {
                mySprite.sprite = myTile.floorTileSprite[5];
            }
            else if(rnd2 < 0.5f) {
                mySprite.sprite = myTile.floorTileSprite[6];
            }
            else if(rnd2 < 0.75f) {
                mySprite.sprite = myTile.floorTileSprite[7];
            }
            else  {
                mySprite.sprite = myTile.floorTileSprite[8];
            }
        }
        else if(rnd < 0.9f) {
            float rnd2 = Random.Range(0f, 1f);
            if( rnd2 < 0.5f) {
                mySprite.sprite = myTile.floorTileSprite[9];
            }
            else {
                mySprite.sprite = myTile.floorTileSprite[10];
            }
        }
        else {
            mySprite.sprite = myTile.floorTileSprite[11];
        }
        Destroy(this);
    }
}
