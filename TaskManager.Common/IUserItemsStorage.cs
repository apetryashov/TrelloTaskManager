namespace TaskManager.Common
{
    public interface IUserItemsStorage<TItem>
    {
        void Set(long id, TItem item);
        TItem Get(long id);
        void Delete(long id);
        bool Has(long id);
    }
}