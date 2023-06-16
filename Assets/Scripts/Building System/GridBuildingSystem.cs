using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using UnityEngine.WSA;

// This script is the main logic of the grid building system
public class GridBuildingSystem : MonoBehaviour
{
    public static GridBuildingSystem current;

    public GridLayout gridLayout;
    public Tilemap mainTilemap;
    public Tilemap tempTilemap;

    private static Dictionary<TileType, TileBase> tileBases = new Dictionary<TileType, TileBase>();

    public float ghostOpacity;
    private Building tempBuilding;
    private SpriteRenderer rend;
    private Vector3 prevPos;
    private BoundsInt prevArea;
    private InventoryHolder playerInventory;
    public InventoryItemData AutoShopBlueprint;

    public TileBase redTile;
    public TileBase greenTile;
    public TileBase whiteTile;


    #region Unity Methods

    private void Awake()
    {
        current = this;
        playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<InventoryHolder>();
    }
    private void Start()
    {
    
        tileBases.Add(TileType.empty, null);
        tileBases.Add(TileType.white, whiteTile);
        tileBases.Add(TileType.green, greenTile);
        tileBases.Add(TileType.red, redTile);
    }

    private void Update()
    {
        if(tempBuilding == null)
        {
            return;
        }

        if(!tempBuilding.placed && !PauseMenu.getGameIsPaused())
        {
            mainTilemap.gameObject.SetActive(true);
            tempTilemap.gameObject.SetActive(true);
            Vector2 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 ghostPos = gridLayout.LocalToWorld(cursorPos);
            tempBuilding.transform.localPosition = ghostPos;
            Vector3Int cellPos = gridLayout.LocalToCell(cursorPos);

            if(prevPos != cellPos)
            {
                tempBuilding.transform.localPosition = gridLayout.CellToLocalInterpolated(cellPos + new Vector3(.5f, .5f, 0f));
                prevPos = cellPos;
                followBuilding();
            }

            if (Input.GetMouseButtonDown(0))
            {
                if (playerInventory.InventorySystem.ContainsItem(AutoShopBlueprint, out List<InventorySlot> invSlot) && tempBuilding.canBePlaced())
                {
                    mainTilemap.gameObject.SetActive(false);
                    tempTilemap.gameObject.SetActive(false);
                    invSlot[invSlot.Count - 1].RemoveFromStack(1);
                    rend.color = new Color(1f, 1f, 1f, 1f);
                    tempBuilding.transform.localPosition = gridLayout.CellToLocalInterpolated(cellPos + new Vector3(.5f, .5f, 0f));
                    tempBuilding.place();
                }
            }
            if (Input.GetMouseButtonDown(1))
            {
                clearArea();
                mainTilemap.gameObject.SetActive(false);
                tempTilemap.gameObject.SetActive(false);
                Destroy(tempBuilding.gameObject);
                tempBuilding = null;
            }
        }
    }

    #endregion

    #region Tilemap Management

    // gets the position of all tiles in the area
    private static TileBase[] getTilesBlock(BoundsInt area, Tilemap tilemap)
    {
        TileBase[] array = new TileBase[area.size.x * area.size.y * area.size.z];
        int counter = 0;

        foreach(var v in area.allPositionsWithin)
        {
            Vector3Int pos = new Vector3Int(v.x, v.y, 0);
            array[counter] = tilemap.GetTile(pos);
            counter++;
               
        }

        return array;
    }

    // fills the files in area by calling fill tiles helper method
    private static void setTilesBlock(BoundsInt area, TileType type, Tilemap tilemap)
    {
        int size = area.size.x * area.size.y * area.size.z;
        TileBase[] tileArray = new TileBase[size];
        fillTiles(tileArray, type);
        tilemap.SetTilesBlock(area, tileArray);
    }

    // Fills every tile in arr with the passed tile type
    private static void fillTiles(TileBase[] arr, TileType type)
    {
        for (int i = 0; i < arr.Length; i++)
        {
            arr[i] = tileBases[type];
        }
    }

    #endregion

    #region Building Placement

    public void initializeWithBuilding(GameObject building)
    {
        tempBuilding = Instantiate(building, Vector3.zero, Quaternion.identity).GetComponent<Building>();
        rend = tempBuilding.gameObject.GetComponentInChildren<SpriteRenderer>();
        rend.color = new Color(1f, 1f, 1f, ghostOpacity);
        followBuilding();
    }

    // Clears area where building previously was
    private void clearArea()
    {
        TileBase[] toClear = new TileBase[prevArea.size.x * prevArea.size.y * prevArea.size.z];
        fillTiles(toClear, TileType.empty);
        tempTilemap.SetTilesBlock(prevArea, toClear);
    }

    private void followBuilding()
    {
        clearArea();

        tempBuilding.area.position = gridLayout.WorldToCell(tempBuilding.gameObject.transform.position);
        BoundsInt buildingArea = tempBuilding.area;

        TileBase[] baseArray = getTilesBlock(buildingArea, mainTilemap);

        int size = baseArray.Length;
        TileBase[] tileArray = new TileBase[size];


        // Checks if there is a empty building spot of size "size"
        for(int i = 0; i < baseArray.Length; i++)
        {
            // fills area with green if it is valid
            if (baseArray[i] == tileBases[TileType.white])
            {
                tileArray[i] = tileBases[TileType.green];
            }
            // breaks and fills with red if not valid
            else
            {
                fillTiles(tileArray, TileType.red);
                break;
            }
        }

        tempTilemap.SetTilesBlock(buildingArea, tileArray);
        prevArea = buildingArea;
    }

    public bool canTakeArea(BoundsInt area)
    {
        TileBase[] baseArray = getTilesBlock(area, mainTilemap);
        foreach(var b in baseArray)
        {
            // If all tiles are not white, return false
            if(b != tileBases[TileType.white])
            {
                Debug.Log("Cannot place here.");
                return false;
            }
        }
        return true;
    }

    public void takeArea(BoundsInt area)
    {
        setTilesBlock(area, TileType.empty, tempTilemap);
        setTilesBlock(area, TileType.green, mainTilemap);
    }

    #endregion

    public enum TileType
    {
        empty,
        white,
        green,
        red
    }
}
