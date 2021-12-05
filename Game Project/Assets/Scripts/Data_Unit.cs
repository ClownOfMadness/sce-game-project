using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Data_Unit : MonoBehaviour
{
    // Input unit name
    public string unitJob = "None";
    
    // Input unit design
    public List<RuntimeAnimatorController> design;
    
    // Input unit workable tiles
    public List<Data_Tile> workableTiles;

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
    private Transform townHall;
    private Transform workPlace;

    // Recieved from PlayerControl
    [HideInInspector] public GameObject target;
    private Data_Tile Data_Tile;

    private void Awake()
    {
        //if (!unitList)
        //{
        //    Debug.LogError("Unit_List is missing from the Data_Unit, Check in Unit_List if the Unit_List script is transferred correctly");
        //}
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
        Animator();
        WorkRoutine();
        WanderAround();
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
            //if (target != null && Data_Tile != null)
            //{
            //    if (path.reachedDestination && CheckTile(Data_Tile.name))
            //    {
            //        animator.SetBool("working", true);
            //        animator.SetBool("hasCard", true);
            //    }
            //    else
            //    {
            //        animator.SetBool("working", false);
            //    }
            //}
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
            Data_Tile = tile.GetComponent<Data_Tile>();
            path.destination = tile.transform.position;
        }
    }

    public void RemoveTargetLocation() // Method to cancel units work
    {
        // Called from PlayerControl
        busy = false;
        path.speed = 3f;
        path.destination = this.transform.position;
    }

    private void WanderAround() // Makes the unit to wander around when jobless
    {
        if (!busy)
        {
            if (!path.pathPending && (path.reachedEndOfPath || !path.hasPath))
            {
                if (Time.time > nextWander)
                {
                    path.destination = WanderAroundLocation();
                    nextWander = Time.time + waitToWander;
                }
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

    private bool CheckTile(string name) // Checks if the tile is workable for the unit
    {
        for (int i = 0; i < workableTiles.Count; i++)
        {
            if (name == workableTiles[i].name)
            {
                return true;
            }
        }
        return false;
    }

    private void WorkRoutine() // [[WIP]]
    {
        if (busy)
        {
            // If the unit is free

        }
    }
}
