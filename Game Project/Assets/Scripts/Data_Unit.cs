using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Data_Unit : MonoBehaviour
{
    // Input unit name
    public string unitJob = "None";

    // Unit Data
    [HideInInspector] public bool holdingCard = false;
    
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
    private bool working = false;
    private int workIndex = -1;
    private float workTime = 0f;
    private float workDone = 0f;
    private bool workBegun = false;
    private bool toTownHall = false;

    // Recieved from PlayerControl
    [HideInInspector] public GameObject target;
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
        if (!busy)
        {
            busy = true;
            path.speed = 7f;
            target = tile;
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
        path.speed = 3f;
        target = null;
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
                        animator.SetBool("working", true);
                        if (!workBegun)
                        {
                            workBegun = true;
                            workDone = Time.time + workTime;
                        }
                        if (Time.time > workDone)
                        {
                            Debug.Log("Finished work");
                            animator.SetBool("working", false);
                            animator.SetBool("hasCard", true);
                            workBegun = false;
                            holdingCard = true;
                            path.destination = townHall.transform.position;
                        }
                    }
                }
                else if (path.destination == townHall.transform.position)
                {
                    // if target is townhall
                    if (path.reachedDestination)
                    {
                        // [[Here add additional check if the hand is free or full and the unit will wait if needed]]
                        animator.SetBool("hasCard", false);
                        holdingCard = false;
                        path.destination = workPlace.transform.position;
                    }
                }
            }
        }
        else if (toTownHall)
        {
            if (path.reachedDestination)
            {
                if (holdingCard)
                {
                    // [[Add here function that gets the card to hand]]
                    animator.SetBool("hasCard", false);
                    holdingCard = false;
                    busy = false;
                    path.speed = 3f;
                    tileData.DetachWork();
                    target = null;
                    tileData = null;
                    path.destination = this.transform.position;
                }
                else
                {
                    // [[Add function that turns the peasant into a card]]
                    tileData.DetachWork();
                    Destroy(this.gameObject);
                }
            }
        }
    }
}
