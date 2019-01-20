using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

    // Use this for initialization
    public Transform target;//获取旋转目标
    //public float rotateSpeed;
    public float distance = 4f;
    public float zoomSpped = 5f;
    public float minZoom = 1f;
    public float maxZoom = 6f;
    public Vector3 offset;
    private Vector3 predictCameraPosition;
    public float dampening = 10f;
    private Vector3 collisonHit;
    //protected Vector3 cameraLook;
    Vector2 mouseLook;
    Vector2 smoothV;
    public float sensitivity = 5.0f;
    public float smoothing = 2.0f;

    InputManager playerInput;
    public Player localPlayer;
    void Awake()
    {
        playerInput = GameManager.Instance.InputManager;
        GameManager.Instance.OnLocalPlayerJoined += Instance_OnLocalPlayerJoined;
    }

    private void Instance_OnLocalPlayerJoined(Player obj)
    {
        localPlayer = obj;
    }

    void Start() {
        offset = new Vector3(0.7f, 0, 0);
    }

    //detecte camera collison
    bool camaeraCollison()
    {
        RaycastHit hit;
        LayerMask mask = (1 << LayerMask.NameToLayer("Player")) | (1 << LayerMask.NameToLayer("NPC")) | (1 << LayerMask.NameToLayer("Weapon"));//ignore player, enimy, weapon
        mask = ~mask;
        //Debug.DrawLine(predictCameraPosition, target.position, Color.green);
        if (Physics.Linecast(target.position, predictCameraPosition, out hit, mask))
        {
            collisonHit = hit.point;
            return true;
        }
        else { return false; }
    }
    void Update()
    {
        /*cameraLook.x += Input.GetAxis("Mouse X") * rotateSpeed;
        cameraLook.y -= Input.GetAxis("Mouse Y") * rotateSpeed;
        //Clamp the y Rotation to horizon and not flipping over at the top
        cameraLook.y = Mathf.Clamp(cameraLook.y, -90f, 90f);
        */
        var mouseIuput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        mouseIuput = Vector2.Scale(mouseIuput, new Vector2(sensitivity * smoothing, sensitivity * smoothing));
        smoothV.x = Mathf.Lerp(smoothV.x, mouseIuput.x, 1f / smoothing);
        smoothV.y = Mathf.Lerp(smoothV.y, mouseIuput.y, 1f / smoothing);
        mouseLook += smoothV;
        mouseLook.y = Mathf.Clamp(mouseLook.y, -90f, 90f);

        Debug.DrawRay(transform.position, transform.forward*500, Color.green);
        Debug.DrawRay(target.parent.position, transform.forward * 500, Color.green);
    }
    // Update is called once per frame
    void LateUpdate() {
        if (playerInput.InventoryShow || playerInput.PauseGame || playerInput.Talking)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else if (!playerInput.InventoryShow && !playerInput.PauseGame && !playerInput.Talking)
        {
            // || playerInput.BattleMod
            if (playerInput.Vertical != 0 || playerInput.Horizontal != 0 || playerInput.BattleMod ||distance<0.5f)
            {
                //target.parent.LookAt(new Vector3(PlayerAim.instance.hitpoint.position.x, target.parent.position.y, PlayerAim.instance.hitpoint.position.z));
                Quaternion playerRotation = Quaternion.Euler(0, mouseLook.x, 0);
                //Quaternion playerRotation = Quaternion.Euler(0, PlayerAim.instance.hitpoint.position.x, 0);
                target.parent.rotation = Quaternion.Slerp(target.parent.rotation, playerRotation, Time.deltaTime * dampening);
            }
                            
                //target.parent.LookAt(PlayerAim.instance.hitpoint);
                //target.parent.rotation = Quaternion.Euler(0, lookPoint.x, 0);
                //Quaternion rota = Quaternion.LookRotation((PlayerAim.instance.hitpoint.position - target.parent.position), Vector3.up);
                //target.parent.rotation = rota;
                
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Quaternion cameraRotation = Quaternion.Euler(-mouseLook.y, mouseLook.x, 0);
        transform.rotation = cameraRotation;
        predictCameraPosition = target.position - cameraRotation * Vector3.forward * distance + cameraRotation * offset;

        float scrollAmount = Input.GetAxis("Mouse ScrollWheel") * zoomSpped;
        scrollAmount *= (distance * 0.3f);
        distance -= scrollAmount;
        distance = Mathf.Clamp(distance, minZoom, maxZoom);
        //offset = offset.normalized*distance;

        if (camaeraCollison())
        {
            transform.position = Vector3.Slerp(transform.position, (target.position + (collisonHit - target.position) * 0.9f) + cameraRotation * offset, 1f / smoothing);
        } else if(distance<0.5f)
            {
                transform.position = target.position + new Vector3(0.1f, 0.1f, 0);
            }
        else
        {
                transform.position = Vector3.Slerp(transform.position, predictCameraPosition, 1f / smoothing);
                //transform.position = predictCameraPosition;
        }
    } }
}