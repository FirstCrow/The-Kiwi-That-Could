using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using System.Linq;


// This script controlls unplanted crops

public class UnplantedCropScript : MonoBehaviour
{


    [Header("Links")]
    public GameObject plantedCrop;
    private GameObject player;
    private float playerRange;

    [Header("Color Varibles and Links")]
    public Color outOfRangeColor;
    public Color inRangeColor;
    public InventoryItemData kiwiSeedData;
    public InventoryDisplay playerHotbar;
    private SpriteRenderer rend;
    private Color originalColor;
    private InventoryHolder playerInventory;


    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerRange = FindObjectOfType<PlayerController>().GetCropRange();
        rend = GetComponent<SpriteRenderer>();
        originalColor = rend.color;
        playerInventory = player.GetComponent<InventoryHolder>();
    }
 
    private void OnMouseOver()
    {
        float distanceFromPlant = Mathf.Abs((transform.position - player.transform.position).magnitude);
        if (!PauseMenu.getGameIsPaused())
        {
            if (distanceFromPlant <= playerRange)
            {
                if (playerInventory.InventorySystem.ContainsItem(kiwiSeedData, out List<InventorySlot> invSlot))
                {
                    Debug.Log("Player has seeds");
                    if (Input.GetMouseButton(0))
                    {
                        invSlot[invSlot.Count - 1].RemoveFromStack(1);
                        
                        PlantCrop();
                    }
                    rend.color = inRangeColor;
                }
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

