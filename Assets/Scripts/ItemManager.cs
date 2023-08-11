using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

// This script creates a dictonary of all items using the resources folder.
public class ItemManager : MonoBehaviour
{
    public Dictionary<int, InventoryItemData> ItemDictionary;
    public static ItemManager instance;
    private void Awake()
    {
        instance = this;
        ItemDictionary = new Dictionary<int, InventoryItemData>();

        var items = Resources.LoadAll<InventoryItemData>("Items");
        foreach(var item in items)
        {
            ItemDictionary.Add(item.ID, item);
        }
    }   
}
