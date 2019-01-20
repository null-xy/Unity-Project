using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour {

    // Use this for initialization
    /*[SerializeField]
    float shootRate;

    [SerializeField]
    Transform projectile;

    [HideInInspector]
    public Transform muzzle;
    float nextShootAllowed;
    public bool canShoot;
    void Awake()
    {
        muzzle = transform.Find("Muzzle");
    }
    public virtual void Shoot()
    {
        canShoot = false;
        if (Time.time < nextShootAllowed)
            return;
        nextShootAllowed = Time.time + shootRate;
        Instantiate(projectile, muzzle.position, muzzle.rotation);
        canShoot = true;
    }*/
}
