using Microsoft.Xna.Framework.Input;
using MPTanks.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Client.Backend.Sound
{
    public class MusicHelper
    {
        private SoundInstance _active;
        private Engine.Sound.Sound _activeSound;
        private GameCore _game;
        private SoundPlayer _player;
        public MusicHelper(GameCore game, SoundPlayer player)
        {
            _player = player;
            _game = game;
            game.SoundEngine.OnBackgroundSongChanged += SoundEngine_OnBackgroundSongChanged;
        }

        private void SoundEngine_OnBackgroundSongChanged(object sender, Engine.Sound.Sound e)
        {
            if (_activeSound != null && !_active.HasEnded)
            {
                var asnd = _activeSound;
                _activeSound = null;
                asnd.CompletionCallback(_activeSound);
            }

            if (_active != null)
                _active.End();

            _activeSound = e;

            _active = _player.Cache.GetSound(e.AssetName).Play(SoundPlayer.ChannelGroup.Background);
            _active.Ended = (o) =>
            {
                if (_activeSound.CompletionCallback != null)
                    _activeSound.CompletionCallback(_activeSound);
            };
            _active.LoopCount = e.LoopCount;
            _active.Pitch = e.Pitch;
            _active.Playing = e.Playing;
            _active.Position = _player.PlayerPosition;
            _active.Velocity = _player.PlayerVelocity;
            _active.Time = e.Time;
            _active.Timescale = (float)_game.Timescale.Fractional * _activeSound.Timescale;
            _active.Volume = e.Volume;
        }

        public void Update()
        {
            if (_activeSound != null && _active != null && !_active.HasEnded)
            {
                _active.LoopCount = _activeSound.LoopCount;
                _active.Pitch = _activeSound.Pitch;
                _active.Playing = _activeSound.Playing;
                _active.Position = _player.PlayerPosition;
                _active.Velocity = _player.PlayerVelocity;
                if (_activeSound.TimeDirty)
                    _active.Time = _activeSound.Time;
                _activeSound.UnsafeSetTime(_active.Time);
                _active.Timescale = (float)_game.Timescale.Fractional * _activeSound.Timescale;
                _active.Volume = (float)Math.Pow(_activeSound.Volume, 2);
            }
        }
    }
}
