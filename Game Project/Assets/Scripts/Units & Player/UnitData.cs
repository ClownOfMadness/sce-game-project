using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class UnitData : MonoBehaviour
{
    // Input unit name
    public string unitJob = "None";
    
    // Input unit design
    public List<RuntimeAnimatorController> design;
    
    // Input unit workable tiles
    public List<TileData> workableTiles;

    // Input unit sprite
    public SpriteRenderer sprite;
    public Animator animator;

    // Navigator settings
    private AIPath path;
    private AIDestinationSetter setter;

    // Work routine
    private bool busy = false;

    // Recieved from PlayerControl
    [HideInInspector] public GameObject target;
    private TileData tileData;

    private void Awake()
    {
        // Sets random design
        animator.runtimeAnimatorController = design[Random.Range(0, (design.Count))];

        // Sets AIPath
        path = GetComponent<AIPath>();
        setter = GetComponent<AIDestinationSetter>();
    }

    private void Update()
    {
        Animator();
        WorkRoutine();
    }

    private void Animator()
    {
        if (path.velocity.x > 0.01)
        {
            sprite.flipX = true;
        }
        else if (path.velocity.x < -0.01)
        {
            sprite.flipX = false;
        }
        
        if (path.velocity.magnitude > 0)
        {
            animator.SetBool("running", true);
        }
        else
        {
            animator.SetBool("running", false);
            if (target != null && tileData != null)
            {
                if (path.reachedDestination && CheckTile(tileData.name))
                {
                    animator.SetBool("working", true);
                    animator.SetBool("hasCard", true);
                }
                else
                {
                    animator.SetBool("working", false);
                }
            }
        }
    }

    public void UpdateTargetInfo(GameObject tile)
    {
        // Updated from PlayerControl
        target = tile;
        tileData = tile.GetComponent<TileData>();
        UpdateTargetLocation(tile);
    }

    public void UpdateTargetLocation(GameObject tile)
    {
        if (!busy)
        {
            busy = true;
            setter.target = tile.transform;
        }
    }

    private bool CheckTile(string name)
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

    private void WorkRoutine()
    {
        if (!busy)
        {
            // If the unit is free

        }
        else
        {
            // If the unit is busy
            if (path.reachedDestination)
            {
                busy = false;
            }
        }
    }
}
