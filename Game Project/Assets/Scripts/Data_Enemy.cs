using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Data_Enemy : MonoBehaviour
{
    //--------------------------------------[To-Do List]-----------------------------------------------

    // to add:
    // - hurt system
    // - Add tile walking priority
    // - Add die animation

    //-------------------------------------[Configuration]---------------------------------------------

    // [Input Data]
    public string enemyType = "None"; // Units job name
    public int enemyHealth = 10;
    public int damage = 5;
    public float attackCD = 1f;
    public List<RuntimeAnimatorController> design; // Units sprite controller
    public SpriteRenderer sprite; // Units sprite renderer
    public Animator animator; // Units animator
    public GameObject detector;

    //---------------------------------------[Automatic]-----------------------------------------------

    // [Navigator settings]
    private AIPath path; // Pathfinding component
    private float wanderMaxRadius = 10f; // Max radius the unit will wander
    private float wanderMinRadius = 2f; // Min radius the unit will wander
    private float maxWaitToWander = 15f; // Max delay between each wander
    private float minWaitToWander = 5f; // Min delay between each wander
    private float nextWander = 0f; // Trigger for next wander
    private bool impassable = false;
    private GameObject currentTileOn = null;
    private GameObject previousTile = null;
    private Data_Tile dataTile = null;

    // [Enemy Chase]
    private bool reachedTown = false;
    private bool reachedAbyss = false;
    [HideInInspector] public GameObject target = null;
    private GameObject spottedUnit = null;
    private GameObject spottedBuilding = null;
    private GameObject spottedTownHall = null;
    private GameObject spottedPlayer = null;
    private float nextAttack = 0f;

    // [Enemy Control]
    [HideInInspector] public GameObject abyss; // TownHall object
    [HideInInspector] public Data_Tile abyssData;
    public Data_Card card; // Shows the card that the unit carries
    [HideInInspector] public bool busy = false; // Toggles on if the unit is doing something

    // [Enemy Health]
    private Rigidbody enemyRigidbody;
    private int health = 0;

    // [Find Once function]
    [HideInInspector] public Enemy_List enemyList; // Unit list script
    private Unit_List unitList;
    private System_DayNight time; // DayNight script
    private Screen_Cards screenCards; // ScreenCards script
    private Player_Control player;
    private Data_CommonDataHolder commonData;
    private int loadCount = 0; // Amount of time to try to find components until claiming a fail

    private void Awake()
    {
        if (!sprite)
        {
            Debug.LogError("SpriteRenderer Component is missing in the " + enemyType + " Data_Enemy");
        }
        if (!animator)
        {
            Debug.LogError("Animator Component is missing in the " + enemyType + " Data_Enemy");
        }
        if (!detector)
        {
            Debug.LogError("Detector gameobject is missing in the " + enemyType + " Data_Enemy");
        }
        if (design.Count <= 0)
        {
            Debug.LogError("Design list is empty in the " + enemyType + " Data_Enemy");
        }

        animator.runtimeAnimatorController = design[Random.Range(0, (design.Count))];

        if (!(path = GetComponent<AIPath>()))
        {
            Debug.LogError("AIPath Component is missing in the " + enemyType + " Data_Enemy");
        }

        health = enemyHealth;

        if (!(enemyRigidbody = GetComponent<Rigidbody>()))
        {
            Debug.LogError("Rigidbody Component is missing in the " + enemyType + " Data_Enemy");
        }
    }

    private void Update()
    {
        FindOnce(); // Seeks needed scripts once
        Animator(); // Controls the animation
        WanderAround(); // Wander around if not busy
        GroundCheck();
        Hunt();
        Priority();
        Health();
    }

    private void FindOnce() // Seeks needed scripts once
    {
        if (loadCount > 60)
        {
            Debug.LogError("Failed to find needed parameters in FindOnce() in the Data_Unit script");
        }
        else
        {
            if (!enemyList)
            {
                if (!(enemyList = GameObject.Find("Enemies").GetComponent<Enemy_List>()))
                {
                    loadCount++;
                }
            }

            if (!time)
            {
                if (!(time = GameObject.Find("Day/Night Cycle").GetComponent<System_DayNight>()))
                {
                    loadCount++;
                }
            }

            if (!unitList)
            {
                if (!(unitList = GameObject.Find("Units").GetComponent<Unit_List>()))
                {
                    loadCount++;
                }
            }

            if (!player)
            {
                if (!(player = GameObject.Find("PlayerControl").GetComponent<Player_Control>()))
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
    private void Animator() // Controls units animation
    {
        sprite.sortingOrder = -(int)Mathf.Abs(this.transform.position.z); // Changes its layer order depending on its z position

        if (currentTileOn)
        {
            if (previousTile != currentTileOn)
            {
                previousTile = currentTileOn;
                dataTile = currentTileOn.GetComponent<Data_Tile>();
            }
            else
            {
                if (dataTile.revealed)
                {
                    sprite.enabled = true;
                }
                else
                {
                    sprite.enabled = false;
                }
            }
        }

        if (path.velocity.x > 0.01) // Flips the sprite if unit moves the opposite side
        {
            sprite.flipX = true;
        }
        else if (path.velocity.x < -0.01)
        {
            sprite.flipX = false;
        }

        if (!target)
        {
            animator.SetBool("spotted", false);
        }

        if (card) // Checks if the card has card and shows it
        {
            animator.SetBool("hasCard", true);
        }
        else
        {
            animator.SetBool("hasCard", false);
            if (target)
            {
                animator.SetBool("spotted", true);
            }
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
            if ((path.reachedDestination || impassable) && !target)
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

    private void Health()
    {
        if (health >= enemyHealth)
        {
            health = enemyHealth;
        }
        else if (health <= 0)
        {
            // Drop card - maybe in future
            DestroyEnemy();
        }
    }

    private void GroundCheck()
    {
        RaycastHit hit;
        Debug.DrawRay(detector.transform.position, Vector3.down, Color.yellow);
        if (Physics.Raycast(detector.transform.position, Vector3.down, out hit, 0.1f))
        {
            currentTileOn = hit.transform.gameObject;
        }
    }

    private void Priority()
    {
        if (spottedUnit)
        {
            target = spottedUnit;
        }
        else
        {
            if (spottedBuilding)
            {
                target = spottedBuilding;
            }
            else
            {
                if (spottedPlayer)
                {
                    target = spottedPlayer;
                }
                else
                {
                    if (spottedTownHall)
                    {
                        target = spottedTownHall;
                    }
                    else
                    {
                        target = null;
                    }
                }
            }
        }
    }

    private void Hunt()
    {
        if (!card)
        {
            if ((target && !time.isDay) || (target && reachedAbyss && time.isDay))
            {
                busy = true;
                path.speed = 10f;
                path.destination = target.transform.position;
            }
            else
            {
                if (!time.isDay)
                {
                    reachedAbyss = false;
                    if (!reachedTown)
                    {
                        busy = true;
                        path.speed = 20f;
                        path.destination = unitList.townhall.transform.position;
                        if (path.reachedDestination)
                        {
                            busy = false;
                            reachedTown = true;
                        }
                    }
                }
                else if (time.isDay)
                {
                    reachedTown = false;
                    if (!reachedAbyss)
                    {
                        busy = true;
                        path.speed = 20f;
                        path.destination = abyss.transform.position;
                        if (path.reachedDestination)
                        {
                            busy = false;
                            reachedAbyss = true;
                            health = enemyHealth;
                        }
                    }
                }
                else
                {
                    busy = false;
                }
            }
        }
        else
        {
            busy = true;
            path.speed = 7f;
            path.destination = abyss.transform.position;
            if (path.reachedDestination)
            {
                busy = false;
                reachedAbyss = true;
                health = enemyHealth;
                if (card)
                    abyssData.TransferCard(card);
                card = null;
            }
        }
    }

    public void Hurt(int damage, Data_Unit _unit)
    {
        StartCoroutine(HurtAnim());
        Vector3 moveDirection = this.transform.position - _unit.gameObject.transform.position;
        enemyRigidbody.AddForce(moveDirection.normalized * 4000f);
        if (health - damage <= 0 && card)
        {
            _unit.card = card;
            card = null;
            DestroyEnemy();
        }
        else
        {
            health -= damage;
        }
    }

    private void Attack()
    {
        if (target)
        {
            if (Time.time > nextAttack)
            {
                if (target == spottedUnit)
                {
                    nextAttack = Time.time + attackCD;
                    StartCoroutine(AttackUnit(target));
                }
                else if (target == spottedBuilding)
                {
                    nextAttack = Time.time + attackCD;
                    StartCoroutine(AttackBuilding(target));
                }
                else if (target == spottedTownHall)
                {
                    nextAttack = Time.time + attackCD;
                    StartCoroutine(AttackBuilding(target));
                }
                else if (target == spottedPlayer)
                {
                    nextAttack = Time.time + attackCD;
                    StartCoroutine(AttackPlayer());
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!spottedBuilding && other.gameObject.layer == 9)
        {
            spottedBuilding = other.gameObject;
        }
        if (!spottedTownHall && other.gameObject.layer == 11)
        {
            spottedTownHall = other.gameObject;
        }
        if (!spottedUnit && other.gameObject.layer == 8)
        {
            spottedUnit = other.gameObject;
        }
        if (!spottedPlayer && other.gameObject.layer == 3)
        {
            spottedPlayer = other.gameObject;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (spottedBuilding == other.gameObject)
        {
            spottedBuilding = null;
        }
        if (spottedTownHall == other.gameObject)
        {
            spottedTownHall = null;
        }
        if (spottedUnit == other.gameObject)
        {
            spottedUnit = null;
        }
        if (spottedPlayer == other.gameObject)
        {
            spottedPlayer = null;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (target != null && path.canMove == true)
        {
            if (target == collision.gameObject)
            {
                Attack();
            }
        }
    }

    private void DestroyEnemy()
    {
        StartCoroutine(RemoveEnemy());
    }

    public IEnumerator RemoveEnemy()
    {
        animator.SetBool("die", true);
        path.canMove = false;
        yield return new WaitForSeconds(0.35f);
        this.transform.position = new Vector3(-500, -500, -500);
        yield return new WaitForSeconds(1);
        Destroy(this.gameObject);
        yield return null;
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

    private IEnumerator AttackUnit(GameObject _target)
    {
        animator.SetTrigger("attack");
        yield return new WaitForSeconds(0.1f);
        _target.GetComponent<Data_Unit>().Hurt(damage, this);
        yield return null;
    }

    private IEnumerator AttackBuilding(GameObject _target)
    {
        animator.SetTrigger("attack");
        yield return new WaitForSeconds(0.1f);
        _target.GetComponent<Data_Building>().Hurt(damage, this);
        yield return null;
    }

    private IEnumerator AttackPlayer()
    {
        animator.SetTrigger("attack");
        yield return new WaitForSeconds(0.1f);
        player.Hurt(this);
        yield return null;
    }
}
