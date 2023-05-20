using BlazeLounge.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazeLounge.Repositories.Users
{
    public interface IDBUserRepository
    {
        // user =======================================================
        IQueryable<User> GetUsersDB();
        Task<User> GetUserByUUID(string uuid);
        Task<User> GetUserByName(string username);
        Task<bool> AuthenticateUserDB(string username, string password);
        Task AddUserDB(User user);
        Task<bool> UpdateUserGamesStats(string username, string gameName, bool won, double amount);

        // profile ====================================================
        Task<Profile> GetUserProfile(string id);
        Task<bool> ChangePasswordDB(ChangePasswordModel changePasswordModel);
		Task<bool> ChangeBalance(string username, bool addBalance, double amount, string? game);
		Task<bool> CheckBalance(string username, double amount);
        Task<IQueryable<Item>> GetUserItems(string username);
        Task<bool> ChangeUserColor(string username, Item item);
        Task<DateTime?> GetSpinTime(string username);
        Task<bool> UpdateSpin(string username);
        Task<IQueryable<GamesStat>> GetUserGamesStats(string username);
    }
}
