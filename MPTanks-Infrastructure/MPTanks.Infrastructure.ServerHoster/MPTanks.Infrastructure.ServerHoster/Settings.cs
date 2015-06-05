using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Infrastructure.ServerHoster
{
    public static class Settings
    {
        const int ParallelVpsCount = 4;
        const int MaxOversubscription = 2; //Book double the resources

        /// <summary>
        /// The maximum number of games that can be run in parallel on the server
        /// </summary>
        public static int MaxParallelGamesCount { get; set; } = 100 / ParallelVpsCount;
        //100 servers, average of 8 players = 800 players
        //each player uses ~50kbps
        //= approx. 120 mbps or 15 mb/sec
        //+ mod downloads, overhead, logging
        //so the server will probably use 200mpbs
        //which is about 65 tb/month

        /// <summary>
        /// The maximum memory usage allowed for each game. If (outside of the grace period), the
        /// CPU usage is higher than this, the app will force a GC and then, if it's still too high,
        /// it will kill the application.
        /// </summary>
        public static long MaxMemoryUsageBytesPerGame { get; set; } = (16384L * MaxOversubscription / ParallelVpsCount) * 1024L * 1024L / MaxParallelGamesCount;
        // 16gb over number of games
        //We use 16gb servers
        //segmented into a few VPSs

        /// <summary>
        /// The absolute maximum cpu usage allowed per game (excluding the grace period).
        /// If at any point, for more than a second, the usage is larger than this, the
        /// game will be terminated.
        /// The units are 1 unit = 1 CPU for 1 second
        /// </summary>
        public static double MaxCPUUsagePerGame { get; set; } = 1;
        //We let them spike and use a full CPU, but no more.

        /// <summary>
        /// The maximum cpu usage the game is allowed to continuously consume (averaged over 1 minute intervals).
        /// If at any point, for more than a minute, the CPU usage is higher than this, the application will be terminated.
        /// The units are 1 unit = 1 CPU for 1 minute
        /// </summary>
        public static double MaxAverageCPUUsagePerGame { get; set; } = Environment.ProcessorCount * MaxOversubscription / MaxParallelGamesCount;
        //8 threads, so we limit the average to about 8% cpu per server
    }
}
