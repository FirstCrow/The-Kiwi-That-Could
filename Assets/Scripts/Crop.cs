using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Can be inherited by all crop types

public class Crop : MonoBehaviour
{
    [Header("Crop Varibles")]
    
    public float daysPerStage;           //How many days each crop stage will take
    public float numCropStages;          //How many crop stages this crop has (the final crop stage is when it is harvestable)
    private int cropStage;               //Used for the animator to display crop sprites
    private float stageDaysElapsed;      //How many days have passed this crop stage
    private bool harvestable;            //is the crop harvestable

    [Header("Player Varibles")]
    public GameObject player;
    private float playerRange;

    [Header("Color Varibles")]
    public Color outOfRangeColor;
    public Color inRangeColor;
    private Color originalColor;

    [Header("Links")]
    public Animator animator;      //Used to change crop sprite/animation depending on crop stage
    public GameObject graphics;

    private void Start()
    {

        // Initializes crop and player varibles
        playerRange = player.GetComponent<PlayerController>().GetCropRange();
        cropStage = 1;
        stageDaysElapsed = 0;
        harvestable = false;
        DayNightScript.current.onNewDay += cropNewDay;

        /* Gets original color
        rend = GetComponent<SpriteRenderer>();
        originalColor = rend.color;*/
    }

    // Called whenever it is a new day via event in DayNightScript
    // Logic for how long each crop stage is
    public void cropNewDay()
    {
        stageDaysElapsed++;
        if(stageDaysElapsed >= daysPerStage)
        {
            updateCropState();
        }
    }

    // Called when the crop stage needs to be incremented
    public void updateCropState()
    {
        stageDaysElapsed = 0;
        cropStage++;
        graphics.transform.GetChild(cropStage - 2).gameObject.SetActive(false);
        graphics.transform.GetChild(cropStage - 1).gameObject.SetActive(true); ;
       

        // If the crop is fully grown it is harvestable
        if (cropStage >= numCropStages)
        {
            harvestable = true;
            Debug.Log("Plant ready to harvest!");
        }
    }

    private void harvestCrop()
    {

    }

    private void OnMouseOver()
    {
        float distanceFromPlant = Mathf.Abs((transform.position - player.transform.position).magnitude);
        if (!PauseMenu.getGameIsPaused())
        {
            if (distanceFromPlant <= playerRange && harvestable)
            {
                if (Input.GetMouseButton(0))
                {
                    harvestCrop();
                }
                //rend.color = inRangeColor;
            }
            //else
               //rend.color = outOfRangeColor;
        }
    }

    private void OnMouseExit()
    {
        //rend.color = originalColor;
    }

}
