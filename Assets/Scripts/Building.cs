using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class Building : MonoBehaviour
{
    public bool placed;
    public BoundsInt area;

    void Start()
    {
        
    }

    public bool getPlaced()
    {
        return placed;
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
        GridBuildingSystem.current.takeArea(areaTemp);
    }

    #endregion 
}
