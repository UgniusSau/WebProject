using BlazeLounge.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazeLounge.Repositories.Shop
{
    public interface IShopRepository
    {
        IQueryable<Item> GetShopItems();
        Task<Item> GetItemById(string id);
        Task<bool> BuyItem(string id, string username);
    }
}
