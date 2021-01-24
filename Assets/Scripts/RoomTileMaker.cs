using System.Collections;
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
    public HallwayTileMaker myMaker;//The HallwayTileMaker that create this room Creator (if it exists) - Used for pausing/unpausing
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
        //Need to add a Quaternion of the room's position/size to the Manager's true room List
        //These two positions are going to be changed to be the bottom left corner of the Room
        float xPos = 100000000000;
        float yPos = 100000000000;
        List<Vector3> posList = new List<Vector3>();//Gets a list of positions
        //The list is created by checking the 4 possible corners
        Vector3 firstCorner = transform.position - transform.right * (roomOffset+1);
        posList.Add(firstCorner);
        posList.Add(firstCorner + transform.up * roomHeight);
        posList.Add(firstCorner + transform.right * roomWidth);
        posList.Add(firstCorner + transform.up * roomHeight + transform.right * roomWidth);
        //Runs through position list to get the bottom left corner
        for(int i = 0; i < posList.Count; i++) {
            //The way it works is that if the position is lower than the old one, it is replaced
            if(posList[i].x < xPos) {
                xPos = posList[i].x;
            }
            if(posList[i].y < yPos) {
                yPos = posList[i].y;
            }
        }
        myManager.roomList.Add(new Quaternion(xPos, yPos, roomWidth, roomHeight));

    }

    void Update()
    {
        if(!pause) {//Only does stuff if it isn't paused
            //Every update, this will place a Tile at its position, and then move 1 in the correct direction
            //But only if there is not already a tile at that position
            //if(Input.GetKeyDown(KeyCode.Space)) {
                NextRoomFloorTile();
            //}
        }
    }
    void OnDestroy() {
        //When this is destroyed, it is removed from the manager
        myManager.myRooms.Remove(this);
        if(myMaker != null) {
            myMaker.pause = false;
        }
    }


    void NextRoomFloorTile() {
        //A different method I'm going to try
        /*if(floorTilemap.GetTile(new Vector3Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), 0)) == null) { 
                floorTilemap.SetTile(new Vector3Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), 0), floorTile);
                myManager.floorTilePos.Add(new Vector3Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), 0)); // Add the tile to the manager object
                
                //The tile is added to the Manager List of Floor Tiles
                //If it is on the edge, -2 is added (a roomEdge)
                if(x == 0 || y == 0 || x == roomWidth || y == roomHeight) {
                    myManager.floorTileType.Add(-2);
                }
                //Otherwise, a -1 is added (normal Room Tile)
                else {
                    myManager.floorTileType.Add(-1);//The tile added is a floor tile
                }
                globalFloorCount++;
            }
            else {
                //Otherwise, it replaces that tile with the correct room tile
                //Get the position in the list of the tile it is on
                int posCheck = myManager.floorTilePos.IndexOf(new Vector3Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), 0));
                //Make sure the position exists
                if(posCheck != -1) {
                    //Only replaces hallways
                    if(myManager.floorTileType[posCheck] > 0) {
                        //If it is an edge, that position in the list is made to be a room edge
                        if(x == 0 || y == 0 || x == roomWidth || y == roomHeight) {
                            myManager.floorTileType[posCheck] = -2;
                        }
                        else {
                            //Otherwise, it is a normal room tile
                            myManager.floorTileType[posCheck] = -1;
                        }
                    }
                }
            }*/
            //If it is an edge, it Tile checks the edge
            if(x == 0 || y == 0 || x == roomWidth || y == roomHeight) {
                TileCheck(new Vector3Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), 0), -2);
            }
            //Otherwise, it TileChecks normally
            else {
                TileCheck(new Vector3Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), 0), -1);
            }
            //It moves along 1 in the correct direction (x-axis), or moves all the way over to the next layer
            //If it is in its first spot, it will move all the way over to its start
            if(x == 0 && y == 0) {
                //Moves over to the left an amount = to its offset
                transform.position -= transform.right * (roomOffset+1);
                //First tile is added to the tilemap
                TileCheck(new Vector3Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), 0), -2);
                x++;
                transform.position += transform.right;
                //only if there is not already a tile at that position, is a new tile placed
                TileCheck(new Vector3Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), 0), -2);
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
    //A function called whenever a new floor tile needs to be placed
    //Needs Inputs for the Position of the tile, and the FloorType that is being compared
    void TileCheck(Vector3Int pos, int floorType) {
        //Check whether a tile exists
        if(floorTilemap.GetTile(pos) == null) {
            //If it doesn't, a new tile is just added
            floorTilemap.SetTile(pos, floorTile);
            //Everything is added to the manager object
            myManager.floorTilePos.Add(pos);
            myManager.floorTileType.Add(floorType);
            globalFloorCount++;
        }
        else {
            //If it does exist, it may be replaced
            int posCheck = myManager.floorTilePos.IndexOf(pos);
            if(posCheck != -1) {
                //If the tile exists, it may change its tile type
                if(myManager.floorTileType[posCheck] > 0) {
                    myManager.floorTileType[posCheck] = floorType;
                }
            }
        }
    }
}
