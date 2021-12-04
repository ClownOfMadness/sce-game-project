using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Player_Control : MonoBehaviour
{
    // General
    private GameObject camera; // Camera gameobject

    // Unit Command
    [SerializeField] private LayerMask layerMask; // List of layers that the mouse can interact with
    public GameObject selectedObject; // An object that the player has clicked on
    public Data_Unit Data_Unit; // Data of the unit
    public Unit_List Unit_List; // List of units script
    public int selectedJob = 0; // Current selected unit job group to command
    public GameObject selectedUnit; // Current selected unit from the job
    private Data_Tile selectedData_Tile; // Current selected tile data

    // Screen Panning
    public Vector3 pos; // Camera movement vector
    public float panSpeed = 40f; // Pan speed
    public float panBorderTHICCNess = 10f; // Pan offscreen border THICCness
    public Vector2 panLimit; // Screen panning limit [[Should be changed by map size]]
    public float scrollSpeed = 10f; // Scroll speed
    public float minScroll = 30f; // Min zoom
    public float maxScroll = 50f; // Max zoom
    public float scroll = 40f; // Original zoom
    private Vector3 originalCameraPos; // Stores camera original position

    // Card
    public Zone_Map zoneMap; // Zonemap script for passing location

    private void Awake()
    {
        camera = Camera.main.gameObject; // Finds the main camera in game
        originalCameraPos = camera.transform.position; // Default camera position
    }

    private void Update()
    {
        CameraControl(); // Screen panning functions
        UnitCommand(); // Unit pathfinding control;
        //SwitchJob();
    }

    private void UnitCommand()
    {
        // Raycast from camera to the mouse position on the game field
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, layerMask))
        {
            // Functions for when the mouse hovers over an interactable layer
            if (raycastHit.transform.gameObject.layer == 6 || raycastHit.transform.gameObject.layer == 7) // if it is terrain or impassable
            {
                if (Input.GetMouseButton(0)) // if it is clicked on terrain or impassable
                {
                    // Functions for when the mouse clicks on interactable layer

                    // Gives order to the unit if they are available
                    selectedObject = raycastHit.transform.gameObject;
                    selectedData_Tile = selectedObject.GetComponent<Data_Tile>();
                    if (!selectedData_Tile.GetData())
                    {
                        selectedUnit = UnitSelection(selectedJob);
                        if (selectedUnit)
                        {
                            Data_Unit = selectedUnit.GetComponent<Data_Unit>();
                            if (Data_Unit)
                            {
                                selectedData_Tile.AttachWork(selectedUnit);
                                Data_Unit.UpdateTargetLocation(selectedObject);
                            }
                        }
                    }
                }

                if (Input.GetMouseButton(1))
                {
                    // Cancels units current job
                    selectedObject = raycastHit.transform.gameObject;
                    selectedData_Tile = selectedObject.GetComponent<Data_Tile>();
                    if (selectedData_Tile.GetData())
                    {
                        (selectedData_Tile.GetObject()).GetComponent<Data_Unit>().RemoveTargetLocation();
                        selectedData_Tile.DetachWork();
                    }
                }
            }
            
            if (raycastHit.transform.gameObject.layer == 6)
            {
                // Checks if the selected object is a terrain tile
                zoneMap.selectedTile = raycastHit.transform.gameObject; // Pass the current selected tile to the zonemap script
            }
        }
    }

    private GameObject UnitSelection(int _selectedJob) // Searches for a free unit in a job category
    {
        switch (_selectedJob)
        {
            case 0: // Peasants
                foreach (Transform unitInGroup in Unit_List.units[0].unitGroup.transform)
                {
                    GameObject unitInGroupObject = unitInGroup.gameObject;
                    if (unitInGroupObject.GetComponent<Data_Unit>().busy == false)
                    {
                        return unitInGroupObject;
                    }
                }
                return null;
            default:
                return null;
        }
    }

    private void CameraControl()
    {
        // [[In future, create a script or scriptable object that will contain key settings
        // and from there draw out the correct keys to be used in game]]
        
        pos = camera.transform.position; // Stores original camera position
        if (Input.GetKey("w")) // Up - old: Input.mousePosition.y >= Screen.height - panBorderTHICCNess
        {
            pos.z += panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("s")) // Down - old: Input.mousePosition.y <= panBorderTHICCNess
        {
            pos.z -= panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("d")) // Right - old: Input.mousePosition.x >= Screen.width - panBorderTHICCNess
        {
            pos.x += panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("a")) // Left - old: Input.mousePosition.x <= panBorderTHICCNess
        {
            pos.x -= panSpeed * Time.deltaTime;
        }

        // Controls the zoom in and zoom out and limits it
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        scroll -= scrollInput * scrollSpeed * 100f * Time.deltaTime;
        scroll = Mathf.Clamp(scroll, minScroll, maxScroll);
        Camera.main.orthographicSize = scroll;

        // Limits the camera from moving too far away
        pos.x = Mathf.Clamp(pos.x, originalCameraPos.x - panLimit.x, originalCameraPos.x + panLimit.x);
        pos.z = Mathf.Clamp(pos.z, originalCameraPos.z - panLimit.y, originalCameraPos.z + panLimit.y);

        camera.transform.position = pos; // Updates camera position;
    }
}
