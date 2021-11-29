using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class UnitData : MonoBehaviour
{
    public AIPath path;
    public SpriteRenderer sprite;
    public Animator animator;
    public Sprite idleAnim;
    public Sprite runAnim;
    public Sprite workAnim;
    public Sprite idleCardAnim;
    public Sprite runCardAnim;

    public GameObject target;

    private void Awake()
    {

    }

    private void Update()
    {
        Animator();
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
            if (path.reachedDestination == true && target.GetComponent<TileData>().name == "Forest")
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
