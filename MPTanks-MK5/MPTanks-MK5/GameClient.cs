#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using MPTanks_MK5.Rendering;
using Engine.Tanks;
using Engine;
using System.Diagnostics;
using System.Runtime;
#endregion

namespace MPTanks_MK5
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class GameClient : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private GameWorldRenderer renderer;
        private Tank tank;
        private Tank tank2;
        private Engine.GameCore game;
        private float zoom = 2f;
        private SpriteFont font;

        public GameClient()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            GCSettings.LatencyMode = GCLatencyMode.SustainedLowLatency;
            // IsMouseVisible = true;
            // IsFixedTimeStep = false;
            //  graphics.SynchronizeWithVerticalRetrace = false;
            // TargetElapsedTime = TimeSpan.FromMilliseconds(33.333333 * 4);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            game = new Engine.GameCore(new EngineInterface.FileLogger(), new Engine.Gamemodes.TeamDeathMatchGamemode());
            game.Authoritative = true;
            renderer = new GameWorldRenderer(this);
            renderer.SetAnimations(game.AnimationEngine);

            font = Content.Load<SpriteFont>("font");
            
            game.AddGameObject(new Engine.Maps.MapObjects.Building(game, true, new Vector2(50, 50), 33), null, true);
            game.AddGameObject(new Engine.Maps.MapObjects.Building(game, true, new Vector2(150, 30), 33), null, true);
            game.AddGameObject(new Engine.Maps.MapObjects.Building(game, true, new Vector2(30, 80), 33), null, true);

            tank = new BasicTank(Guid.NewGuid(), game, true) { Position = new Vector2(15, 20), ColorMask = Color.Orange };
            tank2 = new BasicTank(Guid.NewGuid(), game, true) { Position = new Vector2(15, 20), ColorMask = Color.Green };
            game.AddGameObject(tank, null, true);
            game.AddGameObject(tank2, null, true);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        private float physicsMs = 0;
        private float renderMs = 0;
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            var sw = Stopwatch.StartNew();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.OemTilde))
            {
                game.RemoveGameObject(tank);
                game.RemoveGameObject(tank2);
                tank = new BasicTank(Guid.NewGuid(), game, true) { Position = new Vector2(15, 20), ColorMask = Color.Orange };
                tank2 = new BasicTank(Guid.NewGuid(), game, true) { Position = new Vector2(15, 20), ColorMask = Color.Green };
                game.AddGameObject(tank, null, true);
                game.AddGameObject(tank2, null, true);
            }


            var iState = new InputState();
            iState.LookDirection = tank.Rotation;

            if (Keyboard.GetState().IsKeyDown(Keys.W))
                iState.MovementSpeed = 1;
            if (Keyboard.GetState().IsKeyDown(Keys.S))
                iState.MovementSpeed = -1;
            if (Keyboard.GetState().IsKeyDown(Keys.A))
                iState.RotationSpeed = -1;
            if (Keyboard.GetState().IsKeyDown(Keys.D))
                iState.RotationSpeed = 1;

            if (Keyboard.GetState().IsKeyDown(Keys.LeftControl))
                iState.FirePressed = true;

            tank.Input(iState);

            var iState2 = new InputState();
            iState2.LookDirection = tank2.Rotation;

            if (Keyboard.GetState().IsKeyDown(Keys.Up))
                iState2.MovementSpeed = 1;
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
                iState2.MovementSpeed = -1;
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
                iState2.RotationSpeed = -1;
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
                iState2.RotationSpeed = 1;


            if (Keyboard.GetState().IsKeyDown(Keys.M))
                iState2.FirePressed = true;

            tank2.Input(iState2);
            
            //Complicated look state calcuation below
            //var screenCenter = new Vector2(GraphicsDevice.Viewport.Bounds.Width / 2, //vertex
            //    GraphicsDevice.Viewport.Bounds.Height / 2);
            //var mousePos = new Vector2(Mouse.GetState().Position.X,  //point a
            //    Mouse.GetState().Position.Y);
            //var ctr = screenCenter - mousePos;
            //iState.LookDirection = (float)-Math.Atan2(ctr.X, ctr.Y);
            LockCursor();

            if (Keyboard.GetState().IsKeyDown(Keys.X))
                zoom += 0.1f;
            if (Keyboard.GetState().IsKeyDown(Keys.Z))
                zoom -= 0.1f;
            game.Update(gameTime);

            //Update the render list if the game has added or removed objects
            if (game.IsDirty)
                renderer.SetObjects(game.GameObjects);

            renderer.SetParticles(game.ParticleEngine.Particles);
            sw.Stop();

            physicsMs = (float)sw.Elapsed.TotalMilliseconds;
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        private void LockCursor()
        {

            const float outAmount = 100;
            var offset = new Vector2(GraphicsDevice.Viewport.Width / 2,
                GraphicsDevice.Viewport.Height / 2);

            var rel = new Vector2(Math.Abs(Mouse.GetState().Position.X - offset.X),
                Math.Abs(Mouse.GetState().Position.Y - offset.Y));
            var relSigned = new Vector2(Mouse.GetState().Position.X - offset.X,
                Mouse.GetState().Position.Y - offset.Y);

            if (rel.X > outAmount || rel.Y > outAmount)
            {
                if (rel.X > rel.Y)
                {
                    var factor = outAmount / rel.X;
                    relSigned *= factor;
                    Mouse.SetPosition((int)(offset.X + relSigned.X), (int)(offset.Y + relSigned.Y));
                }
                else
                {
                    var factor = outAmount / rel.Y;
                    relSigned *= factor;
                    Mouse.SetPosition((int)(offset.X + relSigned.X), (int)(offset.Y + relSigned.Y));
                }
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            var sw = Stopwatch.StartNew();
            GraphicsDevice.Clear(Color.Black);

            RectangleF drawRect;
            drawRect = new RectangleF(
               0, 0,// tank.Position.X - (15 * zoom),
                //tank.Position.Y - (10 * zoom),
                30 * zoom,
                20 * zoom);

            if (Keyboard.GetState().IsKeyDown(Keys.RightControl))
                drawRect = new RectangleF(
                    tank2.Position.X - (15 * zoom),
                    tank2.Position.Y - (10 * zoom),
                    30 * zoom,
                    20 * zoom);
            if (Keyboard.GetState().IsKeyDown(Keys.RightShift))
                drawRect = new RectangleF(
                    tank.Position.X - (15 * zoom),
                    tank.Position.Y - (10 * zoom),
                    30 * zoom,
                    20 * zoom);

            renderer.Render(spriteBatch, drawRect, gameTime);

            spriteBatch.Begin();
            var tanksCount = 0;
            var projCount = 0;
            foreach (var obj in game.GameObjects)
            {
                if (obj.GetType().IsSubclassOf(typeof(Engine.Tanks.Tank)))
                    tanksCount++;
                if (obj.GetType().IsSubclassOf(typeof(Engine.Projectiles.Projectile)))
                    projCount++;
            }
            var fps = calcfps((float)gameTime.ElapsedGameTime.TotalMilliseconds).ToString("N1");
            spriteBatch.DrawString(font, "Tanks: " + tanksCount + ", Projectiles: " + projCount
                + ", Update: " + physicsMs.ToString("N2") + ", \nRender: " + renderMs.ToString("N2") +
            ", Mouse: " + Mouse.GetState().Position.ToString() + ", Tank: " + tank.Position.ToString() + ",\n" +
            "Active timers: " + game.TimerFactory.ActiveTimersCount + ", Animation layers: " +
            game.AnimationEngine.Animations.Count + ",\nParticles: " +
            game.ParticleEngine.LivingParticlesCount + ", FPS: " + fps + ", GC (gen 0, 1, 2): " +
            GC.CollectionCount(0) + " " + GC.CollectionCount(1) + " " + GC.CollectionCount(2) + ",\n" +
            "Memory: " + (GC.GetTotalMemory(false) / (1024f * 1024)).ToString("N1") + "MB"
            , new Vector2(10, 10), Color.MediumPurple);
            //spriteBatch.DrawString(font, "P1: " + tank.Health + ", P2: " + tank2.Health, new Vector2(10, 10), Color.Red);
            spriteBatch.End();

            base.Draw(gameTime);
            sw.Stop();
            renderMs = (float)sw.Elapsed.TotalMilliseconds;
        }

        private float[] fps;

        private float calcfps(float deltaMs)
        {
            if (fps == null)
            {
                fps = new float[30];
                for (int i = 0; i < fps.Length; i++)
                    fps[i] = 16.666666f;
            }

            for (int i = 0; i < fps.Length - 1; i++)
                fps[i] = fps[i + 1];

            fps[fps.Length - 1] = deltaMs;

            return 1000 / avg();
        }
        private float avg()
        {
            float tot = 0;
            foreach (var f in fps)
                tot += f;
            return tot / fps.Length;
        }
    }
}
