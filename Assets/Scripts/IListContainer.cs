public interface IListContainer<T> where T : IContainerItem
{
    void AddItem(T item);
    void RemoveItem(T item);
}

public interface IContainerItem
{
    void SetContainer<T>(IListContainer<T> container) where T : IContainerItem;
    void RemoveSelf();
}