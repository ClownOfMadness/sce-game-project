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
    private GameObject cameraObject; // Camera gameobject

    // Unit Command
    [SerializeField] private LayerMask layerMask; // List of layers that the mouse can interact with
    public GameObject selectedObject; // An object that the player has clicked on
    public GameObject gizmoObject;
    public string draggedType = "none";
    public Sprite draggedSprite = null;
    public Data_Unit Data_Unit; // Data of the unit
    public Unit_List unitList; // List of units script
    public int selectedJob = 0; // Current selected unit job group to command
    public GameObject selectedUnit; // Current selected unit from the job
    private Data_Tile selectedData_Tile; // Current selected tile data

    // Key Binding
    [HideInInspector] public KeyCode panScreen = KeyCode.Space; //yields nothing atm
    [HideInInspector] public KeyCode sprint; 
    [HideInInspector] public KeyCode up;
    [HideInInspector] public KeyCode down;
    [HideInInspector] public KeyCode right;
    [HideInInspector] public KeyCode left;

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

    // Player
    public GameObject playerPrefab = null;
    private GameObject player = null;
    private Rigidbody rb = null;
    private SpriteRenderer sprite = null;
    private Animator animator = null;
    private float zMov = 0f;
    private float xMov = 0f;
    private Vector3 rawDirection = Vector3.zero;
    private Vector3 normDirection = Vector3.zero;
    private float normMagnitude = 0f;
    private float speed = 0f;
    private float amplitude = 0f;
    private bool sprinting = false;
    public int maxStamina = 5;
    public int stamina = 5;
    private bool tired = false;
    private float nextDrain = 0f;
    private float drainCD = 0.25f;
    private bool chargeDone = false;
    private Vector3 mousePosition = Vector3.zero;
    public bool gameLost = false;
    private Data_CommonDataHolder commonData;
    public GameObject detector;
    public GameObject currentTileOn;

    // Card
    public Screen_Cards screenCards;

    private void Awake()
    {
        cameraObject = Camera.main.gameObject; // Finds the main camera in game
        originalCameraPos = cameraObject.transform.position; // Default camera position

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
        //Keycodes: In order to update the keycodes they have to be called in awake
        sprint = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Sprint", "LeftShift"));
        up = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("MoveUp", "W"));
        down = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("MoveDown", "S"));
        right = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("MoveRight", "D"));
        left = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("MoveLeft", "A"));
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
        PlayerInput();
        Animator();
        PlayerController();
        Tired();
        if (player) GroundCheck(); 

    }

    private void FixedUpdate()
    {
        if (player)
        {
            Movement();
            CameraFollow();
        }
    }

    private void LateUpdate()
    {
        if (!player)
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

            if (!commonData)
            {
                if (!(commonData = GameObject.Find("Map Generator").GetComponent<Data_CommonDataHolder>()))
                {
                    loadCount++;
                }
            }
        }
    }

    private void GroundCheck()
    {
        RaycastHit hit;
        Debug.DrawRay(detector.transform.position, Vector3.down, Color.yellow);
        if (Physics.Raycast(detector.transform.position, Vector3.down, out hit, 1.1f))
        {
            currentTileOn = hit.transform.gameObject;
        }
    }

    private void UnitCommand()
    {
        // Raycast from camera to the mouse position on the game field
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, layerMask))
        {
            if (Time.timeScale != 0 && !gameLost)
            {
                mousePosition = raycastHit.point;
                // Functions for when the mouse hovers over an interactable layer
                if (EventSystem.current.currentSelectedGameObject == null)
                {
                    // Checks if raycast doesnt hit ui
                    if (raycastHit.transform.gameObject.layer == 6 || raycastHit.transform.gameObject.layer == 7)
                    {
                        // if it is terrain or impassable or townhall
                        if (Input.GetMouseButtonDown(0) && !Input.GetKey(panScreen))
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
                                        if ((Data_Unit.canBuild && selectedData_Tile.hasBuilding && !selectedData_Tile.buildingComplete) || (!selectedData_Tile.hasBuilding))
                                        {
                                            Data_Unit.UpdateTargetLocation(selectedObject);
                                            selectedData_Tile.DrawPointer();
                                        }
                                    }
                                }
                            }
                        }

                        if (Input.GetMouseButtonDown(1))
                        {
                            // Cancels units current job
                            selectedObject = raycastHit.transform.gameObject;
                            selectedData_Tile = selectedObject.GetComponent<Data_Tile>();
                            
                            if (selectedData_Tile.GetData() || selectedData_Tile.GetBuildData())
                            {
                                GameObject theUnit = selectedData_Tile.GetObject();
                                if (theUnit)
                                    theUnit.GetComponent<Data_Unit>().RemoveTargetLocation();
                                selectedData_Tile.DetachWork();
                                selectedData_Tile.DetachBuild(theUnit);
                            }
                        }

                        // [[Temporary]]
                        if (Input.GetKeyDown(KeyCode.P))
                        {
                            selectedObject = raycastHit.transform.gameObject;
                            CreatePlayer(selectedObject);
                        }
                    }
                }

                if (raycastHit.transform.gameObject.layer == 6 || raycastHit.transform.gameObject.layer == 7)
                {
                    if ((screenCards.draggedBuilding || screenCards.draggedUnit) && screenCards.draggedSprite)
                    {
                        gizmoObject = raycastHit.transform.gameObject;
                        draggedSprite = screenCards.draggedSprite;
                        if (screenCards.draggedBuilding)
                            draggedType = "building";
                        else if (screenCards.draggedUnit)
                            draggedType = "unit";
                        // Checks if the selected object is a terrain tile
                        if (screenCards.selectedTile != raycastHit.transform.gameObject)
                        {
                            screenCards.selectedTile = raycastHit.transform.gameObject;
                        }
                    }
                    else
                    {
                        gizmoObject = null;
                        draggedSprite = null;
                        draggedType = "none";
                    }
                }
            }
        }
    }
    private void PlayerInput()
    {
        if (Input.GetKey("1"))
        {
            selectedJob = 0;
        }
        if (Input.GetKey("2"))
        {
            selectedJob = 1;
        }
        if (Input.GetKey("3"))
        {
            selectedJob = 2;
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
        
        pos = cameraObject.transform.position; // Stores original camera position
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
            cameraObject.transform.position += newPos;
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
        if (Input.GetMouseButtonDown(2) || (Input.GetKey(panScreen) && Input.GetMouseButtonDown(0)))
        {
            startPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        if (Input.GetMouseButton(2) || (Input.GetKey(panScreen) && Input.GetMouseButton(0)))
        {
            Vector3 newPos = startPosition - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            cameraObject.transform.position = ClampCamera(cameraObject.transform.position + newPos);
        }

        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        scroll -= scrollInput * scrollSpeed * 100f * Time.deltaTime;
        scroll = Mathf.Clamp(scroll, minScroll, maxScroll);
        Camera.main.orthographicSize = scroll;
        cameraObject.transform.position = ClampCamera(cameraObject.transform.position);
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

    public bool CreatePlayer(GameObject tile)
    {
        if (!player)
        {
            Data_Tile dataTile = tile.GetComponent<Data_Tile>();
            if (!dataTile.hasBuilding && dataTile.gameObject.layer != 7)
            {
                player = Instantiate(playerPrefab, tile.transform.position + new Vector3(0,0.01f,0), Quaternion.Euler(0, 0, 0));
                sprite = player.GetComponentInChildren<SpriteRenderer>();
                animator = player.GetComponentInChildren<Animator>();
                rb = player.GetComponent<Rigidbody>();
                cameraObject.GetComponent<Camera>().orthographicSize = 40f;
                detector = player.transform.GetChild(1).gameObject;
                return true;
            }
        }
        return false;
    }

    private void Animator()
    {
        if (sprite && animator)
        {
            sprite.sortingOrder = -(int)Mathf.Abs(player.transform.position.z);

            if (normDirection.x > 0.01f)
            {
                sprite.flipX = true;
            }
            else if (normDirection.x < -0.01f)
            {
                sprite.flipX = false;
            }

            if (screenCards.CreationRevealed)
            {
                animator.SetBool("hasCard", true);
            }
            else
            {
                animator.SetBool("hasCard", false);
            }

            if (tired)
            {
                animator.SetBool("tired", true);
            }
            else
            {
                animator.SetBool("tired", false);
            }

            if (normMagnitude > 0)
            {
                animator.SetBool("running", true);
            }
            else
            {
                animator.SetBool("running", false);
            }
        }
    }

    private void PlayerController()
    {
        if (player && !gameLost)
        {
            if (!tired)
            {
                if (Input.GetKey(sprint))
                {
                    amplitude = 20f;
                    sprinting = true;
                    if (animator.GetBool("running") == true)
                        animator.speed = 2f;
                }
                else
                {
                    amplitude = 10f;
                    sprinting = false;
                    if (animator.speed != 1f)
                        animator.speed = 1f;
                }
            }
            else
            {
                amplitude = 0f;
                sprinting = false;
                if (animator.speed != 1f)
                    animator.speed = 1f;
            }
            
            if (Input.GetKey(up) && !Input.GetKey(down))
            {
                zMov = 1;
            }
            else if (Input.GetKey(down) && !Input.GetKey(up))
            {
                zMov = -1;
            }
            else
            {
                zMov = 0;
            }

            if (Input.GetKey(right) && !Input.GetKey(left))
            {
                xMov = 1;
            }
            else if (Input.GetKey(left) && !Input.GetKey(right))
            {
                xMov = -1;
            }
            else
            {
                xMov = 0;
            }

            rawDirection = new Vector3(xMov, 0, zMov);

            if (rawDirection.magnitude > 1)
            {
                normDirection = rawDirection.normalized;
            }
            else
            {
                normDirection = rawDirection;
            }

            normMagnitude = normDirection.magnitude;
            speed = normMagnitude * amplitude;
        }
    }

    private void Movement()
    {
        if (rb && !gameLost)
        {
            //camera.transform.position = rb.position + new Vector3(0f, 148.99f, 0f);
            rb.velocity = normDirection * speed;
        }
    }

    public void Hurt(Data_Enemy enemy)
    {
        StartCoroutine(HurtAnim());
        if (screenCards.CreationRevealed)
        {
            Vector3 moveDirection = rb.transform.position - enemy.gameObject.transform.position;
            rb.AddForce(moveDirection.normalized * 1000f);
            Data_Card tmp = screenCards.DamageMaster();
            enemy.card = tmp;
            tmp = null;
            gameLost = true;
            player.gameObject.layer = 0;
            animator.SetBool("die", true);
        }
        else
        {
            Vector3 moveDirection = rb.transform.position - enemy.gameObject.transform.position;
            rb.AddForce(moveDirection.normalized * 4000f);
            Data_Card tmp = screenCards.DamageMaster();
            enemy.card = tmp;
            tmp = null;
        }
    }

    private void Tired()
    {
        if (player && !gameLost)
        {
            if (stamina >= maxStamina)
            {
                stamina = maxStamina;
            }

            if (stamina <= 0)
            {
                stamina = 0;
                tired = true;
            }
            else
            {
                tired = false;
            }

            if (sprinting && normMagnitude > 0)
            {
                if (!tired)
                {
                    if (Time.time > nextDrain)
                    {
                        nextDrain = Time.time + drainCD;
                        stamina--;
                    }
                }
            }
            else
            {
                if (stamina < maxStamina)
                {
                    if (!chargeDone)
                    {
                        chargeDone = true;
                        StartCoroutine(Recharge());
                    }
                }
            }
        }
    }

    private IEnumerator Recharge()
    {
        chargeDone = true;
        yield return new WaitForSeconds(1f);
        stamina++;
        chargeDone = false;
        yield return null;
    }

    private void CameraFollow()
    {
        if (!gameLost)
        {
            Vector3 mousePosition2D = new Vector3(mousePosition.x, rb.position.y, mousePosition.z);
            Vector3 combinedPosition = (rb.position + ((rb.position + mousePosition2D) / 2f)) / 2f;
            Vector3 targetPosition = combinedPosition + new Vector3(0f, 148.99f, 0f);
            Vector3 smoothedPosition = Vector3.Lerp(cameraObject.transform.position, targetPosition, 9f * Time.fixedDeltaTime);
            cameraObject.transform.position = smoothedPosition;
        }
        else
        {
            cameraObject.transform.position = rb.position + new Vector3(0f, 148.99f, 0f);
        }
    }

    private IEnumerator HurtAnim()
    {
        sprite.material.shader = commonData.shaderGUItext;
        sprite.color = Color.white;
        yield return new WaitForSeconds(0.1f);
        sprite.material.shader = commonData.shaderSpritesDefault;
        sprite.color = Color.white;
        yield return null;
    }
}
