using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;

namespace Utilities
{
    public interface IGameStateManager
    {
        event EventHandler OnStateChange;
        GameState State { get; }
        void PopState();
        void PushState(GameState state);
        bool ContainsState(GameState state);
        void ChangeState(GameState newState);
    }
    public class GameStateManager: GameComponent, IGameStateManager
    {
        private Stack<GameState> states = new Stack<GameState>();

        public event EventHandler OnStateChange;

        private int initialDrawOrder = 1000;
        private int drawOrder;

        public GameStateManager(Game game)
            : base(game)
        {
            game.Services.AddService(typeof(IGameStateManager), this);
            drawOrder = initialDrawOrder;
        }

        private void RemoveState()
        {
            GameState oldState = (GameState)states.Peek();

            OnStateChange -= oldState.StateChanged;

            Game.Components.Remove(oldState.Value);

            states.Pop();
        }

        private void AddState(GameState state)
        {
            states.Push(state);

            Game.Components.Add(state);

            OnStateChange += state.StateChanged;
        }

        public void PopState()
        {
            RemoveState();
            drawOrder -= 100;

            //Lets everyone registered know state has changed
            if (OnStateChange != null)
                OnStateChange(this, null);
        }

        public void PushState(GameState newState)
        {
            drawOrder += 100;
            newState.DrawOrder = drawOrder;

            AddState(newState);
            
            //Can pass a message through if needed.
            if (OnStateChange != null)
                OnStateChange(this, null);
        }

        public void ChangeState(GameState newState)
        { 
            //State is changing so pop everything
            //if we don't want to change states, just modify
            //Use PushState and PopState
            while (states.Count > 0)
                RemoveState();

            //Changing states so reset draw order
            newState.DrawOrder = drawOrder = initialDrawOrder;
            AddState(newState);

            //Passes message
            if (OnStateChange != null)
                OnStateChange(this, null);
        }

        public bool ContainsState(GameState state)
        {
            return (states.Contains(state));
        }

        public GameState State
        {
            get { return (states.Peek()); }
        }
    }
}
