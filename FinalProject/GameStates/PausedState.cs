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
    public interface IPausedState : IGameState { }

    public class PausedState : BaseGameState, IPausedState
    {
        private Texture2D pausedTexture;
        private SpriteFont font;

        public PausedState(Game game)
            : base(game)
        {
            game.Services.AddService(typeof(IPausedState), this);
        }

        protected override void LoadContent()
        {
            this.pausedTexture = theGame.Content.Load<Texture2D>("Paused");
            this.font = theGame.Content.Load<SpriteFont>("MyFont");
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (input.WasKeyPressed(Keys.Escape) || input.WasKeyPressed(Keys.P))
                gameManager.PopState();

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            Rectangle fullScreen = new Rectangle(0, 0, theGame.Window.ClientBounds.Width, theGame.Window.ClientBounds.Height);

            theGame.SpriteBatch.Draw(pausedTexture, fullScreen, Color.Black);
            theGame.SpriteBatch.DrawString(font, "Paused Press Esc to Resume.", new Vector2(theGame.Window.ClientBounds.Width / 2, theGame.Window.ClientBounds.Height / 2)
                , Color.Red);
            base.Draw(gameTime);
        }
    }
}
