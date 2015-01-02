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
    public interface IGameState
    {
        GameState Value { get; }
    }
    public abstract partial class GameState: DrawableGameComponent, IGameState
    {
        protected IGameStateManager gameManager;
        protected InputHandler input;

        public GameState(Game game)
            : base(game)
        {
            gameManager = (IGameStateManager)game.Services.GetService(typeof(IGameStateManager));
            input = (InputHandler)game.Services.GetService(typeof(IInputHandler));
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        internal protected virtual void StateChanged(object sender, EventArgs e)
        {
            if (gameManager.State == this.Value)
                Visible = Enabled = true;
            else
                Visible = Enabled = false;
        }

        public GameState Value
        {
            get { return (this); }
        }
    }
}
