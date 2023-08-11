using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

// Can be inherited by all crop types

public class Crop : MonoBehaviour
{
    [Header("Crop Varibles")]

    public float hoursPerStage;           //How many days each crop stage will take
    public float numCropStages;          //How many crop stages this crop has (the final crop stage is when it is harvestable)
    public int cropStage;               //Used for the animator to display crop sprites
    private float stageHoursElapsed;      //How many days have passed this crop stage
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

    [Header("TEMP")]
    public GameObject KiwiFruit;
    public GameObject KiwiSeeds;

    private void Start()
    {

        // Initializes crop and player varibles
        playerRange = player.GetComponent<PlayerController>().GetCropRange();
        cropStage = 1;
        stageHoursElapsed = 0;
        harvestable = false;
        DayNightScript.current.onNewHour += cropNewHour;

        Debug.Log(numCropStages);
        /* Gets original color
        rend = GetComponent<SpriteRenderer>();
        originalColor = rend.color;*/
    }

    // Called whenever it is a new day via event in DayNightScript
    // Logic for how long each crop stage is
    public void cropNewHour()
    {
        stageHoursElapsed++;
        if (stageHoursElapsed >= hoursPerStage)
        {
            updateCropState();
        }
    }

    // Called when the crop stage needs to be incremented
    public void updateCropState()
    {
        stageHoursElapsed = 0;
        cropStage++;
        Debug.Log(cropStage);

        // If the crop is fully grown it is harvestable
        if (cropStage >= numCropStages)
        {
            harvestable = true;
            GetComponentInParent<UnplantedCropScript>().setHarvestable(true);
        }
        if (cropStage <= numCropStages)
        {
            Debug.Log("Change Graphics");
            graphics.transform.GetChild(cropStage - 2).gameObject.SetActive(false);
            graphics.transform.GetChild(cropStage - 1).gameObject.SetActive(true);
        }
    }

    public void harvestCrop()
    {

        DayNightScript.current.onNewHour -= cropNewHour;
        Instantiate(KiwiFruit, transform.position, transform.rotation);
        for(int i = 0; i <= Mathf.RoundToInt(UnityEngine.Random.Range(0,3)); i++)
        {
            Instantiate(KiwiSeeds, transform.position, transform.rotation);
        }
        Destroy(gameObject);
        Destroy(this);
    }
}
