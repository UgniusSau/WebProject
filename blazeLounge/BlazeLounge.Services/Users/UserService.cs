using BlazeLounge.Data.Models;
using BlazeLounge.Repositories.Users;
using System;
using System.Threading.Tasks;

namespace BlazeLounge.Services
{
    public class UserService : IUserService
    {
        private readonly IDBUserRepository _DBUserRepository;

        public UserService(IDBUserRepository dBUserRepository)
        {
            _DBUserRepository = dBUserRepository;
        }

        public IQueryable<User> GetUsersDB()
        {
            return _DBUserRepository.GetUsersDB();
        }

        public async Task<User> GetUserByUUID(string uuid)
        {
            return await _DBUserRepository.GetUserByUUID(uuid);
        }

        public async Task AddUserDB(User user)
        {
            await _DBUserRepository.AddUserDB(user);
        }

        public async Task<bool> AuthenticateUserDB(string username, string password)
        {
            return await _DBUserRepository.AuthenticateUserDB(username, password);
        }

        public async Task<User> GetUserByName(string username)
        {
            return await _DBUserRepository.GetUserByName(username);
        }

        public async Task<Profile> GetUserProfile(string id)
        {
            return await _DBUserRepository.GetUserProfile(id);
        }

        public async Task<bool> ChangePasswordDB(ChangePasswordModel changePasswordModel)
        {
            return await _DBUserRepository.ChangePasswordDB(changePasswordModel);
        }

		public async Task<bool> ChangeBalance(string username, bool addBalance, double amount, string? game)
		{
			return await _DBUserRepository.ChangeBalance(username, addBalance, amount, game);
		}

		public async Task<bool> CheckBalance(string username, double amount)
		{
			return await _DBUserRepository.CheckBalance(username, amount);
		}
        public Task<IQueryable<Item>> GetUserItems(string username)
        {
            return _DBUserRepository.GetUserItems(username);
        }

        public Task<bool> ChangeUserColor(string username, Item item)
        {
            return _DBUserRepository.ChangeUserColor(username, item);
        }
    

        public async Task<DateTime?> GetSpinTime(string username)
        {
            return await _DBUserRepository.GetSpinTime(username);
        }

        public async Task<bool> UpdateSpin(string username)
        {
            return await _DBUserRepository.UpdateSpin(username);
        }

        public async Task<IQueryable<GamesStat>> GetUserGamesStats(string username)
        {
            return await _DBUserRepository.GetUserGamesStats(username);
        }

        public async Task<bool> UpdateUserGamesStats(string username, string gameName, bool won, double amount)
        {
            return await _DBUserRepository.UpdateUserGamesStats(username, gameName, won, amount);
        }
    }
}
