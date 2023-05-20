using Microsoft.AspNetCore.SignalR;
using BlazeLounge.Data.Models;
using System.Threading.Tasks;
using System.Timers;
using BlazeLounge.Services.Crash;
using Microsoft.EntityFrameworkCore;

namespace BlazeLounge.Server.Hubs
{
    public class CrashHub : Hub
    {
        private static System.Timers.Timer _timer = new System.Timers.Timer(Interval);
        private static CrashGameStateService _gameStateService;
        private static IHubContext<CrashHub> _hubContext;

        private const int Interval = 10;

        static CrashHub()
        {
            _timer.Elapsed += TimerElapsed;
        }
        public CrashHub(IHubContext<CrashHub> hubContext, CrashGameStateService gameStateService)
        {
            _hubContext = hubContext;
            _gameStateService = gameStateService;
            _timer.AutoReset = true;
            _timer.Start();
        }

        private static void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            Update();
            _hubContext.Clients.All.SendAsync("UpdateGameState", _gameStateService.GameState);
        }

        private static void Update()
        {
            var gameState = _gameStateService.GameState;

            if (gameState.CurrentStage == CrashGameState.Stage.BETTING)
            {
                gameState.Countdown -= TimeSpan.FromMilliseconds(Interval);

                if (gameState.Countdown <= TimeSpan.Zero)
                {
                    _hubContext.Clients.All.SendAsync("StageChange", CrashGameState.Stage.IN_PROGRESS);
                    gameState.CurrentStage = CrashGameState.Stage.IN_PROGRESS;
                    gameState.CrashingPoint = (float)(Random.Shared.NextDouble() * 15 + 1);
                    gameState.CrashValue = 1;
                }
            }
            else if (gameState.CurrentStage == CrashGameState.Stage.IN_PROGRESS)
            {
                var distanceToCrashPoint = gameState.CrashingPoint - gameState.CrashValue;
                var speed = 0.01f + (0.01f * distanceToCrashPoint / 50);

                gameState.CrashValue += speed;

                if (gameState.CrashValue >= gameState.CrashingPoint)
                {
                    _hubContext.Clients.All.SendAsync("StageChange", CrashGameState.Stage.CRASHED);
                    gameState.CurrentStage = CrashGameState.Stage.CRASHED;
                    gameState.Countdown = TimeSpan.FromSeconds(3);
                }
            }
            else if (gameState.CurrentStage == CrashGameState.Stage.CRASHED)
            {
                gameState.Countdown -= TimeSpan.FromMilliseconds(Interval);  // ?? kazkas fronte padaryta, kad laikas desyncinasi

                if (gameState.Countdown <= TimeSpan.Zero)
                {
                    _hubContext.Clients.All.SendAsync("StageChange", CrashGameState.Stage.BETTING);
                    gameState.CurrentStage = CrashGameState.Stage.BETTING;
                    gameState.Countdown = TimeSpan.FromSeconds(5);
                }
            }
        }


        public override Task OnDisconnectedAsync(Exception? exception)
        {
            _timer.Stop();
            return base.OnDisconnectedAsync(exception);
        }
    }
}