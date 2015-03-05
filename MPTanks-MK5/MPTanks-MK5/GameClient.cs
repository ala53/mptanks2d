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
        private Engine.GameCore game;
        private float zoom = 2f;
        private SpriteFont font;

        public GameClient()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
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

            game = new Engine.GameCore(new EngineInterface.FileLogger());
            renderer = new GameWorldRenderer(this);
            renderer.SetAnimations(game.Animations);

            font = Content.Load<SpriteFont>("font");

            for (var i = 1; i < 30; i++)
                game.AddGameObject(new BasicTank(Guid.NewGuid(), game) { Position = new Vector2(i * 3.5f, 5) });
            for (var i = 0; i < 30; i++)
                game.AddGameObject(new BasicTank(Guid.NewGuid(), game) { Position = new Vector2(i * 3.5f, 40) });
            for (var i = 0; i < 30; i++)
                game.AddGameObject(new BasicTank(Guid.NewGuid(), game) { Position = new Vector2(5, i * 3.5f) });

            game.AddGameObject(new Engine.Maps.MapObjects.Building(game, new Vector2(50, 50), 33));

            tank = new BasicTank(Guid.NewGuid(), game) { Position = new Vector2(15, 20) };
            game.AddGameObject(tank);
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

            var iState = new InputState();
            iState.LookDirection = tank.InputState.LookDirection;

            if (Keyboard.GetState().IsKeyDown(Keys.W))
                iState.YMovementState = 1;
            if (Keyboard.GetState().IsKeyDown(Keys.S))
                iState.YMovementState = -1;
            if (Keyboard.GetState().IsKeyDown(Keys.A))
                iState.XMovementState = -1;
            if (Keyboard.GetState().IsKeyDown(Keys.D))
                iState.XMovementState = 1;


            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                iState.FireState = true;

            //Complicated look state calcuation below
            var screenCenter = new Vector2(GraphicsDevice.Viewport.Bounds.Width / 2, //vertex
                GraphicsDevice.Viewport.Bounds.Height / 2);
            var mousePos = new Vector2(Mouse.GetState().Position.X,  //point a
                Mouse.GetState().Position.Y);

            var ctr = screenCenter - mousePos;
            iState.LookDirection = (float)-Math.Atan2(ctr.X, ctr.Y);

            LockCursor();

            if (Keyboard.GetState().IsKeyDown(Keys.X))
                zoom += 0.1f;
            if (Keyboard.GetState().IsKeyDown(Keys.Z))
                zoom -= 0.1f;

            tank.Input(iState);

            game.Update(gameTime);

            //Update the render list if the game has added or removed objects
            if (game.IsDirty)
                renderer.SetObjects(game.GameObjects);
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

            var drawRect = new RectangleF(
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

            spriteBatch.DrawString(font, "Tanks: " + tanksCount + ", Projectiles: " + projCount
                + ", Update: " + physicsMs.ToString("N2") + ", \nRender: " + renderMs.ToString("N2") +
            ", Mouse: " + Mouse.GetState().Position.ToString() + ", Tank: " + tank.Position.ToString() + ",\n" +
            "Active timers: " + game.TimerFactory.ActiveTimersCount + ", Animation layers: " +
            game.Animations.Animations.Count, new Vector2(10, 10), Color.Red);
            spriteBatch.End();

            base.Draw(gameTime);
            sw.Stop();
            renderMs = (float)sw.Elapsed.TotalMilliseconds;
        }
    }
}
