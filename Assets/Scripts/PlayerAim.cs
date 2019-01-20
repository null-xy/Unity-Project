using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAim : MonoBehaviour {

    // Use this for initialization
    #region Singleton
    public static PlayerAim instance;
    void Awake()
    {
        instance = this;
    }
    #endregion
    public Camera mainCamera;
    Transform player;
    public Transform interactedObj=null;

    //public Vector3 aimPoint;
    //public Transform hitpoint;
    public float distance;
    public float attackDistance=5;
    // public float maxInteractDistance=5;
    float camDistanceFix;

    public static Vector3 aimPosition;
    public enum aimState { normal, aim, grab, talk }
    public static aimState myState;

    bool behanndYou;

    void Start () {
        player = this.transform;
        //aimState myState;
    }
	
	// Update is called once per frame
	void Update () {
        int x = Screen.width / 2;
        int y = Screen.height / 2;
        Ray ray = mainCamera.ScreenPointToRay(new Vector3(x,y));
       // Debug.DrawRay(ray.origin, ray.direction * 5f, Color.cyan);
        RaycastHit hit;
        LayerMask mask = 1 << LayerMask.NameToLayer("Player") ;//ignore player, enimy, weapon
        if (Physics.Raycast(ray, out hit, 100,~mask))
        {
            aimPosition = hit.point;
            interactedObj = hit.transform;
            distance = Vector3.Distance(player.position, interactedObj.position);
            camDistanceFix = Vector3.Distance(mainCamera.transform.position, aimPosition);
            if (camDistanceFix < mainCamera.GetComponent<CameraControl>().distance)
            {
                Vector3 lookPoint = ray.GetPoint(100);
                aimPosition = lookPoint;
                behanndYou = true;
            }
            else { behanndYou = false; }
            SetAimState();
        }
        else
        {
            Vector3 lookPoint = ray.GetPoint(100);
            aimPosition = lookPoint;
        }
        //hitpoint.position = aimPosition;
    }

    void SetAimState() {
        myState = aimState.normal;
        if (distance<5 && interactedObj.GetComponent<ItemPickUp>() != null&&!behanndYou)
            myState = aimState.grab;
        else if(distance<5 && interactedObj.GetComponent<NPC>() != null && !behanndYou)
            myState = aimState.talk;
        else if (distance < attackDistance && interactedObj.tag == "enemy" && !behanndYou)
            myState = aimState.aim;
    }
}
