using System.Collections.Generic;

namespace TaskManager.Common
{
    public interface IUserItemsStorage<TItem>
    {
        void Set(long id, TItem item);
        TItem Get(long id);
        void Delete(long id);
        bool Has(long id);
        IEnumerable<(long id, TItem item)> GetAllItems();
    }
}