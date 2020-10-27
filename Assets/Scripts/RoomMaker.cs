using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//PURPOSE: creates a room around its object, then that object is destroyed

public class RoomMaker : MonoBehaviour
{
    public static int globalFloorCount;
    int roomWidth = 0;
    int x = 0;
    int roomHeight = 0;
    int roomOffset;
    int y = 0;
    public Transform floorPrefab;//	Declare a public Transform called floorPrefab, assign the prefab in inspector;
    public HallwayMaker hallwayMakerPrefab;//	Declare a public Transform called floorMakerPrefab, assign the prefab in inspector; 
    public ManageRoom myManager;
    bool numHallwayMaker;
    public bool pause;
    // Start is called before the first frame update
    void Start()
    {
        roomWidth = Random.Range(1, 5);
		roomHeight = Random.Range(1, 5);
		roomOffset = Random.Range(0, roomHeight-3);
    }

    // Update is called once per frame
    void Update()
    {
    if(!pause) {
        Transform Tile = Instantiate(floorPrefab, transform.position, transform.rotation);
        FloorTileSpriter tileSpriter = Tile.GetComponent<FloorTileSpriter>();
        tileSpriter.myManager = myManager;
        myManager.Tiles.Add( Tile );
        Tile.eulerAngles = new Vector3(0f, 0f, 0f);
		//Moves along creating a room
		if(x == 0 && y == 0) {
			transform.position-= transform.right * roomOffset;
			//Instantiate(floorPrefab, transform.position, transform.rotation);
			x++;
		}
		else if(x == roomWidth) {
			if(y==roomHeight) {
                myManager.myRooms.Remove(this);
				Destroy(this.gameObject);
			}
			else {
				x = 0;
				y++;
				transform.position += transform.up;
				transform.position -= transform.right * roomWidth;
			}
		}
		else {
			x++;
			transform.position += transform.right;
		}
        //Commented out the code below because it was breaking everything
        //if(!numHallwayMaker) {
           // float randomNumber = Random.Range(0f, 100f);
           // if(randomNumber > 90f ) {
             //   numHallwayMaker = true;
             //   HallwayMaker myClone = Instantiate(hallwayMakerPrefab, transform.position, transform.rotation);
				//The clone will also be rotated and pushed forward
			//	float randomNumber2 = Random.Range(0.0f, 100f);
			//	if(randomNumber2 < 50f) {
			//		myClone.transform.eulerAngles += new Vector3(0f, 0f, 90f);
			//	}
			//	else {
		//			myClone.transform.eulerAngles += new Vector3(0f, 0f, -90f);
			//	}
		//		myClone.transform.position += myClone.transform.up;
		//		myClone.clone = true;
          //  }
      //  }
    }
    }
}
