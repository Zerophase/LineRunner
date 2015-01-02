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
    public class SpriteCharacter : SpriteComponent
    {
        protected Color color;

        public Matrix SpriteTransform;

        public float speed;
        protected float accel;
        protected float speedMax;
        protected float friction;

        

        public SpriteCharacter(Game game) 
            : base(game)
        {
            color = Color.White;
        }

        public override void Update(GameTime gameTime)
        {
            //bounds();
            SetTransform();
            base.Update(gameTime);
        }

        protected void SetTransform()
        {
            SpriteTransform =
                Matrix.CreateTranslation(new Vector3(Origin * -1, 0.0f)) *
                Matrix.CreateScale(this.scale) *
                Matrix.CreateRotationZ(0.0f) *
                Matrix.CreateTranslation(new Vector3(this.Location, 0.0f));
        }
        protected virtual void bounds()
        {
            if (Location.Y < 0)
            {
                Location.Y = 0;
            }
            if (Location.X < 0)
            {
                Location.X = 0;
            }

            if (spriteTexture != null)
            {

                if (Location.X > GraphicsDevice.PresentationParameters.BackBufferWidth - SpriteTexture.Width)
                {
                    Location.X = GraphicsDevice.PresentationParameters.BackBufferWidth - SpriteTexture.Width;
                }
            }


            
        }

        protected virtual void move(GameTime gameTime, float time)
        {

        }

        protected virtual Vector2 acceleration(Vector2 direction)
        {
            return direction;
        }

        protected virtual Vector2 braking(Vector2 direction)
        {
            return direction;
        }

        protected virtual Vector2 normalize(Vector2 direction)
        {
            if (direction != Vector2.Zero)
            {
                Vector2.Normalize(direction);
            }
            return direction;
        }

        protected virtual Vector2 facing()
        {
            return Direction;
        }

        public override void Draw(GameTime gameTime)
        {
            if (spriteTexture != null)
            {
                spriteBatch.Draw(spriteTexture, Location, null, color, 0f, Origin, 1f, SpriteEffects.None, 0f);
            }
            base.Draw(gameTime);
        }
    }
}
