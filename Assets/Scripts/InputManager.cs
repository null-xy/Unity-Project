using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

    // Use this for initialization
    public float Vertical;
    public float Horizontal;
    public Vector2 MouseInput;
    public bool Fire1;
    public bool Jump;
    public bool Run;

    public bool OpenInventory;
    public bool OpenGameSetting;
    public bool OpenQuestPanel;

    public bool PauseGame;
    public bool InventoryShow;
    public bool questPanelShow;

    public bool StartBattle;
    public bool BattleMod;
    public bool Aiming;
    public bool Shooting;
    public bool Crouch;
    public bool OnCrouch;

    public bool Talking;
    public bool DropItem;
    void Start () {
        PauseGame = false;
        BattleMod = false;
        OnCrouch = false;
        InventoryShow = false;
        questPanelShow = false;
    }
	
	// Update is called once per frame
	void Update () {
        Vertical = Input.GetAxis("Vertical");
        Horizontal = Input.GetAxis("Horizontal");
        MouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        //Fire1 = Input.GetMouseButtonDown(0);
        //bool kmark = Input.GetMouseButtonUp(0);
        Fire1 = Input.GetButtonDown("Fire1");
        Shooting = Input.GetButtonUp("Fire1");
        Jump = Input.GetButtonDown("Jump");
        Run = Input.GetKey(KeyCode.LeftShift);
        OpenInventory = Input.GetKeyDown(KeyCode.I);
        OpenQuestPanel = Input.GetKeyDown(KeyCode.J);
        OpenGameSetting = Input.GetKeyDown(KeyCode.Escape);
        StartBattle = Input.GetKeyDown(KeyCode.R);
        Crouch= Input.GetKeyDown(KeyCode.C);
        DropItem = Input.GetKeyDown(KeyCode.P);

        if (!PauseGame)
        {
            if (OpenInventory&&!questPanelShow)
            {
                InventoryShow = !InventoryShow;
            }
            if (OpenQuestPanel&&!InventoryShow)
            {
                questPanelShow = !questPanelShow;
            }
            if (Crouch)
            {
                OnCrouch = !OnCrouch;
            }
        }
        
        if (OpenGameSetting)
        {
            if (InventoryShow||questPanelShow) { InventoryShow = false; questPanelShow = false; }
            else if(!InventoryShow&&!questPanelShow) { PauseGame = !PauseGame; }
        }
        
        
        
    }
}
