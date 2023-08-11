using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using System.Linq;
using TMPro;


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
    private InventoryItemData kiwiSeedData;
    public InventoryDisplay playerHotbar;
    private SpriteRenderer rend;
    private Color originalColor;
    private InventoryHolder playerInventory;
    private Crop currentCrop;
    private bool cropPlanted;
    private bool harvestable;

    private void Start()
    {
        player = PlayerController.instance.GameObject();
        playerRange = PlayerController.instance.GetCropRange();
        rend = GetComponent<SpriteRenderer>();
        originalColor = rend.color;
        playerInventory = player.GetComponent<InventoryHolder>();
        ItemManager.instance.ItemDictionary.TryGetValue(1, out InventoryItemData tempKiwiSeedData);
        kiwiSeedData = tempKiwiSeedData;
    }
 
    private void OnMouseOver()
    {
        if (PauseMenu.getGameIsPaused())
            return;



       //float distanceFromPlant = Mathf.Abs((transform.position - player.transform.position).magnitude);
       if (!cropPlanted && Input.GetMouseButton(0) && playerInventory.InventorySystem.ContainsItem(kiwiSeedData, out List<InventorySlot> invSlot))
       {
            cropPlanted = true;
            invSlot[invSlot.Count - 1].RemoveFromStack(1);
            player.GetComponent<InventoryHolder>().InventorySystem.UpdateAllSlots();
            PlantCrop();
 
       }

        if (harvestable && Input.GetMouseButton(1))
        {
            currentCrop.harvestCrop();
            currentCrop = null;
            cropPlanted = false;
            harvestable = false;
        }
        //else
        //rend.color = outOfRangeColor;
    }

    private void OnMouseEnter()
    {
        rend.color = inRangeColor;
    }

    private void OnMouseExit()
    {
        rend.color = originalColor;
    }

    private void PlantCrop()
    {
        currentCrop = Instantiate(plantedCrop, transform.position, transform.rotation, transform).GetComponent<Crop>();
    }

    public void setHarvestable(bool harvestable)
    {
        this.harvestable = harvestable;
    }
    
}

