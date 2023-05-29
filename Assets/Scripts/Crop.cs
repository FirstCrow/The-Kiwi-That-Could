using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Can be inherited by all crop types

public class Crop : MonoBehaviour
{
    [Header("Crop Varibles")]
    public int cropStage;           //Used for the animator to display crop sprites
    private float daysToGrow;       //How many days it will take for the crop to be harvestable
    public Crop(int cropStage, float daysToGrow)
    {
        this.cropStage = cropStage;
        this.daysToGrow = daysToGrow;
    }
}
