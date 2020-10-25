using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//PURPOSE: As a switch for the procedural generation, the code is split into 2 parts: this one creates hallways between rooms
//USAGE: attached to a hallway creator object: two types: a normal one which keeps going forever, and clones which are destroyed after a period of time
public class HallwayMaker : MonoBehaviour
{
    public bool clone = false;//determines if this object is a clone
    public static int globalFloorCount;
    int counterLife;
    public int minHallLength;
    int hallLength = 0; //This variable is used to determine how long hallways are in this game
    int myCounter = 0;// count how many floor tiles this FloorMaker has instantiated
    public Transform floorPrefab;//	Declare a public Transform called floorPrefab, assign the prefab in inspector;
    public HallwayMaker HallwayMakerPrefab;//	Declare a public Transform called floorMakerPrefab, assign the prefab in inspector; 
    public RoomMaker roomMakerPrefab;
    public ManageRoom myManager;
    public bool pause;
    // Start is called before the first frame update
    void Start()
    {
        if(!clone) {
            globalFloorCount = 0;
            float rnd = Random.Range(0f, 1.0f);
            if(rnd < 0.25f) {
                transform.eulerAngles = new Vector3(0f, 0f, 90f);
            }
            else if(rnd < 0.5f) {
                transform.eulerAngles = new Vector3(0f, 0f, 180f);
            }
            else if( rnd < 0.75f) {
                transform.eulerAngles = new Vector3(0f, 0f, 270f);
            }
            else {
                transform.eulerAngles = new Vector3(0f, 0f, 0f);
            }
		}
        else {
            counterLife = Random.Range(20, 50);
            minHallLength += Random.Range(-3, 5);
        }
    }

    // Update is called once per frame
    void Update()
    {
    if(!pause) {
        //If counter is less than counterLife, than keep moving forward
        if( (myCounter < counterLife || !clone)) {
//          Generate a random number from 0.0f to 1.0f;
			float randomNumber = Random.Range(0.0f, 1.0f);
//			If random number is less than 0.25f, then rotate myself 90 degrees on Z axis;
			if(randomNumber < 0.08f && hallLength > minHallLength) {
				transform.eulerAngles += new Vector3(0f, 0f, 90f);
                hallLength = 0;
			}
//				... Else if number is 0.25f-0.5f, then rotate myself -90 degrees on Z axis;
			else if( randomNumber < 0.16f && hallLength > minHallLength) {
				transform.eulerAngles += new Vector3(0f, 0f, -90f);
                hallLength = 0;
			}
//				... Else if number is 0.99f-1.0f, then instantiate a HallwayMakerPrefab clone at my current position;
			else if(randomNumber >= 1.0f) {
				HallwayMaker myClone = Instantiate(HallwayMakerPrefab, transform.position, transform.rotation);
                myClone.myManager = myManager;
                myManager.myHallways.Add( myClone );
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
				hallLength += 4;
			}
            else if( randomNumber >= 0.95) {
                RoomMaker myCloneRoom = Instantiate(roomMakerPrefab, transform.position, transform.rotation);
                myCloneRoom.myManager = myManager;
                myManager.myRooms.Add( myCloneRoom );
                hallLength += 4;
                if(!clone) {
                    float randomNumber3 = Random.Range(0f, 1.0f);
                    if(randomNumber3 > 0.5f) {
                        HallwayMaker myClone = Instantiate(HallwayMakerPrefab, transform.position, transform.rotation);
				        myManager.myHallways.Add( myClone );
                        myClone.myManager = myManager;
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
				        hallLength += 4;
                        }
                }
            }
//			// end elseIf
//			Instantiate a floorPrefab clone at current position;
			Transform Tile = Instantiate(floorPrefab, transform.position, transform.rotation);
            myManager.Tiles.Add( Tile );
//			Move 1 unit "upwards" based on this object's local rotation (e.g. with rotation 0,0,0 "upwards" is (0,1,0)... but with rotation 0,0,180 then "upwards" is (0,-1, 0)... )
			transform.position += transform.up;
//			Increment counter;
			myCounter++;
			globalFloorCount++;
            Debug.Log(globalFloorCount.ToString());
            hallLength++;
        }
        else {
            myManager.myHallways.Remove(this);
            Destroy(this.gameObject);
        }
        if(globalFloorCount >= 5000f && !clone) {
            myManager.myHallways.Remove(this);
            myManager.endgame = 1;
            Destroy(this.gameObject);
        }
    }
    }
}
