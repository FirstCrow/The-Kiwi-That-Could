using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Purchasing;
using UnityEngine;
using static UnityEditor.Progress;


[System.Serializable]
public class ShopOffer : MonoBehaviour
{
    public ShopItem[] costItems;
    public ShopItem[] priceItems;
    private InventoryHolder playerInventory;

    public void Start()
    {
        playerInventory = PlayerInventory.instance;
    }

    public void purchaseOffer()
    {
        if (playerInventory == null)
        {
            Debug.Log("Player Inventory is null");
            return;
        }



        List<InventorySlot> invSlot;
        foreach (var item in costItems)
        {
            if (playerInventory.InventorySystem.ContainsNumberOfItem(item.item, out invSlot) < item.amount)
            {
                Debug.Log("Player has: " + playerInventory.InventorySystem.ContainsNumberOfItem(item.item, out invSlot) + item.item.name);
                Debug.Log("Insufficent amount of: " + item.item.name);
                return;
            }
        }

        int numberFreeSlots = playerInventory.InventorySystem.NumFreeSlots();

        foreach (var item in costItems)
        {
            if (!playerInventory.InventorySystem.CheckAddToExistingStacks(item.item))
            {
                if(numberFreeSlots > 0)
                {
                    numberFreeSlots--;
                    continue;
                }
                Debug.Log("Requires more inventory space");
                return;
            }
        }



        foreach (var item in costItems)
        {
            playerInventory.InventorySystem.RemoveFromInventory(item.item, item.amount);
        }

        foreach (var item in priceItems)
        {
            playerInventory.InventorySystem.AddToInventory(item.item, item.amount);
        }

    }
}
