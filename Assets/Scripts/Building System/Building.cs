using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class Building : MonoBehaviour
{
    public bool placed;
    public BoundsInt area;
    public InventoryItemData blueprint;
    public string type;                     // List of types: "building", "path" (changes what color of tile occupies the building square)

    void Start()
    {
        
    }

    public bool getPlaced()
    {
        return placed;
    }

    public InventoryItemData getBlueprint()
    {
        return blueprint;
    }

    private void setPlaced(bool p)
    {
        placed = p;
    }

    #region Building Methods

    public bool canBePlaced()
    {
        Vector3Int positionInt = GridBuildingSystem.current.gridLayout.LocalToCell(transform.position);
        BoundsInt areaTemp = area;
        areaTemp.position = positionInt;

        if(GridBuildingSystem.current.canTakeArea(areaTemp))
        {
            return true;
        }
        return false;
    }

    public void place()
    {
        Vector3Int positionInt = GridBuildingSystem.current.gridLayout.LocalToCell(transform.position);
        BoundsInt areaTemp = area;
        areaTemp.position = positionInt;
        placed = true;
        GridBuildingSystem.current.takeArea(areaTemp, type);
    }

    public void setSprite()
    {

    }

    #endregion 
}
