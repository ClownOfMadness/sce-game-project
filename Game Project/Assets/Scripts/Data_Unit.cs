using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Data_Unit : MonoBehaviour
{
    //-------------------------------------[Configuration]---------------------------------------------

    // [Input Data]
    public string unitJob = "None"; // Units job name
    public int unitHealth = 10;
    public int regen = 5;
    public int jobDurability = 0; // Units job durability
    public List<RuntimeAnimatorController> design; // Units sprite controller
    public SpriteRenderer sprite; // Units sprite renderer
    public Animator animator; // Units animator
    public Data_Card unitCard; // Its own card
    public GameObject detector;

    // [Build routine]
    public bool canBuild = false; // Can the unit build?
    public bool canFight = false;
    public int damage = 0;
    public float attackCD = 1f;

    //---------------------------------------[Automatic]-----------------------------------------------

    // [Navigator settings]
    public AIPath path; // Pathfinding component
    private float wanderMaxRadius = 10f; // Max radius the unit will wander
    private float wanderMinRadius = 2f; // Min radius the unit will wander
    private float maxWaitToWander = 15f; // Max delay between each wander
    private float minWaitToWander = 5f; // Min delay between each wander
    private float nextWander = 0f; // Trigger for next wander
    private bool impassable = false;
    public GameObject currentTileOn = null;
    private bool reachedTownHall = false;
    private GameObject detectedImpassable = null;
    public Vector3 destinationTile;

    // [Unit Control]
    private GameObject townHall; // TownHall object
    public Data_Card card; // Shows the card that the unit carries
    [HideInInspector] public bool busy = false; // Toggles on if the unit is doing something
    private bool toTownHall = false; // Determines if the unit was send to townhall
    [HideInInspector] public bool cardToDeliver = false; // Toggles on when the unit is in waiting mode
    private string buildingInteraction = null;

    // [Unit Health]
    private bool hurt = false; // Becomes true if an enemy attacked the unit
    private Rigidbody unitRigidbody;
    public int durability = 0;
    public int health = 0;
    private float regenCD = 5f;
    private float nextRegen = 0f;
    
    // [Work Routine]
    private Data_Card workCard; // Work card that the unit will recieve after finishin job
    private GameObject workPlace; // WorkPlace object
    public bool working = false; // Shows if the unit is currently working
    private bool workBegun = false; // Shows if the work has begun
    private int workIndex = -1; // Work index to determine which work it does from the tile
    private float workTime = 0f; // Work time needed to get the card
    private float workDone = 0f; // Time until work is done
    private bool workExtra = false;

    // [Build Routine]
    private GameObject buildPlace; // BuildPlace object
    private bool building = false; // Shows if the unit is currently building
    private bool buildBegun = false; // Shows if the build has begun

    // [Patrol Routine]
    private bool patroling = false;
    private GameObject patrolPlace;
    private GameObject target = null;
    //private GameObject spottedAbyss = null;
    private GameObject spottedEnemy = null;
    private float nextAttack = 0f;

    // [Work in memory]
    private bool workInMemory = false; // Shows if the unit got a work remembered
    private Data_Tile rmbTileData; // Remembered TileData
    private Data_Card rmbWorkCard; // Remembered WorkCard
    private GameObject rmbWorkPlace; // Remembered WorkPlace
    private int rmbWorkIndex; // Remembered WorkIndex
    private float rmbWorkTime; // Remembered WorkTime
    private bool rmbWorkExtra = false;
    
    // [Recieved from PlayerControl]
    private Data_Tile tileData; // Tile Data that is recieved from the player

    // [Find Once function]
    [HideInInspector] public Unit_List unitList; // Unit list script
    private System_DayNight time; // DayNight script
    private Screen_Cards screenCards; // ScreenCards script
    private Data_CommonDataHolder commonData;
    private int loadCount = 0; // Amount of time to try to find components until claiming a fail

    private void Awake()
    {
        // Checks if everything is in place
        if (!sprite)
        {
            Debug.LogError("SpriteRenderer Component is missing in the " + unitJob + " Data_Unit");
        }
        if (!animator)
        {
            Debug.LogError("Animator Component is missing in the " + unitJob + " Data_Unit");
        }
        if (!unitCard)
        {
            Debug.LogError("Unit's Data_Card is missing in the " + unitJob + " Data_Unit");
        }
        if (!detector)
        {
            Debug.LogError("Detector gameobject is missing in the " + unitJob + " Data_Unit");
        }
        if (design.Count <= 0)
        {
            Debug.LogError("Design list is empty in the " + unitJob + " Data_Unit");
        }
        
        // Sets random design
        animator.runtimeAnimatorController = design[Random.Range(0, (design.Count))];

        // Sets AIPath
        if (!(path = GetComponent<AIPath>()))
        {
            Debug.LogError("AIPath Component is missing in the " + unitJob + " Data_Unit");
        }

        durability = jobDurability;
        health = unitHealth;

        if (!(unitRigidbody = GetComponent<Rigidbody>()))
        {
            Debug.LogError("Rigidbody Component is missing in the " + unitJob + " Data_Unit");
        }
    }

    private void Update()
    {
        FindOnce(); // Seeks needed scripts once
        Animator(); // Controls the animation
        WorkRoutine(); // Units work routine
        WanderAround(); // Wander around if not busy
        Health();
        JobCheck();
        GroundCheck();
        if (canFight)
        {
            Priority();
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

            if (!commonData)
            {
                if(!(commonData = GameObject.Find("Map Generator").GetComponent<Data_CommonDataHolder>()))
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

        if (!target && canFight)
        {
            animator.SetBool("spotted", false);
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
            if (canFight && target)
            {
                animator.SetBool("spotted", true);
            }
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

    private void Health()
    {
        if (health >= unitHealth)
        {
            health = unitHealth;
        }
        else if (health <= 0)
        {
            // Drop card - maybe in future
            DestroyUnit();
        }

        if (time.isDay)
        {
            if (health < unitHealth && Time.time > nextRegen)
            {
                nextRegen = Time.time + regenCD;
                health += regen;
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
            impassable = false;
            detectedImpassable = null;
            path.speed = 7f;
            tileData = tile.GetComponent<Data_Tile>();
            //tileData.AttachWork(this.gameObject);
            path.destination = tile.transform.position;
            destinationTile = path.destination;
            if (tileData.hasTownHall) // If townhall
            {
                toTownHall = true;
            }
            else // Anything else
            {
                if ((workIndex = tileData.CanWork(this)) != -1)
                {
                    tileData.AttachWork(this.gameObject);
                    working = true;
                    workPlace = tile;
                    workTime = tileData.works[workIndex].workTime;
                    workCard = tileData.works[workIndex].card;
                    workExtra = tileData.works[workIndex].extra;
                }
                else if (tileData.CanBuild(this))
                {
                    building = true;
                    buildPlace = tile;
                }
                else if (canFight)
                {
                    tileData.AttachPatrol(this.gameObject);
                    patroling = true;
                    patrolPlace = tile;
                }
            }
        }
    }

    public void RemoveTargetLocation() // Method to cancel units work
    {
        // Called from PlayerControl
        busy = false;
        working = false;
        building = false;
        patroling = false;
        patrolPlace = null;
        buildBegun = false;
        buildPlace = null;
        workPlace = null;
        workIndex = -1;
        workTime = 0f;
        workDone = 0f;
        workBegun = false;
        workCard = null;
        workExtra = false;
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
            if ((path.reachedDestination || (impassable)) && !working && !building && !patroling)
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

    private void Priority()
    {
        if (spottedEnemy)
        {
            target = spottedEnemy;
        }
        else
        {
            target = null;
        }
    }

    private void Attack()
    {
        if (target)
        {
            if (Time.time > nextAttack)
            {
                if (target == spottedEnemy)
                {
                    nextAttack = Time.time + attackCD;
                    StartCoroutine(AttackAnim(target));
                    if (jobDurability > 0)
                        durability--;
                }
            }
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

    private void JobCheck()
    {
        if (jobDurability > 0)
        {
            if (durability <= 0)
            {
                unitList.SummonUnit(0, currentTileOn, card, false);
                durability = 1;
                DestroyUnit();
            }
        }
    }

    private void WorkRoutine()
    {
        if (time.isDay)
        {
            reachedTownHall = false;
        }
        
        if (hurt && !canFight)
        {
            busy = true;
            path.speed = 7f;
            if (path.destination != townHall.transform.position)
                path.destination = townHall.transform.position;
            else
            {
                if (buildingInteraction == "TownHall")
                {
                    hurt = false;
                    busy = false;
                    path.speed = 3f;
                    path.destination = this.transform.position;
                }
            }
        }
        else if (working)
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
                        if (tileData.hasResources && workExtra == tileData.GetExtra())
                        {
                            if (!workBegun)
                            {
                                workBegun = true;
                                workDone = Time.time + workTime;
                            }
                            if (Time.time > workDone)
                            {
                                if (jobDurability > 0 && durability > 0)
                                    durability--;
                                card = workCard;
                                workBegun = false;
                                tileData.durability--;
                                path.destination = townHall.transform.position;
                                RememberWork();
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
                //else if (path.destination == townHall.transform.position)
                //{
                //    // if target is townhall
                //    if (buildingInteraction == "TownHall")
                //    {
                //        // [[Here add additional check if the hand is free or full and the unit will wait if needed]]
                //        if (screenCards.AddGathered(this, true))
                //        {
                //            card = null;
                //            cardToDeliver = false;
                //            if (!time.isDay)
                //            {
                //                // It is nightTime
                //                RememberWork();
                //            }
                //            else
                //            {
                //                // If it is daytime
                //                path.destination = workPlace.transform.position;
                //            }
                //        }
                //        else
                //        {
                //            WaitingWithCard();
                //        }
                //    }
                //}
            }
        }
        else if (building)
        {
            if (!buildPlace || !tileData)
            {
                building = false;
            }
            else if (buildingInteraction == tileData.building.name && !tileData.buildingComplete)
            {
                if (!buildBegun)
                {
                    buildBegun = true;
                    tileData.AttachBuild(this.gameObject);
                }
            }
            else if (tileData.buildingComplete)
            {
                building = false;
                buildBegun = false;
                buildPlace = null;
                busy = false;
                path.speed = 3f;
                tileData.DetachBuild(this.gameObject);
                tileData = null;
                path.destination = this.transform.position;
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
                if (buildingInteraction == "TownHall")
                {
                    hurt = false;
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
        else if (canFight)
        {
            if (!card)
            {
                if (target)
                {
                    busy = true;
                    path.speed = 10f;
                    path.destination = target.transform.position;
                }
                else if (patroling)
                {
                    busy = true;
                    path.speed = 7f;
                    path.destination = patrolPlace.transform.position;
                    if (detectedImpassable)
                    {
                        if (detectedImpassable == patrolPlace)
                        {
                            busy = false;
                            path.speed = 3f;
                            path.destination = this.transform.position;
                            tileData.DetachWork();
                            detectedImpassable = null;
                            patrolPlace = null;
                            patroling = false;
                        }
                    }
                }
                else
                {
                    busy = false;
                }
            }
        }
        else if (toTownHall)
        {
            // If the units target location is townhall
            if (buildingInteraction == "TownHall")
            {
                // If the unit has reached the townhall
                hurt = false;
                unitCard = commonData.peasantCard;
                screenCards.AddGathered(this, false);
                
                tileData.DetachWork();
                DestroyUnit();
                toTownHall = false;
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
        else if (!time.isDay && !busy && !working && !building && !canFight)
        {
            // Send unit back to townhall
            if (!reachedTownHall)
            {
                path.speed = 7f;
                path.destination = townHall.transform.position;
                if (buildingInteraction == "TownHall")
                {
                    hurt = false;
                    reachedTownHall = true;
                    path.speed = 3f;
                    path.destination = this.transform.position;
                }
            }
        }
    }

    public void Hurt(int damage, Data_Enemy enemy)
    {
        StartCoroutine(HurtAnim());
        Vector3 moveDirection = this.transform.position - enemy.gameObject.transform.position;
        unitRigidbody.AddForce(moveDirection.normalized * 4000f);
        if (card)
        {
            enemy.card = card;
            card = null;
        }
        else
        {
            if (health - damage <= 0)
            {
                enemy.card = unitCard;
            }
            health -= damage;
        }
        hurt = true;
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
        rmbWorkExtra = workExtra;
        workExtra = false;
        workBegun = false;
        path.destination = this.transform.position;
    }

    private void BackToWork()
    {
        workInMemory = false;

        if (rmbTileData)
        {
            if (!rmbTileData.GetData() && rmbTileData.gameObject.activeSelf == true && rmbWorkExtra == rmbTileData.GetExtra())
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
                workExtra = rmbWorkExtra;
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
                workExtra = false;
                workBegun = false;
                path.destination = this.transform.position;
            }
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
            workExtra = false;
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
            rmbWorkExtra = workExtra;
            workExtra = false;
            workBegun = false;
            path.destination = this.transform.position;
        }
        else
        {
            busy = false;
            path.speed = 3f;
            if (tileData)
            {
                tileData.DetachWork();
                tileData = null;
            }
            path.destination = this.transform.position;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 14)
        {
            buildingInteraction = other.gameObject.name;
        }
        else if (other.gameObject.layer == 7 && tileData)
        {
            if (other.gameObject == tileData.gameObject)
            {
                impassable = true;
                detectedImpassable = other.gameObject;
            }
        }
        if (!spottedEnemy && other.gameObject.layer == 15 && canFight)
        {
            spottedEnemy = other.gameObject;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 14)
        {
            buildingInteraction = null;
        }
        else if (other.gameObject.layer == 7)
        {
            impassable = false;
            detectedImpassable = null;
        }
        if (spottedEnemy == other.gameObject && canFight)
        {
            spottedEnemy = null;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (target != null && canFight)
        {
            if (target == collision.gameObject)
            {
                Attack();
            }
        }
    }

    private void DestroyUnit()
    {
        StartCoroutine(RemoveUnit());
    }

    public IEnumerator RemoveUnit()
    {
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

    private IEnumerator AttackAnim(GameObject _target)
    {
        animator.SetTrigger("attack");
        yield return new WaitForSeconds(0.1f);
        _target.GetComponent<Data_Enemy>().Hurt(damage, this);
        yield return null;
    }

    public void LoadData(Unit_Info data)
    {
        nextWander = data.nextWander;
        impassable = data.impassable;
        currentTileOn = data.currentTileOn;
        reachedTownHall = data.reachedTownHall;
        detectedImpassable = data.detectedImpassable;
        townHall = data.townHall;
        card = data.card;
        busy = data.busy;
        toTownHall = data.toTownHall;
        cardToDeliver = data.cardToDeliver;
        buildingInteraction = data.buildingInteraction;
        hurt = data.hurt;
        durability = data.durability;
        health = data.health;
        regenCD = data.regenCD;
        nextRegen = data.nextRegen;
        workCard = data.workCard;
        workPlace = data.workPlace;
        working = data.working;
        workBegun = data.workBegun;
        workIndex = data.workIndex;
        workTime = data.workTime;
        workDone = data.workDone;
        workExtra = data.workExtra;
        buildPlace = data.buildPlace;
        building = data.building;
        buildBegun = data.buildBegun;
        patroling = data.patroling;
        patrolPlace = data.patrolPlace;
        target = data.target;
        //spottedAbyss = data.spottedAbyss;
        spottedEnemy = data.spottedEnemy;
        nextAttack = data.nextAttack;
        workInMemory = data.workInMemory;
        rmbTileData = data.rmbTileData;
        rmbWorkCard = data.rmbWorkCard;
        rmbWorkPlace = data.rmbWorkPlace;
        rmbWorkIndex = data.rmbWorkIndex;
        rmbWorkTime = data.rmbWorkTime;
        rmbWorkExtra = data.rmbWorkExtra;
        tileData = data.tileData;
    }

    public Unit_Info SaveData()
    {
        Unit_Info unit_info = new Unit_Info();
        unit_info.nextWander = nextWander;
        unit_info.impassable = impassable;
        unit_info.currentTileOn = currentTileOn;
        unit_info.reachedTownHall = reachedTownHall;
        unit_info.detectedImpassable = detectedImpassable;
        unit_info.townHall = townHall;
        unit_info.card = card;
        unit_info.busy = busy;
        unit_info.toTownHall = toTownHall;
        unit_info.cardToDeliver = cardToDeliver;
        unit_info.buildingInteraction = buildingInteraction;
        unit_info.hurt = hurt;
        unit_info.durability = durability;
        unit_info.health = health;
        unit_info.regenCD = regenCD;
        unit_info.nextRegen = nextRegen;
        unit_info.workCard = workCard;
        unit_info.workPlace = workPlace;
        unit_info.working = working;
        unit_info.workBegun = workBegun;
        unit_info.workIndex = workIndex;
        unit_info.workTime = workTime;
        unit_info.workDone = workDone;
        unit_info.workExtra = workExtra;
        unit_info.buildPlace = buildPlace;
        unit_info.building = building;
        unit_info.buildBegun = buildBegun;
        unit_info.patroling = patroling;
        unit_info.patrolPlace = patrolPlace;
        unit_info.target = target;
        //unit_info.spottedAbyss = spottedAbyss;
        unit_info.spottedEnemy = spottedEnemy;
        unit_info.nextAttack = nextAttack;
        unit_info.workInMemory = workInMemory;
        unit_info.rmbTileData = rmbTileData;
        unit_info.rmbWorkCard = rmbWorkCard;
        unit_info.rmbWorkPlace = rmbWorkPlace;
        unit_info.rmbWorkIndex = rmbWorkIndex;
        unit_info.rmbWorkTime = rmbWorkTime;
        unit_info.rmbWorkExtra = rmbWorkExtra;
        unit_info.tileData = tileData;
        return unit_info;
    }
    

    public class Unit_Info
    {
        public float nextWander;
        public bool impassable;
        public GameObject currentTileOn;
        public bool reachedTownHall;
        public GameObject detectedImpassable;
        public GameObject townHall;
        public Data_Card card; 
        public bool busy;
        public bool toTownHall;
        public bool cardToDeliver;
        public string buildingInteraction;
        public bool hurt;
        public int durability;
        public int health;
        public float regenCD;
        public float nextRegen;
        public Data_Card workCard;
        public GameObject workPlace; 
        public bool working;
        public bool workBegun; 
        public int workIndex; 
        public float workTime;
        public float workDone;
        public bool workExtra;
        public GameObject buildPlace;
        public bool building;
        public bool buildBegun;
        public bool patroling;
        public GameObject patrolPlace;
        public GameObject target;
        //public GameObject spottedAbyss;
        public GameObject spottedEnemy;
        public float nextAttack;
        public bool workInMemory;
        public Data_Tile rmbTileData;
        public Data_Card rmbWorkCard;
        public GameObject rmbWorkPlace;
        public int rmbWorkIndex;
        public float rmbWorkTime;
        public bool rmbWorkExtra;
        public Data_Tile tileData;
    }
}