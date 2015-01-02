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
    public class SpriteComponent : Microsoft.Xna.Framework.DrawableGameComponent
    {
        protected SpriteBatch spriteBatch;
        public Texture2D spriteTexture;
        public SpriteEffects SpriteEffects;
        protected ContentManager content;
        protected GraphicsDeviceManager graphics;

        protected Matrix spriteTransform;
        //public Matrix SpriteTransform;

        protected Rectangle locationRect;
        public Rectangle LocationRect { get { return locationRect; } set { locationRect = value; } }

        public Vector2 Location;
        public Vector2 Direction;
        public Vector2 Origin;
        protected float scale = 1.0f;
        public float Scale
        {
            get { return this.scale; }
            set
            {
                if (value != this.scale)
                {
                    SetTransformAndRect();
                }
                this.scale = value;
            }
        }
        protected float lastUpdateTime;
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
            content = this.Game.Content;//game.Content;
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
            lastUpdateTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            SetTransformAndRect();
            base.Update(gameTime);
        }

        protected virtual void SetTransformAndRect()
        {
            try
            {
                spriteTransform =
                    Matrix.CreateTranslation(new Vector3(Origin * -1, 0.0f)) *
                    Matrix.CreateScale(this.scale) *
                    Matrix.CreateRotationZ(0.0f) *
                    Matrix.CreateTranslation(new Vector3(this.Location, 0.0f));

                this.locationRect = CalculateBoundingRectangle(
                             new Rectangle(0, 0, this.spriteTexture.Width,
                                 this.spriteTexture.Height),
                             spriteTransform);
            }
            catch (NullReferenceException nu)
            {
                //nothing
                if (this.spriteTexture == null)
                {
                    //first time this will fail because load content hasn't been called yet

                }
                else
                {
                    throw nu;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Rectangle CalculateBoundingRectangle(Rectangle rectangle, Matrix transform)
        {
            // Get all four corners in local space
            Vector2 leftTop = new Vector2(rectangle.Left, rectangle.Top);
            Vector2 rightTop = new Vector2(rectangle.Right, rectangle.Top);
            Vector2 leftBottom = new Vector2(rectangle.Left, rectangle.Bottom);
            Vector2 rightBottom = new Vector2(rectangle.Right, rectangle.Bottom);

            // Transform all four corners into work space
            Vector2.Transform(ref leftTop, ref transform, out leftTop);
            Vector2.Transform(ref rightTop, ref transform, out rightTop);
            Vector2.Transform(ref leftBottom, ref transform, out leftBottom);
            Vector2.Transform(ref rightBottom, ref transform, out rightBottom);

            // Find the minimum and maximum extents of the rectangle in world space
            Vector2 min = Vector2.Min(Vector2.Min(leftTop, rightTop),
                                      Vector2.Min(leftBottom, rightBottom));
            Vector2 max = Vector2.Max(Vector2.Max(leftTop, rightTop),
                                      Vector2.Max(leftBottom, rightBottom));

            // Return that as a rectangle
            return new Rectangle((int)min.X, (int)min.Y,
                                 (int)(max.X - min.X), (int)(max.Y - min.Y));
        }

        //ASK WHY SPRITE WAS DRAWING TWICE
        //public virtual void Draw(SpriteBatch spriteBatch)
        //{
        //    spriteBatch.Draw(spriteTexture,
        //        new Rectangle(
        //            (int)Location.X,
        //            (int)Location.Y,
        //            (int)(spriteTexture.Width * this.Scale),
        //            (int)(spriteTexture.Height * this.Scale)),
        //        null,
        //        Color.White,
        //        0.0f,//rotate
        //        this.Origin,
        //        SpriteEffects,
        //        0);
        //}
    }
}
