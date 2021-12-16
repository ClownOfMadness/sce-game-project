using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Data_Unit : MonoBehaviour
{
    //--------------------------------------[To-Do List]-----------------------------------------------

    // to add:
    // - durabilty to jobs
    // - hurt system
    // - build buildings

    //-------------------------------------[Configuration]---------------------------------------------

    // Test
    public float distance = 0f;

    // [Input Data]
    public string unitJob = "None"; // Units job name
    public List<RuntimeAnimatorController> design; // Units sprite controller
    public SpriteRenderer sprite; // Units sprite renderer
    public Animator animator; // Units animator
    public Data_Card unitCard; // Its own card

    // [Build routine]
    public bool canBuild = false; // Can the unit build?

    //---------------------------------------[Automatic]-----------------------------------------------

    // [Navigator settings]
    private AIPath path; // Pathfinding component
    private float wanderMaxRadius = 10f; // Max radius the unit will wander
    private float wanderMinRadius = 2f; // Min radius the unit will wander
    private float maxWaitToWander = 15f; // Max delay between each wander
    private float minWaitToWander = 5f; // Min delay between each wander
    private float nextWander = 0f; // Trigger for next wander

    // [Unit Control]
    private GameObject townHall; // TownHall object
    [HideInInspector] public Data_Card card; // Shows the card that the unit carries
    [HideInInspector] public bool busy = false; // Toggles on if the unit is doing something
    private bool toTownHall = false; // Determines if the unit was send to townhall
    [HideInInspector] public bool cardToDeliver = false; // Toggles on when the unit is in waiting mode
    private bool nearTownHall = false;

    // [Unit Health]
    private bool hurt = false; // Becomes true if an enemy attacked the unit
    
    // [Work routine]
    private Data_Card workCard; // Work card that the unit will recieve after finishin job
    private GameObject workPlace; // WorkPlace object
    private bool working = false; // Shows if the unit is currently working
    private bool workBegun = false; // Shows if the work has begun
    private int workIndex = -1; // Work index to determine which work it does from the tile
    private float workTime = 0f; // Work time needed to get the card
    private float workDone = 0f; // Time until work is done

    // [Build routine]
    private GameObject buildPlace; // BuildPlace object
    private bool building = false; // Shows if the unit is currently building
    private bool buildBegun = false; // Shows if the build has begun
    
    // [Work in memory]
    private bool workInMemory = false; // Shows if the unit got a work remembered
    private Data_Tile rmbTileData; // Remembered TileData
    private Data_Card rmbWorkCard; // Remembered WorkCard
    private GameObject rmbWorkPlace; // Remembered WorkPlace
    private int rmbWorkIndex; // Remembered WorkIndex
    private float rmbWorkTime; // Remembered WorkTime
    
    // [Recieved from PlayerControl]
    private Data_Tile tileData; // Tile Data that is recieved from the player

    // [Find Once function]
    [HideInInspector] public Unit_List unitList; // Unit list script
    private System_DayNight time; // DayNight script
    private Screen_Cards screenCards; // ScreenCards script
    private int loadCount = 0; // Amount of time to try to find components until claiming a fail

    private void Awake()
    {
        // Checks if everything is in place
        if (!sprite)
        {
            Debug.LogError("SpriteRenderer Component is missing in the Data_Unit");
        }
        if (!animator)
        {
            Debug.LogError("Animator Component is missing in the Data_Unit");
        }
        if (!unitCard)
        {
            Debug.LogError("Unit's Data_Card is missing in the Data_Unit");
        }
        
        // Sets random design
        animator.runtimeAnimatorController = design[Random.Range(0, (design.Count))];

        // Sets AIPath
        if (!(path = GetComponent<AIPath>()))
        {
            Debug.LogError("AIPath Component is missing in a Data_Unit");
        }
    }

    private void Update()
    {
        FindOnce(); // Seeks needed scripts once
        Animator(); // Controls the animation
        WorkRoutine(); // Units work routine
        WanderAround(); // Wander around if not busy
        if (path.hasPath)
        {
            distance = path.remainingDistance;
        }
    }

    private void FindOnce() // Seeks needed scripts once
    {
        if (loadCount > 60)
        {
            Debug.LogError("Failed to find needed parameters in FindOnce() in the Data_Unit script");
        }
        else
        {
            if (!unitList)
            {
                if (!(unitList = GameObject.Find("Units").GetComponent<Unit_List>()))
                {
                    loadCount++;
                }
            }

            if (unitList && !townHall)
            {
                if (!(townHall = unitList.townhall))
                {
                    loadCount++;
                }
            }

            if (!time)
            {
                if(!(time = GameObject.Find("Day/Night Cycle").GetComponent<System_DayNight>()))
                {
                    loadCount++;
                }
            }

            if (!screenCards)
            {
                if(!(screenCards = GameObject.Find("CardsScreen").GetComponent<Screen_Cards>()))
                {
                    loadCount++;
                }
            }
        }
    }

    private void Animator() // Controls units animation
    {
        sprite.sortingOrder = -(int)Mathf.Abs(this.transform.position.z); // Changes its layer order depending on its z position
        
        if (path.velocity.x > 0.01) // Flips the sprite if unit moves the opposite side
        {
            sprite.flipX = true;
        }
        else if (path.velocity.x < -0.01)
        {
            sprite.flipX = false;
        }

        if (hurt) // Checks if the unit is hurt
        {
            animator.SetBool("hurt", true);
        }
        else
        {
            animator.SetBool("hurt", false);
        }

        if (card) // Checks if the card has card and shows it
        {
            animator.SetBool("hasCard", true);
        }
        else
        {
            animator.SetBool("hasCard", false);
        }

        if (!workBegun) // Checks if the unit is not working
        {
            animator.SetBool("working", false);
        }

        if (canBuild && !buildBegun) // Checks if the unit is not building
        {
            animator.SetBool("building", false);
        }

        if (path.velocity.magnitude > 0) // Checks if the unit is moving and if so play running animation
        {
            animator.SetBool("running", true);
        }
        else // All animations related unit being idle
        {
            animator.SetBool("running", false);
            if (workBegun) // Checks if the unit has started work
            {
                animator.SetBool("working", true);
            }

            if (canBuild && buildBegun) // Checks if the unit has started building
            {
                animator.SetBool("building", true);
            }
        }
    }

    public void UpdateTargetLocation(GameObject tile) // Method to set target location and info for the unit
    {
        // Unit Rule: the unit will never listen to player if it is busy or has a card (unless player order to move to the townhall)
        
        // Called from PlayerControl
        if (!busy && !card)
        {
            busy = true;
            path.speed = 7f;
            tileData = tile.GetComponent<Data_Tile>();
            tileData.AttachWork(this.gameObject);
            path.destination = tile.transform.position;
            if (tileData.hasTownHall) // If townhall
            {
                toTownHall = true;
            }
            else // Anything else
            {
                if ((workIndex = tileData.CanWork(this)) != -1)
                {
                    working = true;
                    workPlace = tile;
                    workTime = tileData.works[workIndex].workTime;
                    workCard = tileData.works[workIndex].card;
                }
            }
        }
    }

    public void RemoveTargetLocation() // Method to cancel units work
    {
        // Called from PlayerControl
        busy = false;
        working = false;
        workPlace = null;
        workIndex = -1;
        workTime = 0f;
        workDone = 0f;
        workBegun = false;
        workCard = null;
        path.speed = 3f;
        tileData = null;
        path.destination = this.transform.position;
    }

    private void WanderAround() // Makes the unit to wander around when jobless
    {
        // Unit Rule: the unit will always wonder if it is not busy
        
        if (!busy)
        {
            path.speed = 3f;
            if (!path.pathPending && (path.reachedEndOfPath || !path.hasPath))
            {
                if (Time.time > nextWander)
                {
                    path.destination = WanderAroundLocation();
                    nextWander = Time.time + Random.Range(minWaitToWander, maxWaitToWander);
                }
            }
        }
        else // if busy
        {
            if (path.reachedDestination && !working && !building)
            {
                busy = false;
            }
        }
    }

    private Vector3 WanderAroundLocation() // Gives wander location for the WanderAround function
    {
        Vector3 point = Random.insideUnitSphere * Random.Range(wanderMinRadius, wanderMaxRadius);
        point.y = 1f;
        point += this.transform.position;
        return point;
    }

    private void WorkRoutine()
    {
        if (working)
        {
            workInMemory = false;
            if (!workPlace || !townHall)
            {
                Debug.LogError("Could not find work Tile or TownHall in Data_Unit");
                working = false;
            }
            else
            {
                if (path.destination == workPlace.transform.position)
                {
                    // if target is workplace
                    if (path.reachedDestination)
                    {
                        if (tileData.hasResources)
                        {
                            if (!workBegun)
                            {
                                workBegun = true;
                                workDone = Time.time + workTime;
                            }
                            if (Time.time > workDone)
                            {
                                card = workCard;
                                workBegun = false;
                                tileData.durability--;
                                path.destination = townHall.transform.position;
                            }
                        }
                        else
                        {
                            busy = false;
                            working = false;
                            workPlace = null;
                            workIndex = -1;
                            workTime = 0f;
                            workDone = 0f;
                            workBegun = false;
                            workCard = null;
                            path.speed = 3f;
                            tileData.DetachWork();
                            tileData = null;
                            path.destination = this.transform.position;
                        }
                    }
                }
                else if (path.destination == townHall.transform.position)
                {
                    // if target is townhall
                    if (nearTownHall)
                    {
                        // [[Here add additional check if the hand is free or full and the unit will wait if needed]]
                        if (screenCards.AddGathered(this, true))
                        {
                            card = null;
                            cardToDeliver = false;
                            if (!time.isDay)
                            {
                                // It is nightTime
                                RememberWork();
                            }
                            else
                            {
                                // If it is daytime
                                path.destination = workPlace.transform.position;
                            }
                        }
                        else
                        {
                            WaitingWithCard();
                        }
                    }
                }
            }
        }
        else if (card && !cardToDeliver)
        {
            busy = true;
            path.speed = 7f;
            if (path.destination != townHall.transform.position)
                path.destination = townHall.transform.position;
            else
            {
                if (nearTownHall)
                {
                    if (screenCards.AddGathered(this, true))
                    {
                        card = null;
                        busy = false;
                        cardToDeliver = false;
                        path.speed = 3f;
                        tileData = null;
                        path.destination = this.transform.position;
                    }
                    else
                    {
                        WaitingWithCard();
                    }
                }
            }
        }
        else if (cardToDeliver && !card)
        {
            cardToDeliver = false;
        }
        else if (toTownHall)
        {
            // If the units target location is townhall
            if (nearTownHall && path.destination == townHall.transform.position)
            {
                // If the unit has reached the townhall
                screenCards.AddGathered(this, false);

                tileData.DetachWork();
                Destroy(this.gameObject);

                //else
                //{
                //    toTownHall = false;
                //    busy = false;
                //    path.speed = 3f;
                //    tileData.DetachWork();
                //    tileData = null;
                //    path.destination = this.transform.position;
                //}
            }
        }
        else if (time.isDay && workInMemory)
        {
            // If it is now daytime and unit remembers work
            if (!busy && !card)
            {
                BackToWork();
            }
        }
        else if (!time.isDay)
        {
            // Send unit back to townhall
        }
    }

    private void RememberWork()
    {
        workInMemory = true;

        tileData.DetachWork();
        busy = false;
        working = false;
        path.speed = 3f;
        rmbTileData = tileData;
        tileData = null;
        rmbWorkPlace = workPlace;
        workPlace = null;
        rmbWorkIndex = workIndex;
        workIndex = -1;
        rmbWorkTime = workTime;
        workTime = 0f;
        workDone = 0f;
        rmbWorkCard = workCard;
        workCard = null;
        workBegun = false;
        path.destination = this.transform.position;
    }

    private void BackToWork()
    {
        workInMemory = false;

        if (!rmbTileData.GetData())
        {
            rmbTileData.AttachWork(this.gameObject);
            busy = true;
            working = true;
            path.speed = 7f;
            tileData = rmbTileData;
            workPlace = rmbWorkPlace;
            workIndex = rmbWorkIndex;
            workTime = rmbWorkTime;
            workCard = rmbWorkCard;
            workDone = 0f;
            workBegun = false;
            path.destination = workPlace.transform.position;
        }
        else
        {
            busy = false;
            working = false;
            path.speed = 3f;
            tileData = null;
            workPlace = null;
            workIndex = -1;
            workTime = 0f;
            workDone = 0f;
            workCard = null;
            workBegun = false;
            path.destination = this.transform.position;
        }
    }

    private void WaitingWithCard()
    {
        cardToDeliver = true;
        if (working)
        {
            workInMemory = true;

            tileData.DetachWork();
            busy = false;
            working = false;
            path.speed = 3f;
            rmbTileData = tileData;
            tileData = null;
            rmbWorkPlace = workPlace;
            workPlace = null;
            rmbWorkIndex = workIndex;
            workIndex = -1;
            rmbWorkTime = workTime;
            workTime = 0f;
            workDone = 0f;
            rmbWorkCard = workCard;
            workCard = null;
            workBegun = false;
            path.destination = this.transform.position;
        }
        else
        {
            busy = false;
            path.speed = 3f;
            tileData.DetachWork();
            tileData = null;
            path.destination = this.transform.position;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 14)
        {
            nearTownHall = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 14)
        {
            nearTownHall = false;
        }
    }
}
