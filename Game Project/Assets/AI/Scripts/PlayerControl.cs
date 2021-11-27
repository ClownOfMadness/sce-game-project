using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class PlayerControl : MonoBehaviour
{
    // General
    private GameObject camera;

    // Unit Command
    [SerializeField] private LayerMask layerMask; // List of layers that the mouse can interact with
    public GameObject selectedObject;
    public AIDestinationSetter targetLocation;

    // Screen Panning
    public float panSpeed = 40f; // Pan speed
    public float panBorderTHICCNess = 10f; // Pan offscreen border THICCness
    public Vector2 panLimit; // Screen panning limit [[Should be changed by map size]]
    public float scrollSpeed = 10f; // Scroll speed
    public float minScroll = 30f; // Min zoom
    public float maxScroll = 50f; // Max zoom
    private float scroll = 40f; // Original zoom
    private Vector3 originalCameraPos; // Stores camera original position

    private void Awake()
    {
        camera = Camera.main.gameObject; // Finds the main camera in game
        originalCameraPos = camera.transform.position;
    }

    private void Update()
    {
        CameraControl(); // Screen panning functions
        UnitCommand(); // Unit pathfinding control;
    }

    private void UnitCommand()
    {
        // Raycast from camera to the mouse position on the game field
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, layerMask))
        {
            // Functions for when the mouse hovers over an interactable layer
            if (Input.GetMouseButton(0))
            {
                // Functions for when the mouse clicks on interactable layer
                if (raycastHit.transform.gameObject.layer == 6 || raycastHit.transform.gameObject.layer == 7) // if it is clicked on terrain or impassable
                {
                    // [[Later make an option to switch betwenn units]]

                    selectedObject = raycastHit.transform.gameObject;

                    // For test purposes
                    targetLocation.target = selectedObject.transform;
                }
            }
        }
    }

    private void CameraControl()
    {
        // [[In future, create a script or scriptable object that will contain key settings
        // and from there draw out the correct keys to be used in game]]
        
        Vector3 pos = camera.transform.position; // Stores original camera position
        if (Input.GetKey("w") || Input.mousePosition.y >= Screen.height - panBorderTHICCNess) // Up
        {
            pos.z += panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("s") || Input.mousePosition.y <= panBorderTHICCNess) // Down
        {
            pos.z -= panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("d") || Input.mousePosition.x >= Screen.width - panBorderTHICCNess) // Right
        {
            pos.x += panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("a") || Input.mousePosition.x <= panBorderTHICCNess) // Left
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
