using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destractable : MonoBehaviour {
    [SerializeField]
    float hitPonts;
    public event System.Action OnDeath;
    public event System.Action OnDamageRecived;

    float damageTaken;
    public float hitPointsRemain
    {
        get
        {
            return hitPonts - damageTaken;
        }

    }
    public bool isAlive
    {
        get
        {
            return hitPonts > 0;
        }
    }
    public virtual void Die()
    {
        if (!isAlive)
            return;
        if (OnDeath != null)
            OnDeath();
    }
    public virtual void TakeDamage(float amout)
    {
        damageTaken += amout;
        if (OnDamageRecived != null)
        {
            OnDamageRecived();
        }
        if (hitPointsRemain <= 0)
        {
            Die();
        }
    }
    public void Rest()
    {
        damageTaken = 0;
    }
}
