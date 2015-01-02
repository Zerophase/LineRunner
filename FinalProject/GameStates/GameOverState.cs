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
using Utilities;

namespace FinalProject.GameStates
{
    public interface IGameOverState : IGameState { }

    public class GameOverState : BaseGameState, IGameOverState
    {
        private SpriteFont font;

        public GameOverState(Game game)
            : base(game)
        {
            game.Services.AddService(typeof(IGameOverState), this);
        }

        protected override void LoadContent()
        {
            this.font = theGame.Content.Load<SpriteFont>("MyFont");

            base.LoadContent();
        }

        protected override void StateChanged(object sender, EventArgs e)
        {
            base.StateChanged(sender, e);
        }

        public override void Update(GameTime gameTime)
        {
            if (input.WasKeyPressed(Keys.Enter))
            {
                gameManager.ChangeState(theGame.StartMenuState.Value);
            }
                
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            theGame.GraphicsDevice.Clear(Color.DarkCyan);

            theGame.SpriteBatch.DrawString(font, "Game Over", new Vector2((theGame.Window.ClientBounds.Width / 2) - 100,
                (theGame.Window.ClientBounds.Height / 2) - 200), Color.Gold, 0f, new Vector2(0f, 0f), 3f, SpriteEffects.None, 0f);
            theGame.SpriteBatch.DrawString(font, "Press Enter to return to the Start Menu", new Vector2((theGame.Window.ClientBounds.Width / 2) - 300,
                (theGame.Window.ClientBounds.Height / 2) - 100), Color.Gold, 0f, new Vector2(0f, 0f), 2f, SpriteEffects.None, 0f);
            base.Draw(gameTime);
        }
    }
}
