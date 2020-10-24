using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//PURPOSE: used as a key for the player to help them find the y value of the exit
//USAGE: attached to a circle sprite that begins being generated on a random square on the board
public class Y_Tile : MonoBehaviour
{
    public Transform myPlayer;
    public Transform myExit;
    public SpriteRenderer mySprite;
    public Sprite Arrow;
    public ManageRoom myManager;
    public Camera myCamera;
    public X_Tile myXCompass;
    public Transform CompassCenter;
    public float maxY;
    public float minY;
    float maxX;
    float minX;
    int found = 0;
    void Update()
    {
        if( myPlayer.position.x == transform.position.x && myPlayer.position.y == transform.position.y ) {
            found = 1;
            mySprite.sprite = Arrow;
            transform.eulerAngles = new Vector3(0f, 0f, -90f);
        }
        if(found == 1) {
            float shift_percentage = 0;
            if(myExit.position.y > myPlayer.position.y) {
                shift_percentage = ( myExit.position.y - transform.position.y ) / (maxY - transform.position.y );
            }
            else if(myExit.position.y < myPlayer.position.y) {
                shift_percentage = ( myExit.position.y - transform.position.y ) / ( transform.position.y - minY );
            }
            Debug.Log(shift_percentage.ToString());
            transform.position = new Vector3(myCamera.transform.position.x + myCamera.orthographicSize * myCamera.aspect - transform.localScale.x / 2, myCamera.transform.position.y, -5f);
            CompassCenter.position = transform.position;
            transform.position += new Vector3(0f, myCamera.orthographicSize  * shift_percentage, 0f);
        }
        //ISSUE WHEN YOU GET CLOSE TO IT THAT IT ISN"T ACCURATE IN ITS MOVEMENT
        else if(found == 2) {
            float x_dist = myExit.position.x - myPlayer.position.x;
            float y_dist = myExit.position.y - myPlayer.position.y;
            float myAngle = Mathf.Tan(x_dist / y_dist) * 180 / Mathf.PI;
            transform.position = new Vector3( myPlayer.position.x, myPlayer.position.y, transform.position.z);
            transform.eulerAngles = new Vector3(0f, 0f, myAngle);
            transform.position += transform.up;
        }
        if(myXCompass == null) {//Switch to != null to ahve it start working, at the moment i don't want it to
            if(found ==1 && myXCompass.found) {
                maxX = myXCompass.maxX;
                minX = myXCompass.minX;
                Destroy(myXCompass.gameObject);
                mySprite.color = Color.magenta;
                found = 2;
            }
        }
    }
}
