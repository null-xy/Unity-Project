using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharactorStats {

    public override void Die()
    {
        base.Die();
        //death animation
        Destroy(gameObject);
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
