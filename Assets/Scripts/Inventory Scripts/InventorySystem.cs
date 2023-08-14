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

    public bool CheckAddToExistingStacks(InventoryItemData itemToAdd) // Checks whether item can be added to an existing slot
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

        return false;
    }

    public bool RemoveFromInventory(InventoryItemData itemToRemove, int amountToRemove)
    {
        if (!ContainsItem(itemToRemove, out List<InventorySlot> invSlot))
        {
            return false;
        }

        foreach (var slot in invSlot)
        {
            
            if (slot.StackSize < amountToRemove)
            {
                slot.RemoveFromStack(slot.StackSize);
                OnInventorySlotChanged?.Invoke(slot);
                //Debug.Log("Removed " + slot.StackSize + "Item");
                amountToRemove -= slot.StackSize;
            }
            else
            {
                //Debug.Log("Removed " + amountToRemove + "Item");
                slot.RemoveFromStack(amountToRemove);
                OnInventorySlotChanged?.Invoke(slot);
                return true;
            }
            
        }
        return true;
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
            if (slot.ItemData == null)
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

    public int NumFreeSlots()
    {
        int numberOfFreeSlots = 0;
        foreach(var slot in InventorySlots)
        {
            if(slot.ItemData == null)
            {
                numberOfFreeSlots++;
            }
        }
        return numberOfFreeSlots;
    }

    public void UpdateAllSlots()
    {
        foreach(var slot in inventorySlots)
        {
            OnInventorySlotChanged?.Invoke(slot);
        }
    }
}
