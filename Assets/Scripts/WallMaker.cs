using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//PURPOSE: Used after finishing creating the dungeon to create walls
//USAGE: attached to a manager object

public class WallMaker : MonoBehaviour
{
    public ManageRoom myManager;
    public Transform wallPrefab;
    public List<Transform> Walls = new List<Transform>();
    int tileCounter = 0;
    // Update is called once per frame
    void Update()
    {
        if(myManager.endgame == 1) {
            if(tileCounter < myManager.Tiles.Count) {
                Ray2D myRay = new Ray2D(myManager.Tiles[tileCounter].position, transform.up);
                RaycastHit2D myRayHit = Physics2D.Raycast(myRay.origin, myRay.direction, 1);
                if(myRayHit.collider == null) {
                    Transform myWall = Instantiate(wallPrefab, myManager.Tiles[tileCounter].position + transform.up, transform.rotation);
                    Walls.Add(myWall);
                }
                else {
                }
            }
            tileCounter++;
        }
    }
}
