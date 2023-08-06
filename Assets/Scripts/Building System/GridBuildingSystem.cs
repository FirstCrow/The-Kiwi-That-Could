using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.IO;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
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
    private bool buildModeEnabled;
    private List<InventorySlot> invSlot;

    public TileBase redTile;
    public TileBase greenTile;
    public TileBase whiteTile;
    public TileBase yellowTile;

    [Header("TEMP VARIBLES")]

    public GameObject AutoShop;
    public GameObject Castle;
    public GameObject Storage;
    public GameObject PathPrefab;
    public GameObject player;

    



    #region Unity Methods

    private void Awake()
    {
        current = this;
        playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<InventoryHolder>();
    }
    private void Start()
    {
        buildModeEnabled = false;
    
        tileBases.Add(TileType.empty, null);
        tileBases.Add(TileType.white, whiteTile);
        tileBases.Add(TileType.green, greenTile);
        tileBases.Add(TileType.red, redTile);
        tileBases.Add(TileType.yellow, yellowTile);
    }

    private void Update()
    {

        if (!buildModeEnabled && !PauseMenu.getGameIsPaused())
        {
            if(Input.GetKeyDown(KeyCode.Alpha1))
            {
                initializeWithBuilding(AutoShop);
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                initializeWithBuilding(Castle);
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                initializeWithBuilding(Storage);
            }

            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                initializeWithBuilding(PathPrefab);
            }


        }



        if (tempBuilding == null)
        {
            return;
        }

        if(!tempBuilding.placed && !PauseMenu.getGameIsPaused())
        {
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
                if (tempBuilding.canBePlaced())
                {
                    placeBuilding(cellPos);
                }
            }
            if (Input.GetMouseButtonDown(1))
            {
                cancelBuilding();
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
    private static TileBase[] getTilesBlockPath(Vector3Int tileCoordinates, Tilemap tilemap)
    {
        TileBase[] array = new TileBase[4];
        int counter = 0;
        Debug.Log(tileCoordinates);
        
        while(counter < 4)
        {
            var v = tileCoordinates;

            if(counter == 0)            // First Pass
            {
                v += new Vector3Int(1, 0, 0);
            }
            else if(counter == 1)       // Second Pass
            {
                v += new Vector3Int(0, 1, 0);
            }
            else if (counter == 2)      // Third Pass
            {
                v += new Vector3Int(-1, 0, 0);
            }
            else                        // Final Pass
            {
                v += new Vector3Int(0, -1, 0);
            }
            array[counter] = tilemap.GetTile(v);
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
        var blueprint = building.GetComponent<Building>().getBlueprint();
        bool hasBlueprint = playerInventory.InventorySystem.ContainsItem(blueprint, out List<InventorySlot> invSlot);

        if(!hasBlueprint)
            return;

        this.invSlot = invSlot;
        tempBuilding = Instantiate(building, Vector3.zero, Quaternion.identity).GetComponent<Building>();
        rend = tempBuilding.gameObject.GetComponentInChildren<SpriteRenderer>();
        rend.color = new Color(1f, 1f, 1f, ghostOpacity);
        setBuildMode(true);
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

    public List<bool> checkAdjacent(out int numAdjacent)
    {
        TileBase[] baseArray = getTilesBlockPath(tempBuilding.area.position, mainTilemap);              
        List<bool> directions = new List<bool>();
        numAdjacent = 0;                                                                      // Direction Key:
        foreach (var b in baseArray)                                                          // 1st element: up right
        {                                                                                     // 2nd element: up left
            if(b == tileBases[TileType.yellow])                                               // 3rd element: down left
            {                                                                                 // 4th element: down right
                directions.Add(true);
                numAdjacent++;
            }
            else
                directions.Add(false);
        }

        foreach(var b in directions)
        {
            Debug.Log(b);
        }
        return directions;                                  // If the element in coordinates is true, that tile is yellow (occupied by a path)
    }

    public void takeArea(BoundsInt area, string type)
    {
        if (type.Equals("path"))
        {
            setTilesBlock(area, TileType.empty, tempTilemap);
            setTilesBlock(area, TileType.yellow, mainTilemap);
            List<bool> directions = checkAdjacent(out int numAdjacent);

            if (numAdjacent == 0)
            {
                return;
            }

            else if(numAdjacent == 1)
            {
                // Check for which direction it is and change texture
                int counter = 0;
                foreach(var v in directions)
                {
                    if (v == true)
                        break;
                    counter++;
                }


                switch(counter)
                {
                    case 0:
                        //Change to up right texture
                        break;

                    case 1:
                        //Change to up left texture
                        break;

                    case 2:
                        //Change to down left texture
                        break;

                    case 3:
                        //Change to down right texture
                        break;
                }


            }

            else if (numAdjacent == 2)
            {
                // Check for which 2 directions and change texture
                Vector2Int direction = new Vector2Int();

                for (int i = 0; i < directions.Count; i++)
                {
                    if (directions[i] == true)
                    {
                        direction = new Vector2Int(i, 0);
                        break;
                    }
                }

                for (int i = directions.Count - 1; i >= 0; i--)
                {
                    if (directions[i] == true)
                    {
                        direction = new Vector2Int(direction.x, i);
                        break;
                    }
                }

                if(direction.Equals(new Vector2Int(0,1)))
                {
                    //Change to upwards facing corner
                }
                else if (direction.Equals(new Vector2Int(1, 2)))
                {
                    //Change to leftwards facing corner
                }
                else if (direction.Equals(new Vector2Int(2, 3)))
                {
                    //Change to downwards facing corner
                }
                else if (direction.Equals(new Vector2Int(0, 3)))
                {
                    //Change to rightward facing corner
                }
                else if (direction.Equals(new Vector2Int(0, 2)))
                {
                    //Change to "/" path
                }
                else if (direction.Equals(new Vector2Int(1, 3)))
                {
                    //Change to "\" path
                }

            }

            else if (numAdjacent == 3)
            {
                int counter = 0;
                foreach (var v in directions)
                {
                    if (v == false)
                        break;
                    counter++;
                }


                switch (counter)
                {
                    case 0:
                        //Change to missing up right crossroad texture
                        break;

                    case 1:
                        //Change to missing up left crossroad texture
                        break;

                    case 2:
                        //Change to missing down left crossroad texture
                        break;

                    case 3:
                        //Change to missing down right crossroad texture
                        break;
                }
            }

            else if (numAdjacent == 4)
            {
                // Change texture to crossroad
            }



        }
        else
        {
            setTilesBlock(area, TileType.empty, tempTilemap);
            setTilesBlock(area, TileType.green, mainTilemap);
        }
    }

    #endregion

    #region Helper Methods
    private void setBuildMode(bool isBuildModeOn)
    {
        mainTilemap.gameObject.SetActive(isBuildModeOn);
        tempTilemap.gameObject.SetActive(isBuildModeOn);
        buildModeEnabled = isBuildModeOn;
    }

    private void placeBuilding(Vector3Int cellPos)
    {
        setBuildMode(false);
        invSlot[invSlot.Count - 1].RemoveFromStack(1);
        rend.color = new Color(1f, 1f, 1f, 1f);
        tempBuilding.transform.localPosition = gridLayout.CellToLocalInterpolated(cellPos + new Vector3(.5f, .5f, 0f));
        player.GetComponent<InventoryHolder>().InventorySystem.UpdateAllSlots();
        tempBuilding.place();
    }

    private void cancelBuilding()
    {
        clearArea();
        setBuildMode(false);
        Destroy(tempBuilding.gameObject);
        tempBuilding = null;
    }
    #endregion


    public enum TileType
    {
        empty,
        white,
        green,
        red,
        yellow,
    }
}
