using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.EventSystems;

public class Player_Control : MonoBehaviour
{
    //--------------------------------------[To-Do List]-----------------------------------------------

    // To add: (optional) add option to also move with keyboard

    // General
    private GameObject camera; // Camera gameobject

    // Unit Command
    [SerializeField] private LayerMask layerMask; // List of layers that the mouse can interact with
    public GameObject selectedObject; // An object that the player has clicked on
    public Data_Unit Data_Unit; // Data of the unit
    public Unit_List unitList; // List of units script
    public int selectedJob = 0; // Current selected unit job group to command
    public GameObject selectedUnit; // Current selected unit from the job
    private Data_Tile selectedData_Tile; // Current selected tile data

    // Screen Panning
    public Vector3 startPosition;
    public Vector3 pos; // Camera movement vector
    public float panSpeed = 40f; // Pan speed
    public float panBorderTHICCNess = 10f; // Pan offscreen border THICCness
    public Vector2 panLimit; // Screen panning limit [[Should be changed by map size]]
    public float border;
    public float scrollSpeed = 10f; // Scroll speed
    public float minScroll = 30f; // Min zoom
    public float maxScroll = 50f; // Max zoom
    public float scroll = 40f; // Original zoom
    private Vector3 originalCameraPos; // Stores camera original position
    public Map_Gen mapGen;
    private int loadCount = 0;

    // Card
    public Screen_Cards screenCards;

    private void Awake()
    {
        camera = Camera.main.gameObject; // Finds the main camera in game
        originalCameraPos = camera.transform.position; // Default camera position

        // Safe Check
        if (!unitList)
        {
            Debug.LogError("Unit_List script object is missing in Player_Control");
        }
        if (!screenCards)
        {
            Debug.LogError("Screen_Cards script object is missing in Player_Control");
        }
        if (!mapGen)
        {
            Debug.LogError("Map_Gen script is missing in Player_Control");
        }
    }
    private void Start()
    {
        // Screen Pan
        //border = (mapGen.mapSize - 1f) * 10f;
    }
    private void Update()
    {
        UnitCommand(); // Unit pathfinding control;
        FindOnce();
    }

    private void LateUpdate()
    {
        CameraControl(); // Screen panning functions
    }
    private void FindOnce()
    {
        if (loadCount > 60)
        {
            Debug.LogError("Failed to find needed parameters in FindOnce() in the Player_Control script");
        }
        else
        {
            if (border==0)
            {
                if ((border = (mapGen.mapSize - 1f) * 10f) ==0)
                {
                    loadCount++;
                }
            }
        }
    }
    private void UnitCommand()
    {
        // Raycast from camera to the mouse position on the game field
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, layerMask))
        {
            // Functions for when the mouse hovers over an interactable layer
            if (EventSystem.current.currentSelectedGameObject == null) 
            {
                // Checks if raycast doesnt hit ui
                if (raycastHit.transform.gameObject.layer == 6 || raycastHit.transform.gameObject.layer == 7)
                {
                    // if it is terrain or impassable or townhall
                    if (Input.GetMouseButtonDown(0) && !Input.GetKey("space"))
                    {
                        // Functions for when the mouse clicks on interactable layer

                        // Gives order to the unit if they are available
                        selectedObject = raycastHit.transform.gameObject;
                        selectedData_Tile = selectedObject.GetComponent<Data_Tile>();
                        if (!selectedData_Tile.GetData())
                        {
                            selectedUnit = UnitSelection(selectedJob, selectedObject, selectedData_Tile.hasTownHall);
                            if (selectedUnit)
                            {
                                Data_Unit = selectedUnit.GetComponent<Data_Unit>();
                                if (Data_Unit)
                                {
                                    //selectedData_Tile.AttachWork(selectedUnit);
                                    Data_Unit.UpdateTargetLocation(selectedObject);
                                }
                            }
                        }
                    }

                    if (Input.GetMouseButtonDown(1))
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
            }

            if (screenCards.draggedCard)
            {
                if (raycastHit.transform.gameObject.layer == 6)
                {
                    // Checks if the selected object is a terrain tile
                    if (screenCards.selectedTile != raycastHit.transform.gameObject)
                    {
                        screenCards.selectedTile = raycastHit.transform.gameObject;
                    }
                }
            }
        }
    }

    private GameObject UnitSelection(int _selectedJob, GameObject target, bool townhall) // Searches for a free unit in a job category
    {
        float distance = 0f;
        float lowestDistance = 99999f;
        GameObject bestOption = null;
        if (_selectedJob < 0 || _selectedJob >= unitList.units.Length)
            return null;
        else
        {
            foreach (Transform unitInGroup in unitList.units[_selectedJob].unitGroup.transform)
            {
                GameObject unitInGroupObject = unitInGroup.gameObject;
                distance = Vector3.Distance(unitInGroup.position, target.transform.position);
                Data_Unit tempData = unitInGroupObject.GetComponent<Data_Unit>();
                if (tempData.busy == false && tempData.card == null)
                {
                    if (lowestDistance > distance)
                    {
                        lowestDistance = distance;
                        bestOption = unitInGroupObject;
                    }
                }
            }
            return bestOption;
        }
    }

    private void OldCameraControl() // [[Can be removed]]
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

        if (Input.GetMouseButtonDown(2))
        {
            startPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        if (Input.GetMouseButton(2))
        {
            Vector3 newPos = startPosition - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            camera.transform.position += newPos;
        }

        // Controls the zoom in and zoom out and limits it
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        scroll -= scrollInput * scrollSpeed * 100f * Time.deltaTime;
        scroll = Mathf.Clamp(scroll, minScroll, maxScroll);
        Camera.main.orthographicSize = scroll;

        // Limits the camera from moving too far away
        pos.x = Mathf.Clamp(pos.x, originalCameraPos.x - panLimit.x, originalCameraPos.x + panLimit.x);
        pos.z = Mathf.Clamp(pos.z, originalCameraPos.z - panLimit.y, originalCameraPos.z + panLimit.y);

        //camera.transform.position = pos; // Updates camera position;
    }

    private void CameraControl()
    {
        if (Input.GetMouseButtonDown(2) || (Input.GetKey("space") && Input.GetMouseButtonDown(0)))
        {
            startPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        if (Input.GetMouseButton(2) || (Input.GetKey("space") && Input.GetMouseButton(0)))
        {
            Vector3 newPos = startPosition - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            camera.transform.position = ClampCamera(camera.transform.position + newPos);
        }

        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        scroll -= scrollInput * scrollSpeed * 100f * Time.deltaTime;
        scroll = Mathf.Clamp(scroll, minScroll, maxScroll);
        Camera.main.orthographicSize = scroll;
        camera.transform.position = ClampCamera(camera.transform.position);
    }

    private Vector3 ClampCamera(Vector3 targetPosition)
    {
        float camHeight = Camera.main.orthographicSize;
        float camWidth = Camera.main.orthographicSize * Camera.main.aspect;

        float minX = 5f + camWidth;
        float maxX = border - 5f - camWidth;
        float minY = 5f + camHeight;
        float maxY = border - 5f - camHeight;

        float newX = Mathf.Clamp(targetPosition.x, minX, maxX);
        float newZ = Mathf.Clamp(targetPosition.z, minY, maxY);

        return new Vector3(newX, 150f, newZ);
    }
}
