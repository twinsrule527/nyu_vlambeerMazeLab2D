using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
//PURPOSE: Used to create tiles for the hallways for the game's tilemap
//USAGE: attached to a hallway creator sprite
public class HallwayTileMaker : MonoBehaviour
{
    public bool clone;//A boolean that says whether this is the original hallway maker, or a clone
    public static int globalFloorCount;//A tracker for how many tiles have been placed in total
    public int floorCountTracker;
    public int maxFloorCount;
    int counterLife;//For clones, how long this creator will last before it disappears
    public int minHallLength;//An integer value for the shortest amount forward a hallway may go before it turns
    public int maxHallLength;//A currently unused integer value for when you want to have a maximum value for a straight hallway
    int hallLength = 0; //This variable is used to determine how long hallways are in this game
    int myCounter = 0;// count how many floor tiles this FloorMaker has instantiated
    public TileBase floorTile;//The tile (eventually a list of floor Tiles) that this hallway maker places
    public Tilemap floorTilemap;
    public HallwayTileMaker HallwayMakerPrefab;//a prefab for the other hallwayTileMakers
    public RoomTileMaker roomMakerPrefab; //The prefab for other room makers
    public ManagerScript myManager; //The roomManager that exists that this uses
    public bool pause;//Bool for whether this object is paused or not
    public float turnPercent;//The probability that the hallwayMaker will turn one direction or another
    public float hallwayGeneratePercent;//The probability that the hallwayMaker will create another (if it is not a clone)
    public float roomGeneratePercent;//The probability that the hallway will generate a room
    public int minRoomSize;//Used as a reference for when the RoomTileMaker needs to know how big of a room to make
    public int maxRoomSize;//See minRoomSize
    public float endRoomGeneratePercent;//The probability that this hallway will create a room when it is destroyed
    
    //The two below are only used for Clones, to see how long they live for
    public int minHallwayLength;//When a clone, the minimum length this hallway maker can be
    public int maxHallwayLength;//See minHallwayLength
    public bool JogTurns;//Whether the hallway can jog over 1 block while moving
    public float jogTurnPercent;//Chance for it to make a jog of a turn
    void Start()
    {
        //This is added to the Manager object's list of Hallways
        myManager.myHallways.Add(this);
        if(!clone) {//Only occurs for the first room creator, a start of room sort of thing
            //The number of tiles is completely reset
            globalFloorCount = 0;
            //Direction is randomly determined
            float rnd = Random.Range(0f, 1.0f);
            if(rnd < 0.25f) {
                //Left
                transform.eulerAngles = new Vector3(0f, 0f, 90f);
            }
            else if(rnd < 0.5f) {
                //Down
                transform.eulerAngles = new Vector3(0f, 0f, 180f);
            }
            else if( rnd < 0.75f) {
                //Right
                transform.eulerAngles = new Vector3(0f, 0f, 270f);
            }
            else {
                //Up
                transform.eulerAngles = new Vector3(0f, 0f, 0f);
            }
        }
        else {
            //If it is a clone, it has a certain lifeTime
            counterLife = Random.Range(minHallwayLength, maxHallwayLength);
        }
    }

