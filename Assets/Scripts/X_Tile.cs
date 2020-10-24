﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//PURPOSE: used as a key for the player to help them find the x value of the exit
//USAGE: attached to a hexagon sprite that begins being generated on a random square on the board
public class X_Tile : MonoBehaviour
{
    public Transform myPlayer;
    public Transform myExit;
    public SpriteRenderer mySprite;
    public Sprite Arrow;
    public ManageRoom myManager;
    public Camera myCamera;
    public Transform CompassCenter;
    public float maxX;
    public float minX;
    public bool found = false;
    void Update()
    {
        if( myPlayer.position.x == transform.position.x && myPlayer.position.y == transform.position.y ) {
            found = true;
            mySprite.sprite = Arrow;
        }
        if(found) {
            float shift_percentage = 0;
            if(myExit.position.x > myPlayer.position.x) {
                shift_percentage = ( myExit.position.x - transform.position.x ) / (maxX - transform.position.x );
            }
            else if(myExit.position.x < myPlayer.position.x) {
                shift_percentage = ( myExit.position.x - transform.position.x ) / ( transform.position.x - minX );
            }
            Debug.Log(shift_percentage.ToString());
            transform.position = new Vector3(myCamera.transform.position.x, myCamera.transform.position.y + myCamera.orthographicSize - transform.localScale.y / 2, -5f);
            CompassCenter.position = transform.position;
            transform.position += new Vector3(myCamera.orthographicSize * myCamera.aspect * shift_percentage, 0f, 0f);
        }
        
    }
}