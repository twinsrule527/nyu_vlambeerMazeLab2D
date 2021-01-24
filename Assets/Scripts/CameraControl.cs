using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public ManagerScript myManager;//Keeps track of the manager object, mainly so it knows it is able to move independently
    
    //Keeps track of 6 keycodes: 4 directional, plus zoom in and zoom out
    public KeyCode zoomOutKey;
    public KeyCode zoomInKey;
    public KeyCode Up;
    public KeyCode Down;
    public KeyCode Right;
    public KeyCode Left;
    //Has a maximum and minimum size for the camera?
    public int minCamSize;
    public int maxCamSize;
    //Has a speed at which it moves
    public float moveSpeed;
    //Has a speed at which it zooms in/out
    public float zoomSpeed;
    Camera cameraSelf;//Also keeps track of its own camera

    void Start() {
        cameraSelf = GetComponent<Camera>();
    }
    void Update()
    {
        if(myManager.building) {
            //When moving, its speed is determined as a function of speed * cameraSize
            if(Input.GetKey(Up)) {
                transform.position += transform.up * (moveSpeed * cameraSelf.orthographicSize) * Time.deltaTime;
            }
            if(Input.GetKey(Down)) {
                transform.position -= transform.up * (moveSpeed * cameraSelf.orthographicSize) * Time.deltaTime;
            }
            if(Input.GetKey(Left)) {
                transform.position -= transform.right * (moveSpeed * cameraSelf.orthographicSize) * Time.deltaTime;
            }
            if(Input.GetKey(Right)) {
                transform.position += transform.right * (moveSpeed * cameraSelf.orthographicSize) * Time.deltaTime;
            }
            //Zooms in/out at a constant rate
            if(Input.GetKey(zoomInKey)) {
                cameraSelf.orthographicSize -= zoomSpeed * Time.deltaTime * cameraSelf.orthographicSize;
                if(cameraSelf.orthographicSize < minCamSize) {
                    cameraSelf.orthographicSize = minCamSize;
                }
            }
            else if(Input.GetKey(zoomOutKey)) {
                cameraSelf.orthographicSize += zoomSpeed * Time.deltaTime * ((maxCamSize + 5) - cameraSelf.orthographicSize);
                if(cameraSelf.orthographicSize > maxCamSize) {
                    cameraSelf.orthographicSize = maxCamSize;
                }
            }
        }
    }
}
