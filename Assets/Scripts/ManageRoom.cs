using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//PURPOSE: A manager program for dealing with any issues, plus key input
//USAGE: Attached to an empty manager object
public class ManageRoom : MonoBehaviour
{
    public List<HallwayMaker> myHallways = new List<HallwayMaker>();
    public List<RoomMaker> myRooms = new List<RoomMaker>();
    public List<Transform> Tiles = new List<Transform>();
    public Transform wallPrefab;
    public List<Transform> Walls = new List<Transform>();
    public HallwayMaker startHallway;
    public Camera myCamera;
    public PlayerScript myPlayer;
    public static int globalFloorCount;
    public bool playerMove;
    bool pause = false;//Used to keep track if the game is paused
    bool endgame = false;//Used to keep track if the game has ended
    int currentTile = 100000; //Used at the end of the game to create walls around the floor
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
        if(Input.GetKeyDown(KeyCode.P) && !endgame) {
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
        if(Input.GetKeyDown(KeyCode.Space) && !endgame ) {
            endgame = true;
            for( int i = 0; i < myHallways.Count; i++ ) {
                Destroy(myHallways[i].gameObject);
            }
        }
        if( endgame ) {
            if(currentTile < Tiles.Count ) {
                //For the current tile, create walls on all sides that raycasting finds null
                List<Ray2D> myRay = new List<Ray2D>();
                List<Vector3> wallShift = new List<Vector3>();
                //Adds the 8 rays around the square: up, right, down, left, up-right, up-left, right-down, left-down
                Vector2 ph_Vector = transform.up + transform.right;
                //wallShift.Add(ph_Vector);
                //myRay.Add(new Ray2D(Tiles[currentTile].position, ph_Vector.normalized));//45
                ph_Vector = -transform.up + transform.right;
                //wallShift.Add(ph_Vector);
                //myRay.Add(new Ray2D(Tiles[currentTile].position, ph_Vector.normalized));//-45
                ph_Vector = -transform.up - transform.right;
                //wallShift.Add(ph_Vector);
                //myRay.Add(new Ray2D(Tiles[currentTile].position, ph_Vector.normalized));//-135
                ph_Vector = transform.up - transform.right;
               // wallShift.Add(ph_Vector);
                //myRay.Add(new Ray2D(Tiles[currentTile].position, ph_Vector.normalized));//135
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
                    Debug.Log(myRay.Count.ToString());
                    Debug.DrawRay(myRay[i].origin, myRay[i].direction);
                    RaycastHit2D myRayHit = Physics2D.Raycast(myRay[i].origin, myRay[i].direction);
                    if(myRayHit.collider == null) {
                        Transform myWall = Instantiate(wallPrefab, Tiles[currentTile].position + wallShift[i], Tiles[currentTile].rotation);
                        Walls.Add(myWall);
                    }
                }
                //for(int i = 0; i < 8; i++) {
                   // Vector2 angle_direction = new Vector2(0f, i * 45f);
                  //  myRay.Add(new Ray2D(Tiles[currentTile].position, angle_direction));
                //}
                //Trying it all in a different way:

                currentTile++ ;
            }
        }
        if( endgame && Input.GetKeyDown(KeyCode.C)) {
            if(!playerMove) {
                //When you first press 'C', a finish point will be set, and the camera will zoom in on the player
                playerMove = true;
                myCamera.orthographicSize = 5;
                int randomTileChoice = Random.Range(Mathf.RoundToInt(Tiles.Count / 3 * 2), Tiles.Count-1);
                SpriteRenderer tileSprite = Tiles[randomTileChoice].gameObject.GetComponent<SpriteRenderer>();
                tileSprite.color = Color.red;
                Tiles[randomTileChoice].gameObject.AddComponent<TileEnd>();
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
                    cameraSizeControl += Time.deltaTime;
                }
            }
            if(Input.GetKey(KeyCode.Equals)) {
                if(cameraSizeControl > minCameraSizeControl) {
                    cameraSizeControl -= Time.deltaTime;
                }
            }
            //myCamera.transform.position = new Vector3((camera_max_x +camera_min_x) / 2, (camera_max_y + camera_min_y) / 2, -10f);
            myCamera.orthographicSize = baseCameraSize * cameraSizeControl;
            if(Input.GetKey(KeyCode.LeftArrow)) {
                myCamera.transform.position -= transform.right * Time.deltaTime * myCamera.orthographicSize / 3;
            }
            if(Input.GetKey(KeyCode.RightArrow)) {
                myCamera.transform.position += transform.right * Time.deltaTime * myCamera.orthographicSize / 3;
            }
            if(Input.GetKey(KeyCode.UpArrow)) {
                myCamera.transform.position += transform.up * Time.deltaTime * myCamera.orthographicSize / 3;
            }
            if(Input.GetKey(KeyCode.DownArrow)) {
                myCamera.transform.position -= transform.up * Time.deltaTime * myCamera.orthographicSize / 3;
            }
        }
        else {
            //myCamera.transform.position = new Vector3(myPlayer.transform.position.x, myPlayer.transform.position.y, -10f);
        }
    }
}
