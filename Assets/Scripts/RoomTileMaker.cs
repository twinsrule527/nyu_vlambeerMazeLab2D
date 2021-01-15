﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
//PURPOSE: Tiles out rooms (rather than hallways)
//USAGE: Attached to a RoomMaker object, which is a prefab for the HallwayMaker
public class RoomTileMaker : MonoBehaviour
{
    public static int globalFloorCount;
    public int roomWidth;//The width of the room being created
    public int roomHeight;//The height of the room being created
    int roomOffset;//The amount the room is offset compared to the hallway it started by
    int x;//Current position of the maker along the x-axis (relative to its width)
    int y;//Current position of the maker along the y-axis (relative to room height)
    public TileBase floorTile;//Prefab for the floor tile which is used
    public Tilemap floorTilemap;//The tilemap for floor tiles
    public HallwayTileMaker HallwayMakerPrefab;//Prefab to create more HallwayMakers (if it needs to)
    public ManagerScript myManager; //The roomManager that exists that this uses
    public bool pause;//Gameobject can be paused independently
    void Start()
    {
        //RoomWidth and RoomHeight setting have been moved to the HallwayyTileMaker that makes these
        //roomWidth = Random.Range(1, 5);//Randomly set the room width (eventually will be more variable)
        //roomHeight = Random.Range(1, 5);//Randomly set room height
        //Randomly set the room's offset
        roomOffset = Random.Range(0, roomHeight - 1);

        //This is added to the Manager's Room List
        myManager.myRooms.Add(this);

    }

    void Update()
    {
        if(!pause) {//Only does stuff if it isn't paused
            //Every update, this will place a Tile at its position, and then move 1 in the correct direction
            //But only if there is not already a tile at that position
            if(floorTilemap.GetTile(new Vector3Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), 0)) == null) { 
                floorTilemap.SetTile(new Vector3Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), 0), floorTile);
                myManager.floorTilePos.Add(new Vector3Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), 0)); // Add the tile to the manager object
                myManager.floorTileType.Add(true);//The tile added is a floor tile
                globalFloorCount++;
            }
            //It moves along 1 in the correct direction (x-axis), or moves all the way over to the next layer
            //If it is in its first spot, it will move all the way over to its start
            if(x == 0 && y == 0) {
                //Moves over to the left an amount = to its offset
                transform.position -= transform.right * roomOffset;
                x++;
                //only if there is not already a tile at that position, is a new tile placed
                if(floorTilemap.GetTile(new Vector3Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), 0)) == null) { 
            
                    floorTilemap.SetTile(new Vector3Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), 0), floorTile);
                    //Adds the tile to the manager script
                    myManager.floorTilePos.Add(new Vector3Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), 0)); // Add the tile to the manager object
                    myManager.floorTileType.Add(true);//The tile added is a floor tile
                    globalFloorCount++;
                }
            }
            //Once it moves over to the right edge, it moves to the next y position
                //It destroys itself if it doesn't have anywhere further to go
            else if(x == roomWidth) {
                if(y == roomHeight) {
                    //myManager.Remove this;
                    Destroy(gameObject);
                }
                else {
                    //Resets x position
                    x = 0;
                    //Y value is increased by one
                    y++;
                    //Moves up, and resets x position
                    transform.position += transform.up;
                    transform.position -= transform.right * roomWidth;
                    
                }
            }
            else {//Otherwise, it just moves 1 to the right
                x++;
                transform.position += transform.right;
            }
        }
    }
    void OnDestroy() {
        //When this is destroyed, it is removed from the manager
        myManager.myRooms.Remove(this);
    }
}
