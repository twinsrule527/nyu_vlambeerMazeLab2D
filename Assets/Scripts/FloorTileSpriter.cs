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
        else if(rnd < 0.525f) {
            mySprite.sprite = myTile.floorTileSprite[2];
        }
        else if(rnd < 0.65f) {
            mySprite.sprite = myTile.floorTileSprite[3];
        }
        else if(rnd < 0.85f) {
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
        else if(rnd < 0.925f) {
            float rnd2 = Random.Range(0f, 1f);
            if( rnd2 < 0.5f) {
                mySprite.sprite = myTile.floorTileSprite[9];
            }
            else {
                mySprite.sprite = myTile.floorTileSprite[10];
            }
        }
        else if(rnd < 0.975f) {
            mySprite.sprite = myTile.floorTileSprite[4];
        }
        else {
            float rnd2 = Random.Range(0f, 1f);
            if( rnd2 < 1f / 11f) {
                mySprite.sprite = myTile.floorTileSprite[11];
            }
            else if( rnd2 < 2f / 11f) {
                mySprite.sprite = myTile.floorTileSprite[12];
            }
            else if( rnd2 < 3f / 11f) {
                mySprite.sprite = myTile.floorTileSprite[13];
            }
            else if( rnd2 < 4f / 11f) {
                mySprite.sprite = myTile.floorTileSprite[14];
            }
            else if( rnd2 < 5f / 11f) {
                mySprite.sprite = myTile.floorTileSprite[15];
            }
            else if( rnd2 < 6f / 11f) {
                mySprite.sprite = myTile.floorTileSprite[16];
            }
            else if( rnd2 < 7f / 11f) {
                mySprite.sprite = myTile.floorTileSprite[17];
            }
            else if( rnd2 < 8f / 11f) {
                mySprite.sprite = myTile.floorTileSprite[18];
            }
            else if( rnd2 < 9f / 11f) {
                mySprite.sprite = myTile.floorTileSprite[19];
            }
            else if( rnd2 < 10f / 11f) {
                mySprite.sprite = myTile.floorTileSprite[20];
            }
            else {
                mySprite.sprite = myTile.floorTileSprite[21];
            }
        }
        Destroy(this);
    }
}
