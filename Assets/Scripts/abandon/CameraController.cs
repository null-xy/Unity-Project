using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    // Use this for initialization
    public Transform target;
    public Vector3 offset;
    public bool useOffsetValues;
    public float rotateSpeed;
    public Transform pivot;

    public float zoomSpped = 5f;
    public float minZoom = 0f;
    public float maxZoom = 6f;
    protected float distance = 3f;

    private Vector3 predictCameraPosition;
    private Vector3 collisonHit;
    public float dampening = 10f;
    protected Vector3 _LocalRotation;

    void Start () {
        if (!useOffsetValues)
        {
            offset = pivot.position - transform.position;
        }
       // pivot.transform.position = target.transform.position;
       // pivot.transform.parent = target.transform;

        Cursor.lockState = CursorLockMode.Locked;
    }
    bool camaeraCollison()
    {
        RaycastHit hit;
        LayerMask mask = (1 << LayerMask.NameToLayer("Player"));
        mask = ~mask;
        Debug.DrawLine(predictCameraPosition, pivot.position, Color.green);
        if (Physics.Linecast(pivot.position, predictCameraPosition, out hit, mask))
        {
            collisonHit = hit.point;
            return true;
        }
        else { return false; }
    }

    // Update is called once per frame
    void Update()
    {

    }
    void LateUpdate () {
        float horizontal = Input.GetAxis("Mouse X") * rotateSpeed;
        //target.Rotate(0, horizontal, 0);
        pivot.Rotate(0, horizontal, 0);
        float vertical = Input.GetAxis("Mouse Y") * rotateSpeed;
        pivot.Rotate(-vertical, 0, 0);

        if (pivot.rotation.eulerAngles.x < 180f && pivot.rotation.eulerAngles.x>45f)
        {
            pivot.rotation = Quaternion.Euler(45f, 0, 0);
        }
        if (pivot.rotation.eulerAngles.x > 180f && pivot.rotation.eulerAngles.x < 315f)
        {
            pivot.rotation = Quaternion.Euler(315f, 0, 0);
        }
        float desiredYAngle = pivot.eulerAngles.y;
        float desiredXAngle = pivot.eulerAngles.x;
        Quaternion rotation = Quaternion.Euler(desiredXAngle, desiredYAngle, 0);
        
        /*_LocalRotation.x += Input.GetAxis("Mouse X") * rotateSpeed;
        _LocalRotation.y -= Input.GetAxis("Mouse Y") * rotateSpeed;
        _LocalRotation.y = Mathf.Clamp(_LocalRotation.y, -90f, 90f);
        Quaternion QTy = Quaternion.Euler(_LocalRotation.y, 0, 0);
        Quaternion QTx = Quaternion.Euler(0, _LocalRotation.x, 0);
        Quaternion rotation = Quaternion.Euler(-_LocalRotation.y, _LocalRotation.x, 0);*/
        //pivot.rotation = QTy;
      //  target.rotation = QTx;
        //desiredXAngle = Mathf.Clamp(desiredXAngle, -90f, 90f);

        //-----------------zoom------------------------------
        float ScrollAmount = Input.GetAxis("Mouse ScrollWheel") * zoomSpped;
        ScrollAmount *= (distance * 0.3f);
        distance -= ScrollAmount;
        distance = Mathf.Clamp(distance, minZoom, maxZoom);
        offset.z = distance;


        predictCameraPosition = pivot.position - (rotation * offset);

        if (camaeraCollison())
        {
            transform.position = Vector3.Lerp(transform.position, (pivot.position + (collisonHit - pivot.position) * 0.8f), Time.deltaTime * dampening);
        }
        else
        {
            transform.position = predictCameraPosition;
        }
        //transform.LookAt(pivot);
        transform.rotation = rotation;
        
        Ray ray = new Ray(transform.position, transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * 20.5f, Color.red);
    }
}
