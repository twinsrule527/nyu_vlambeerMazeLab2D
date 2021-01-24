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
        if(minHallwayStraightLength > maxHallwayStraightLength) {
            maxHallwayStraightLength = minHallwayStraightLength;
            maxHallwayStraightLengthSlider.value = maxHallwayStraightLength;
        }
    }
    public int maxHallwayStraightLength;//The point at which the hallway will immediately have to turn (50 = no Maximum)
    public Slider maxHallwayStraightLengthSlider;
    public void changeMaxHallwayStraightLength() {
        //A value of 50 = infinity
        maxHallwayStraightLength = Mathf.RoundToInt(maxHallwayStraightLengthSlider.value);
        if(minHallwayStraightLength > maxHallwayStraightLength) {
            minHallwayStraightLength = maxHallwayStraightLength;
            minHallwayStraightLengthSlider.value = maxHallwayStraightLength;
        }
    }
    public int minMaxHallwayLength;//The minimum length a hallway clone will be before destroying itself
    public Slider minMaxHallwayLengthSlider;
    public void changeMinMaxHallwayLength() {
        minMaxHallwayLength = Mathf.RoundToInt(minMaxHallwayLengthSlider.value);
        if(minMaxHallwayLength > maxMaxHallwayLength) {
            maxMaxHallwayLength = minMaxHallwayLength;
            maxMaxHallwayLengthSlider.value = minMaxHallwayLength;
        }
    }
    public int maxMaxHallwayLength;//The longest length a hallway clone can be before destroying itself
    public Slider maxMaxHallwayLengthSlider;
    public void changeMaxMaxHallwayLength() {
        maxMaxHallwayLength = Mathf.RoundToInt(maxMaxHallwayLengthSlider.value);
        if(maxMaxHallwayLength < minMaxHallwayLength) {
            minMaxHallwayLength = maxMaxHallwayLength;
            minMaxHallwayLengthSlider.value = maxMaxHallwayLength;
        }
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
    public bool jogTurnOn;//Whether Hallways can job over 1 block when turning
    public void chanceJogTurnOn() {
        jogTurnOn = !jogTurnOn;
        //Enables the Jog Turn slider to only appear if jog turns are turned on
        jogTurnSlider.enabled = jogTurnOn;
    }
    public float jogTurnPercent;//How likely it is for a hallway to jog over on any given moment (separate from normal turns)
    public Slider jogTurnSlider;
    public void changeJogTurnPercent() {
        jogTurnPercent = jogTurnSlider.value;
    }
    public bool roomOverlapOn;//Declares whether or not rooms can overlap. If turned off, room makes will not spawn while the halway maker is standing over a room tile
    public void changeRoomOverlapOn() {
        roomOverlapOn = !roomOverlapOn;
    }
    public float generateHallwayOnRoomGenerationPercent;//The probability of whether the base hallway Generator will generate more hallways when it generates a room
    public Slider generateHallwayOnRoomGenerationSlider;
    public void changeGenerateHallwayOnRoomGeneration() {
        generateHallwayOnRoomGenerationPercent = generateHallwayOnRoomGenerationSlider.value;
    }
    public bool skipExistingTilesOn;//For hallway generation, this means that halways will immediately skip ahead through straight lines of tiles
    //Will create longer straight paths, and may cause slightly more lag, but overall, will generate the map more efficiently
    public void changeSkipExistingTilesOn() {
        skipExistingTilesOn = !skipExistingTilesOn;
    }
    public bool skipTurnAllowedOn;//When skipExistingTiles is on, this bool determines whether the hallway should follow turning rules while skipping tiles
    public void changeSkipTurnAllowedOn() {
        skipTurnAllowedOn = !skipTurnAllowedOn;
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
        hallwayGeneratePercent = hallwayGenerateSlider.value;
        jogTurnPercent = jogTurnSlider.value;
        generateHallwayOnRoomGenerationPercent = generateHallwayOnRoomGenerationSlider.value;

    }

    void Update()
    {
        
    }
}
