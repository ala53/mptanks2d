using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Infrastructure.ServerHoster
{
    /// <summary>
    /// The watch dog application to limit memory usage for each game, as well as CPU and bandwidth usage.
    /// If a mod misbehaves, we kill the server.
    /// </summary>
    class Watchdog
    {
        private List<ActiveGame> _games = new List<ActiveGame>();
        private Task _monitorTask;
        private bool _run = true;

        public void Stop()
        {
            _run = false;
            _monitorTask.Wait();
        }

        public void AddGame(ActiveGame game)
        {
            lock (_games)
            {
                if (_games.Contains(game))
                    return;
                _games.Add(game);
            }
        }
        private void RemoveGame(long gameId, bool kill = true)
        {
            lock (_games)
            {
                var game = _games.Where(g => g.Id == gameId).First();
                _games.Remove(game);
                if (kill)
                    game.Unload();
            }
        }
        private void RemoveGame(ActiveGame game, bool kill = true)
        {
            lock (_games)
            {
                _games.Remove(game);
                if (kill)
                    game.Unload();
            }
        }
        public Watchdog()
        {
            _monitorTask = Task.Factory.StartNew(Monitor, TaskCreationOptions.LongRunning);
        }

        private async Task Monitor()
        {
            while (_run)
            {

                await Task.Delay(50);
            }
        }
    }
}
