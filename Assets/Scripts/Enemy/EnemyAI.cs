using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour {
    [SerializeField]    float patrolSpeed = 2f;
    [SerializeField]    float chaseSpeed = 5f;
    [SerializeField]    float aimWalkSpeed = 1f;
    [SerializeField]    float chaseWaitTime = 5f;
    [SerializeField]    float patrolWaitTime = 1f;
    public Transform[] patrolWayPoints;
    private EnemySight enemySight;
    private NavMeshAgent nav;
    private LastPlayerSighting lastPlayerSighting;

    public bool chase;
    public float chaseTimer;
    public float patrolTimer;
    private int wayPointIndex;

    public bool inCombat { get; private set; }

    public GameObject gameController;
    PlayerStats playerState;

    public event System.Action OnShooting;

    public float searchRange=10f;

    public bool search;
    EnemyStats enemyStats;
    Vector3 predictPlayerPosition;
    //________________________________________________________________________________________________________________
    ConnectedWaypoint _currentWaypoint;
    ConnectedWaypoint _previousWaypoint;
    //public GameObject[] allWaypoints;
    bool _travelling=true;
    bool _waiting;
    float _waitTimer;
    int _waypointsVisited;
    [SerializeField]    bool _patrolWaiting;
    [SerializeField]    float _totalWaitTime = 3f;
    GameObject[] allWaypoints;
    void Awake()
    {
        enemySight = GetComponent<EnemySight>();
        nav = GetComponent<NavMeshAgent>();
        lastPlayerSighting = gameController.GetComponent<LastPlayerSighting>();
    }
	void Start () {
        //player = PlayerManager.instance.player.transform;
        //playerState = localPlayer.GetComponent<PlayerStats>();
        playerState = PlayerManager.instance.player.GetComponent<PlayerStats>();
        //____________________________________________________________________________
        allWaypoints = GameObject.FindGameObjectsWithTag("wayPoint");//Grab all waypoint objects in scene.

    }

    // Update is called once per frame
    void Update () {
        if (enemySight.playerInSight)
        {
            /*if (enemyStats.attackRange.GetValue() > Vector3.Distance(transform.position, predictPlayerPosition))
            {

            }*/
            
            inCombat = true;
            //Shootimg();
            if (!IsInvoking("Shooting"))
            { InvokeRepeating("Shooting", 0.5f, 2f); }
            chase = false;
            chaseTimer = 0f;
        }
        else if (enemySight.personalLastSighting != lastPlayerSighting.resetPosition)//玩家被侦测到至少一次
        {
            CancelInvoke("Shooting");
            if(!search)
                Chasing();
            else if (search)
            {
                Searching();
                chaseTimer += Time.deltaTime;
                if (chaseTimer >= chaseWaitTime)
                {
                    lastPlayerSighting.position = lastPlayerSighting.resetPosition;//reset 重置敌人到未侦测状态
                    enemySight.personalLastSighting = lastPlayerSighting.resetPosition;
                    chaseTimer = 0f;
                    search = false;
                }
            }

        }
        else
            Patrolling();
            //Searching();
	}
    void Shooting()
    {
        //  nav.Stop();
        playerState.TakeDamge(5);
        OnShooting();
        nav.speed = aimWalkSpeed;
        //chase = false;
    }
    void Chasing()
    {
        chase = true;
        Vector3 sightingDeltaPos = enemySight.personalLastSighting - transform.position;
        if (sightingDeltaPos.sqrMagnitude > 4f)
            nav.destination = enemySight.personalLastSighting;
        nav.speed = chaseSpeed;
        if (nav.remainingDistance <= nav.stoppingDistance)
        {
            search = true;
        }
       // else
         //   chaseTimer = 0f;
    }
    void Patrolling()
    {
        inCombat = false;
        chaseTimer = 0f;
        //nav.isStopped = false;
        nav.speed = patrolSpeed;
        if (nav.destination == lastPlayerSighting.resetPosition || nav.remainingDistance <= nav.stoppingDistance)
        //if (nav.destination == lastPlayerSighting.resetPosition || nav.remainingDistance < 0.5f)
        {
            patrolTimer += Time.deltaTime;
            if (patrolTimer >= patrolWaitTime)
            {
                if (wayPointIndex == patrolWayPoints.Length - 1)
                { wayPointIndex = 0; }
                else
                { wayPointIndex++; }

                patrolTimer = 0f;
            }
        }
        else
            patrolTimer = 0f;

        nav.destination = patrolWayPoints[wayPointIndex].position;
    }
    void Searching()
    {
        inCombat = true;
       // _travelling = true;
        nav.speed = patrolSpeed;
        if (_currentWaypoint == null)
        {
            //Set it at random.
            
            if (allWaypoints.Length > 0)
            {
                while (_currentWaypoint == null)
                {
                    float minDistance = float.MaxValue;
                    int closestPoint = 0;
                    for (int i = 0; i < allWaypoints.Length; i++)
                    {
                        float thisDistance = Vector3.Distance(transform.position, allWaypoints[i].transform.position);
                        if(thisDistance< minDistance)
                        {
                            minDistance = thisDistance;
                            closestPoint = i;
                           // Debug.Log("closePoint =" + closestPoint);
                        }
                    }
                    //int random = Random.Range(0, allWaypoints.Length);
                    ConnectedWaypoint startingWaypoint = allWaypoints[closestPoint].GetComponent<ConnectedWaypoint>();//从最近的一个巡逻点开始

                    //i.e. we found a waypoint.
                    if (startingWaypoint != null)
                    {
                        _currentWaypoint = startingWaypoint;
                    }
                }
            }
            else
            {
                Debug.LogError("Failed to find any waypoints for use in the scene.");
            }
        }
        if (_travelling && nav.remainingDistance <= 1.0f)
        {
            _travelling = false;
            _waypointsVisited++;

            //If we're going to wait, then wait.
            if (_patrolWaiting)
            {
                _waiting = true;
                _waitTimer = 0f;
            }
            else
            {
                SetDestination();
            }
        }

        //Instead if we're waiting.
        if (_waiting)
        {
            _waitTimer += Time.deltaTime;
            if (_waitTimer >= _totalWaitTime)
            {
                _waiting = false;

                SetDestination();
            }
        }
    }
    private void SetDestination()
    {
        if (_waypointsVisited > 0)
        {
            ConnectedWaypoint nextWaypoint = _currentWaypoint.NextWaypoint(_previousWaypoint);
            _previousWaypoint = _currentWaypoint;
            _currentWaypoint = nextWaypoint;
        }

        Vector3 targetVector = _currentWaypoint.transform.position;
        nav.SetDestination(targetVector);
        _travelling = true;
    }
}
