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

namespace FinalProject.Components
{
    public class Score : SpriteComponent
    {
        private SpriteFont font;

        private string scoreDisplayed = "0";
        private string scoreTitle = "Score";

        private float time = 0;
        ChunkConstructor chunkConstructor;

        public Score(Game game)
            : base(game)
        {
            font = game.Content.Load<SpriteFont>("MyFont");
            chunkConstructor = (ChunkConstructor)game.Services.GetService(typeof(IChunkConstructor));
        }

        public override void Update(GameTime gameTime)
        {
            time += gameTime.ElapsedGameTime.Milliseconds;

            if (time > 1000)
            {
                scoreDisplayed = ((int)(chunkConstructor.Location.X * -1)).ToString();
                time = 0;
            }
            

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.DrawString(font, scoreTitle, new Vector2(50, 25), Color.Coral, 0f, new Vector2(0f, 0f), 2f, SpriteEffects.None, 0f);
            spriteBatch.DrawString(font, scoreDisplayed, new Vector2(50, 65), Color.Coral, 0f, new Vector2(0f, 0f), 1.5f, SpriteEffects.None, 0f);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
