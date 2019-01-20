using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonController : MonoBehaviour {

    public float speed = 5f;
    public float jumpForce = 5f;
    private bool IsGrounded;
    public Rigidbody rb;
    protected Collider coll;
    int jumpCount;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        rb = GetComponent<Rigidbody>();
        coll = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {

        if (CameraMouseLook.mouseLock)
        {
            if (Grounded())
            {
                jumpCount = 0;
            }
            if (Input.GetKeyDown(KeyCode.Space) && jumpCount < 2)
            {
                rb.AddForce(0, jumpForce, 0, ForceMode.Impulse);
                jumpCount += 1;
            }
            float moveX = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
            float moveZ = Input.GetAxis("Vertical") * speed * Time.deltaTime;
            transform.Translate(moveX, 0, moveZ);
        }
    }


    /*void OnCollisionStay(Collision collisionInfo){
		IsGrounded = true;
	}
	void OnCollisionExit(Collision collisionInfo){
		IsGrounded = false;
	}*/
    bool Grounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, coll.bounds.extents.y + 0.1f);
    }
}
