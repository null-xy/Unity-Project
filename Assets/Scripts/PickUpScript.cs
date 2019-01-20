using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpScript : MonoBehaviour {

    PlayerAim f;
    private CameraControl o;
    float pressTime=0;

    void Awake()
    {
    }

    void Start()
    {
        f = GetComponent<PlayerAim>();
        //f = GameManager.Instance.PlayerAim;
        o = GameObject.Find("Main Camera").GetComponent<CameraControl>();
    }

    // Update is called once per frame
    void Update()
    {
        if (f.interactedObj == null)
        return;
        if(Input.GetKey(KeyCode.E) && PlayerAim.myState == PlayerAim.aimState.grab)
        {
            pressTime += Time.deltaTime;
        }
        if (Input.GetKeyUp(KeyCode.E) && pressTime<0.25f && PlayerAim.myState == PlayerAim.aimState.grab)
        {
            //if (beingCarried)
            PickUp();
            pressTime = 0;
        }
        if (Input.GetKey(KeyCode.E) && pressTime >= 0.25f && PlayerAim.myState == PlayerAim.aimState.grab)
        {
            Grab();
        }

        if (Input.GetKeyUp(KeyCode.E) || PlayerAim.myState != PlayerAim.aimState.grab)
        {
            PutDown();
            pressTime = 0;
        }
    }

    void Grab()
    {
        //f.interactedObj.GetComponent<Rigidbody>().isKinematic = true;
        
            f.interactedObj.GetComponent<Rigidbody>().AddForce(new Vector3(0, 9.8f, 0), ForceMode.Force);
            //f.interactedObj.transform.parent = player;
            //f.interactedObj.transform.position = f.aimPosition;
            //f.interactedObj.transform.parent = camera;
            //f.interactedObj.GetComponent<Rigidbody>().mass
            f.interactedObj.transform.position = Vector3.Lerp(f.interactedObj.transform.position, o.transform.position + o.transform.forward * (o.distance + 2.5f), 0.15f);
            f.interactedObj.transform.rotation = o.transform.rotation;
            //beingCarried = true;
        
    }
    void PutDown()
    {
        //f.interactedObj.transform.parent = null;
        //f.interactedObj.GetComponent<Rigidbody>().isKinematic = false;
        //beingCarried = false;
        f.interactedObj = null;
    }
    void PickUp()
    {
        //Debug.Log("pick up  " + f.interactedObj.name);
        ItemPickUp itempick = f.interactedObj.gameObject.GetComponent<ItemPickUp>();
        //Inventory.instance.Add(itempick.item);
        bool wasPickedUp= Inventory.instance.Add(Instantiate(itempick.item));
        if(wasPickedUp)
        Destroy(f.interactedObj.gameObject);
    }
}
