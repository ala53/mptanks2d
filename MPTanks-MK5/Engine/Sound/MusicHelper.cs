using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Engine.Sound
{
    public static class MusicHelper
    {
        public static void PlaySongs(GameCore game, params string[] songAssets)
        {
            PlaySongs(game, songAssets, null);
        }

        private static void PlaySongs(GameCore game, string[] songAssets, string previous = null)
        {
            var song = Helpers.BasicHelpers.ChooseRandom(songAssets);
            while (previous == song && songAssets.Length > 1)
                song = Helpers.BasicHelpers.ChooseRandom(songAssets);

            game.SoundEngine.BackgroundSong =
                new Sound(game.SoundEngine, song)
                {
                    CompletionCallback = (s) => PlaySongs(game, songAssets, previous),
                    LoopCount = 0,
                    Volume = 1
                };
        }
    }
}
