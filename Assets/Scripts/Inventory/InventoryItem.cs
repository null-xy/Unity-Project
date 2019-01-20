public interface IInventoryItem
{
    int ItemCount(string itemID);
    //bool ContainsItem(string itemID);
    Item RemoveItem(string itemID);
    bool Remove(Item item);
    bool Add(Item item);
    bool CanAddItem(Item item,int Amount=1);
    void Clear();
    //bool IsFull();
}