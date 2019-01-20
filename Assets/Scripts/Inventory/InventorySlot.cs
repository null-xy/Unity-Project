using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class InventorySlot : MonoBehaviour,IPointerClickHandler,IPointerEnterHandler,IPointerExitHandler {
    //ItemTooltip tooltip;
    public Text itemName;
    public Button itemBoxImage;
    public Image itemEquipImage;

    public event Action<InventorySlot> OnRightClickEvent;
    public event Action<InventorySlot> OnPointerEnterEvent;
    public event Action<InventorySlot> OnPointerExitEvent;

    private Item _item;
    public Item item
    {
        get { return _item; }
        set
        {
            _item = value;
            if (_item == null && amount != 0) amount = 0;
            if (_item == null) {
                ClearSlot();
            } else {
                AddItem(_item);
            }
        }
    }
    //bool selected;
    private int _amount;
    public int amount
    {
        get { return _amount; }
        set { _amount = value;
            if (_amount > 1) {
                itemName.text = item.name + " " + "(" + _amount + ")";
            } else if (_amount == 1) { itemName.text = item.name; }
            else if (_amount < 0) _amount = 0;
            else if (_amount == 0 && item != null) item = null;
        }
    }

    protected virtual void OnValidate()
    {
        //if (tooltip == null)
        //    tooltip = FindObjectOfType<ItemTooltip>();
        //tooltip.HideTooltip();
        item = _item;
        amount = _amount;
    }
    public void EquipItem()
    {
        itemEquipImage.enabled = true;
    }
    public void UnEquipItem()
    {
        itemEquipImage.enabled = false;
    }
    public void AddItem(Item newItem)
    {
        itemName.enabled = true;
        itemBoxImage.interactable = true;
        itemBoxImage.image.color = Color.clear;
    }

    public void ClearSlot()
    {
        itemName.text = null;
        itemName.enabled = false;
        itemBoxImage.interactable = false;
        itemEquipImage.enabled = false;
    }
    public void OnRemoveButton()
    {
        MeshRenderer newMesh = Instantiate<MeshRenderer>(item.meshInScene);
        newMesh.name = item.name;
        newMesh.gameObject.AddComponent<Rigidbody>();
        MeshCollider newMeshCollider=newMesh.gameObject.AddComponent<MeshCollider>();
        newMeshCollider.convex=true;
        ItemPickUp dropItem =newMesh.gameObject.AddComponent<ItemPickUp>();
        dropItem.item = item;

        newMesh.transform.position = PlayerManager.instance.player.transform.position + new Vector3(1.3f, 0, 0);
        newMesh.transform.rotation = Quaternion.identity; 
        
        Inventory.instance.Remove(item);
    }
    public void UseItem()
    {
        if (item != null)
        {
            if (!itemEquipImage.enabled)
            {
                item.Use();
            }else if (itemEquipImage.enabled)
            {
                item.UnEquip();
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //throw new NotImplementedException();
        if (eventData != null && eventData.button == PointerEventData.InputButton.Right)
        {
            if (OnRightClickEvent != null)
                OnRightClickEvent(this);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (OnPointerEnterEvent != null)
            OnPointerEnterEvent(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (OnPointerExitEvent != null)
            OnPointerExitEvent(this);
    }
}
