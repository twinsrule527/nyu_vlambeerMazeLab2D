using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public ManageRoom myManager;
    // Update is called once per frame
    void Update()
    {   
        if(myManager.playerMove) {
            if(Input.GetKeyDown(KeyCode.UpArrow)) {
                transform.eulerAngles = new Vector3(0f, 0f, 0f);
                Ray2D myRay = new Ray2D(transform.position, transform.up);
                RaycastHit2D myRayHit = Physics2D.Raycast(myRay.origin, myRay.direction);
                if(myRayHit.collider != null && myRayHit.collider.CompareTag("Floor")) {
                    transform.position += transform.up;
                }
            }
            if(Input.GetKeyDown(KeyCode.DownArrow)) {
                transform.eulerAngles = new Vector3(0f, 0f, 180f);
                Ray2D myRay = new Ray2D(transform.position, transform.up);
                RaycastHit2D myRayHit = Physics2D.Raycast(myRay.origin, myRay.direction);
                if(myRayHit.collider != null && myRayHit.collider.CompareTag("Floor")) {
                    transform.position += transform.up;
                }
            }
            if(Input.GetKeyDown(KeyCode.RightArrow)) {
                transform.eulerAngles = new Vector3(0f, 0f, 270f);
                Ray2D myRay = new Ray2D(transform.position, transform.up);
                RaycastHit2D myRayHit = Physics2D.Raycast(myRay.origin, myRay.direction);
                if(myRayHit.collider != null && myRayHit.collider.CompareTag("Floor")) {
                    transform.position += transform.up;
                }
            }
            if(Input.GetKeyDown(KeyCode.LeftArrow)) {
                transform.eulerAngles = new Vector3(0f, 0f, 90f);
                Ray2D myRay = new Ray2D(transform.position, transform.up);
                RaycastHit2D myRayHit = Physics2D.Raycast(myRay.origin, myRay.direction);
                if(myRayHit.collider != null && myRayHit.collider.CompareTag("Floor")) {
                    transform.position += transform.up;
                }
            }
        }
        Debug.DrawRay(transform.position, transform.up);
        Debug.DrawRay(transform.position, -transform.up);
        Debug.DrawRay(transform.position, transform.right);
        Debug.DrawRay(transform.position, -transform.right);
    }
}
