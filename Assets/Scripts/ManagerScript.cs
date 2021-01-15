using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
//PURPOSE: Manages other scripts and objects
//USAGE: Attached to an empty manager object that has reference to all other objects
public class ManagerScript : MonoBehaviour
{
    //A list of positions for each floor tile. There will be a corresponding list of enumerators for what the tiles do
    public List<Vector3Int> floorTilePos = new List<Vector3Int>();
    public List<bool> floorTileType = new List<bool>();//Will eventually be a list of enumerators, at the moment only checks to see if this position is a floor tile
    //Need to create an Enum for this

    //These two lists below keep track of all of the hallways/rooms that still are being made
    public List<HallwayTileMaker> myHallways = new List<HallwayTileMaker>();
    public List<RoomTileMaker> myRooms = new List<RoomTileMaker>();
    public Tilemap floorTilemap;
    public TileBase wallTile;//Declared in the inspector
    public List<Vector3Int> cardinalDirection = new List<Vector3Int>();//A cheap and easy way to get directions for when placing walls
    public int endgame;//Where the map building is, stage-wise
    public bool playerMove;//Determines whether the player should be able to move
    public Camera myCamera;//Reference to the main camera
    public PlayerControl myPlayerPrefab;//A prefab to spawn in the player when the time comes
    public int exitPos;//Declares which tile in the List is an exit tile
    public TileBase exitTile;//The Tilebase/Sprite for the exit tile sprite
    public HallwayTileMaker myHallwayMakerPrefab;//Used to create the first HallwayMaker in the game
    public Canvas myCanvas;//References the canvas so it can turn it off at the right time
    public ValueTracker myValueTracker;//Uses the script which keeps track of all values
    void Start()
    {
        
    }

    void Update()
    {
        if(endgame == 1) {
            if(myRooms.Count == 0 && myHallways.Count == 0 ) {
                endgame = 0;
                buildWalls();
            }
        }
    }

    //A script which is called to create all the walls to the map
    public void buildWalls() {//NEED TO DELAY THIS UNTIL ALL ROOM/HALLWAY MAKERS ARE DEAD
        int build = 0;
        while(build < floorTilePos.Count) {
            //If this position tile is a floor tile, it builds walls
            if(floorTileType[build]) {
                //Builds in each direction, relative to the script's List of directions
                for(int i = 0; i< cardinalDirection.Count; i++) {
                    //If there is no tile on the existing spot, it builds a wall
                    if(floorTilemap.GetTile(floorTilePos[build] + cardinalDirection[i]) == null) {
                        floorTilemap.SetTile(floorTilePos[build] + cardinalDirection[i], wallTile);
                        floorTilePos.Add(floorTilePos[build] + cardinalDirection[i]);
                        floorTileType.Add(false);
                    }
                }
            }    
            build++;
        }
        playerSetup();
    }
    //Sets up everything for the player
    void playerSetup() {
        //Resets camera position
        myCamera.transform.position = new Vector3(0f, 0f, -10f);
        //Creates the player
        PlayerControl myPlayer = Instantiate(myPlayerPrefab, new Vector3(0.5f, 0.5f, -1f), Quaternion.identity);
        //Assigns attributes to the player
        myPlayer.myCamera = myCamera;
        myPlayer.myManager = this;
        playerMove = true;
        //Also needs to create the exit square
        //First, get a list of all possible tiles
        List<int> floorList = new List<int>();
        for(int i = 0; i< floorTileType.Count; i++) {
            if(floorTileType[i]) {
                floorList.Add(i);
            }
        }
        //Set the exit position as a random position in the second half of the map
        exitPos = Random.Range(floorList.Count / 2, floorList.Count);
        //Give it its sprite14
        floorTilemap.SetTile(floorTilePos[exitPos], exitTile);
    }
    public void runBuild() {
        //Called by the Start Build Button, where it creates a hallway tile placer, and removes all of the canvas
        myCanvas.enabled = false;
        //Creates the hallway Tile Maker
        HallwayTileMaker baseHallwayMaker = Instantiate(myHallwayMakerPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity);
        //Sets all variables to the correct values
        baseHallwayMaker.clone = false;
        baseHallwayMaker.maxFloorCount = myValueTracker.mapSize;
        baseHallwayMaker.minHallLength = myValueTracker.minHallwayStraightLength;
        baseHallwayMaker.maxHallLength = myValueTracker.maxHallwayStraightLength;
        baseHallwayMaker.turnPercent = myValueTracker.hallwayTurnPercent;
        baseHallwayMaker.hallwayGeneratePercent = myValueTracker.hallwayGeneratePercent;
        baseHallwayMaker.roomGeneratePercent = myValueTracker.roomGeneratePercent;
        baseHallwayMaker.minRoomSize = myValueTracker.minRoomSize;
        baseHallwayMaker.maxRoomSize = myValueTracker.maxRoomSize;
        baseHallwayMaker.endRoomGeneratePercent = myValueTracker.hallwayEndRoomGeneratePercent;
        baseHallwayMaker.minHallwayLength = myValueTracker.minMaxHallwayLength;
        baseHallwayMaker.maxHallwayLength = myValueTracker.maxMaxHallwayLength;
        baseHallwayMaker.myManager = this;
        baseHallwayMaker.floorTilemap = floorTilemap;
    }
}
