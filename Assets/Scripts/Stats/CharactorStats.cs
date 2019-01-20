using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactorStats : MonoBehaviour {
    public int maxHealth = 100;
    public int currentHealth { get; private set; }
    public Stat damage;
    public Stat armor;
    public Stat attackRange;
    public Stat attackSpeed;
    void Awake()
    {
        currentHealth = maxHealth;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            TakeDamge(10);
        }
    }
    public void TakeDamge(int damage)
    {
        damage -= armor.GetValue();
        damage = Mathf.Clamp(damage, 0, int.MaxValue);
        currentHealth -= damage;
        Debug.Log(transform.name + " takes " + damage + " damage.");
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    public virtual void Die()
    {
        //die in some way
        //this method is meant to be overwritten
        Debug.Log(transform.name + "died");
    }
}
