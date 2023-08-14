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
        List<InventorySlot> invSlot;
        foreach (var item in costItems)
        {
            if (playerInventory.InventorySystem.ContainsNumberOfItem(item.item, out invSlot) < item.amount)
            {
                Debug.Log("Insufficent Items");
                return;
            }
        }

        foreach (var item in costItems)
        {
            playerInventory.InventorySystem.RemoveFromInventory(item.item, item.amount);
        }

        foreach (var item in costItems)
        {
            playerInventory.InventorySystem.AddToInventory(item.item, item.amount);
        }

    }
}
