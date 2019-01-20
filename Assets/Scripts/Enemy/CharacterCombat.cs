using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCombat : MonoBehaviour {

    // Use this for initialization
    public float attackSpeed = 1f;
    private float attackCooldown = 0f;
    const float combatCooldown = 5f;
    float lastAttackTime;

    public float attackDelay = 0.6f;
    public bool InCombat { get; private set; }
    public event System.Action OnAttack;

    CharactorStats myStats;
	void Start () {
        myStats = GetComponent<CharactorStats>();
	}
	
	// Update is called once per frame
	void Update () {
        attackCooldown -= Time.deltaTime;
        if (Time.time - lastAttackTime > combatCooldown)
        {
            InCombat = false;
        }
	}
    public void Attack(CharactorStats tagetStats)
    {
        if (attackCooldown <= 0)
        {
            StartCoroutine(DoDamage(tagetStats, attackDelay));
            if (OnAttack != null)
                OnAttack();
            attackCooldown = 1f / attackSpeed;
            InCombat = true;
            lastAttackTime = Time.time;
        }
    }
    IEnumerator DoDamage(CharactorStats stats,float delay)
    {
        yield return new WaitForSeconds(delay);
        stats.TakeDamge(myStats.damage.GetValue());
        if (stats.currentHealth <= 0)
        {
            InCombat = false;
        }
    }
}
