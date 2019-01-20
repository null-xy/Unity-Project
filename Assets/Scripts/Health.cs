using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : Destractable {
    public override void Die()
    {
        base.Die();
        print("hr died");
    }
    public override void TakeDamage(float amout)
    {
        
        base.TakeDamage(amout);
        print("remin health:" + hitPointsRemain);
    }
}
