using System.Collections;
using System.Collections.Generic;
//using UnityEngine.EventSystems;
using UnityEngine;

public class PlayerControll : MonoBehaviour {

    // Use this for initialization
    public float moveSpeed;
    public float jumpForce;
    public CharacterController controller;
    private Vector3 moveDirection;
    public float gravityScale;


    InputManager playerInput;
    void Awake()
    {
        playerInput = GameManager.Instance.InputManager;
    }
	void Start () {
        controller = GetComponent<CharacterController>();
	}

    // Update is called once per frame
    void Update () {
        if (playerInput.PauseGame)
            return;
        //Vector2 direction = new Vector2(playerInput.Vertical*moveSpeed, playerInput.Horizontal*moveSpeed);
        
        //moveDirection = new Vector3(Input.GetAxis("Horizontal") * moveSpeed, moveDirection.y, Input.GetAxis("Vertical") * moveSpeed);
        float YStore = moveDirection.y;
       // moveDirection = (transform.forward * Input.GetAxis("Vertical")) + (transform.right * Input.GetAxis("Horizontal"));
        moveDirection = ((transform.forward * playerInput.Vertical) + (transform.right * playerInput.Horizontal));
        moveDirection = moveDirection.normalized * moveSpeed;
        if (playerInput.Run)
        {
            moveSpeed = 6;
        }
        else { moveSpeed = 3f; }
        moveDirection.y = YStore;
        if (controller.isGrounded)
        {
            moveDirection.y = 0f;
            if (Input.GetButtonDown("Jump"))
            {
                moveDirection.y = jumpForce;
            }
        }
        moveDirection.y = moveDirection.y + (Physics.gravity.y * gravityScale * Time.deltaTime);
        controller.Move(moveDirection*Time.deltaTime);

       // mouseInput.x = Mathf.Lerp(mouseInput.x, Input.GetAxis("Mouse X"), 1f/ dampening);
      //  transform.Rotate(Vector3.up * mouseInput.x * rotateSpeed);
	}
    public void ArrowShow()
    {
        print("fucking shit");
    }
}
