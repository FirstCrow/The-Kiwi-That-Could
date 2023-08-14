using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

[System.Serializable]
public class InventorySystem
{
    [SerializeField] private List<InventorySlot> inventorySlots;

    public List<InventorySlot> InventorySlots => inventorySlots;
    public int InventorySize => InventorySlots.Count;

    public UnityAction<InventorySlot> OnInventorySlotChanged;

    public InventorySystem(int size)
    {
        inventorySlots = new List<InventorySlot>();

        for (int i = 0; i < size; i++)
        {
            inventorySlots.Add(new InventorySlot());
        }
    }

    public bool AddToInventory(InventoryItemData itemToAdd, int amountToAdd)
    {
        if (ContainsItem(itemToAdd, out List<InventorySlot> invSlot)) // Check whether item exists in inventory.
        {
            foreach (var slot in invSlot)
            {
                if (slot.RoomLeftInStack(amountToAdd))
                {
                    slot.AddToStack(amountToAdd);
                    OnInventorySlotChanged?.Invoke(slot);
                    return true;
                }
            }
        }

        if (HasFreeSlot(out InventorySlot freeSlot)) // Gets the first available slot
        {
            freeSlot.UpdateInventorySlot(itemToAdd, amountToAdd);
            OnInventorySlotChanged?.Invoke(freeSlot);
            return true;
        }
        return false;
    }

    public bool CheckAddToInventory(InventoryItemData itemToAdd)
    {
        if (ContainsItem(itemToAdd, out List<InventorySlot> invSlot)) // Check whether item exists in inventory.
        {
            foreach (var slot in invSlot)
            {
                if (slot.RoomLeftInStack(1))
                {
                    return true;
                }
            }
        }

        if (HasFreeSlot(out InventorySlot freeSlot)) // Gets the first available slot
        {
            return true;
        }
        return false;
    }

    public void RemoveFromInventory(InventoryItemData itemToRemove, int amountToRemove)
    {

        if (ContainsItem(itemToRemove, out List<InventorySlot> invSlot)) // Check whether item exists in inventory.
        {
            foreach (var slot in invSlot)
            {
                if (slot.StackSize <= amountToRemove)
                {
                    slot.RemoveFromStack(slot.StackSize);
                    OnInventorySlotChanged?.Invoke(slot);
                    amountToRemove -= slot.StackSize;
                }
                else
                {
                    slot.RemoveFromStack(amountToRemove);
                    return;
                }
            }
        }
    }
    public bool ContainsItem(InventoryItemData itemToAdd, out List<InventorySlot> invSlot)
    {
        invSlot = InventorySlots.Where(i => i.ItemData == itemToAdd).ToList();
        foreach (var slot in invSlot)
        {
            if (slot.ItemData != null)
            {
                return true;
            }
        }
        return false;
    }

    public int ContainsNumberOfItem(InventoryItemData itemToAdd, out List<InventorySlot> invSlot)
    {
        invSlot = InventorySlots.Where(i => i.ItemData == itemToAdd).ToList();
        int numberOfItem = 0;
        foreach (var slot in invSlot)
        {
            if (slot.ItemData != null)
            {
                continue;
            }

            numberOfItem += slot.StackSize;
        }

        return numberOfItem;

        
    }

    public bool HasFreeSlot(out InventorySlot freeSlot)
    {
        freeSlot = InventorySlots.FirstOrDefault(i => i.ItemData == null);
        return freeSlot == null ? false : true;
    }

    public void UpdateAllSlots()
    {
        foreach(var slot in inventorySlots)
        {
            OnInventorySlotChanged?.Invoke(slot);
        }
    }
}
