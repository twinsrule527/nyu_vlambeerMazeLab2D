using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class InGameCanvasManager : MonoBehaviour
{
    
    public Text StepsTaken;
    public Text StopWatch;
    float timeKeep;//Keeps track of the time
    public ManagerScript myManager;
    Canvas myCanvas;
    public PlayerControl myPlayer;//Gets set by the manager script
    void Start()
    {
        myCanvas = GetComponent<Canvas>();
    }

    void Update()
    {
        timeKeep += Time.deltaTime;
        //If the step tracker is on, and this is enabled, it keeps track of steps taken
        if(StepsTaken.enabled) {
            StepsTaken.text = "Steps Taken: " + myPlayer.mySteps.ToString();
        }
        if(StopWatch.enabled) {
            //Gets the time
            float seconds = timeKeep % 60;
            //seconds become more precise
            seconds = Mathf.Round(seconds * 100f) / 100f;
            //A string is created to deal with the inconsistency of small numbers
            string secondsString = "";
            if(seconds < 10f) {
                secondsString = "0" + seconds.ToString();
            }
            else {
                secondsString = seconds.ToString();
            }
            int minutes = Mathf.FloorToInt(timeKeep / 60);
            //String is created again
            string minuteString = "";
            if(minutes < 10) {
                minuteString = "0" + minutes.ToString();
            }
            else {
                minuteString = minutes.ToString();
            }
            StopWatch.text = minuteString + ":" + secondsString;
        }
    }
    void OnEnable() {
        StepsTaken.enabled = myManager.myValueTracker.stepTrackerOn;
        StopWatch.enabled = myManager.myValueTracker.timeTrackerOn;
    }
}
