using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Data_Unit : MonoBehaviour
{
    // Input unit name
    public string unitJob = "None";

    // Unit Data
    //[HideInInspector] public bool holdingCard = false;
    
    // Input unit design
    public List<RuntimeAnimatorController> design;

    // Input unit sprite
    public SpriteRenderer sprite;
    public Animator animator;

    // Navigator settings
    private AIPath path;
    private float wanderRadius = 10f;
    private float waitToWander = 10f;
    private float nextWander = 0f;

    // Work routine
    public Unit_List unitList;
    [HideInInspector] public bool busy = false;
    private GameObject townHall;
    private GameObject workPlace;
    private System_DayNight time;
    private bool working = false;
    private int workIndex = -1;
    private float workTime = 0f;
    private float workDone = 0f;
    private bool workBegun = false;
    private bool toTownHall = false;
    private bool workInMemory = false;
    private Data_Card workCard;
    public Data_Card card;
    private Screen_Cards screenCards;
    public bool cardToDeliver = false;
    public Data_Card unitCard;

    // Work in memory
    private Data_Tile rmbTileData;
    private GameObject rmbWorkPlace;
    private int rmbWorkIndex;
    private float rmbWorkTime;
    private Data_Card rmbWorkCard;

    // Recieved from PlayerControl
    private Data_Tile tileData;

    // Find Once function
    private int loadCount = 0;

    private void Awake()
    {
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

        // Gets town hall
    }

    private void Update()
    {
        FindOnce();
        Animator();
        WorkRoutine();
        WanderAround();
    }

    private void FindOnce()
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

        if (path.velocity.magnitude > 0) // Checks if the unit is moving and if so play running animation
        {
            animator.SetBool("running", true);
        }
        else // All animations related unit being idle
        {
            animator.SetBool("running", false);
        }
    }

    public void UpdateTargetLocation(GameObject tile) // Method to set target location and info for the unit
    {
        // Called from PlayerControl
        if (!busy && !card)
        {
            busy = true;
            path.speed = 7f;
            tileData = tile.GetComponent<Data_Tile>();
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
        animator.SetBool("working", false);
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
        if (!busy)
        {
            path.speed = 3f;
            if (!path.pathPending && (path.reachedEndOfPath || !path.hasPath))
            {
                if (Time.time > nextWander)
                {
                    path.destination = WanderAroundLocation();
                    nextWander = Time.time + waitToWander;
                }
            }
        }
        else // if busy
        {
            if (path.reachedDestination && !working)
            {
                busy = false;
            }
        }
    }

    private Vector3 WanderAroundLocation() // Gives wander location for the WanderAround function
    {
        Vector3 point = Random.insideUnitSphere * wanderRadius;
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
                            animator.SetBool("working", true);
                            if (!workBegun)
                            {
                                workBegun = true;
                                workDone = Time.time + workTime;
                            }
                            if (Time.time > workDone)
                            {
                                animator.SetBool("working", false);
                                animator.SetBool("hasCard", true);
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
                    if (path.reachedDestination)
                    {
                        // [[Here add additional check if the hand is free or full and the unit will wait if needed]]
                        if (screenCards.AddGathered(card))
                        {
                            animator.SetBool("hasCard", false);
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
                if (path.reachedDestination)
                {
                    if (screenCards.AddGathered(card))
                    {
                        animator.SetBool("hasCard", false);
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
            animator.SetBool("hasCard", false);
            cardToDeliver = false;
        }
        else if (toTownHall)
        {
            // If the units target location is townhall
            if (path.reachedDestination)
            {
                // If the unit has reached the townhall
                if (screenCards.AddGathered(unitCard))
                {
                    tileData.DetachWork();
                    Destroy(this.gameObject);
                }
                else
                {
                    toTownHall = false;
                    busy = false;
                    path.speed = 3f;
                    tileData.DetachWork();
                    tileData = null;
                    path.destination = this.transform.position;
                }
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
}
