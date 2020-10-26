using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
//PURPOSE: A manager program for dealing with any issues, plus key input
//USAGE: Attached to an empty manager object
public class ManageRoom : MonoBehaviour
{
    public List<HallwayMaker> myHallways = new List<HallwayMaker>();
    public List<RoomMaker> myRooms = new List<RoomMaker>();
    public List<Transform> Tiles = new List<Transform>();
    public Transform wallPrefab;
    public List<Transform> Walls = new List<Transform>();
    public TileTracker myTileTracker;
    public HallwayMaker startHallway;
    public Camera myCamera;
    public PlayerScript myPlayer;
    public Transform xKey, yKey;
    public Transform xCompCenter, yCompCenter;
    public Transform Edge;
    public Text bottomText;
    public Text bottomText2;
    public Text controlText;
    public static int globalFloorCount;
    public bool playerMove;
    bool pause = false;//Used to keep track if the game is paused
    public int endgame = 0;//Used to keep track if the game has ended
    int currentTile = 0; //Used at the end of the game to create walls around the floor
    float cameraSizeControl = 1;
    public float baseCameraSize = 15f;
    public float maxCameraSizeControl = 10f;
    public float minCameraSizeControl = 0.1f;
    void Start() {
        myHallways.Add(startHallway);
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R)) {
            globalFloorCount = 0;
            SceneManager.LoadScene(0);
        }
        if(Input.GetKeyDown(KeyCode.P) && endgame == 0) {
            if(pause == true) {
                pause = false;
                for( int i = 0; i < myHallways.Count; i++ ) {
                    myHallways[i].pause = false;
                }
                for( int i = 0; i < myRooms.Count; i++ ) {
                    myRooms[i].pause = false;
                }
            }
            else {
                pause = true;
                for( int i = 0; i < myHallways.Count; i++ ) {
                    myHallways[i].pause = true;
                }
                for( int i = 0; i < myRooms.Count; i++ ) {
                    myRooms[i].pause = true;
                }
            }
        }
        if(Input.GetKeyDown(KeyCode.Space) && endgame == 0) {
            endgame ++;
            for( int i = 0; i < myHallways.Count; i++ ) {
                Destroy(myHallways[i].gameObject);
            }
        }
        if( endgame == 1 ) {
            if(myRooms.Count == 0) {
            int repetition = 0;
            if(Tiles.Count <= 5000) {
                repetition = Mathf.RoundToInt(Tiles.Count / 100f);
            }
            else {
                repetition = 50;
            }
            repetition++;
            for(int j = 0; j< repetition; j++) {
            if(currentTile < Tiles.Count ) {
                //For the current tile, create walls on all sides that raycasting finds null
                List<Ray2D> myRay = new List<Ray2D>();
                List<Vector3> wallShift = new List<Vector3>();
                //Adds the 8 rays around the square: up, right, down, left, up-right, up-left, right-down, left-down
                Vector2 ph_Vector = transform.up + transform.right;
                wallShift.Add(ph_Vector);
                myRay.Add(new Ray2D(Tiles[currentTile].position, ph_Vector.normalized));//45
                ph_Vector = -transform.up + transform.right;
                wallShift.Add(ph_Vector);
                myRay.Add(new Ray2D(Tiles[currentTile].position, ph_Vector.normalized));//-45
                ph_Vector = -transform.up - transform.right;
                wallShift.Add(ph_Vector);
                myRay.Add(new Ray2D(Tiles[currentTile].position, ph_Vector.normalized));//-135
                ph_Vector = transform.up - transform.right;
                wallShift.Add(ph_Vector);
                myRay.Add(new Ray2D(Tiles[currentTile].position, ph_Vector.normalized));//135
                for(int i = 0; i < myRay.Count; i++) {
                    //Debug.Log(myRay.Count.ToString());
                    Debug.DrawRay(myRay[i].origin, myRay[i].direction);
                    RaycastHit2D myRayHit = Physics2D.Raycast(myRay[i].origin, myRay[i].direction, 1);
                    if(myRayHit.collider == null) {
                        Transform myWall = Instantiate(wallPrefab, Tiles[currentTile].position + wallShift[i], Tiles[currentTile].rotation);
                        Walls.Add(myWall);
                    }
                    else {//if(wallShift[i] == transform.up + transform.right) {
                        //Creates 2 Vector3s: one which is where the floor should be relative for the wall collider, the other of which is the actual position of the floor
                        Vector3 rayCheck = new Vector3((myRayHit.transform.position - wallShift[i]).x, (myRayHit.transform.position - wallShift[i]).y, 0);
                        Vector3 floorCheck = new Vector3(Tiles[currentTile].position.x, Tiles[currentTile].position.y, 0f);
                        float x = floorCheck.x - rayCheck.x;
                        if(x != 0 && x != 4 && x != -1 && x != 1) {
                            //Transform myWall = Instantiate(wallPrefab, Tiles[currentTile].position + wallShift[i], Tiles[currentTile].rotation);
                        
                        }
                        //Need to figure out how to make corners regardless
                        //Going to try to do it with Physics2D.OverlapBox
                        //Ended up not doing this, but keeping it here for reference
                    }
                }

                currentTile++ ;
            }
            else {
                endgame = 2;
                j = 50;
                currentTile = 0;
            }
            }
            }
        }
        else if(endgame == 2) {
            int repetition = 0;
            if(Tiles.Count <= 5000) {
                repetition = Mathf.RoundToInt(Tiles.Count / 100f);
            }
            else {
                repetition = 50;
            }
            repetition++;
            for(int j = 0; j< repetition; j++) {
            if(currentTile < Tiles.Count ) {
                List<Ray2D> myRay = new List<Ray2D>();
                List<Vector3> wallShift = new List<Vector3>();
                //Adds the 8 rays around the square: up, right, down, left, up-right, up-left, right-down, left-down
                Vector2 ph_Vector = transform.up + transform.right;
                ph_Vector = transform.up;
                wallShift.Add(ph_Vector);
                myRay.Add(new Ray2D(Tiles[currentTile].position, transform.up));//90
                ph_Vector = transform.right;
                wallShift.Add(ph_Vector);
                myRay.Add(new Ray2D(Tiles[currentTile].position, transform.right));//0
                ph_Vector = -transform.up;;
                wallShift.Add(ph_Vector);
                myRay.Add(new Ray2D(Tiles[currentTile].position, -transform.up));//-90
                ph_Vector = -transform.right;
                wallShift.Add(ph_Vector);
                myRay.Add(new Ray2D(Tiles[currentTile].position, -transform.right));//180
                for(int i = 0; i < myRay.Count; i++) {
                    Debug.DrawRay(myRay[i].origin, myRay[i].direction);
                    RaycastHit2D myRayHit = Physics2D.Raycast(myRay[i].origin, myRay[i].direction, 1);
                    if(myRayHit.collider == null) {
                        Transform myWall = Instantiate(wallPrefab, Tiles[currentTile].position + wallShift[i], Tiles[currentTile].rotation);
                        Walls.Add(myWall);
                    }
                }

                currentTile++ ;
            }
            else {
                endgame = 3;
                j = 50;
            }
            }
        }
        if( endgame == 3 && Input.GetKeyDown(KeyCode.C)) {
            if(!playerMove) {
                //When you first press 'C', a finish point will be set, and the camera will zoom in on the player
                playerMove = true;
                int cSize = 0;
                if(Tiles.Count < 500) {
                    cSize = 5;
                }
                else if(Tiles.Count < 1000) {
                    cSize = 6;
                }
                else if(Tiles.Count < 1500) {
                    cSize = 7;
                }
                else if(Tiles.Count < 2000) {
                    cSize = 8;
                }
                else if(Tiles.Count < 3500) {
                    cSize = 9;
                }
                else if(Tiles.Count < 4250) {
                    cSize = 10;
                }
                else if(Tiles.Count < 5000) {
                    cSize = 11;
                }
                else {
                    cSize = 12;
                }
                myCamera.orthographicSize = cSize;
                //Also creates a border of sorts around the map, to make clear the player is playing
                Vector3 edgePos = new Vector3(myPlayer.transform.position.x, myPlayer.transform.position.y + myCamera.orthographicSize - 0.5f, -5f);
                myPlayer.Edge1 = Instantiate(Edge, edgePos, Quaternion.Euler(0f, 0f, 0f));
                edgePos = new Vector3(myPlayer.transform.position.x, myPlayer.transform.position.y - myCamera.orthographicSize + 0.5f, -5f);
                myPlayer.Edge2 = Instantiate(Edge, edgePos, Quaternion.Euler(0f, 0f, 0f));
                edgePos = new Vector3(myPlayer.transform.position.x + myCamera.orthographicSize*myCamera.aspect - 0.5f, myPlayer.transform.position.y, -5f);
                myPlayer.Edge3 = Instantiate(Edge, edgePos, Quaternion.Euler(0f, 0f, 90f));
                edgePos = new Vector3(myPlayer.transform.position.x - myCamera.orthographicSize*myCamera.aspect + 0.5f, myPlayer.transform.position.y, -5f);
                myPlayer.Edge4 = Instantiate(Edge, edgePos, Quaternion.Euler(0f, 0f, 90f));
                //Also creates a final spot + more depending on map size
                int randomTileChoice = Random.Range(Mathf.RoundToInt(Tiles.Count / 3 * 2), Tiles.Count-1);
                SpriteRenderer tileSprite = Tiles[randomTileChoice].gameObject.GetComponent<SpriteRenderer>();
                tileSprite.color = Color.yellow;
                tileSprite.transform.position += new Vector3(0f, 0f, -1f);
                TileEnd myTileEnd = Tiles[randomTileChoice].gameObject.AddComponent<TileEnd>();
                myTileEnd.myPlayer = myPlayer;
                myTileEnd.myManager = this;
                if(Tiles.Count > 500) {
                    randomTileChoice = Random.Range(Mathf.RoundToInt(Tiles.Count / 4), Tiles.Count -1);
                    Transform myX = Instantiate(xKey, Tiles[randomTileChoice].position + new Vector3(0f, 0f, -1f), transform.rotation);
                    X_Tile myXScript = myX.GetComponent<X_Tile>();
                    myXScript.myPlayer = myPlayer.transform;
                    myXScript.myExit = tileSprite.transform;
                    myXScript.myManager = this;
                    myXScript.myCamera = myCamera;
                    myXScript.CompassCenter = xCompCenter;
                    float camera_min_x = 0;
                    float camera_min_y = 0;
                    float camera_max_x = 0;
                    float camera_max_y = 0;
                    for(int i =0; i < Tiles.Count; i++ ) {
                        if(Tiles[i].position.x > camera_max_x ) {
                            camera_max_x = Tiles[i].position.x;
                        }
                        else if( Tiles[i].position.x < camera_min_x ) {
                            camera_min_x = Tiles[i].position.x;
                        }
                        if(Tiles[i].position.y > camera_max_y ) {
                            camera_max_y = Tiles[i].position.y;
                        }
                        else if( Tiles[i].position.y < camera_min_y ) {
                            camera_min_y = Tiles[i].position.y;
                        }
                    }
                    myXScript.maxX = camera_max_x;
                    myXScript.minX = camera_min_x;
                    randomTileChoice = Random.Range(Mathf.RoundToInt(Tiles.Count / 4), Tiles.Count -1);
                    Transform myY = Instantiate(yKey, Tiles[randomTileChoice].position + new Vector3(0f, 0f, -1f), transform.rotation);
                    Y_Tile myYScript = myY.GetComponent<Y_Tile>();
                    myYScript.myPlayer = myPlayer.transform;
                    myYScript.myExit = tileSprite.transform;
                    myYScript.myManager = this;
                    myYScript.myCamera = myCamera;
                    myYScript.maxY = camera_max_y;
                    myYScript.minY = camera_min_y;
                    myYScript.myXCompass = myXScript;
                    myYScript.CompassCenter = yCompCenter;
                }
            }
            else {
                //playerMove = false;
            }
        }
        //This script also consists of camera management
        //To start, average the x and y positions of each tile
        if(!playerMove) {
            float camera_min_x = 0;
            float camera_min_y = 0;
            float camera_max_x = 0;
            float camera_max_y = 0;
            for(int i =0; i < Tiles.Count; i++ ) {
                if(Tiles[i].position.x > camera_max_x ) {
                    camera_max_x = Tiles[i].position.x;
                }
                else if( Tiles[i].position.x < camera_min_x ) {
                    camera_min_x = Tiles[i].position.x;
                }
                if(Tiles[i].position.y > camera_max_y ) {
                    camera_max_y = Tiles[i].position.y;
                }
                else if( Tiles[i].position.y < camera_min_y ) {
                    camera_min_y = Tiles[i].position.y;
                }
            }
            if(Input.GetKey(KeyCode.Minus)) {
                if(cameraSizeControl < maxCameraSizeControl) {
                    cameraSizeControl += Time.deltaTime * 2f;
                }
            }
            if(Input.GetKey(KeyCode.Equals)) {
                if(cameraSizeControl > minCameraSizeControl) {
                    cameraSizeControl -= Time.deltaTime * 2f;
                }
            }
            //myCamera.transform.position = new Vector3((camera_max_x +camera_min_x) / 2, (camera_max_y + camera_min_y) / 2, -10f);
            myCamera.orthographicSize = baseCameraSize * cameraSizeControl;
            if(Input.GetKey(KeyCode.LeftArrow)) {
                myCamera.transform.position -= transform.right * Time.deltaTime * myCamera.orthographicSize;
            }
            if(Input.GetKey(KeyCode.RightArrow)) {
                myCamera.transform.position += transform.right * Time.deltaTime * myCamera.orthographicSize;
            }
            if(Input.GetKey(KeyCode.UpArrow)) {
                myCamera.transform.position += transform.up * Time.deltaTime * myCamera.orthographicSize;
            }
            if(Input.GetKey(KeyCode.DownArrow)) {
                myCamera.transform.position -= transform.up * Time.deltaTime * myCamera.orthographicSize;
            }
        }
        else {
            //myCamera.transform.position = new Vector3(myPlayer.transform.position.x, myPlayer.transform.position.y, -10f);
        }
        //Text: if currently procedurally generating, the number of squares is recorded. If player is moving, number of steps is recorded
        if(endgame == 0) {
            bottomText.text = "Floor Tiles Generated: " + Tiles.Count.ToString();
            controlText.text = "Controls: Arrows to move camera, -/= to zoom out/zoom in. SPACE to stop generation.";
        }
        if(playerMove) {
            bottomText2.text = "Player steps taken: " + myPlayer.mySteps.ToString();
            controlText.text = "Controls: Arrows to move player. Find the exit (yellow). Red/Blue are compasses.";
            }
        else if(endgame == 1 || endgame == 2) {
            controlText.text = "Controls: Arrows to move camera, -/= to zoom out/zoom in.";
        }
        else if(endgame == 3) {
            controlText.text = "Controls: Arrows to move camera, -/= to zoom out/zoom in. C to play the map as a maze.";
        }
        else if(endgame == 4) {
            controlText.text = "Controls: Arrows to move camera, -/= to zoom out/zoom in. R to restart map-building.";
        }
    }
}
