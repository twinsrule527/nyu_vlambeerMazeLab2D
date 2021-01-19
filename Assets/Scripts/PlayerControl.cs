using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
//PURPOSE: Used to control the player's movement in the basic version of the game
//USAGE: Attached to a player prefab object, with reference to the Manager
public class PlayerControl : MonoBehaviour
{
    public ManagerScript myManager;//The Manager script this player is attached to
    public Camera myCamera;//The camera will follow the player
    public Transform myShadow;//Places shadows behind itself to know where its gone
    public int mySteps;//Keeps track of number of steps taken
    
    //A set of public keycodes, which says the movement keys
    public KeyCode Up;
    public KeyCode Down;
    public KeyCode Left;
    public KeyCode Right;
    void Start()
    {
        
    }

    void Update()
    {
        if(myManager.playerMove) {//If the player is allowed to move
            //Move up
            if(Input.GetKeyDown(Up)) {
                //Rotate to be pointing up
                transform.eulerAngles = new Vector3(0f, 0f, 0f);
                //Call the Move function
                Move();
            }
            //Move down
            if(Input.GetKeyDown(Down)) {
                transform.eulerAngles = new Vector3(0f, 0f, 180f);
                //Then, Moves
                Move();
            }
            //Move Right
            if(Input.GetKeyDown(Right)) {
                transform.eulerAngles = new Vector3(0f, 0f, 270f);
                //Then, Moves
                Move();
            }
            //Move Left
            if(Input.GetKeyDown(Left)) {
                transform.eulerAngles = new Vector3(0f, 0f, 90f);
                //Then, Moves
                Move();
            }
        }
    }
    //This is called regardless of which direction you are pointing, so that you attempt to move in that direction
    void Move() {
        //Always places a Shadow at its position
        Instantiate(myShadow, transform.position + new Vector3(0f, 0f, 1f), transform.rotation);
        //Checks the tile immediately in front of the player
        //Gets a Vector3Int of pos in front of the player:
        Vector3 posIntermediary = transform.position + transform.up;
        Vector3Int nextPos = new Vector3Int(Mathf.FloorToInt(posIntermediary.x),Mathf.FloorToInt(posIntermediary.y), 0);
        int tilePosNum = myManager.floorTilePos.IndexOf(nextPos);
        Debug.Log(tilePosNum.ToString());
        //After getting the integer position of the tile in the list, we can compare it to the TileType List
        if(tilePosNum != 0) {//Doublechecking that the tile exists
            //IF the tileType is not wall, we can pass through
            if(myManager.floorTileType[tilePosNum] != 0) {
                //Move forward
                transform.position += transform.up;
                myCamera.transform.position += transform.up;
                mySteps++;//Increases steps
                if(myManager.exitPos == tilePosNum) {
                    Debug.Log("DONE");
                }
            }
        }
    }
}
