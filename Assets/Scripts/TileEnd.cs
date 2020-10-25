using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileEnd : MonoBehaviour
{
    public PlayerScript myPlayer;
    public ManageRoom myManager;

    // Update is called once per frame
    void Update()
    {
        if(transform.position.x == myPlayer.transform.position.x && transform.position.y == myPlayer.transform.position.y ) {
            myManager.endgame = 4;
            myManager.playerMove = false;
            Destroy(myPlayer.Edge1.gameObject);
            Destroy(myPlayer.Edge2.gameObject);
            Destroy(myPlayer.Edge3.gameObject);
            Destroy(myPlayer.Edge4.gameObject);
            Destroy(this);
        }
    }
}
