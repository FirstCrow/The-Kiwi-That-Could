using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class MouseItemData : MonoBehaviour
{
    public Image ItemSprite;
    public TextMeshProUGUI ItemCount;
    public int ItemID;
    public InventorySlot AssignedInventorySlot;

    private void Awake()
    {
        ItemSprite.color = Color.clear;
        ItemCount.text = "";
        
    }

    private void Update()
    {
        if(AssignedInventorySlot != null)
        {
            transform.position = Input.mousePosition;

            if (Input.GetMouseButtonDown(0) && !IsPointerOverUIObject())
            {
                ClearSlot();
                return;
            }
        }
    }

    public void UpdateMouseSlot(InventorySlot invSlot)
    {
        AssignedInventorySlot.AssignItem(invSlot);
        ItemSprite.sprite = invSlot.ItemData.Icon;
        ItemCount.text = invSlot.StackSize.ToString();
        ItemID = invSlot.ItemData.ID;
        ItemSprite.color = Color.white;
    }

    public void ClearSlot()
    {
        AssignedInventorySlot.ClearSlot();
        ItemCount.text = "";
        ItemSprite.color = Color.clear;
        ItemSprite.sprite = null;
        ItemID = 0;
    }

    private static bool IsPointerOverUIObject()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

}
