using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour {

    public Transform fix;

    public float angle;
    Animator animator;
    InputManager playerInput;
    CharacterController characterController;
    GameObject player;

    Transform leftShoulder;

    public Vector3 lookPoint;
    //PlayerAim playerAim;
    //public Transform handArrow;
    GameObject arrow;
    GameObject bow;
    Weapon arrowItem;
    Inventory inventory;
    bool onAiming;
    bool arrowShow;
    PlayerStats playerStats;
    WeaponManager weaponManager;

    void Start () {
        
        // playerAim = GetComponent<PlayerAim>();
        player = PlayerManager.instance.player;
        characterController=player.GetComponent<CharacterController>();
        playerStats = player.GetComponent<PlayerStats>();
        playerInput = GameManager.Instance.InputManager;
        animator = GetComponent<Animator>();
        inventory = Inventory.instance;
        //EquipmentManager.instance.onEquipmentChanged += OnEquipmentChanged;
        WeaponManager.instance.onWeaponChanged += OnWeaponChanged;
        weaponManager = WeaponManager.instance;
}
    //左右手分离
	/*void OnEquipmentChanged(Equipment newItem, Equipment oldItem)
    {
        if(newItem!=null && newItem.equipSlot == EquipmentSlot.LeftWeapon)
        {
            animator.SetLayerWeight(1, 1);
        }else if(newItem==null && oldItem!=null && oldItem.equipSlot == EquipmentSlot.LeftWeapon)
        {
            animator.SetLayerWeight(1, 0);
        }
        if (newItem != null && newItem.equipSlot == EquipmentSlot.RightWeapon)
        {
            animator.SetLayerWeight(2, 1);
        }
        else if (newItem == null && oldItem != null && oldItem.equipSlot == EquipmentSlot.RightWeapon)
        {
            animator.SetLayerWeight(2, 0);
        }
    }*/
    void OnWeaponChanged(Weapon newItem,Weapon oldItem)
    {
        if (newItem != null && newItem.weaponSlot == WeaponSlot.Bow)
        {
            if (inventory.searchArrow() != null && arrowItem == null)//如果装备弓，如果背包里面有箭，自动装备箭
            {
                Weapon equipArrow = inventory.searchArrow();
                equipArrow.Use();
            }
        }
        else if (newItem == null && oldItem != null && oldItem.weaponSlot == WeaponSlot.Bow)//脱下装备
        {
            if (arrowItem != null)
            {
                WeaponManager.instance.Unequip((int)arrowItem.weaponType);
                arrowItem = null; 
            }
        }
        if (newItem != null && newItem.weaponSlot == WeaponSlot.Arrow)
        {
            arrowItem = newItem;
        }
        else if (newItem == null && oldItem != null && oldItem.weaponSlot == WeaponSlot.Arrow)
        {
            arrowItem = null;
        }
    }
    void OnAnimatorIK()
    {
        leftShoulder = animator.GetBoneTransform(HumanBodyBones.LeftShoulder);
        Vector3 dir = lookPoint - leftShoulder.position;
        dir.Normalize();
        angle = Vector3.Angle(transform.forward, dir);
        if (lookPoint.y > leftShoulder.position.y)
        {
            angle = -angle;
        }
        angle = Mathf.Clamp(angle, -90, 90);

        Quaternion rot1 = Quaternion.Euler(0, 0, angle/3);
        Quaternion rot2 = Quaternion.Euler(0, 0, angle/3);
        Quaternion rot3 = Quaternion.Euler(0, 0, angle/3);
        if (playerInput.BattleMod)
        {
            animator.SetLookAtPosition(lookPoint);
            animator.SetLookAtWeight(0.5f, 0, 0.5f, 0, 0.6f);
            //animator.SetLookAtWeight(1, 1, 1f, 0, 1);
            //animator.SetLookAtWeight(0.25f, 0.5f, 1f, 1f, 0.6f);//头部旋转
            //animator.SetLookAtWeight(0.5f, 0, 1f, 0, 0.6f);            
            //animator.SetLookAtWeight(0.5f, 0, 1f, 0, 0.6f);
        }
        if (playerInput.Aiming&&playerInput.BattleMod&&onAiming)
        {
            if (angle >= 45 || angle <= -45)
            {
                animator.SetBoneLocalRotation(HumanBodyBones.Spine, rot1);
                animator.SetBoneLocalRotation(HumanBodyBones.Chest, rot2);
                animator.SetBoneLocalRotation(HumanBodyBones.UpperChest, rot3);
            }
            else if (angle < 45 || angle > -45)
            {
                animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
                animator.SetIKPosition(AvatarIKGoal.LeftHand, dir*0.8f+leftShoulder.position+new Vector3(0,0.05f,0));

                Transform leftHand = animator.GetBoneTransform(HumanBodyBones.LeftHand);
                Transform rightHand = animator.GetBoneTransform(HumanBodyBones.RightHand);
                Vector3 direction = leftHand.position - rightHand.position;
                direction.Normalize();
                Quaternion rightRot = Quaternion.LookRotation(direction);
                animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
                animator.SetIKRotation(AvatarIKGoal.RightHand, rightRot);
                //animator.SetIKHintPositionWeight(AvatarIKHint.RightElbow, 1);
                //animator.SetIKHintPosition(AvatarIKHint.RightElbow, fix.position);
            }
            //rightElbow.position = rightShoulder.position - dir1*distance;
        }


    }
    void Update () {
        if (weaponManager.weaponRight.childCount > 0)
        {
            arrow = weaponManager.weaponRight.GetChild(0).gameObject;
            if (arrowShow) { arrow.SetActive(true); }
            else if (!arrowShow) { arrow.SetActive(false); }
        }
        if (!playerInput.InventoryShow && !playerInput.PauseGame &&!playerInput.Talking)
        {
            if (playerInput.StartBattle)
            {
                if (playerInput.Aiming)
                {
                    playerInput.Aiming = false;
                }
                else { playerInput.BattleMod = !playerInput.BattleMod; }
            }
            if (playerInput.Fire1 && playerInput.BattleMod)
            {
                if (arrowItem != null)
                {
                    playerInput.Aiming = true;
                }
                else if (arrowItem==null)
                {
                    if (inventory.searchArrow() != null)
                    {
                        inventory.searchArrow().Use();
                        playerInput.Aiming = true;
                    }
                    else
                    {
                        playerInput.Aiming = false;
                        print("you don't have any arrow");
                    }
                }
            }
            else if (playerInput.Fire1 && !playerInput.BattleMod)
            {
                playerInput.BattleMod = true;
            }
            if (playerInput.Shooting)
            {
                playerInput.Aiming = false;
            }
            /*if (weaponManager.currentWeapon[0] != null)
            {
                if (weaponManager.currentWeapon[0].weaponSlot == WeaponSlot.Bow&&playerInput.BattleMod)
                {
                }
            }*/
        }
        
        //lookPoint =  PlayerAim.instance.hitpoint.position;
        lookPoint = Vector3.Slerp(lookPoint, PlayerAim.aimPosition, 0.5f);
        fix.position = lookPoint;
        if (!playerInput.PauseGame&&!playerInput.InventoryShow)
        {
            animator.SetFloat("Vertical", playerInput.Vertical);
            animator.SetFloat("Horizontal", playerInput.Horizontal);
            animator.SetBool("Run", playerInput.Run);
            animator.SetBool("BattleMod", playerInput.BattleMod);
            animator.SetBool("Aiming", playerInput.Aiming);
            animator.SetBool("Crouch", playerInput.OnCrouch);
            animator.SetBool("isGround", characterController.isGrounded);
            if (playerInput.Jump)
            {
                animator.SetTrigger("Jump");
            }
            if (playerInput.Shooting && playerInput.BattleMod)
            {
                animator.SetTrigger("Attack");
            }
        }
        
    }
    void ArrowShow()
    {
        arrowShow = true;
    }
    void ArrowHidden()//把箭射出去
    {
        arrowShow = false;

        leftShoulder = animator.GetBoneTransform(HumanBodyBones.LeftShoulder);
        Vector3 dir2 = lookPoint - leftShoulder.position;
        dir2.Normalize();
        Quaternion targetRot = Quaternion.LookRotation(dir2);
        Vector3 spawnP = animator.GetBoneTransform(HumanBodyBones.LeftHand).position;
        GameObject go = Instantiate(arrow, spawnP, targetRot) as GameObject;
        go.AddComponent<Rigidbody>();
        ItemPickUp dropItem = go.AddComponent<ItemPickUp>();
        dropItem.item = arrowItem;
        go.name = arrowItem.name;
        ArrowLogic arrowLogic = go.AddComponent<ArrowLogic>();
        arrowLogic.damage = playerStats.damage.GetValue();
        go.SetActive(true);
        inventory.Remove(arrowItem);
    }
    /*public void OnJump()
    {
        Debug.Log("on jump");
    }
    public void OnJumpExit()
    {
        Debug.Log("on jump exit");
    }*/
    void GrabBow()
    {
        if (weaponManager.weaponBack.childCount > 0)
        {
            bow= weaponManager.weaponBack.GetChild(0).gameObject;
            bow.transform.parent = weaponManager.weaponLeft;
            bow.transform.localPosition = Vector3.zero;
            bow.transform.localRotation = Quaternion.identity;
            bow.transform.localScale = new Vector3(2, 2, 2);
        }
    }
    void PutBackBow()
    {
        if (weaponManager.weaponLeft.childCount > 0)
        {
            bow = weaponManager.weaponLeft.GetChild(0).gameObject;
            bow.transform.parent = weaponManager.weaponBack;
            bow.transform.localPosition = Vector3.zero;
            bow.transform.localRotation = Quaternion.identity;
            bow.transform.localScale = new Vector3(2, 2, 2);
        }
    }
    public void OnAiming()
    {
        onAiming = true;
    }
    public void OnAimingExit()
    {
        onAiming = false;
    }
}
