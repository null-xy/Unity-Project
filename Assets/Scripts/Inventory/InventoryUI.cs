using UnityEngine;


public class InventoryUI : MonoBehaviour {

    public GameObject inventoryUI;
    Inventory inventory;

    InputManager playerInput;
    [SerializeField]
    ItemTooltip itemTooltip;
    private InventorySlot selectedSlot;



    // public delegate void InventoryClosing();
    //public static event InventoryClosing inventoryClosingComplete;
    private void OnValidate()
    {
        if (itemTooltip == null)
            itemTooltip = FindObjectOfType<ItemTooltip>();
    }
    void Awake()
    {
        playerInput = GameManager.Instance.InputManager;
    }
	void Start () {
        inventory=Inventory.instance;
        //inventory.onItemChangedCallBack += UpdateUI;
        inventoryUI.SetActive(false);
        inventory.OnPointerEnterEvent += ShowTooltip;
        inventory.OnPointerExitEvent += HideTooltip;
        WeaponManager.instance.onWeaponChanged += OnWeaponChanged;
        EquipmentManager.instance.onEquipmentChanged += OnEquipmentChanged;
    }
    private void OnWeaponChanged(Weapon newItem, Weapon oldItem)
    {
        if (newItem != null)
        {
            int i = inventory.searchItemSlot(newItem.ID);
            InventorySlot slot = inventory.slots[i];
            slot.EquipItem();
        }
        if (oldItem != null)
        {
            int i = inventory.searchItemSlot(oldItem.ID);
            InventorySlot slot = inventory.slots[i];
            slot.UnEquipItem();
        }
    }
    private void OnEquipmentChanged(Equipment newItem, Equipment oldItem)
    {
        if (newItem != null && !newItem.isDefaultItem)
        {
            int i = inventory.searchItemSlot(newItem.ID);
            InventorySlot slot = inventory.slots[i];
            slot.EquipItem();
        }
        if (oldItem != null&& !oldItem.isDefaultItem)
        {
            int i = inventory.searchItemSlot(oldItem.ID);
            InventorySlot slot = inventory.slots[i];
            slot.UnEquipItem();
        }
    }
    private void ShowTooltip(InventorySlot slot)
    {
        Item item = slot.item as Item;
        selectedSlot = slot;
        if (item != null)
        {
            itemTooltip.ShowTooltip(item);
            slot.itemBoxImage.image.color = new Color(255, 255, 255, 0.1f);
        }else if (item == null)
        {
            slot.itemBoxImage.image.color = Color.clear;
            itemTooltip.HideTooltip();
        }
    }
    private void HideTooltip(InventorySlot slot)
    {
        selectedSlot = null;
        slot.itemBoxImage.image.color = Color.clear;
        itemTooltip.HideTooltip();
    }
    // Update is called once per frame
    void Update () {
        inventory.AutoOrganize();

            if (!playerInput.InventoryShow)
            {
                CloseInventory();
            }
            else if(playerInput.InventoryShow)
            {
                OpenInventory();
            }
            
        if (selectedSlot != null)
        {
            ShowTooltip(selectedSlot);
            if(selectedSlot.item != null && playerInput.DropItem)
            {
                selectedSlot.OnRemoveButton();
            }
        }
	}

    void OpenInventory()
    {
        inventoryUI.SetActive(true);
    }
    void CloseInventory()
    {
        //int x = Screen.width / 2;
        //int y = Screen.height / 2;
        //SetCursorPos(x, y);
        inventoryUI.SetActive(false);
        if(selectedSlot!=null)
            HideTooltip(selectedSlot);
    }
    void UpdateUI()
    {
        //Debug.Log("Updating UI");
        /*for(int i=0; i < slots.Length; i++)
        {
            if (i < inventory.items.Count)
            {
                //slots[i].Amount++;
                slots[i].AddItem(inventory.items[i]);
            }
            else
            {
                slots[i].ClearSlot();
            }
        }*/
        /*int i = 0;
        for (; i < inventory.items.Count && i < slots.Length; i++)
        {
            slots[i].item = inventory.items[i];
            //slots[i].AddItem(inventory.items[i]);
        }
        for (; i < slots.Length; i++)
        {
            slots[i].item = null;
            //slots[i].ClearSlot();
        }*/
    }
}