    void Update()
    {
        floorCountTracker = globalFloorCount;
        //The TileMaker only does something if its not paused
        if(!pause) {
            //Only if it is not the clone, or its life is less than its maxCounter number, will it do something
            if( (myCounter < counterLife || !clone) ) {
                //A random number is generated to determine what the hallway maker does
                float rnd = Random.Range(0.0f, 1.0f);
                //Depending on the percentage of the random number (which will eventually be able to be changed), different things will occur
                //First, it may get rotated +90 degrees
                    //Only occurs if it has already gone its min hallway length
                if(rnd < turnPercent / 2 && hallLength > minHallLength) {
                    transform.eulerAngles += new Vector3(0f, 0f, 90f);
                    //It also resets the hallway length
                    hallLength = 0;
                }
                //Next, it may be rotated -90 degrees - follows same rules as option before
                else if(rnd < turnPercent && hallLength > minHallLength) {
                    transform.eulerAngles += new Vector3(0f, 0f, -90f);
                    //It also resets the hallway length
                    hallLength = 0;
                }
                //...else, it has a tiny chance to instantiate another hallwaymaker
                else if(rnd < turnPercent + hallwayGeneratePercent) {
                    if(!clone) {
                    HallwayTileMaker myClone = Instantiate(HallwayMakerPrefab, transform.position, transform.rotation);//Instantiates the clone
                    myClone.myManager = myManager;
                    //Add clone to the Manager hallways//Records this all in the manager object
                    //The clone gets all of its needed attributes
                    myClone.floorTilemap = floorTilemap;
                    myClone.maxFloorCount = maxFloorCount;
                    myClone.turnPercent = turnPercent;
                    myClone.roomGeneratePercent = roomGeneratePercent;
                    myClone.minRoomSize = minRoomSize;
                    myClone.maxRoomSize = maxRoomSize;
                    myClone.minHallwayLength = minHallwayLength;
                    myClone.maxHallwayLength = maxHallwayLength;
                    myClone.minHallLength = minHallLength;
                    myClone.maxHallLength = maxHallLength;
                    myClone.JogTurns = JogTurns;
                    myClone.jogTurnPercent = jogTurnPercent;
                    myClone.clone = true;
                    //The clone is also rotated and pushed forward in some direction
                    float rnd2 = Random.Range(0f, 1f);
                    if(rnd2 < 0.5f) {
                        //turn left? 90 degrees
                        myClone.transform.eulerAngles += new Vector3(0f, 0f, 90f);
                    }
                    else {
                        //turn right? 90 degrees
                        myClone.transform.eulerAngles += new Vector3(0f, 0f, -90f);
                    }
                    myClone.transform.position += myClone.transform.up;//Move forwards 1
                    myClone.clone = true;//Records the clone as being a clone
                    //Some other changes may occur to the clone?
                    }
                }
                //...Else it has a small chance of creating a room
                else if(rnd < turnPercent + hallwayGeneratePercent + roomGeneratePercent) {
                    //First, needs to check if it can create a room:
                        //1 of 2 conditions need to be met: either roomOverlap needs to be allowed, or the tileMaker needs to not be on a room/roomEdge Tile
                    bool createAllowed = true;
                    if(myManager.myValueTracker.roomOverlapOn) {
                        createAllowed = true;
                    }
                    else {
                        //Finds the corresponding tile in the List of Tile positions
                        for(int i =0; i < minRoomSize; i++) {
                            int posCheck = myManager.floorTilePos.IndexOf(new Vector3Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), 0) + Vector3Int.RoundToInt(transform.up * i));
                            if(posCheck > -1) {
                                //Uses that position to check the tileType
                                if(myManager.floorTileType[posCheck] < 0) {
                                    //If the tileType is negative, it is part of a room, and...
                                    //...if overlap is not allowed, no RoomTileMaker can be placed
                                    createAllowed = false;
                                    break;
                                }
                            }
                        }
                    }
                    //Creates a new RoomTileMaker
                    if(createAllowed) {//A secondary check to double check regarding roomOverlap
                    RoomTileMaker myCloneRoom = Instantiate(roomMakerPrefab, transform.position, transform.rotation);
                    myCloneRoom.floorTilemap = floorTilemap;
                    myCloneRoom.myManager = myManager;//The clone Room's manager becomes this
                    myCloneRoom.roomWidth = Random.Range(minRoomSize, maxRoomSize);
                    myCloneRoom.roomHeight = Random.Range(minRoomSize, maxRoomSize);
                    //myManager.myRooms.Add( myCloneRoom );
                    hallLength += 4;//Automatically increases its hallway length by a lot, so it can turn away from the room possibly early
                    if(!clone) {
                        //Non-clone has a chance of creating another clone hallway
                        float randomNumber3 = Random.Range(0f, 1.0f);
                        if(randomNumber3 > 0.5f) {
                            //If it does, it will also be rotated
                            HallwayTileMaker myClone = Instantiate(HallwayMakerPrefab, transform.position, transform.rotation);
				            //myManager.myHallways.Add( myClone );
                            myClone.myManager = myManager;
                            myClone.floorTilemap = floorTilemap;
                            myClone.maxFloorCount = maxFloorCount;
                            myClone.turnPercent = turnPercent;
                            myClone.roomGeneratePercent = roomGeneratePercent;
                            myClone.minRoomSize = minRoomSize;
                            myClone.maxRoomSize = maxRoomSize;
                            myClone.minHallwayLength = minHallwayLength;
                            myClone.maxHallwayLength = maxHallwayLength;
                            myClone.minHallLength = minHallLength;
                            myClone.maxHallLength = maxHallLength;
                            myClone.JogTurns = JogTurns;
                            myClone.jogTurnPercent = jogTurnPercent;
                            myClone.clone = true;
				            //The clone will also be rotated and pushed forward
				            float randomNumber2 = Random.Range(0.0f, 100f);
				            if(randomNumber2 < 50f) {
				        	    myClone.transform.eulerAngles += new Vector3(0f, 0f, 90f);
			        	    }
				            else {
					            myClone.transform.eulerAngles += new Vector3(0f, 0f, -90f);
				            }
				            myClone.transform.position += myClone.transform.up;
				            myClone.clone = true;
				            //hallLength += 4;
                            }
                    }
                    }
                    
                }
                //Might have a chance to jog over, if jogging over is turned on
                else if(JogTurns && rnd < turnPercent + hallwayGeneratePercent + roomGeneratePercent + jogTurnPercent) {
                    //Creates a tile, then moves over, either to the left or the right
                    if(floorTilemap.GetTile(new Vector3Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), 0)) == null) {
                        floorTilemap.SetTile(new Vector3Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), 0), floorTile);
                    
                        //The tile is added to the tile manager (which will also deal with special traits of tiles)
                        myManager.floorTilePos.Add(new Vector3Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), 0));
                        myManager.floorTileType.Add(1);
                    
                        //Global floor count only increases if a new tile is placed
                        globalFloorCount++;
                    }
                    //50% chance to jog right, 50% to jog left
                    float rnd2 = Random.Range(0f, 1f);
                    if(rnd2 < 0.5f) {
                        transform.position += transform.right;
                    }
                    else {
                        transform.position -= transform.right;
                    }
                }
                //Next, the tile creates the floor tile and moves forward
                //But only if the tile is not filled already
                if(floorTilemap.GetTile(new Vector3Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), 0)) == null) {
                    floorTilemap.SetTile(new Vector3Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), 0), floorTile);
                    
                    //The tile is added to the tile manager (which will also deal with special traits of tiles)
                    myManager.floorTilePos.Add(new Vector3Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), 0));
                    myManager.floorTileType.Add(1);
                    
                    //Global floor count only increases if a new tile is placed
                    globalFloorCount++;
                }
                
                transform.position += transform.up;
                //The straight length of the hall increases by 1
                //Other traits are also increased by 1
                myCounter++;
                hallLength++;
                
            }
            else {//Otherwise, you destroy it
                //myManager.myHallways.Remove(this);//Remove it from the manager object
                //At the end of it's life, the clone has a 50% chance of creating a room
                float rnd = Random.Range(0f, 1f);
                if(rnd < endRoomGeneratePercent) {
                    RoomTileMaker myCloneRoom = Instantiate(roomMakerPrefab, transform.position, transform.rotation);
                    myCloneRoom.floorTilemap = floorTilemap;
                    myCloneRoom.roomWidth = Random.Range(minRoomSize, maxRoomSize);
                    myCloneRoom.roomHeight = Random.Range(minRoomSize, maxRoomSize);
                    myCloneRoom.myManager = myManager;
                    //myManager.myRooms.Add( myCloneRoom );
                }
                Destroy(gameObject);
            }
            if(globalFloorCount > maxFloorCount) {
                //myManager.myHallways.Remove(this);
                //myManager.endgame = 1;
                myManager.endgame = 1;
                //At the end of it's life, the clone has a 50% chance of creating a room
                float rnd = Random.Range(0f, 1f);
                if(rnd < endRoomGeneratePercent) {
                    RoomTileMaker myCloneRoom = Instantiate(roomMakerPrefab, transform.position, transform.rotation);
                    myCloneRoom.floorTilemap = floorTilemap;
                    myCloneRoom.roomWidth = Random.Range(minRoomSize, maxRoomSize);
                    myCloneRoom.roomHeight = Random.Range(minRoomSize, maxRoomSize);
                    myCloneRoom.myManager = myManager;
                    //myManager.myRooms.Add( myCloneRoom );
                }
                Destroy(gameObject);
            }
        }
    }
    void OnDestroy() {
        //When this is destroyed, it is removed from the Manager
        myManager.myHallways.Remove(this);
    }
}
