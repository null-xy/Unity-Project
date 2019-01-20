using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour {

    // Use this for initialization
    /*[SerializeField]
    float speed;
    [SerializeField]
    float timeToLive;
    public float damage;
	void Start () {
        Destroy(gameObject, timeToLive);
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
	}
    void OnTriggerEnter(Collider other)
    {
        //print("hit"+other.name);
        var destractable = other.GetComponent<Destractable>();
        if (destractable == null)
            return;

        destractable.TakeDamage(damage);
    }*/
}
