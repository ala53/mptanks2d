using MPTanks.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Networking.Common.Game
{
    /// <summary>
    /// A pseudo-full state. It contains the physical information for all the game objects but not their 
    /// actual private state data, which is computationally expensive
    /// </summary>
    public class PseudoFullGameWorldState
    {
        public IReadOnlyDictionary<ushort, PseudoFullObjectState> ObjectStates { get { return _objectStates; } }
        private Dictionary<ushort, PseudoFullObjectState> _objectStates
            = new Dictionary<ushort, PseudoFullObjectState>();

        public GameCore.CurrentGameStatus CurrentGameStatus { get; set; }
        public float CurrentGameTimeMilliseconds { get; set; }
        public bool FriendlyFireEnabled { get; set; }

        public static PseudoFullGameWorldState Create(GameCore game)
        {
            var state = new PseudoFullGameWorldState();
            foreach (var obj in game.GameObjects)
            {
                state._objectStates.Add(obj.ObjectId, new PseudoFullObjectState(obj));
            }

            state.CurrentGameStatus = game.GameStatus;
            state.CurrentGameTimeMilliseconds = game.TimeMilliseconds;
            state.FriendlyFireEnabled = game.FriendlyFireEnabled;

            return state;
        }

        public PseudoFullGameWorldState MakeDelta(PseudoFullGameWorldState lastState)
        {
            var state = new PseudoFullGameWorldState();

            foreach (var obj in lastState.ObjectStates.Values)
            {
                //It was destroyed, flag it
                if (!ObjectStates.ContainsKey(obj.ObjectId))
                    state._objectStates.Add(obj.ObjectId, new PseudoFullObjectState(obj, true));

                //Otherwise, compute state differences
                state._objectStates.Add(obj.ObjectId, new PseudoFullObjectState(obj, ObjectStates[obj.ObjectId]));
            }

            //Then do a reverse search to find the new ones
            foreach (var obj in ObjectStates.Values)
            {
                if (!lastState.ObjectStates.ContainsKey(obj.ObjectId) &&
                    !state.ObjectStates.ContainsKey(obj.ObjectId))
                {
                    state._objectStates.Add(obj.ObjectId, obj);
                }
            }

            state.CurrentGameStatus = CurrentGameStatus;
            state.CurrentGameTimeMilliseconds = CurrentGameTimeMilliseconds;
            state.FriendlyFireEnabled = FriendlyFireEnabled;

            return state;
        }
    }
}
