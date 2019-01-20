using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour {

    public Transform target;
    //public Transform target;
    //public Vector3 offset;
    //public Transform targetBody;
    public Transform pivot;

    protected Vector3 _LocalRotation;
    protected float distance = 5f;
 
    public float rotateSpeed = 4f;
    public float dampening = 10f;

    public float zoomSpped = 3f;
    public float minZoom = 0f;
    public float maxZoom = 5f;
    private Vector3 predictCameraPosition;
    private Vector3 collisonHit;

    public bool CameraDisabled = false;

    // Use this for initialization
    void Start() {
        //target = this.transform.parent;
       // pivot = this.transform.parent.Find("pivot");
       // offset = transform.position - pivot.position;
    }

    bool camaeraCollison() {
        RaycastHit hit;
        LayerMask mask = (1 << LayerMask.NameToLayer("Player"));
        mask = ~mask;
        //predictCameraPosition = pivot.position + target.forward * offset.z + target.up * offset.y + target.right * offset.x;
        //predictCameraPosition= target.position - (rotation * offset);
        Debug.DrawLine(predictCameraPosition, pivot.position, Color.green);
        if (Physics.Linecast(pivot.position, predictCameraPosition, out hit, mask))
        {
            collisonHit= hit.point;
            return true;
        }
        else { return false; }
    }

    void LateUpdate() {
        
        if (Input.GetKeyDown(KeyCode.LeftShift))
            CameraDisabled = !CameraDisabled;
        if (!CameraDisabled)
        {
            //Rotation of the Camera based on Mouse Coordinates
            _LocalRotation.x += Input.GetAxis("Mouse X") * rotateSpeed;
            _LocalRotation.y -= Input.GetAxis("Mouse Y") * rotateSpeed;


            //Clamp the y Rotation to horizon and not flipping over at the top

             _LocalRotation.y = Mathf.Clamp(_LocalRotation.y, -90f, 90f);
            
            //Zooming Input from our Mouse Scroll Wheel
            float ScrollAmount = Input.GetAxis("Mouse ScrollWheel") * zoomSpped;
                ScrollAmount *= (distance * 0.3f);
                distance -= ScrollAmount;
                distance = Mathf.Clamp(distance, minZoom, maxZoom);
            // offset = offset.normalized * distance;
         //   offset.z = distance;
           // offset.z = Mathf.Cos(_LocalRotation.y / 180f) * distance;
            //offset.y = Mathf.Sin(_LocalRotation.y / 180f) * distance;

           // print(Mathf.Sin(_LocalRotation.y / 180f));
        }

        // predictCameraPosition = pivot.position + target.forward * offset.z + target.up * offset.y + target.right * offset.x;
        predictCameraPosition = pivot.position - target.forward*distance;
       // Quaternion QTy = Quaternion.Euler(_LocalRotation.y, 0, 0);
        //Quaternion QTx = Quaternion.Euler(0, _LocalRotation.x, 0);
        Quaternion QT = Quaternion.Euler(-_LocalRotation.y, _LocalRotation.x, 0);
        //pivot.rotation = QTy;
        //pivot.position = target.position - (QTx * new Vector3(1, 0, 1));
        //predictCameraPosition = target.position - (QTy * offset);
        target.rotation = Quaternion.Lerp(target.rotation, QT, Time.deltaTime * dampening);

    //    Quaternion cameraRotation = Quaternion.LookRotation(pivot.position - predictCameraPosition, Vector3.up);
      //  transform.rotation = cameraRotation;
        // transform.rotation = target.rotation;


        Ray ray = new Ray(transform.position, transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * 20.5f, Color.red);
        //targetBody.rotation = cameraRotation;

        //transform.position = target.position - (rotation * offset);


        //-------------------camera collision----------------
        if (camaeraCollison())
        {
            // Vector3 fixHitPoint = new Vector3(collisonHit.x + collisonHit.x * 0.2f, collisonHit.y, collisonHit.z + collisonHit.normalized.z * 0.2f);
            //transform.position = new Vector3(fixHitPoint.x, target.position.y, fixHitPoint.z);
            transform.position = Vector3.Lerp(transform.position, (pivot.transform.position + (collisonHit - pivot.position) * 0.8f),Time.deltaTime*dampening);
        }
        else
        {
            //transform.localPosition = new Vector3(0f, 0f, Mathf.Lerp(transform.localPosition.z, -_CameraDistance, Time.deltaTime * ScrollDampening)) + offset;
            //-------camera move spped-----------
           // transform.position = Vector3.Lerp(transform.position, predictCameraPosition, Time.deltaTime * dampening);
            transform.position = predictCameraPosition;
        }
    }
}