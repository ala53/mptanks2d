using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Starbound.Input
{
    /// <summary>
    /// An abstraction around game pad input that turns XNA's underlying polling model into an event-based
    /// model for game pad input.
    /// </summary>
    public class GamePadEvents : GameComponent
    {
        /// <summary>
        /// Represents the amount that a trigger needs to be pressed for it to count as a trigger button press.
        /// The default is 0.5f (half way pressed).
        /// </summary>
        public static float TriggerThreshold { get; set; }

        /// <summary>
        /// Represents the amount that a thumbstick needs to be pressed as a distance from the center for
        /// it to register as a thumbstick press in any direction. The default is half way out (0.5f).
        /// </summary>
        public static float ThumbstickThreshold { get; set; }

        /// <summary>
        /// Stores the state of the game pad in the previous update.
        /// </summary>
        private GamePadState previous;

        /// <summary>
        /// The physical player index, represented by the PlayerIndex of the polled GamePad.
        /// </summary>
        public PlayerIndex PhysicalIndex { get; set; }

        /// <summary>
        /// The logical player index. This is an abstraction over the top of the player index field that
        /// allows the logical first (or second, or third, or fourth) player to be controlled by any of
        /// the game pads.
        /// </summary>
        public PlayerIndex LogicalIndex { get; set; }

        /// <summary>
        /// Creates a new GamePadEvents object.
        /// </summary>
        static GamePadEvents()
        {
            TriggerThreshold = 0.5f;
            ThumbstickThreshold = 0.5f;
        }

        /// <summary>
        /// Creates a new GamePadEvents object with a player index that maps to both physical and logical
        /// indices.
        /// </summary>
        /// <param name="physicalIndex"></param>
        /// <param name="game"></param>
        public GamePadEvents(PlayerIndex physicalIndex, Game game)
            : this(physicalIndex, physicalIndex, game)
        {
        }

        /// <summary>
        /// Creates a new GamePadEvents object, with a mapping from a physical index to a logical index.
        /// </summary>
        /// <param name="physicalIndex"></param>
        /// <param name="logicalIndex"></param>
        /// <param name="game"></param>
        public GamePadEvents(PlayerIndex physicalIndex, PlayerIndex logicalIndex, Game game)
            : base(game)
        {
            PhysicalIndex = physicalIndex;
            LogicalIndex = logicalIndex;
        }


        /// <summary>
        /// Updates the GamePadEvents, performing polling and raising any events that have occurred.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            GamePadState current = GamePad.GetState(PhysicalIndex);

            // Button Down
            if (current.Buttons.A == ButtonState.Pressed && previous.Buttons.A == ButtonState.Released) { OnButtonDown(this, GetGamePadButtonEventArgs(gameTime.TotalGameTime, LogicalIndex, Buttons.A, current)); }
            if (current.Buttons.B == ButtonState.Pressed && previous.Buttons.B == ButtonState.Released) { OnButtonDown(this, GetGamePadButtonEventArgs(gameTime.TotalGameTime, LogicalIndex, Buttons.B, current)); }
            if (current.Buttons.X == ButtonState.Pressed && previous.Buttons.X == ButtonState.Released) { OnButtonDown(this, GetGamePadButtonEventArgs(gameTime.TotalGameTime, LogicalIndex, Buttons.X, current)); }
            if (current.Buttons.Y == ButtonState.Pressed && previous.Buttons.Y == ButtonState.Released) { OnButtonDown(this, GetGamePadButtonEventArgs(gameTime.TotalGameTime, LogicalIndex, Buttons.Y, current)); }
            if (current.Buttons.Back == ButtonState.Pressed && previous.Buttons.Back == ButtonState.Released) { OnButtonDown(this, GetGamePadButtonEventArgs(gameTime.TotalGameTime, LogicalIndex, Buttons.Back, current)); }
            if (current.Buttons.Start == ButtonState.Pressed && previous.Buttons.Start == ButtonState.Released) { OnButtonDown(this, GetGamePadButtonEventArgs(gameTime.TotalGameTime, LogicalIndex, Buttons.Start, current)); }
            if (current.Buttons.LeftShoulder == ButtonState.Pressed && previous.Buttons.LeftShoulder == ButtonState.Released) { OnButtonDown(this, GetGamePadButtonEventArgs(gameTime.TotalGameTime, LogicalIndex, Buttons.LeftShoulder, current)); }
            if (current.Buttons.RightShoulder == ButtonState.Pressed && previous.Buttons.RightShoulder == ButtonState.Released) { OnButtonDown(this, GetGamePadButtonEventArgs(gameTime.TotalGameTime, LogicalIndex, Buttons.RightShoulder, current)); }

            if (current.DPad.Left == ButtonState.Pressed && previous.DPad.Left == ButtonState.Released) { OnButtonDown(this, GetGamePadButtonEventArgs(gameTime.TotalGameTime, LogicalIndex, Buttons.DPadLeft, current)); }
            if (current.DPad.Right == ButtonState.Pressed && previous.DPad.Right == ButtonState.Released) { OnButtonDown(this, GetGamePadButtonEventArgs(gameTime.TotalGameTime, LogicalIndex, Buttons.DPadRight, current)); }
            if (current.DPad.Up == ButtonState.Pressed && previous.DPad.Up == ButtonState.Released) { OnButtonDown(this, GetGamePadButtonEventArgs(gameTime.TotalGameTime, LogicalIndex, Buttons.DPadUp, current)); }
            if (current.DPad.Down == ButtonState.Pressed && previous.DPad.Down == ButtonState.Released) { OnButtonDown(this, GetGamePadButtonEventArgs(gameTime.TotalGameTime, LogicalIndex, Buttons.DPadDown, current)); }

            if (current.Triggers.Left > TriggerThreshold && previous.Triggers.Left <= TriggerThreshold) { OnButtonDown(this, GetGamePadButtonEventArgs(gameTime.TotalGameTime, LogicalIndex, Buttons.LeftTrigger, current)); }
            if (current.Triggers.Right > TriggerThreshold && previous.Triggers.Right <= TriggerThreshold) { OnButtonDown(this, GetGamePadButtonEventArgs(gameTime.TotalGameTime, LogicalIndex, Buttons.RightTrigger, current)); }

            if (current.Buttons.LeftStick == ButtonState.Pressed && previous.Buttons.LeftStick == ButtonState.Released) { OnButtonDown(this, GetGamePadButtonEventArgs(gameTime.TotalGameTime, LogicalIndex, Buttons.LeftThumbstick, current)); }
            if (current.Buttons.RightStick == ButtonState.Pressed && previous.Buttons.RightStick == ButtonState.Released) { OnButtonDown(this, GetGamePadButtonEventArgs(gameTime.TotalGameTime, LogicalIndex, Buttons.RightThumbstick, current)); }

            PolarCoordinate oldLeft = PolarCoordinate.FromCartesian(previous.ThumbSticks.Left);
            PolarCoordinate newLeft = PolarCoordinate.FromCartesian(current.ThumbSticks.Left);
            PolarCoordinate newRight = PolarCoordinate.FromCartesian(current.ThumbSticks.Right);
            PolarCoordinate oldRight = PolarCoordinate.FromCartesian(previous.ThumbSticks.Right);

            if (InLeftSection(newLeft) && !InLeftSection(oldLeft)) { OnButtonDown(this, GetGamePadButtonEventArgs(gameTime.TotalGameTime, LogicalIndex, Buttons.LeftThumbstickLeft, current)); }
            if (InRightSection(newLeft) && !InRightSection(oldLeft)) { OnButtonDown(this, GetGamePadButtonEventArgs(gameTime.TotalGameTime, LogicalIndex, Buttons.LeftThumbstickRight, current)); }
            if (InUpSection(newLeft) && !InUpSection(oldLeft)) { OnButtonDown(this, GetGamePadButtonEventArgs(gameTime.TotalGameTime, LogicalIndex, Buttons.LeftThumbstickUp, current)); }
            if (InDownSection(newLeft) && !InDownSection(oldLeft)) { OnButtonDown(this, GetGamePadButtonEventArgs(gameTime.TotalGameTime, LogicalIndex, Buttons.LeftThumbstickDown, current)); }

            if (InLeftSection(newRight) && !InLeftSection(oldRight)) { OnButtonDown(this, GetGamePadButtonEventArgs(gameTime.TotalGameTime, LogicalIndex, Buttons.RightThumbstickLeft, current)); }
            if (InRightSection(newRight) && !InRightSection(oldRight)) { OnButtonDown(this, GetGamePadButtonEventArgs(gameTime.TotalGameTime, LogicalIndex, Buttons.RightThumbstickRight, current)); }
            if (InUpSection(newRight) && !InUpSection(oldRight)) { OnButtonDown(this, GetGamePadButtonEventArgs(gameTime.TotalGameTime, LogicalIndex, Buttons.RightThumbstickUp, current)); }
            if (InDownSection(newRight) && !InDownSection(oldRight)) { OnButtonDown(this, GetGamePadButtonEventArgs(gameTime.TotalGameTime, LogicalIndex, Buttons.RightThumbstickDown, current)); }

            // Button up
            if (current.Buttons.A == ButtonState.Released && previous.Buttons.A == ButtonState.Pressed) { OnButtonUp(this, GetGamePadButtonEventArgs(gameTime.TotalGameTime, LogicalIndex, Buttons.A, current)); }
            if (current.Buttons.B == ButtonState.Released && previous.Buttons.B == ButtonState.Pressed) { OnButtonUp(this, GetGamePadButtonEventArgs(gameTime.TotalGameTime, LogicalIndex, Buttons.B, current)); }
            if (current.Buttons.X == ButtonState.Released && previous.Buttons.X == ButtonState.Pressed) { OnButtonUp(this, GetGamePadButtonEventArgs(gameTime.TotalGameTime, LogicalIndex, Buttons.X, current)); }
            if (current.Buttons.Y == ButtonState.Released && previous.Buttons.Y == ButtonState.Pressed) { OnButtonUp(this, GetGamePadButtonEventArgs(gameTime.TotalGameTime, LogicalIndex, Buttons.Y, current)); }
            if (current.Buttons.Back == ButtonState.Released && previous.Buttons.Back == ButtonState.Pressed) { OnButtonUp(this, GetGamePadButtonEventArgs(gameTime.TotalGameTime, LogicalIndex, Buttons.Back, current)); }
            if (current.Buttons.Start == ButtonState.Released && previous.Buttons.Start == ButtonState.Pressed) { OnButtonUp(this, GetGamePadButtonEventArgs(gameTime.TotalGameTime, LogicalIndex, Buttons.Start, current)); }
            if (current.Buttons.LeftShoulder == ButtonState.Released && previous.Buttons.LeftShoulder == ButtonState.Pressed) { OnButtonUp(this, GetGamePadButtonEventArgs(gameTime.TotalGameTime, LogicalIndex, Buttons.LeftShoulder, current)); }
            if (current.Buttons.RightShoulder == ButtonState.Released && previous.Buttons.RightShoulder == ButtonState.Pressed) { OnButtonUp(this, GetGamePadButtonEventArgs(gameTime.TotalGameTime, LogicalIndex, Buttons.RightShoulder, current)); }

            if (current.DPad.Left == ButtonState.Released && previous.DPad.Left == ButtonState.Pressed) { OnButtonUp(this, GetGamePadButtonEventArgs(gameTime.TotalGameTime, LogicalIndex, Buttons.DPadLeft, current)); }
            if (current.DPad.Right == ButtonState.Released && previous.DPad.Right == ButtonState.Pressed) { OnButtonUp(this, GetGamePadButtonEventArgs(gameTime.TotalGameTime, LogicalIndex, Buttons.DPadRight, current)); }
            if (current.DPad.Up == ButtonState.Released && previous.DPad.Up == ButtonState.Pressed) { OnButtonUp(this, GetGamePadButtonEventArgs(gameTime.TotalGameTime, LogicalIndex, Buttons.DPadUp, current)); }
            if (current.DPad.Down == ButtonState.Released && previous.DPad.Down == ButtonState.Pressed) { OnButtonUp(this, GetGamePadButtonEventArgs(gameTime.TotalGameTime, LogicalIndex, Buttons.DPadDown, current)); }

            if (current.Triggers.Left <= TriggerThreshold && previous.Triggers.Left > TriggerThreshold) { OnButtonUp(this, GetGamePadButtonEventArgs(gameTime.TotalGameTime, LogicalIndex, Buttons.LeftTrigger, current)); }
            if (current.Triggers.Right <= TriggerThreshold && previous.Triggers.Right > TriggerThreshold) { OnButtonUp(this, GetGamePadButtonEventArgs(gameTime.TotalGameTime, LogicalIndex, Buttons.RightTrigger, current)); }

            if (current.Buttons.LeftStick == ButtonState.Released && previous.Buttons.LeftStick == ButtonState.Pressed) { OnButtonUp(this, GetGamePadButtonEventArgs(gameTime.TotalGameTime, LogicalIndex, Buttons.LeftThumbstick, current)); }
            if (current.Buttons.RightStick == ButtonState.Released && previous.Buttons.RightStick == ButtonState.Pressed) { OnButtonUp(this, GetGamePadButtonEventArgs(gameTime.TotalGameTime, LogicalIndex, Buttons.RightThumbstick, current)); }

            if (!InLeftSection(newLeft) && InLeftSection(oldLeft)) { OnButtonUp(this, GetGamePadButtonEventArgs(gameTime.TotalGameTime, LogicalIndex, Buttons.LeftThumbstickLeft, current)); }
            if (!InRightSection(newLeft) && InRightSection(oldLeft)) { OnButtonUp(this, GetGamePadButtonEventArgs(gameTime.TotalGameTime, LogicalIndex, Buttons.LeftThumbstickRight, current)); }
            if (!InUpSection(newLeft) && InUpSection(oldLeft)) { OnButtonUp(this, GetGamePadButtonEventArgs(gameTime.TotalGameTime, LogicalIndex, Buttons.LeftThumbstickUp, current)); }
            if (!InDownSection(newLeft) && InDownSection(oldLeft)) { OnButtonUp(this, GetGamePadButtonEventArgs(gameTime.TotalGameTime, LogicalIndex, Buttons.LeftThumbstickDown, current)); }

            if (!InLeftSection(newRight) && InLeftSection(oldRight)) { OnButtonUp(this, GetGamePadButtonEventArgs(gameTime.TotalGameTime, LogicalIndex, Buttons.RightThumbstickLeft, current)); }
            if (!InRightSection(newRight) && InRightSection(oldRight)) { OnButtonUp(this, GetGamePadButtonEventArgs(gameTime.TotalGameTime, LogicalIndex, Buttons.RightThumbstickRight, current)); }
            if (!InUpSection(newRight) && InUpSection(oldRight)) { OnButtonUp(this, GetGamePadButtonEventArgs(gameTime.TotalGameTime, LogicalIndex, Buttons.RightThumbstickUp, current)); }
            if (!InDownSection(newRight) && InDownSection(oldRight)) { OnButtonUp(this, GetGamePadButtonEventArgs(gameTime.TotalGameTime, LogicalIndex, Buttons.RightThumbstickDown, current)); }

            // Triggers moved.
            if (current.Triggers.Left != previous.Triggers.Left)
            {
                OnTriggerMoved(this, GetGamePadTriggerEventArgs(gameTime.TotalGameTime, LogicalIndex, Triggers.Left, current.Triggers.Left, current));
                OnLeftTriggerMoved(this, GetGamePadTriggerEventArgs(gameTime.TotalGameTime, LogicalIndex, Triggers.Left, current.Triggers.Left, current));
            }

            if (current.Triggers.Right != previous.Triggers.Right)
            {
                OnTriggerMoved(this, GetGamePadTriggerEventArgs(gameTime.TotalGameTime, LogicalIndex, Triggers.Right, current.Triggers.Right, current));
                OnRightTriggerMoved(this, GetGamePadTriggerEventArgs(gameTime.TotalGameTime, LogicalIndex, Triggers.Right, current.Triggers.Right, current));
            }

            // Thumbsticks moved.
            if (current.ThumbSticks.Left.X != previous.ThumbSticks.Left.X || current.ThumbSticks.Left.Y != previous.ThumbSticks.Left.Y)
            {
                OnThumbstickMoved(this, GetGamePadThumbStickEventArgs(gameTime.TotalGameTime, LogicalIndex, Thumbsticks.Left, new Vector2(current.ThumbSticks.Left.X, current.ThumbSticks.Left.Y), current));
                OnLeftThumbstickMoved(this, GetGamePadThumbStickEventArgs(gameTime.TotalGameTime, LogicalIndex, Thumbsticks.Left, new Vector2(current.ThumbSticks.Left.X, current.ThumbSticks.Left.Y), current));
            }

            if (current.ThumbSticks.Right.X != previous.ThumbSticks.Right.X || current.ThumbSticks.Right.Y != previous.ThumbSticks.Right.Y)
            {
                OnThumbstickMoved(this, GetGamePadThumbStickEventArgs(gameTime.TotalGameTime, LogicalIndex, Thumbsticks.Right, new Vector2(current.ThumbSticks.Right.X, current.ThumbSticks.Right.Y), current));
                OnRightThumbstickMoved(this, GetGamePadThumbStickEventArgs(gameTime.TotalGameTime, LogicalIndex, Thumbsticks.Right, new Vector2(current.ThumbSticks.Right.X, current.ThumbSticks.Right.Y), current));
            }

            // Connected and Disconnected events.
            if (current.IsConnected && !previous.IsConnected) { OnConnected(this, GetGamePadEventArgs(gameTime.TotalGameTime, LogicalIndex, current)); }
            if (!current.IsConnected && previous.IsConnected) { OnDisconnected(this, GetGamePadEventArgs(gameTime.TotalGameTime, LogicalIndex, current)); }

            previous = current;

            ReleaseAll();
        }

        #region Pooling
        #region GamePadEventArgs
        private List<GamePadEventArgs> _freeGPEventArgs = new List<GamePadEventArgs>();
        private List<GamePadEventArgs> _allGPEventArgs = new List<GamePadEventArgs>();
        private GamePadEventArgs GetGamePadEventArgs(TimeSpan time, PlayerIndex logicalIndex, GamePadState state)
        {
            GamePadEventArgs _arg;
            if (_freeGPEventArgs.Count == 0)
            { //If it doesn't exist
                _arg = new GamePadEventArgs(time, logicalIndex, state);
                _allGPEventArgs.Add(_arg);
                return _arg;
            }

            //If it exists
            _arg = _freeGPEventArgs[_freeGPEventArgs.Count - 1];
            _freeGPEventArgs.RemoveAt(_freeGPEventArgs.Count - 1);

            _arg.Time = time;
            _arg.LogicalIndex = logicalIndex;
            _arg.Current = state;

            return _arg;
        }
        #endregion
        #region GamePadTriggerEventArgs
        private List<GamePadTriggerEventArgs> _freeGPTrEventArgs = new List<GamePadTriggerEventArgs>();
        private List<GamePadTriggerEventArgs> _allGPTrEventArgs = new List<GamePadTriggerEventArgs>();
        private GamePadTriggerEventArgs GetGamePadTriggerEventArgs(TimeSpan time, PlayerIndex logicalIndex, Triggers trigger, float amount, GamePadState state)
        {
            GamePadTriggerEventArgs _arg;
            if (_freeGPTEventArgs.Count == 0)
            { //If it doesn't exist
                _arg = new GamePadTriggerEventArgs(time, logicalIndex, trigger, amount, state);
                _allGPTrEventArgs.Add(_arg);
                return _arg;
            }

            //If it exists
            _arg = _freeGPTrEventArgs[_freeGPTrEventArgs.Count - 1];
            _freeGPTrEventArgs.RemoveAt(_freeGPTrEventArgs.Count - 1);

            _arg.Time = time;
            _arg.Trigger = trigger;
            _arg.Value = amount;
            _arg.LogicalIndex = logicalIndex;
            _arg.Current = state;

            return _arg;
        }
        #endregion
        #region GamePadThumbStickEventArgs
        private List<GamePadThumbstickEventArgs> _freeGPTEventArgs = new List<GamePadThumbstickEventArgs>();
        private List<GamePadThumbstickEventArgs> _allGPTEventArgs = new List<GamePadThumbstickEventArgs>();
        private GamePadThumbstickEventArgs GetGamePadThumbStickEventArgs(TimeSpan time, PlayerIndex logicalIndex, Thumbsticks stick, Vector2 position, GamePadState state)
        {
            GamePadThumbstickEventArgs _arg;
            if (_freeGPTEventArgs.Count == 0)
            { //If it doesn't exist
                _arg = new GamePadThumbstickEventArgs(time, logicalIndex, stick, position, state);
                _allGPTEventArgs.Add(_arg);
                return _arg;
            }

            //If it exists
            _arg = _freeGPTEventArgs[_freeGPTEventArgs.Count - 1];
            _freeGPTEventArgs.RemoveAt(_freeGPTEventArgs.Count - 1);

            _arg.Time = time;
            _arg.Position = position;
            _arg.Thumbstick = stick;
            _arg.LogicalIndex = logicalIndex;
            _arg.Current = state;

            return _arg;
        }
        #endregion
        #region GamePadButtonEventArgs

        private List<GamePadButtonEventArgs> _freeGPBEventArgs = new List<GamePadButtonEventArgs>();
        private List<GamePadButtonEventArgs> _allGPBEventArgs = new List<GamePadButtonEventArgs>();
        private GamePadButtonEventArgs GetGamePadButtonEventArgs(TimeSpan time, PlayerIndex logicalIndex, Buttons buttons, GamePadState state)
        {
            GamePadButtonEventArgs _arg;
            if (_freeGPBEventArgs.Count == 0)
            { //If it doesn't exist
                _arg = new GamePadButtonEventArgs(time, logicalIndex, buttons, state);
                _allGPBEventArgs.Add(_arg);
                return _arg;
            }

            //If it exists
            _arg = _freeGPBEventArgs[_freeGPBEventArgs.Count - 1];
            _freeGPBEventArgs.RemoveAt(_freeGPBEventArgs.Count - 1);

            _arg.Time = time;
            _arg.Button = buttons;
            _arg.LogicalIndex = logicalIndex;
            _arg.Current = state;

            return _arg;
        }
        #endregion
        private void ReleaseAll()
        {
            //Connect/Disconnect
            _freeGPEventArgs.Clear();
            _freeGPEventArgs.AddRange(_allGPEventArgs);
            //Buttons
            _freeGPBEventArgs.Clear();
            _freeGPBEventArgs.AddRange(_allGPBEventArgs);
            //Thumbsticks
            _freeGPTEventArgs.Clear();
            _freeGPTEventArgs.AddRange(_allGPTEventArgs);
            //Triggers
            _freeGPTrEventArgs.Clear();
            _freeGPTrEventArgs.AddRange(_allGPTrEventArgs);
        }
        #endregion

        private bool InLeftSection(PolarCoordinate value)
        {
            return value.Distance > ThumbstickThreshold && InLeftDirection(value.Angle);
        }

        private bool InRightSection(PolarCoordinate value)
        {
            return value.Distance > ThumbstickThreshold && InRightDirection(value.Angle);
        }

        private bool InUpSection(PolarCoordinate value)
        {
            return value.Distance > ThumbstickThreshold && InUpDirection(value.Angle);
        }

        private bool InDownSection(PolarCoordinate value)
        {
            return value.Distance > ThumbstickThreshold && InDownDirection(value.Angle);
        }

        private bool InRightDirection(float angle)
        {
            return (angle <= MathHelper.PiOver4 && (angle > -MathHelper.PiOver4));
        }

        private bool InLeftDirection(float angle)
        {
            return (
                angle > MathHelper.Pi - MathHelper.PiOver4 ||
                angle <= -MathHelper.Pi + MathHelper.PiOver4);
        }


        private bool InUpDirection(float angle)
        {
            return (
                angle > MathHelper.PiOver2 - MathHelper.PiOver4 &&
                angle <= MathHelper.PiOver2 + MathHelper.PiOver4);
        }

        private bool InDownDirection(float angle)
        {
            return (
                angle < -MathHelper.PiOver2 + MathHelper.PiOver4 &&
                angle >= -MathHelper.PiOver2 - MathHelper.PiOver4);
        }

        /// <summary>
        /// Raises the ButtonDown event. This is automatically raised by an appropriately configured
        /// GamePadEvents object, but this allows for programmatic raising of events.
        /// </summary>
        public void OnButtonDown(object sender, GamePadButtonEventArgs args)
        {
            if (ButtonDown != null) { ButtonDown(sender, args); }
        }

        /// <summary>
        /// Raises the ButtonUp event. This is automatically raised by an appropriately configured
        /// GamePadEvents object, but this allows for programmatic raising of events.
        /// </summary>
        public void OnButtonUp(object sender, GamePadButtonEventArgs args)
        {
            if (ButtonUp != null) { ButtonUp(sender, args); }
        }

        /// <summary>
        /// Raises the LeftTriggerMoved event. This is automatically raised by an appropriately configured
        /// GamePadEvents object, but this allows for programmatic raising of events.
        /// </summary>
        public void OnLeftTriggerMoved(object sender, GamePadTriggerEventArgs args)
        {
            if (LeftTriggerMoved != null) { LeftTriggerMoved(sender, args); }
        }

        /// <summary>
        /// Raises the RightTriggerMoved event. This is automatically raised by an appropriately configured
        /// GamePadEvents object, but this allows for programmatic raising of events.
        /// </summary>
        public void OnRightTriggerMoved(object sender, GamePadTriggerEventArgs args)
        {
            if (RightTriggerMoved != null) { RightTriggerMoved(sender, args); }
        }

        /// <summary>
        /// Raises the TriggerMoved event. This is automatically raised by an appropriately configured
        /// GamePadEvents object, but this allows for programmatic raising of events.
        /// </summary>
        public void OnTriggerMoved(object sender, GamePadTriggerEventArgs args)
        {
            if (TriggerMoved != null) { TriggerMoved(sender, args); }
        }

        /// <summary>
        /// Raises the LeftThumbstickMoved event. This is automatically raised by an appropriately configured
        /// GamePadEvents object, but this allows for programmatic raising of events.
        /// </summary>
        public void OnLeftThumbstickMoved(object sender, GamePadThumbstickEventArgs args)
        {
            if (LeftThumbstickMoved != null) { LeftThumbstickMoved(sender, args); }
        }

        /// <summary>
        /// Raises the RightThumbstickMoved event. This is automatically raised by an appropriately configured
        /// GamePadEvents object, but this allows for programmatic raising of events.
        /// </summary>
        public void OnRightThumbstickMoved(object sender, GamePadThumbstickEventArgs args)
        {
            if (RightThumbstickMoved != null) { RightThumbstickMoved(sender, args); }
        }

        /// <summary>
        /// Raises the ThumbstickMoved event. This is automatically raised by an appropriately configured
        /// GamePadEvents object, but this allows for programmatic raising of events.
        /// </summary>
        public void OnThumbstickMoved(object sender, GamePadThumbstickEventArgs args)
        {
            if (ThumbstickMoved != null) { ThumbstickMoved(sender, args); }
        }

        /// <summary>
        /// Raises the Connected event. This is automatically raised by an appropriately configured
        /// GamePadEvents object, but this allows for programmatic raising of events.
        /// </summary>
        public void OnConnected(object sender, GamePadEventArgs args)
        {
            if (Connected != null) { Connected(sender, args); }
        }

        /// <summary>
        /// Raises the Disconnected event. This is automatically raised by an appropriately configured
        /// GamePadEvents object, but this allows for programmatic raising of events.
        /// </summary>
        public void OnDisconnected(object sender, GamePadEventArgs args)
        {
            if (Disconnected != null) { Disconnected(sender, args); }
        }

        /// <summary>
        /// An event that is raised whenever a button is pressed. This includes all of the normal buttons,
        /// but it also includes triggers and thumb sticks being treated as buttons.
        /// </summary>
        public static event EventHandler<GamePadButtonEventArgs> ButtonDown;

        /// <summary>
        /// An event that is raised whenever a button is released. This includes all of the normal buttons,
        /// but it also includes triggers and thumb sticks being treated as buttons.
        /// </summary>
        public static event EventHandler<GamePadButtonEventArgs> ButtonUp;

        /// <summary>
        /// An event that is raised any time the left trigger is moved.
        /// </summary>
        public static event EventHandler<GamePadTriggerEventArgs> LeftTriggerMoved;

        /// <summary>
        /// An event that is raised any time that the right trigger is moved.
        /// </summary>
        public static event EventHandler<GamePadTriggerEventArgs> RightTriggerMoved;

        /// <summary>
        /// An event that is raised any time either trigger is moved.
        /// </summary>
        public static event EventHandler<GamePadTriggerEventArgs> TriggerMoved;

        /// <summary>
        /// An event that is raised any time the left thumbstick is moved.
        /// </summary>
        public static event EventHandler<GamePadThumbstickEventArgs> LeftThumbstickMoved;

        /// <summary>
        /// An event that is raised any time that the right thumbstick is moved.
        /// </summary>
        public static event EventHandler<GamePadThumbstickEventArgs> RightThumbstickMoved;

        /// <summary>
        /// An event that is raised any time either thumbstick is moved.
        /// </summary>
        public static event EventHandler<GamePadThumbstickEventArgs> ThumbstickMoved;

        /// <summary>
        /// an event that is raised when this controller connects.
        /// </summary>
        public static event EventHandler<GamePadEventArgs> Connected;

        /// <summary>
        /// An event that is raised when this controller disconnects.
        /// </summary>
        public static event EventHandler<GamePadEventArgs> Disconnected;
    }
}
