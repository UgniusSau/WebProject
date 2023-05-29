using Blazelounge_v2.Data.Models;
using Blazelounge_v2.Services;
using Blazelounge_v2.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blazelounge_v2.Server.Controllers
{
    [ApiController]
    public class Blazelounge_v2Controller : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IShopService _shopService;
        private readonly IConfiguration _configuration;

        public Blazelounge_v2Controller(IUserService userService, IShopService shopService, IConfiguration configuration)
        {
            _userService = userService;
            _shopService = shopService;
            _configuration = configuration;
        }

        //----------------------------------------------------------------------------------------//
        //----------------------------------------- USER -----------------------------------------//
        //----------------------------------------------------------------------------------------//

        [HttpGet("api/blaze-lounge-users-db")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            var result = await _userService.GetUsers().ToListAsync();

            if (!result.Any())
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpGet("api/blaze-lounge/user-UUID/{uuid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<User>> GetUserByUUID(string uuid)
        {
            var user = await _userService.GetUserByUUID(uuid);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpGet("api/blaze-lounge/user-name/{name}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<User>> GetUserByName(string name)
        {
            var user = await _userService.GetUserByName(name);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpGet("api/blaze-lounge-profile/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Profile>> GetUserProfile(string id)
        {
            var profile = await _userService.GetUserProfile(id);

            if (profile == null)
            {
                return NotFound();
            }

            return Ok(profile);
        }

        [HttpPost("api/blaze-lounge-user/register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<ActionResult> AddUser(RegisterModel registerModel)
        {
            try
            {
                await _userService.AddUser(registerModel);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to create user: {ex.Message}");
            }
        }

        [HttpPost("api/blaze-lounge-user/login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<string>> AuthenticateUser(LoginModel loginModel)
        {
            try
            {
                var tokenKey = _configuration.GetSection("AppSettings:Token").Value;
                var response = await _userService.AuthenticateUser(loginModel, tokenKey!);

                if (response != null)
                {
                    return Ok(response);
                }

                return BadRequest();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to login user: {ex.Message}");
            }
        }

        [HttpPost("api/blaze-lounge-user/change-pass")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<bool>> ChangePassword(ChangePasswordModel changePasswordModel)
        {
            try
            {
                if (await _userService.ChangePassword(changePasswordModel))
                {
                    return Ok(true);
                }

                return BadRequest(false);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to change password: {ex.Message}");
            }

        }

        [HttpPost("api/blaze-lounge-user/change-balance")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<string>> ChangeBalance(UniversalGameModel gameModel, string? game = null)
        {
            try
            {
                var ammount = await _userService.ChangeBalance(gameModel, game);

                if (ammount != null)
                {
                    return Ok(ammount);
                }

                return BadRequest("Error occurred while changing balance");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to change balance: {ex.Message}");
            }
        }

        [HttpPost("api/blaze-lounge-user/check-balance")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<bool>> CheckBalance(UniversalGameModel gameModel)
        {
            try
            {
                bool hasEnough = await _userService.CheckBalance(gameModel);

                if (hasEnough)
                {
                    return Ok(true);
                }

                return BadRequest(false);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to check balance: {ex.Message}");
            }
        }

        [HttpGet("api/blaze-lounge-shop/get-user-items/{username}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<Item>>> GetUserItems(string username)
        {
            var result = await _userService.GetUserItems(username);

            if (!result.Any())
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost("api/blaze-lounge-shop/change-user-color/{item}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<bool>> ChangeUserColor(string username, Item item)
        {
            var result = await _userService.ChangeUserColor(username, item);

            if (!result)
            {
                return NotFound(result);
            }

            return Ok(result);
        }

        [HttpGet("api/blaze-lounge/get-user-game-stats/{username}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<GamesStat>>> GetUserGameStats(string username)
        {
            var result = await _userService.GetUserGamesStats(username);

            if (!result.Any())
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost("api/blaze-lounge/update-user-game-stats")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<bool>> UpdateUserGameStats(UniversalGameModel model)
        {
            var result = await _userService.UpdateUserGamesStats(model);

            if (!result)
            {
                return NotFound();
            }

            return Ok(result);
        }

        //----------------------------------------------------------------------------------------//
        //-------------------------------------- Daily wheel -------------------------------------//
        //----------------------------------------------------------------------------------------//

        [HttpGet("api/lucky-wheel/can-spin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<bool>> CanSpin(string userName)
        {
            DateTime? lastSpin = await _userService.GetSpinTime(userName);

            if (lastSpin == null)
            {
                // No previous spin
                return Ok(true);
            }
            else
            {
                return Ok(DateTime.UtcNow - lastSpin >= TimeSpan.FromHours(24));
            }
        }

        [HttpGet("api/lucky-wheel/spin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<int>> Spin(string userName)
        {
            var response = await _userService.GetSpinTime(userName);
            return response;
        }

        //----------------------------------------------------------------------------------------//
        //----------------------------------------- SHOP -----------------------------------------//
        //----------------------------------------------------------------------------------------//

        [HttpGet("api/blaze-lounge-shop/get-shop-items")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<Item>>> GetShopItems()
        {
            var result = await _shopService.GetShopItems().ToListAsync();

            if (!result.Any())
            {
                return NotFound();
            }
            
            return Ok(result);
        }

        [HttpGet("api/blaze-lounge/shop/item/{itemId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Item>> GetItemById(string itemId)
        {
            var item = await _shopService.GetItemById(itemId);

            if (item == null)
            {
                return NotFound();
            }

            return Ok(item);
        }

        [HttpPost("api/blaze-lounge/shop/item/{itemId}/buy")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<string>> BuyItem(string itemId, string username)
        {
            try
            {
                if (await _shopService.BuyItem(itemId, username))
                {
                    return Ok();
                }

                return BadRequest();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to buy item: {ex.Message}");
            }
        }
    }
}