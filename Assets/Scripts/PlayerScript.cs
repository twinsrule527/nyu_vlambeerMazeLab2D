﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//PURPOSE: Used to move a player sprite around the procedurally generated map
//USAGE: Attached to a player sprite
public class PlayerScript : MonoBehaviour
{
    public ManageRoom myManager;
    public Camera myCamera;
    Ray2D lastRay;
    // Update is called once per frame
    void Update()
    {   
        if(myManager.playerMove) {
            if(Input.GetKeyDown(KeyCode.UpArrow)) {
                transform.eulerAngles = new Vector3(0f, 0f, 0f);
                Ray2D myRay = new Ray2D(transform.position, transform.up);
                RaycastHit2D myRayHit = Physics2D.Raycast(myRay.origin, myRay.direction.normalized);
                if(myRayHit.collider != null && myRayHit.collider.CompareTag("Floor")) {
                    Vector2 rayCheck = new Vector2((myRayHit.transform.position - transform.up).x, (myRayHit.transform.position - transform.up).y);
                    Vector2 posCheck = new Vector2(transform.position.x, transform.position.y);
                    if(rayCheck == posCheck) {
                        lastRay = myRay;
                        transform.position = myRayHit.transform.position;
                    }
                    Debug.Log(myRayHit.collider.ToString());
                }
            }
            if(Input.GetKeyDown(KeyCode.DownArrow)) {
                transform.eulerAngles = new Vector3(0f, 0f, 180f);
                Ray2D myRay = new Ray2D(transform.position, transform.up);
                RaycastHit2D myRayHit = Physics2D.Raycast(myRay.origin, myRay.direction.normalized);
                if(myRayHit.collider != null && myRayHit.collider.CompareTag("Floor")) {
                    Vector2 rayCheck = new Vector2((myRayHit.transform.position - transform.up).x, (myRayHit.transform.position - transform.up).y);
                    Vector2 posCheck = new Vector2(transform.position.x, transform.position.y);
                    if(rayCheck == posCheck) {
                        lastRay = myRay;
                        transform.position = myRayHit.transform.position;
                    }
                    Debug.Log(myRayHit.collider.ToString());
                }
            }
            if(Input.GetKeyDown(KeyCode.RightArrow)) {
                transform.eulerAngles = new Vector3(0f, 0f, 270f);
                Ray2D myRay = new Ray2D(transform.position, transform.up);
                RaycastHit2D myRayHit = Physics2D.Raycast(myRay.origin, myRay.direction.normalized);
                if(myRayHit.collider != null && myRayHit.collider.CompareTag("Floor")) {
                    Vector2 rayCheck = new Vector2((myRayHit.transform.position - transform.up).x, (myRayHit.transform.position - transform.up).y);
                    Vector2 posCheck = new Vector2(transform.position.x, transform.position.y);
                    if(rayCheck == posCheck) {
                        lastRay = myRay;
                        transform.position = myRayHit.transform.position;
                    }
                    Debug.Log(myRayHit.collider.ToString());
                }
            }
            if(Input.GetKeyDown(KeyCode.LeftArrow)) {
                transform.eulerAngles = new Vector3(0f, 0f, 90f);
                Ray2D myRay = new Ray2D(transform.position, transform.up);
                RaycastHit2D myRayHit = Physics2D.Raycast(myRay.origin, myRay.direction.normalized);
                if(myRayHit.collider != null && myRayHit.collider.CompareTag("Floor")) {
                    Vector2 rayCheck = new Vector2((myRayHit.transform.position - transform.up).x, (myRayHit.transform.position - transform.up).y);
                    Vector2 posCheck = new Vector2(transform.position.x, transform.position.y);
                    if(rayCheck == posCheck) {
                        lastRay = myRay;
                        transform.position = myRayHit.transform.position;
                    }
                    Debug.Log(myRayHit.collider.ToString());
                    //Debug.Log(myManager.Tiles.Find(myRayHit.collider.transform).ToString());
                }
            }
            myCamera.transform.position = new Vector3(transform.position.x, transform.position.y, -10f);
        }
        Debug.DrawRay(transform.position, transform.up);
        Debug.DrawRay(transform.position, -transform.up);
        Debug.DrawRay(transform.position, transform.right);
        Debug.DrawRay(transform.position, -transform.right);
        Debug.DrawRay(lastRay.origin, lastRay.direction, Color.red);
    }
}