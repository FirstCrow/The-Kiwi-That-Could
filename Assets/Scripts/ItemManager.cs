using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class ItemManager : MonoBehaviour
{
    public static Dictionary<int, InventoryItemData> ItemDictionary;
    public static ItemManager instance;
    private void Start()
    {
        instance = this;
        ItemDictionary = new Dictionary<int, InventoryItemData>();

        var items = Resources.LoadAll<InventoryItemData>("Items");
        foreach(var item in items)
        {
            ItemDictionary.Add(item.ID, item);
            Debug.Log(item.ID, item);
        }

        Debug.Log(ItemDictionary);
    }   
}
