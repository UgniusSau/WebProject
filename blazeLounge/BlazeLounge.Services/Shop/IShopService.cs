using BlazeLounge.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazeLounge.Services.Shop
{
    public interface IShopService
    {
        IQueryable<Item> GetShopItems();
        Task<Item> GetItemById(string id);
        Task<bool> BuyItem(string id, string username);
    }
}
