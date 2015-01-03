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

namespace FinalProject
{
    public class SpriteComponent : Microsoft.Xna.Framework.DrawableGameComponent
    {
        protected SpriteBatch spriteBatch;

        public Color[] SpriteTextureData;

        protected Texture2D spriteTexture;

        //Rework so public isn't needed for setting SpriteTextureData.
        public Texture2D SpriteTexture
        {
            get { return spriteTexture; }
            set
            {
                spriteTexture = value;

                this.SpriteTextureData =
                    new Color[this.spriteTexture.Width * this.spriteTexture.Height];
                this.spriteTexture.GetData(this.SpriteTextureData);
            }
        }

        protected Rectangle locationRect;
        public Rectangle LocationRect { get { return locationRect; } set { locationRect = value; } }

        protected Rectangle shapeRect;
        public Rectangle ShapeRect { get { return shapeRect; } set { shapeRect = value; } }

        protected ContentManager content;
        protected GraphicsDeviceManager graphics;
        
        public Vector2 Location;
        public Vector2 Direction;
        public Vector2 Origin;
        protected float scale = 1;

        protected float x;
        public float X
        {
            get
            {
                return x;
            }
            set
            {
                x = value;
            }
        }
        protected float y;
        public float Y
        {
            get
            {
                return y;
            }
            set
            {
                y = value;
            }
        }

        public SpriteComponent(Game game) 
            : base(game)
        {
            content = this.Game.Content;
        }

        public override void Initialize()
        {
            
            //Believe Game1 was overwriting this, and I couldn't figure out how to fix without breaking everything.
            //graphics = (GraphicsDeviceManager)Game.Services.GetService(typeof(IGraphicsDeviceManager));
            //graphics.PreferredBackBufferHeight = 2024;
            //graphics.PreferredBackBufferWidth = 768;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(this.Game.GraphicsDevice);
            //this.origin = Vector2.Zero;
 	        base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}
