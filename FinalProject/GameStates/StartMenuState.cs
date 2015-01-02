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
    public interface IStartMenuState : IGameState { }

    public sealed class StartMenuState : BaseGameState, IStartMenuState
    {
        private SpriteFont font;

        public StartMenuState(Game game)
            : base(game)
        {
            game.Services.AddService(typeof(IStartMenuState), this);
        }

        protected override void LoadContent()
        {
            font = theGame.Content.Load<SpriteFont>("MyFont");
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (input.WasKeyPressed(Keys.Escape))
                gameManager.ChangeState(theGame.TitleState.Value);
                //ADDED Game1 to base as I could not figure out how to root into my components without it.
                //gameManager.ChangeState((GameState)Game.Components.OfType<GameStates.TitleState>());
            if (input.WasKeyPressed(Keys.Enter))
            {
                gameManager.PopState();

                gameManager.ChangeState(theGame.PlayingState.Value);
            }

            base.Update(gameTime);
        }

        protected override void StateChanged(object sender, EventArgs e)
        {
            base.StateChanged(sender, e);

            if (gameManager.State != this.Value)
            {
                Visible = true;
            } 
        }

        public override void Draw(GameTime gameTime)
        {
            //refactor to use one Draw string and just return a line after each string.
            theGame.GraphicsDevice.Clear(Color.Cornsilk);

            theGame.SpriteBatch.DrawString(font, "Jump with the Up arrow keys.", new Vector2(100, 100), Color.Black);
            theGame.SpriteBatch.DrawString(font, "Jump over the pits and walls.", new Vector2(100, 150), Color.Black);
            theGame.SpriteBatch.DrawString(font, "If you hit a wall you will be knocked back.", new Vector2(100, 200), Color.Black);
            theGame.SpriteBatch.DrawString(font, "If you fall in a pit you die.\n\nIf you fall off the back of the screen you die as well.", new Vector2(100, 250), Color.Black);
            theGame.SpriteBatch.DrawString(font, "Escape pauses and Enter starts the game.", new Vector2(400, 500), Color.Black);
            theGame.SpriteBatch.DrawString(font, "Left Alt Toggles Full Screen.", new Vector2(400, 525), Color.Black);
            base.Draw(gameTime);
        }
    }
}
