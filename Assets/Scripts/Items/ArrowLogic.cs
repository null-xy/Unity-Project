using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowLogic : MonoBehaviour {

    // Use this for initialization
    Transform dks;
    Rigidbody rb;
    float speed = 1;
    public float range = 1.5f;
    public int damage = 1;
    void Start () {
        rb = GetComponent<Rigidbody>();
        //rb.AddForce(transform.forward * speed, ForceMode.Impulse);
        CapsuleCollider capCollider = gameObject.AddComponent<CapsuleCollider>();
        capCollider.height = 0.75f;
        capCollider.radius = 0.03f;
        capCollider.direction = 2;
    }

    void FixedUpdate()
    {
        rb.isKinematic = true;
        rb.useGravity = false;
        rb.mass = 1f;
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        //MeshCollider arrowCollider = gameObject.AddComponent<MeshCollider>();
        //arrowCollider.convex = true;

        RaycastHit hit;
        gameObject.layer = LayerMask.NameToLayer("Arrow");
        //LayerMask mask = (1 << LayerMask.NameToLayer("Player")) | (1 << LayerMask.NameToLayer("NPC")) | (1 << LayerMask.NameToLayer("Weapon"));//ignore player, enimy, weapon
        LayerMask mask = (1<<gameObject.layer);//ignore player, enimy, weapon
        mask = ~mask;
        if (Physics.Raycast(transform.position, transform.forward, out hit, range, mask))
        {
            
            if (hit.transform.gameObject.GetComponent<ItemPickUp>() != null)
            {
                rb.isKinematic = false;
                rb.AddForce(transform.forward * speed / 2, ForceMode.Impulse);
                rb.useGravity = true;
                print("hit 1 " + hit.transform.name);
                this.enabled = false;
            }
            else if (hit.transform.gameObject.GetComponent<EnemyStats>() != null)
            {
                //rb.isKinematic = false;
                EnemyStats enemyStat = hit.transform.gameObject.GetComponent<EnemyStats>();
                transform.parent = hit.transform;
                if (hit.collider.tag == "headShot")
                {
                    damage = 500;
                }
                enemyStat.TakeDamge(damage);
                this.enabled = false;
            }
            else
            {
                //rb.isKinematic = true;
                var emptyObject = new GameObject();
                emptyObject.transform.parent = hit.transform;
                transform.parent = emptyObject.transform;
                print("hit 3 " + hit.transform.name);
                this.enabled = false;
            }
        }
    }
    /*void OnTriggerEnter(Collider other)
    {
        print("hit"+other.name);
        var destractable = other.GetComponent<Destractable>();
        if (destractable == null)
            return;

        destractable.TakeDamage(damage);*/
        /*if (other.GetComponent<ItemPickUp>() != null)
        {
            rb.isKinematic = false;
            rb.AddForce(transform.forward * speed / 2, ForceMode.Impulse);
            rb.useGravity = true;
            print("hit 1 " + other.name);
            this.enabled = false;
        }
        else if (other.GetComponent<EnemyStats>() != null)
        {
            transform.parent = other.transform;
            print("hit 2 " + other.name);
            this.enabled = false;
        }
        else
        {
            var emptyObject = new GameObject();
            emptyObject.transform.parent = other.transform;
            transform.parent = emptyObject.transform;
            print("hit 3 " + other.name);
            this.enabled = false;
        }
    }*/
}
