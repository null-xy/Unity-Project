using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCAnimator : MonoBehaviour {
    const float locomationAnimationSmoothTime = 0.1f;
    // Use this for initialization
    NavMeshAgent agent;
    Animator animator;
    EnemyAI enemyAI;
    float speedPercent;
    Vector3 lookPoint;
    Transform player;
    //Player localPlayer;
    //EnemySight enemySight;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        enemyAI = GetComponent<EnemyAI>();
        //enemySight = GetComponent<EnemySight>();
        //GameManager.Instance.OnLocalPlayerJoined += Instance_OnLocalPlayerJoined;
        enemyAI.OnShooting += OnAttack;
        //player = localPlayer.transform;
    }
    /*private void Instance_OnLocalPlayerJoined(Player obj)
    {
        localPlayer = obj;
    }*/

    void Start () {
        player = PlayerManager.instance.player.transform;
    }
	
	// Update is called once per frame
	void Update () {

        speedPercent = agent.velocity.magnitude / agent.speed;
        if (agent.speed < 3.5f && speedPercent>=0.5) { speedPercent = 0.5f; }
        animator.SetFloat("Speed", speedPercent, locomationAnimationSmoothTime, Time.deltaTime);
        animator.SetBool("InCombat", enemyAI.inCombat);
        lookPoint = player.position + player.up;
    }
    void OnAnimatorIK()
    {
        if(enemyAI.inCombat)
          {
            animator.SetLookAtPosition(lookPoint);
            animator.SetLookAtWeight(0.5f, 0, 1f, 0, 0.6f);
        }
        //animator.SetLookAtWeight(1, 1, 1f, 0, 1);
        //animator.SetLookAtWeight(0.25f, 0.5f, 1f, 1f, 0.6f);//头部旋转

    }
    protected void OnAttack()
    {
        animator.SetTrigger("Attack");
    }
    public void ArrowShow()
    {
        print("WRINIMAB");
    }
    public void ArrowHidden()
    {
        print("sanisnai");
    }

}
