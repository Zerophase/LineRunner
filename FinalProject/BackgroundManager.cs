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

namespace FinalProject.Components
{
    public interface IBackgroundManager
    { 
    
    }
    public class BackgroundManager : SpriteCharacter, IBackgroundManager
    {
        List<BackgroundSprite> backgrounds;

        private float time;

        private int backgroundsToAdd = 3;

        GameConsole console;

        InputHandler input;

        public BackgroundManager(Game game)
            : base(game)
        {
            game.Services.AddService(typeof(IBackgroundManager), this);

            console = (GameConsole)game.Services.GetService(typeof(IGameConsole));

            input = (InputHandler)game.Services.GetService(typeof(IInputHandler));

            this.accel = 30;
            speedMax = 100f;

            backgrounds = new List<BackgroundSprite>();
        }

        public override void Initialize()
        {
            
            base.Initialize();
        }

        protected override void LoadContent()
        {

            for (int i = 0; i < backgroundsToAdd; i++)
            {
                backgrounds.Add(new BackgroundSprite(this.Game));

                if (i > 1)
                {
                    backgrounds.ElementAt<BackgroundSprite>(i).Location.X +=
                        (backgrounds.ElementAt<BackgroundSprite>(i - 1).Location.X
                        + (backgrounds.ElementAt<BackgroundSprite>(i - 1).SpriteTexture.Width - 15));
                }
                else if (i > 0)
                {
                    backgrounds.ElementAt<BackgroundSprite>(i).Location.X +=
                        (backgrounds.ElementAt<BackgroundSprite>(i - 1).Location.X 
                        + (backgrounds.ElementAt<BackgroundSprite>(i - 1).SpriteTexture.Width));
                }
            }
            base.LoadContent();
        }
        public override void Update(GameTime gameTime)
        {
            time = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            move(gameTime, time);

            wrapTexture();
            base.Update(gameTime);
        }

        private void wrapTexture()
        {
            foreach (BackgroundSprite background in backgrounds)
            {
                if (background.Location.X <= background.SpriteTexture.Width * - 1)
                {
                    console.GameConsoleWrite("Wrapped at "
                        + background.Location.ToString()); 

                    background.Location.X = (background.SpriteTexture.Width * 2) - 17;
                    console.GameConsoleWrite("Wrapped to " + background.Location.X.ToString());
                }
            }
        }

        protected override void move(GameTime gameTime, float time)
        {
            if (!PlayerSprite.StopScreenMovement)
            {
                foreach (BackgroundSprite background in backgrounds)
                {
                    background.Direction = facing(background.Direction);

                    background.Location = background.Location + (background.Direction * (time / 1000));

                }
                base.move(gameTime, time);
            }
        }

        //TODO let facing take direction
        protected  Vector2 facing(Vector2 direction)
        {
            direction += new Vector2(-1, 0);
            return normalize(Direction);
        }

        protected override Vector2 normalize(Vector2 direction)
        {
            if (direction != Vector2.Zero)
                Vector2.Normalize(direction);

            return braking(direction);
        }

        //TODO add logic to stop movement when player is knocked back
        protected override Vector2 braking(Vector2 direction)
        {
            return acceleration(direction);
        }

        protected override Vector2 acceleration(Vector2 direction)
        {
            direction.X = Math.Max((speedMax * -1), direction.X - accel);
            return direction;
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            foreach (BackgroundSprite background in backgrounds)
            {
                spriteBatch.Draw(background.SpriteTexture, background.Location, Color.White);
            }
            spriteBatch.End();
        }
    }
}
