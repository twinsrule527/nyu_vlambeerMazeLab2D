using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
//PURPOSE: Keeps track of all variables which can be editted by the player (map Size, hallway Length, etc.)
//USAGE: Attached to a manager object which has a lot of buttons related to it
public class ValueTracker : MonoBehaviour
{
    public int mapSize;//Declares how large the entire map is (approximately)
    public Slider mapSizeSlider;//Every variable also has a Slider or Button Associated with it
    //All of these sliders also will have the variable's maximum and minimum values associated with it
    public void changeMapSize() {
        //The slider cahnges the variable's value
        mapSize = Mathf.RoundToInt(mapSizeSlider.value);
    }
    public int minHallwayStraightLength;//How long the hallway needs to be before the player can turn (0 = can immediately turn)
    public Slider minHallwayStraightLengthSlider;
    public void changeMinHallwayStraightLength() {
        //The slider cahnges the variable's value
        minHallwayStraightLength = Mathf.RoundToInt(minHallwayStraightLengthSlider.value);
    }
    public int maxHallwayStraightLength;//The point at which the hallway will immediately have to turn (50 = no Maximum)
    public Slider maxHallwayStraightLengthSlider;
    public void changeMaxHallwayStraightLength() {
        //A value of 50 = infinity
        maxHallwayStraightLength = Mathf.RoundToInt(maxHallwayStraightLengthSlider.value);
    }
    public int minMaxHallwayLength;//The minimum length a hallway clone will be before destroying itself
    public Slider minMaxHallwayLengthSlider;
    public void changeMinMaxHallwayLength() {
        minMaxHallwayLength = Mathf.RoundToInt(minMaxHallwayLengthSlider.value);
    }
    public int maxMaxHallwayLength;//The longest length a hallway clone can be before destroying itself
    public Slider maxMaxHallwayLengthSlider;
    public void changeMaxMaxHallwayLength() {
        maxMaxHallwayLength = Mathf.RoundToInt(maxMaxHallwayLengthSlider.value);
    }
    public float hallwayEndRoomGeneratePercent;//The probability that a hallway Clone will create a room when it is destroyed
    public Slider hallwayEndRoomGenerateSlider;
    public void changeHallwayEndRoomGeneratePercent() {
        hallwayEndRoomGeneratePercent = hallwayEndRoomGenerateSlider.value;
    }
    public int minRoomSize;//The smallest size (x and y) that a room can be designed for
    public Slider minRoomSizeSlider;
    public void changeMinRoomSize() {
        minRoomSize = Mathf.RoundToInt(minRoomSizeSlider.value);
    }
    public int maxRoomSize;//The largest size (x and y) that a room can be
    public Slider maxRoomSizeSlider;
    public void changeMaxRoomSize() {
        maxRoomSize = Mathf.RoundToInt(maxRoomSizeSlider.value);
    }
    public float hallwayTurnPercent;//The probability that a hallway generator will turn 1 way or another (divide by 2 to get chance to turn left or chance to turn right)
    public Slider hallwayTurnSlider;
    public void changeHallwayTurnPercent() {
        hallwayTurnPercent = hallwayTurnSlider.value;
    }
    public float roomGeneratePercent;//The probability that a hallway will create a room
    public Slider roomGenerateSlider;
    public void changeRoomGeneratePercent() {
        roomGeneratePercent = roomGenerateSlider.value;
    }
    public float hallwayGeneratePercent;//The probability that the base hallway creator will create a clone
    public Slider hallwayGenerateSlider;
    public void changeHallwayGeneratePercent() {
        hallwayGeneratePercent = hallwayGenerateSlider.value;
    }
    public bool stepTrackerOn;//Whether you record the number of steps it takes for the player to get to the end of the level
    public void changeStepTrackerOn() {
        stepTrackerOn = !stepTrackerOn;
    }
    public bool timeTrackerOn;//Whether you record the time it takes for the player to get to the exit
    public void changeTimeTrackerOn() {
        timeTrackerOn = !timeTrackerOn;
    }
    void Start()
    {
        //At the start, all values are set to be equal to that of the slider
        mapSize = Mathf.RoundToInt(mapSizeSlider.value);
        minHallwayStraightLength = Mathf.RoundToInt(minHallwayStraightLengthSlider.value);
        maxHallwayStraightLength = Mathf.RoundToInt(maxHallwayStraightLengthSlider.value);
        minMaxHallwayLength = Mathf.RoundToInt(minMaxHallwayLengthSlider.value);
        maxMaxHallwayLength = Mathf.RoundToInt(maxMaxHallwayLengthSlider.value);
        hallwayEndRoomGeneratePercent = hallwayEndRoomGenerateSlider.value;
        minRoomSize = Mathf.RoundToInt(minRoomSizeSlider.value);
        maxRoomSize = Mathf.RoundToInt(maxRoomSizeSlider.value);
        hallwayTurnPercent = hallwayTurnSlider.value;
        roomGeneratePercent = roomGenerateSlider.value;
        hallwayGeneratePercent = hallwayEndRoomGenerateSlider.value;
    }

    void Update()
    {
        
    }
}
