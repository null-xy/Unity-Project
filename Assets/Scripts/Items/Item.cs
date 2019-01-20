using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName ="New Item", menuName = "inventory/Item")]
public class Item : ScriptableObject {
    [SerializeField] string id;
    public string ID { get { return id; } }
    new public string name = "New Item";
    public Sprite icon = null;
    public string descrip = null;
    [Range(1, 999)]
    public int MaximumStack = 999;
    public MeshRenderer meshInScene;
    public bool isDefaultItem = false;
    //public int stackNum = 1;

    private void OnValidate()
    {
        string path = AssetDatabase.GetAssetPath(this);
        id = AssetDatabase.AssetPathToGUID(path);
    }

    public virtual Item GetCopy()
    {
        return this;
    }
    public virtual void Destroy()
    {

    }

    public virtual void Use()
    {
        Debug.Log("Using" + name);
    }
    public void RemoveFromInventory()
    {
        Inventory.instance.Remove(this);
    }
    public virtual void UnEquip()
    {

    }
}
