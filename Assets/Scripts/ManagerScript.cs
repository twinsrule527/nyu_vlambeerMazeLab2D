using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
//PURPOSE: Manages other scripts and objects
//USAGE: Attached to an empty manager object that has reference to all other objects
public class ManagerScript : MonoBehaviour
{
    //A list of positions for each floor tile. There will be a corresponding list of enumerators for what the tiles do
    public List<Vector3Int> floorTilePos = new List<Vector3Int>();
    public List<int> floorTileType = new List<int>();//Will eventually be a list of enumerators, at the moment only checks to see if this position is a floor tile
    //0 = Wall, negative numbers = room, positive numbers = hallway
    //Need to create an Enum for this

    //These two lists below keep track of all of the hallways/rooms that still are being made
    public List<HallwayTileMaker> myHallways = new List<HallwayTileMaker>();
    public List<RoomTileMaker> myRooms = new List<RoomTileMaker>();
    public Tilemap floorTilemap;
    public TileBase wallTile;//Declared in the inspector
    public List<Vector3Int> cardinalDirection = new List<Vector3Int>();//A cheap and easy way to get directions for when placing walls
    public int endgame;//Where the map building is, stage-wise
    public bool playerMove;//Determines whether the player should be able to move
    public Canvas playerCanvasManager;//Keeps track of the canvas manager that exists with the player, to enable it when the time comes
    public Camera myCamera;//Reference to the main camera
    public PlayerControl myPlayerPrefab;//A prefab to spawn in the player when the time comes
    public int exitPos;//Declares which tile in the List is an exit tile
    public TileBase exitTile;//The Tilebase/Sprite for the exit tile sprite
    public HallwayTileMaker myHallwayMakerPrefab;//Used to create the first HallwayMaker in the game
    public Canvas myCanvas;//References the canvas so it can turn it off at the right time
    public ValueTracker myValueTracker;//Uses the script which keeps track of all values
    public List<Quaternion> roomList = new List<Quaternion>();//This is a list of Rooms - first two values are the x and y position of the lower left corner of the room, while z and w are the width and height of the room
    int roomWallCount;//A tracker variable that keeps track of how many rooms Walls have been built for
    public bool building;//Whether the map is currently being built
    void Start()
    {
        
    }

    void Update()
    {
        if(endgame == 1) {
            if(myRooms.Count == 0 && myHallways.Count == 0 ) {
                endgame = 2;
                buildWalls();
                roomWallCount = 0;
            }
        }
        else if(endgame == 2) {
            //This is when Rooms are actually built, and everything is put in the rooms
            //At the moment though, just builds walls separating rooms
            buildRoomWalls(roomList[roomWallCount]);
            //Add to the roomWallCOunter, which if it is greater than the number of rooms, goes to next phase
            
            roomWallCount++;
            if(roomWallCount >= roomList.Count) {
                endgame = 3;
            }
        }
        else if(endgame == 3) {
            building = false;//Stuff is no longer being built at this time
            playerSetup();
            endgame = 0;
        }
        if(endgame == 0 && Input.GetKeyDown(KeyCode.R)) {
            SceneManager.LoadScene(0);
        }
    }

