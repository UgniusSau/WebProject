using Blazelounge_v2.Data.Models;

namespace Blazelounge_v2.Repositories
{
    public interface IShopRepository
    {
        IQueryable<Item> GetShopItems();

        Task<Item> GetItemById(string id);

        Task<bool> BuyItem(string id, string username);
    }
}
