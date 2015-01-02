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
    public interface ITitleState : IGameState { }

    public sealed class TitleState : BaseGameState, ITitleState
    {
        private Texture2D texture;

        public TitleState(Game game)
            :base(game)
        {
            game.Services.AddService(typeof(ITitleState), this);
        }

        protected override void LoadContent()
        {
            texture = theGame.Content.Load<Texture2D>("Title");
            
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (input.WasKeyPressed(Keys.Escape))
                this.Game.Exit();
           
            if (input.WasKeyPressed(Keys.Enter))
                //Places start menu on top of the stack.
                gameManager.PushState(theGame.StartMenuState.Value);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            Vector2 position = new Vector2(0, 0);
            theGame.SpriteBatch.Draw(texture, position, Color.White);
            base.Draw(gameTime);
        }
    }
}