    //A script which is called to create all the walls to the map
    public void buildWalls() {//NEED TO DELAY THIS UNTIL ALL ROOM/HALLWAY MAKERS ARE DEAD
        int build = 0;
        while(build < floorTilePos.Count) {
            //If this position tile is a floor tile, it builds walls
            if(floorTileType[build] != 0 ) {
                //Builds in each direction, relative to the script's List of directions
                for(int i = 0; i< cardinalDirection.Count; i++) {
                    //If there is no tile on the existing spot, it builds a wall
                    if(floorTilemap.GetTile(floorTilePos[build] + cardinalDirection[i]) == null) {
                        floorTilemap.SetTile(floorTilePos[build] + cardinalDirection[i], wallTile);
                        floorTilePos.Add(floorTilePos[build] + cardinalDirection[i]);
                        floorTileType.Add(0);
                    }
                }
            }    
            build++;
        }
    }
    //Sets up everything for the player
    void playerSetup() {
        //Resets camera position
        myCamera.transform.position = new Vector3(0f, 0f, -10f);
        myCamera.orthographicSize = 5;
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
            if(floorTileType[i] != 0) {
                floorList.Add(i);
            }
        }
        //Set the exit position as a random position in the second half of the map
        exitPos = Random.Range(floorList.Count / 2, floorList.Count);
        //Give it its sprite14
        floorTilemap.SetTile(floorTilePos[exitPos], exitTile);
        playerCanvasManager.enabled = true;
        InGameCanvasManager inGameCanvas = playerCanvasManager.GetComponent<InGameCanvasManager>();
        inGameCanvas.enabled = true;
        inGameCanvas.myPlayer = myPlayer;
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
        baseHallwayMaker.JogTurns = myValueTracker.jogTurnOn;
        baseHallwayMaker.jogTurnPercent = myValueTracker.jogTurnPercent;
        baseHallwayMaker.generateHallwayOnRoomGenerationPercent = myValueTracker.generateHallwayOnRoomGenerationPercent;
        baseHallwayMaker.myManager = this;
        baseHallwayMaker.floorTilemap = floorTilemap;
        building = true;
    }
    void buildRoomWalls(Quaternion currentRoom) {
        //Builds walls between rooms where it is needed
        //Eventually, will be a conditional that can be changed in the options
        //At first, creates a list of the edge of the room
        //Also, get list of wall and normal floor tiles in the area
        List<Vector3Int> roomEdges = new List<Vector3Int>();
        List<Vector3Int> basicRoomTiles = new List<Vector3Int>();
        List<Vector3Int> roomWalls = new List<Vector3Int>();
        //Goes along the x-axis
        for(int i = 0; i < currentRoom.y; i++) {
            //Goes along the y-Axis
            for(int j = 0; j < currentRoom.z; j++) {
                //Get the index of the current position
                    //Position is gotten as: lower left corner +i x, j y
                int index = floorTilePos.IndexOf(new Vector3Int(Mathf.RoundToInt(currentRoom.x) + i, Mathf.RoundToInt(currentRoom.z) + j, 0));
                if(index > -1) {
                    //If the current spot is a roomEdge, you add it to the roomEdge list
                    if(floorTileType[index] == -2) {
                        roomEdges.Add(floorTilePos[index]);
                    }
                    else if(floorTileType[index] == -1 ) {
                        basicRoomTiles.Add(floorTilePos[index]);
                    }
                    else if(floorTileType[index] == 0) {
                        roomWalls.Add(floorTilePos[index]);
                    }
                }
            }
        }
        //After the list is gathered, you check each against wall conditions
        //FOR NOW, JUST USING BASIC WALL CONDITIONS: room tiles on two sides of the wall
        for(int i = 0; i < roomEdges.Count; i++) {
            if((floorTileType[floorTilePos.IndexOf(roomEdges[i] + new Vector3Int(1, 0, 0))] == -1 && floorTileType[floorTilePos.IndexOf(roomEdges[i] + new Vector3Int(-1, 0, 0))] < 0) || ( floorTileType[floorTilePos.IndexOf(roomEdges[i] + new Vector3Int(0, 1, 0))] < 0 && floorTileType[floorTilePos.IndexOf(roomEdges[i] + new Vector3Int(0, -1, 0))] == -1)) {
                floorTilemap.SetTile(roomEdges[i], wallTile);
                floorTileType[floorTilePos.IndexOf(roomEdges[i])] = 0;
            }
        }

        //Commenting out this original script
        /*
        for(int i =0; i < floorTilePos.Count; i++) {
            //Only if it is a room edge does it test things
            if(floorTileType[i] == -2) {
                if((floorTileType[floorTilePos.IndexOf(floorTilePos[i] + new Vector3Int(1, 0, 0))] < 0 && floorTileType[floorTilePos.IndexOf(floorTilePos[i] + new Vector3Int(-1, 0, 0))] == -1) || (floorTileType[floorTilePos.IndexOf(floorTilePos[i] + new Vector3Int(0, 1, 0))] == -1 && floorTileType[floorTilePos.IndexOf(floorTilePos[i] + new Vector3Int(0, -1, 0))] < 0)) {
                    floorTilemap.SetTile(floorTilePos[i], wallTile);
                    floorTileType[i] = 0;
                }
            }
        }
        */
    }

}