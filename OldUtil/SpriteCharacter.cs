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
    //FOR WHEN ANIMATION IS NEEDED.
    public class SpriteCharacter : SpriteComponent
    {
        public Color[] SpriteTextureData;
        
        protected Color color;
        //protected SpriteBatch spriteBatch;
        //public Texture2D SpriteTexture
        //{
        //    get { return spriteTexture; }
        //    set
        //    {
        //        spriteTexture = value;
        //        this.SpriteTextureData =
        //            new Color[this.spriteTexture.Width * this.spriteTexture.Height];
        //        this.spriteTexture.GetData(this.SpriteTextureData);

        //    }
        //}
        
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
           // bounds();
            //SetTransformAndRect();
            base.Update(gameTime);
        }

        
        //private void bounds()
        //{
        //    if (Location.Y < 0)
        //    {
        //        Location.Y = 0;
        //    }
        //    if (spriteTexture != null)
        //    {
        //        if (Location.Y > GraphicsDevice.PresentationParameters.BackBufferHeight - SpriteTexture.Height)
        //        {
        //            Location.Y = GraphicsDevice.PresentationParameters.BackBufferHeight - SpriteTexture.Height;
        //        }

        //        if (Location.X > GraphicsDevice.PresentationParameters.BackBufferWidth - SpriteTexture.Width)
        //        {
        //            Location.X = GraphicsDevice.PresentationParameters.BackBufferWidth - SpriteTexture.Width;
        //        }
        //    }
            

        //    if (Location.X < 0)
        //    {
        //        Location.X = 0;
        //    }
        //}

        //ASK WHY SPRITE WAS DRAWING TWICE
        //public override void Draw(GameTime gameTime)
        //{
        //    spriteBatch.Begin();
        //    this.Draw(spriteBatch);
        //    spriteBatch.End();
        //    //base.Draw(gameTime);
        //}
    }
}
