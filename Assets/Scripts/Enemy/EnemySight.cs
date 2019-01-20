using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySight : MonoBehaviour {
    [SerializeField]
    Transform enemyEyes;
    public float fieldOfViewAngle = 110f;
    public bool playerInSight;
    public Vector3 personalLastSighting;

    private NavMeshAgent nav;
    private SphereCollider col;
   // private Animator anim;
    private LastPlayerSighting lastPlayerSighting;

    public GameObject gameController;
    GameObject player;
    private Animator playerAnim;
    //private PlayerHealth playerHealth;
    //private HashIDs hash;
    private Vector3 previousSighting;

    public Vector3 playerFacing;
    public Transform jiancha;
    void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
        col = GetComponent<SphereCollider>();
       // anim = GetComponent<Animator>();
        lastPlayerSighting = gameController.GetComponent<LastPlayerSighting>();
        //player = GameObject.FindGameObjectWithTag("Player");
        //playerAnim = player.GetComponent<Animator>();
        //playerHealth = player.GetComponent<PlayerHealth>();
        //hash=GameObject.FindObjectWithTag(Tags.gameController).GetComponent<HashID>();
        personalLastSighting = lastPlayerSighting.resetPosition;
        previousSighting = lastPlayerSighting.resetPosition;
    }
    void Start()
    {
        player = PlayerManager.instance.player;
    }
    void Update()
    {
        if (lastPlayerSighting.position != previousSighting)
            personalLastSighting = lastPlayerSighting.position;

        previousSighting = lastPlayerSighting.position;

    }
    void OnTriggerStay(Collider other)
    {
        //Debug.Log(other.name);
        if (other.gameObject == player)
        {
            playerInSight = false;
            jiancha.position = other.transform.position+ other.transform.up;
            Vector3 direction = other.transform.position + other.transform.up - enemyEyes.position;
            float angle = Vector3.Angle(direction, enemyEyes.forward);

            if (angle < fieldOfViewAngle * 0.5)
            {
                RaycastHit hit;
                
                //Debug.DrawRay(transform.position + new Vector3(0, 0.6f, 0), direction.normalized, Color.green);
                //if (Physics.Raycast(transform.position+transform.up,direction.normalized,out hit, col.radius))
                //if (Physics.Raycast(transform.position+new Vector3(0,0.6f,0), direction.normalized, out hit, col.radius))
                LayerMask mask = (1 << LayerMask.NameToLayer("NPC")) | (1 << LayerMask.NameToLayer("Weapon"));//ignore player, enimy, weapon
                mask = ~mask;
                if (Physics.Raycast(enemyEyes.position, direction.normalized, out hit, col.radius, mask))
                {
                    if (hit.collider.gameObject == player)
                    {
                        playerInSight = true;
                        lastPlayerSighting.position = player.transform.position;
                        //playerFacing = player.transform;
                        FaceTarget();
                    }
                }
            }
            //int playerLayerZeroStateHsh = playerAnim.GetCurrentAnimatorStateInfo(0).nameHash;
            //int playerLayerOneStateHash
        }
    }
    void FaceTarget()
    {
        Vector3 direction = (player.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 50f);
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
            playerInSight = false;
    }

    float CalculatePathLength(Vector3 targetPosition)
    {
        NavMeshPath path = new NavMeshPath();
        if (nav.enabled)
            nav.CalculatePath(targetPosition, path);
        Vector3[] allWayPoints = new Vector3[path.corners.Length + 2];
        allWayPoints[0] = transform.position;
        allWayPoints[allWayPoints.Length - 1] = targetPosition;
        for(int i = 0; i < path.corners.Length; i++)
        {
            allWayPoints[i + 1] = path.corners[i];
        }
        float pathLength = 0;

        for(int i = 0; i < allWayPoints.Length - 1; i++)
        {
            pathLength += Vector3.Distance(allWayPoints[i], allWayPoints[i + 1]);
        }
        return pathLength;
    }
}
