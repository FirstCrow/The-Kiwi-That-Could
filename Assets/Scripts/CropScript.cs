using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;


// This script controlls unplanted crops

public class CropScript : MonoBehaviour
{


    [Header("Links")]
    public GameObject plantedCrop;
    private GameObject player;
    private float playerRange;

    [Header("Color Varibles and Links")]
    public Color outOfRangeColor;
    public Color inRangeColor;
    private SpriteRenderer rend;
    private Color originalColor;


    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerRange = FindObjectOfType<PlayerController>().GetCropRange();
        rend = GetComponent<SpriteRenderer>();
        originalColor = rend.color;
    }
 
    private void OnMouseOver()
    {
        float distanceFromPlant = Mathf.Abs((transform.position - player.transform.position).magnitude);
        if (!PauseMenu.getGameIsPaused())
        {
            if (distanceFromPlant <= playerRange)
            {
                if (Input.GetMouseButton(0))
                {
                    PlantCrop();
                }
                rend.color = inRangeColor;
            }
            else
                rend.color = outOfRangeColor;
        }
    }

    private void OnMouseExit()
    {
        rend.color = originalColor;
    }

    private void PlantCrop()
    {
        Instantiate(plantedCrop, transform.position, transform.rotation);
        Destroy(gameObject);
    }
    
}

