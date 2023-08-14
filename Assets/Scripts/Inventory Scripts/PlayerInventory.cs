using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : InventoryHolder
{
    public static InventoryHolder instance;

    private void Awake()
    {
        instance = this;
        inventorySystem = new InventorySystem(inventorySize);
    }
}
