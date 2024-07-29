using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;

public class BuildingSystem : MonoBehaviour
{
    public static BuildingSystem current;

    public GridLayout gridLayout;
    private Grid grid;
    [SerializeField] private Tilemap MainTilemap;
    [SerializeField] private TileBase GreenTile;

    public GameObject[] spawnBuildings;
    public GameObject[] turrets;
    public GameObject[] utilities;
    public Material validBuild;
    public Material invalidBuild;
    public Transform parent;
    public GameObject button;
    public GameObject rangeCircle;

    private PlaceableObject objectToPlace;
    private PlaceableObject selectedObject;

    private static LayerMask mask;
    private static LayerMask friendlyMask;

    private void Awake()
    {
        current = this;
        grid = gridLayout.gameObject.GetComponent<Grid>();
        mask = LayerMask.GetMask("Terrain");
        friendlyMask = LayerMask.GetMask("Friendly");
    }

    private void Update()
    {
        if (!objectToPlace && Input.GetMouseButtonDown(0) && !MouseInputUIBlocker.BlockedByUI)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, 100f, friendlyMask))
            {
                if (raycastHit.collider.gameObject.tag == "Building")
                {
                    selectedObject = raycastHit.collider.gameObject.GetComponent<PlaceableObject>();
                    try
                    {
                        float range = selectedObject.GetComponent<TurretScript>().range;
                        rangeCircle.SetActive(true);
                        rangeCircle.transform.position = selectedObject.transform.position;
                        rangeCircle.transform.localScale = new Vector3(range, range, range);

                        //Do not select object if is an enemy
                        if (selectedObject.GetComponent<TurretScript>().isEnemy)
                        {
                            selectedObject = null;
                        }
                        
                    }
                    catch (NullReferenceException) 
                    {
                        rangeCircle.SetActive(false);
                    }
                }
                else
                {
                    selectedObject = null;
                    rangeCircle.SetActive(false);
                }
            }
            else
            {
                selectedObject = null;
                rangeCircle.SetActive(false);
            }                
        }

        if (selectedObject)
        {
            TranslateButton();
        }
        else
        {
            button.SetActive(false);
        }

        //Anything After this line only applies if an object is being placed
        if (!objectToPlace)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0) && !MouseInputUIBlocker.BlockedByUI)
        {
            if (CanBePlaced(objectToPlace))
            {
                objectToPlace.Place();
                Vector3Int start = gridLayout.WorldToCell(objectToPlace.GetStartPosition());
                TakeArea(start, objectToPlace.Size);

                //Call game manager on place
                GameManager.instance.OnPlace();

                objectToPlace = null;
            }
            else
            {
                DestroyObjectToPlace();
            }
        }
        

        if (Input.GetMouseButtonDown(1))
        {
            DestroyObjectToPlace();
        }
    }

    //Utils
    public static Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 100f, mask))
        {
            return raycastHit.point;
        }
        else
            return Vector3.zero;
    }

    public Vector3 SnapCoordinateToGrid(Vector3 position)
    {
        Vector3Int cellPos = gridLayout.WorldToCell(position);
        position = grid.GetCellCenterWorld(cellPos);

        return position;
    }

    private static TileBase[] GetTilesBlock(BoundsInt area, Tilemap tilemap)
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

    //Buidling Placement

    public void InitializeWithObject(GameObject prefab)
    {
        Vector3 position = SnapCoordinateToGrid(Vector3.zero);

        GameObject obj = Instantiate(prefab, position, Quaternion.identity);
        objectToPlace = obj.GetComponent<PlaceableObject>();
        obj.AddComponent<ObjectDrag>();
    }

    public bool CanBePlaced(PlaceableObject placeableObject)
    {
        BoundsInt area = new BoundsInt();
        area.position = gridLayout.WorldToCell(objectToPlace.GetStartPosition());
        area.size = placeableObject.Size;

        //Check build area
        if(area.position.x < -5 || area.position.x + area.size.x - 1 > 4 || area.position.y < -19 || area.position.y + area.size.y - 1 > -11 ||
            ((area.position.x > -2 && area.position.x < 1) && area.position.y > -17) || ((area.position.x <= -2 && area.position.x + area.size.x - 1 > -2) && area.position.y > -17))
        {
            return false;
        }

        //Check constructed buildings
        TileBase[] baseArray = GetTilesBlock(area, MainTilemap);
        foreach (var b in baseArray)
        {
            if (b == GreenTile)
            {
                return false;
            }
        }

        //Check cost
        if (GameManager.instance.power < placeableObject.powerCost || GameManager.instance.cubits < placeableObject.cubitsCost)
            return false;

        return true;
    }

    public void DestroyObjectToPlace()
    {
        if(objectToPlace)
            Destroy(objectToPlace.gameObject);
    }

    public void TakeArea(Vector3Int start, Vector3Int size)
    {
        MainTilemap.BoxFill(start, GreenTile, start.x, start.y, start.x + size.x - 1, start.y + size.y - 1);
    }

    public void ClearArea(Vector3Int start, Vector3Int size)
    {
        MainTilemap.BoxFill(start, null, start.x, start.y, start.x + size.x - 1, start.y + size.y - 1);
    }

    public void DestroyButton()
    {
        DestroyBuilding(selectedObject);
        rangeCircle.SetActive(false);
    }
    
    public void DestroyBuilding(PlaceableObject building)
    {
        Vector3Int start = gridLayout.WorldToCell(building.GetStartPosition());
        ClearArea(start, building.Size);

        if(building == selectedObject)
        {
            rangeCircle.SetActive(false);
        }

        Destroy(building.gameObject);
    }

    private void TranslateButton()
    {
        button.SetActive(true);
        Vector3 pos = Camera.main.WorldToScreenPoint(selectedObject.gameObject.transform.position);
        button.transform.position = pos;
    }
}
