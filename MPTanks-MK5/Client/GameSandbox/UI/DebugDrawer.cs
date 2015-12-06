using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MPTanks.Engine;
using MPTanks.Networking.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Client.GameSandbox.UI
{
    class DebugDrawer : IDisposable
    {
        private Networking.Client.NetClient _netClient;
        private GameClient _client;
        private GameCore _game => _netClient.Game;
        private GamePlayer _player => _netClient.Player;
        private SpriteBatch _spriteBatch;
        private SpriteFont _debugFont;
        private Texture2D _graphTexture;
        public DebugDrawer(GameClient client, Networking.Client.NetClient netclient)
        {
            _client = client;
            _netClient = netclient;
            _spriteBatch = new SpriteBatch(client.GraphicsDevice);
            _debugFont = client.Content.Load<SpriteFont>("font");
            _graphTexture = new Texture2D(client.GraphicsDevice, 1, 1);
            _graphTexture.SetData(new[] { Color.White });
        }

        #region Debug info

        public bool DebugEnabled { get; set; } = false;
        public bool DrawTextDebug { get; set; } = true;
        public bool DrawGraphDebug { get; set; } = true;
        public bool DebugOverlayGraphsVertical { get; set; } = true;
        public void DrawDebugInfo(GameTime gameTime)
        {
            LogDebugInfo(gameTime);
            if (!DebugEnabled) return;
            if (DrawTextDebug) DrawTextDebugInfo(gameTime);
            if (DrawGraphDebug) DrawGraphDebugInfo(gameTime);
        }

        #region Data Logging
        private float[] _debugFrameTimes;
        private DebugMemoryUsageTick[] _debugMemoryUsages;
        private Stopwatch _frameTimesTimer = Stopwatch.StartNew();
        private void LogDebugInfo(GameTime gameTime)
        {
            if (_debugFrameTimes == null)
            {
                _debugFrameTimes = new float[graphWidth];
                for (var i = 0; i < _debugFrameTimes.Length; i++)
                    _debugFrameTimes[i] = 16.666666666666f;
            }

            if (_debugMemoryUsages == null)
                _debugMemoryUsages = new DebugMemoryUsageTick[graphWidth];

            if ((DateTime.Now - _debugMemoryUsages[_debugMemoryUsages.Length - 1].Measured).TotalMilliseconds > 0)
            {
                //Shift back
                for (var i = 1; i < _debugMemoryUsages.Length; i++)
                    _debugMemoryUsages[i - 1] = _debugMemoryUsages[i];
                //Measure
                var gcData = DebugDetectGC();
                _debugMemoryUsages[_debugMemoryUsages.Length - 1] = new DebugMemoryUsageTick
                {
                    Measured = DateTime.Now,
                    BytesUsed = GC.GetTotalMemory(false),
                    HasGen0GC = gcData.Gen0,
                    HasGen1GC = gcData.Gen1,
                    HasGen2GC = gcData.Gen2
                };
            }

            for (var i = 1; i < _debugFrameTimes.Length; i++)
                _debugFrameTimes[i - 1] = _debugFrameTimes[i];

            _debugFrameTimes[_debugFrameTimes.Length - 1] = (float)_frameTimesTimer.Elapsed.TotalMilliseconds;
            _frameTimesTimer.Restart();
        }
        private struct DebugMemoryUsageTick
        {
            public DateTime Measured;
            public long BytesUsed;
            public bool HasGen0GC;
            public bool HasGen1GC;
            public bool HasGen2GC;
        }


        private int _g0Gc = 0;
        private int _g1Gc = 0;
        private int _g2Gc = 0;
        private DebugGCTuple DebugDetectGC()
        {
            var returnValue = new DebugGCTuple();

            if (GC.MaxGeneration >= 0 && GC.CollectionCount(0) != _g0Gc)
                returnValue.Gen0 = true;
            if (GC.MaxGeneration >= 1 && GC.CollectionCount(1) != _g1Gc)
                returnValue.Gen1 = true;
            if (GC.MaxGeneration >= 2 && GC.CollectionCount(2) != _g2Gc)
                returnValue.Gen2 = true;

            if (GC.MaxGeneration >= 0)
                _g0Gc = GC.CollectionCount(0);
            if (GC.MaxGeneration >= 1)
                _g1Gc = GC.CollectionCount(1);
            if (GC.MaxGeneration >= 2)
                _g2Gc = GC.CollectionCount(2);

            return returnValue;
        }
        struct DebugGCTuple
        {
            public bool Gen0;
            public bool Gen1;
            public bool Gen2;
        }
        #endregion

        #region Text Debug
        private Process _prc;
        private StringBuilder _bldr = new StringBuilder(2000);
        private int[] _kbsSent = new int[240];
        private int[] _kbsReceived = new int[240];
        private void DrawTextDebugInfo(GameTime gameTime)
        {
            _bldr.Clear();
            if (_prc == null)
                _prc = Process.GetCurrentProcess();

            _spriteBatch.Begin();
            var tanksCount = 0;
            var projCount = 0;
            var mapObjectCount = 0;
            var otherCount = 0;
            foreach (var obj in _game.GameObjects)
            {
                if (obj.GetType().IsSubclassOf(typeof(MPTanks.Engine.Tanks.Tank)))
                    tanksCount++;
                else if (obj.GetType().IsSubclassOf(typeof(MPTanks.Engine.Projectiles.Projectile)))
                    projCount++;
                else if (obj.GetType().IsSubclassOf(typeof(MPTanks.Engine.Maps.MapObjects.MapObject)))
                    mapObjectCount++;
                else otherCount++;
            }
            //Note: The debug screen generates a bit of garbage so don't try to use it to nail down allocations
            //Disable it first and then see if there's still a problem (rely on the graphs which are allocation free)

            //Mapobject counts
            _bldr.Append("Tanks: ").Append(tanksCount)
            .Append(", Projectiles: ").Append(projCount)
            .Append(", Map Objects: ").Append(mapObjectCount)
            .Append(", Other: ").Append(otherCount)
            .Append(", Total: ").Append(tanksCount + projCount + mapObjectCount + otherCount);

            //FPS monitor
            if (float.IsInfinity(CalculateAverageFPS()) || float.IsNaN(CalculateAverageFPS()))
                _bldr.Append(", FPS: ").Append("Calculation Error").Append(" avg, ");
            else
                _bldr.Append(", FPS: ").Append((CalculateAverageFPS().ToString("N1"))).Append(" avg, ");

            _bldr.Append((1000 / _debugFrameTimes[_debugFrameTimes.Length - 1]).ToString("N1")).Append(" now")
            .Append("\nMouse: ").Append(Mouse.GetState().Position.ToString());

            //Timers, animations, and particles
            _bldr.Append(", Timers: ").Append(_game.TimerFactory.ActiveTimersCount)
            .Append(", Animations: ").Append(_game.AnimationEngine.Animations.Count)
            .Append(", Particles: ").Append(_game.ParticleEngine.LivingParticlesCount);

            //Sound diagnostics
            _bldr.Append("\nSounds (Engine, Backend): ").Append(_game.SoundEngine.SoundCount)
            .Append(", ").Append(_client.SoundPlayer.ActiveSoundCount)
            .Append(", Volumes (Background, Effects, Voice): ")
            .Append((_client.SoundPlayer.BackgroundVolume * 100).ToString("N0")).Append("%")
            .Append(", ").Append((_client.SoundPlayer.EffectVolume * 100).ToString("N0")).Append("%")
            .Append(", ").Append((_client.SoundPlayer.VoiceVolume * 100).ToString("N0")).Append("%");

            //Sound profiling
            var info = _client.SoundPlayer.Diagnostics;
            _bldr.Append("\nSound CPU (DSP, Streaming, Update, Total): ")
            .Append(info.DSPCPU.ToString("N2")).Append("%, ")
            .Append(info.StreamCPU.ToString("N2")).Append("%, ")
            .Append(info.UpdateCPU.ToString("N2")).Append("%, ")
            .Append(info.TotalCPU.ToString("N2")).Append("%");

            for (var i = 0; i < _kbsSent.Length - 1; i++)
            {
                _kbsSent[i] = _kbsSent[i + 1];
                _kbsReceived[i] = _kbsReceived[i + 1];
            }
            _kbsSent[_kbsSent.Length - 1] = _netClient.NetworkClient.Statistics.SentBytes;
            _kbsReceived[_kbsReceived.Length - 1] = _netClient.NetworkClient.Statistics.ReceivedBytes;
            _bldr.Append("\n");
            if (_netClient?.NetworkClient?.ServerConnection != null)
            {
                _bldr.Append("Ping: ")
                    .Append((_netClient.NetworkClient.ServerConnection.AverageRoundtripTime * 1000d).ToString("N1"));
            }
            _bldr.Append("ms, ").Append("Received: ")
                        .Append((_netClient.NetworkClient.Statistics.ReceivedBytes / 1024d).ToString("N1")).Append("kb total, ")
                        .Append(((_kbsReceived.Last() - _kbsReceived[_kbsReceived.Length - 60]) / 1024d).ToString("N3")).Append("kb/s")
                        .Append(", Sent: ")
                        .Append((_netClient.NetworkClient.Statistics.SentBytes / 1024d).ToString("N1")).Append("kb total, ")
                        .Append(((_kbsSent.Last() - _kbsSent[_kbsSent.Length - 60]) / 1024d).ToString("N3")).Append("kb/s");
            //   }

            _bldr.Append("\nMost used: ");

            foreach (var t in _netClient.MessageProcessor.Diagnostics.GetMostUsed(3))
                _bldr.Append(t.Key.Name.Replace("Action", "").Replace("Message", ""))
                    .Append(": ").Append(t.Value).Append(", ");

            //Find memory usage max
            long maxMem = 0;
            foreach (var pt in _debugMemoryUsages)
                if (pt.BytesUsed > maxMem)
                    maxMem = pt.BytesUsed;
            //GC & memory
            _bldr.Append("\nGC (gen 0, 1, 2): ").Append(GC.CollectionCount(0)).Append(" ")
                .Append(GC.CollectionCount(1)).Append(" ").Append(GC.CollectionCount(2))
                .Append(", Memory: ").Append((GC.GetTotalMemory(false) / (1024d * 1024)).ToString("N1")).Append("MB used")
                .Append(", ").Append((maxMem / (1024d * 1024)).ToString("N1")).Append("MB max");

            //Timescale printout
            if (_game.Running)
                _bldr.Append("\nTimescale: " + _game.Timescale.DisplayString);

            //Game status
            _bldr.Append(", Status: ");

            if (!_game.HasStarted)
                _bldr.Append("waiting to start, ");
            if (_game.WaitingForPlayers)
                _bldr.Append("waiting for players, ");
            if (_game.Running)
                _bldr.Append("running, ");
            if (_game.Ended)
                _bldr.Append("ended, ");

            _bldr.Remove(_bldr.Length - 2, 2);

            if (_game.Gamemode.WinningTeam != Engine.Gamemodes.Team.Null)
                _bldr.Append(", Winner: ").Append(_game.Gamemode.WinningTeam.TeamName);

            //And keybind explanation
            _bldr.Append("\nF12: hide\n")
                    .Append("F10: Enable/Disable graphs\n")
                    .Append("F9: Enable/Disable debug text\n")
                    .Append("F8: Switch between vertical and horizontal graphs\n")
                    .Append("ESC: Exit\n");

            _spriteBatch.DrawString(_debugFont, _bldr.ToString(), new Vector2(8, 8), Color.Black);
            _spriteBatch.DrawString(_debugFont, _bldr.ToString(), new Vector2(10, 10), Color.White);
            _spriteBatch.End();
        }
        #endregion
        #region FPS Calculations
        private float CalculateAverageFPS()
        {
            return _debugFrameTimes.Select(a => 1000 / a).Average();
        }

        #endregion

        const int graphHeight = 150;
        const int graphWidth = 400;
        const int graphOffset = 20;
        private void DrawGraphDebugInfo(GameTime gameTime)
        {
            //3 graphs:
            //1) memory usage: capped to max, colored blue. Spikes are colored red
            //2) frame times: capped to 50ms, items above 20ms and below 6ms are highlighted red and yellow respectively
            //above 50 is black
            //3) frame rate: capped to 60fps, 60 is green, 20 is red - inbetween is lerped
            //above 60 is black

            //drawn at bottom left of screen, 150px high
            //1st graph is 0-200 pixels
            //2nd graph is 220-420 pixels
            //3rd graph is 440-660 pixels
            DrawMemoryUsageGraph();
            DrawFrameRateGraph();
        }

        private void DrawMemoryUsageGraph()
        {
            //Memory Usage graph
            var max = 0L;
            var average = 0L;

            var graphPosX = 0;
            var graphBottomY = _client.GraphicsDevice.Viewport.Height;

            for (var i = 0; i < _debugMemoryUsages.Length; i++)
            {
                average += _debugMemoryUsages[i].BytesUsed;
                if (_debugMemoryUsages[i].BytesUsed > max)
                    max = _debugMemoryUsages[i].BytesUsed;
            }

            average /= _debugMemoryUsages.Length;
            var spikeMinimum = average * 1.5d;

            double pixelsPerByteH = (double)graphHeight / max;
            double pixelsPerDataPointWidth = (double)graphWidth / _debugMemoryUsages.Length;

            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied,
                SamplerState.LinearWrap, DepthStencilState.Default, RasterizerState.CullNone);
            int pixelsUsed = 0;
            for (var i = 0; i < _debugMemoryUsages.Length; i++)
            {
                var value = _debugMemoryUsages[i];
                var maxPixels = pixelsPerDataPointWidth * i;
                var width = (int)maxPixels - pixelsUsed;
                if (pixelsUsed < (int)maxPixels)
                {
                    var color = (value.BytesUsed > spikeMinimum) ? Color.Red : Color.Blue;
                    var height = (int)(pixelsPerByteH * value.BytesUsed);
                    _spriteBatch.Draw(_graphTexture, new Rectangle(graphPosX + pixelsUsed,
                        graphBottomY - height, width, height),
                        color);
                }

                //Draw the GC marker
                if (value.HasGen2GC)
                {
                    _spriteBatch.Draw(_graphTexture, new Rectangle(graphPosX + pixelsUsed - 3,
                        graphBottomY - 13, 8, 6), Color.Black);
                    _spriteBatch.Draw(_graphTexture, new Rectangle(graphPosX + pixelsUsed - 2,
                        graphBottomY - 12, 4, 4), Color.Red);
                }
                else if (value.HasGen1GC)
                {
                    _spriteBatch.Draw(_graphTexture, new Rectangle(graphPosX + pixelsUsed - 3,
                        _client.GraphicsDevice.Viewport.Height - 13, 8, 6), Color.Black);
                    _spriteBatch.Draw(_graphTexture, new Rectangle(graphPosX + pixelsUsed - 2,
                        graphBottomY - 12, 4, 4), Color.Yellow);
                }
                else if (value.HasGen0GC)
                {
                    _spriteBatch.Draw(_graphTexture, new Rectangle(graphPosX + pixelsUsed - 3,
                        graphBottomY - 13, 8, 6), Color.Black);
                    _spriteBatch.Draw(_graphTexture, new Rectangle(graphPosX + pixelsUsed - 2,
                        graphBottomY - 12, 4, 4), Color.Green);
                }

                if (pixelsUsed < (int)maxPixels)
                    pixelsUsed += width;
            }
            //Draw the label 10, 10 px from the bottom left
            //Draw the label 10, 10 px from the bottom left
            var size = _debugFont.MeasureString("Memory Usage (managed only)");
            var pos = new Vector2(graphPosX, graphBottomY - size.Y);
            _spriteBatch.DrawString(_debugFont, "Memory Usage (managed only)", pos,
                new Color(Color.Black, 150));
            _spriteBatch.End();
        }
        private void DrawFrameRateGraph()
        {
            double pixelsPerFpsH = (double)graphHeight / 70;
            double pixelsPerDataPointWidth = (double)graphWidth / _debugFrameTimes.Length;

            var graphPosX = graphOffset + graphWidth;
            var graphBottomY = _client.GraphicsDevice.Viewport.Height;
            if (DebugOverlayGraphsVertical)
            {
                graphPosX = 0;
                graphBottomY -= graphOffset + graphHeight;
            }

            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                SamplerState.LinearWrap, DepthStencilState.Default, RasterizerState.CullNone);
            int pixelsUsed = 0;
            for (var i = 0; i < _debugFrameTimes.Length; i++)
            {
                var value = 1000 / _debugFrameTimes[i];
                var maxPixels = pixelsPerDataPointWidth * i;
                var width = (int)maxPixels - pixelsUsed;
                if (pixelsUsed < (int)maxPixels)
                {
                    Color color = Color.Green;

                    if (value < 55 && value >= 30)
                        color = Color.Yellow;
                    else if (value < 30 && value >= 20)
                        color = Color.Orange;
                    else if (value < 20)
                        color = Color.Red;
                    var height = (int)(pixelsPerFpsH * value);
                    if (value > 70)
                    {
                        color = Color.LimeGreen;
                        height = graphHeight;
                    }
                    _spriteBatch.Draw(_graphTexture, new Rectangle(pixelsUsed + graphPosX,
                        graphBottomY - height, width, height),
                        color);
                }

                if (pixelsUsed < (int)maxPixels)
                    pixelsUsed += width;
            }
            //Draw the label 10, 10 px from the bottom left
            var size = _debugFont.MeasureString("FPS: (max 70)");
            var pos = new Vector2(graphPosX, graphBottomY - size.Y);
            _spriteBatch.DrawString(_debugFont, "FPS: (max 70)", pos,
                new Color(Color.Black, 150));
            _spriteBatch.End();
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    _graphTexture.Dispose();
                    _spriteBatch.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~DebugDrawer() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
        #endregion
    }
}
