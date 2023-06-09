﻿using Blazelounge_v2.Data.Models;

namespace Blazelounge_v2.Repositories
{
    public interface IUserRepository
    {
        IQueryable<User> GetUsers();

        Task<User> GetUserByUUID(string uuid);

        Task<User> GetUserByName(string username);

        Task<bool> AuthenticateUser(string username, string password);

        Task AddUserDB(User user);

        Task<bool> UpdateUserGamesStats(string username, string gameName, bool won, double amount);

        Task<Profile> GetUserProfile(string id);

        Task<bool> ChangePassword(ChangePasswordModel changePasswordModel);

		Task<bool> ChangeBalance(string username, bool addBalance, double amount, string? game);

		Task<bool> CheckBalance(string username, double amount);

        Task<IQueryable<Item>> GetUserItems(string username);

        Task<bool> ChangeUserColor(string username, Item item);

        Task<DateTime?> GetSpinTime(string username);

        Task<bool> UpdateSpin(string username);

        Task<IQueryable<GamesStat>> GetUserGamesStats(string username);
    }
}
