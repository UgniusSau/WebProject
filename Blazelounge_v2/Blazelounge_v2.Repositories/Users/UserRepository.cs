using Blazelounge_v2.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Blazelounge_v2.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DbContext2 _context;

        public UserRepository(DbContext2 context)
        {
            _context = context;
        }

        // user ======================================================================
        public IQueryable<User> GetUsers()
        {
            return _context.Users;
        }

        public async Task<User> GetUserByUUID(string uuid)
        {
            return await _context.Users.SingleOrDefaultAsync(u => u.Uuid == uuid);
        }

        public async Task<User> GetUserByName(string username)
        {
            return await _context.Users.SingleOrDefaultAsync(u => u.Username == username);
        }

        public async Task AddUserDB(User user)
        {

            if (await _context.Users.AnyAsync(u => u.Username == user.Username || u.Email == user.Email))
            {
                throw new ArgumentException("User with this username or email already exists.");
            }

            var userProfile = new Profile
            {
                Currency = 0, // Set default values for UserProfile fields
                BaseNameColor = "",
                BaseBgColor = ""
            };

            await _context.Profiles.AddAsync(userProfile);
            await _context.SaveChangesAsync();

            user.FkProfileidProfile = userProfile.IdProfile; // Set foreign key value for User record
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

        }

        public async Task<bool> AuthenticateUser(string username, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);

            if (user != null)
            {
                // Compare the hashed password in the database with the hash of the password passed to the method
                if (BCrypt.Net.BCrypt.Verify(password, user.PasswordHashed))
                {
                    // Authentication successful
                    return true;
                }
            }

            // Authentication failed
            return false;
        }


        // profile ==========================================================================================
        public async Task<Profile> GetUserProfile(string id)
        {
            return await _context.Profiles.SingleOrDefaultAsync(p => p.IdProfile == long.Parse(id));
        }

        public async Task<bool> ChangePassword(ChangePasswordModel changePasswordModel)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == changePasswordModel.Username);
            if (user != null)
            {
                if (BCrypt.Net.BCrypt.Verify(changePasswordModel.CurrentPassword, user.PasswordHashed))
                {
                    user.PasswordHashed = changePasswordModel.NewPassword;
                    await _context.SaveChangesAsync();
                    return true;
                }
            }
            return false;
        }

        public async Task<bool> ChangeBalance(string username, bool addBalance, double amount, string? game)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user != null)
            {
                var profile = await _context.Profiles.SingleOrDefaultAsync(p => p.IdProfile == user.FkProfileidProfile);

                if (profile != null)
                {
                    if (game == null)
                    {
                        profile.Currency = addBalance ? profile.Currency += 2 * amount : profile.Currency -= amount;
                    }
                    else
                    {
                        profile.Currency = addBalance ? profile.Currency += amount : profile.Currency -= amount;
                    }
                    await _context.SaveChangesAsync();
                    return true;
                }
            }

            return false;
        }

        public async Task<DateTime?> GetSpinTime(string username)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user != null)
            {
                var lastSpin = await _context.DailySpins.SingleOrDefaultAsync(p => p.UUID == user.Uuid);

                if (lastSpin != null)
                {
                    return lastSpin.SpinTime;
                }
            }

            return null;
        }

        public async Task<bool> UpdateSpin(string username)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user != null)
            {
                var lastSpin = await _context.DailySpins.SingleOrDefaultAsync(p => p.UUID == user.Uuid);

                if (lastSpin != null)
                {
                    lastSpin.SpinTime = DateTime.UtcNow;
                    await _context.SaveChangesAsync();
                    return true;
                }
                else
                {
                    await _context.DailySpins.AddAsync(new DailySpin { UUID = user.Uuid, SpinTime = DateTime.UtcNow });
                    await _context.SaveChangesAsync();
                    return true;
                }
            }

            return false;
        }

        public async Task<bool> CheckBalance(string username, double amount)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user != null)
            {
                var profile = await _context.Profiles.SingleOrDefaultAsync(p => p.IdProfile == user.FkProfileidProfile);

                if (profile != null)
                {
                    if (profile.Currency - amount >= 0)
                        return true;

                }
            }

            return false;
        }

        public async Task<IQueryable<Item>> GetUserItems(string username)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            var profile = user != null ? await _context.Profiles.SingleOrDefaultAsync(p => p.IdProfile == user.FkProfileidProfile) : null;
            var inventory = profile != null ? await _context.Inventories.SingleOrDefaultAsync(i => i.FkProfileidProfile == profile.IdProfile) : null;
            var inventoryItems = inventory != null ? await _context.InventoryItems.Where(ii => ii.FkInventoryidInventory == inventory.IdInventory).Select(ii => ii.FkItemidItem).ToListAsync() : null;
            var items = inventoryItems != null ? await _context.Items.Where(it => inventoryItems.Contains(it.IdItem)).ToListAsync() : null;

            return items?.AsQueryable() ?? Enumerable.Empty<Item>().AsQueryable();
        }

        public async Task<bool> ChangeUserColor(string username, Item item)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            var profile = user != null ? await _context.Profiles.SingleOrDefaultAsync(p => p.IdProfile == user.FkProfileidProfile) : null;

            if (profile != null && item != null)
            {
                if (item.Type == 2)
                {
                    var nameColorItem = await _context.NameColors.FirstOrDefaultAsync(i => i.FkItemidItem == item.IdItem);
                    profile.BaseNameColor = nameColorItem != null ? nameColorItem.RgbValue : profile.BaseNameColor;
                    await _context.SaveChangesAsync();
                    return true;
                }

                if (item.Type == 3)
                {
                    var backgroundColorItem = await _context.BackgroundColors.FirstOrDefaultAsync(i => i.FkItemidItem == item.IdItem);
                    profile.BaseBgColor = backgroundColorItem != null ? backgroundColorItem.RgbValue : profile.BaseBgColor;
                    await _context.SaveChangesAsync();
                    return true;
                }
            }
            return false;
        }

        public async Task<IQueryable<GamesStat>> GetUserGamesStats(string username)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            var profile = user != null ? await _context.Profiles.SingleOrDefaultAsync(p => p.IdProfile == user.FkProfileidProfile) : null;
            var userStatistics = profile != null ? await _context.Statistics.SingleOrDefaultAsync(us => us.FkProfileidProfile == profile.IdProfile) : null;
            var gamesStats = userStatistics != null ? await _context.GamesStats.Where(gs => gs.FkStatisticsidStatistics == userStatistics.IdStatistics).ToListAsync() : null;
            return gamesStats?.AsQueryable() ?? Enumerable.Empty<GamesStat>().AsQueryable();
        }

        public async Task<bool> UpdateUserGamesStats(string username, string gameName, bool won, double amount)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            var profile = user != null ? await _context.Profiles.SingleOrDefaultAsync(p => p.IdProfile == user.FkProfileidProfile) : null;
            var userStatistics = profile != null ? await _context.Statistics.SingleOrDefaultAsync(us => us.FkProfileidProfile == profile.IdProfile) : null;

            if (userStatistics is null && profile is not null)
            {
                userStatistics = new Statistic { FkProfileidProfile = profile.IdProfile };
                await _context.Statistics.AddAsync(userStatistics);
                await _context.SaveChangesAsync();
            }

            var stat = userStatistics != null ? await _context.GamesStats.SingleOrDefaultAsync(gs => gs.FkStatisticsidStatistics == userStatistics.IdStatistics && gs.Game == gameName) : null;

            if (stat is null && userStatistics is not null)
            {
                stat = new GamesStat
                {
                    Game = gameName,
                    TimesPlayed = 1,
                    Wins = 0,
                    Loses = 0,
                    CurrencyGained = 0,
                    FkStatisticsidStatistics = userStatistics.IdStatistics
                };
                if (won)
                {
                    stat.Wins++;
                    stat.CurrencyGained += amount;
                }
                else
                {
                    stat.Loses++;
                    stat.CurrencyGained -= amount;
                }

                await _context.GamesStats.AddAsync(stat);
                await _context.SaveChangesAsync();
                return true;
            }
            else if (stat is not null)
            {
                stat.TimesPlayed++;
                if (won)
                {
                    stat.Wins++;
                    stat.CurrencyGained += amount;
                }
                else
                {
                    stat.Loses++;
                    stat.CurrencyGained -= amount;
                }

                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
