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
    interface ICollidableObjects
    { 
    
    }
    public class CollidableObjects : SpriteCharacter, ICollidableObjects
    {
        //TODO Make list of rects that add a rect to the end of the previous one.
        private List<RectangleSpawner> collisionRect;
        public List<RectangleSpawner> CollisionRect { get { return collisionRect; } }
        //Rect Spawner, creates rects when needed.  In here adds those rects to list of rects.
  
        private List<RectangleSpawner> vertCollisionRect;
        public List<RectangleSpawner> VertCollisionRect { get { return vertCollisionRect; } }

        private List<RectangleSpawner> blankRect;
        public List<RectangleSpawner> BlankRect { get { return blankRect; } }

        private int intitialRectangles = 12;
        private int rectangleCount = 0;

        private int multiplier = 1;

        public CollidableObjects(Game game)
            : base(game)
        {
            game.Services.AddService(typeof(ICollidableObjects), this);

            collisionRect = new List<RectangleSpawner>();
            vertCollisionRect = new List<RectangleSpawner>();

            if (collisionRect == null)
                throw new Exception("Add CollisionRect to CollidableObjects");
        }

        public override void Initialize()
        {
            for (int i = 0; i < intitialRectangles; i++)
            {
                //Put back
                //collisionRect.Add( new RectangleSpawner(this.Game, rectangleCount++, RectangleSpawner.RectangleType.BLANK));
                //blankRect.Add(new RectangleSpawner(this.Game, rectangleCount++, RectangleSpawner.RectangleType.BLANK));
                //So test works.
                //All is being refactored for procedural redesign.
                collisionRect.Add(new RectangleSpawner(this.Game, rectangleCount++, RectangleSpawner.RectangleType.HORIZONTAL));
            }
            this.location = new Vector2(0, 0);
            this.accel = 100;

            this.direction = new Vector2(0, 0);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            float time = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            move(gameTime, time);
            
            base.Update(gameTime);
        }

        protected override void move(GameTime gameTime, float time)
        {
            direction = facing();
            direction = normalize(direction);
            //just returns direction
            direction = braking(direction);
            direction = acceleration(direction);

            this.location = this.location + (direction * (time / 1000));

            direction = direction;


            if (collisionRect.ElementAt<RectangleSpawner>(0).LocationRect.X < -100)
            {
                collisionRect.RemoveAt(0);
                collisionRect.Add( new RectangleSpawner(this.Game, rectangleCount++, RectangleSpawner.RectangleType.HORIZONTAL));
            }

            if ((int)location.X <= -500 * multiplier)
            {
                multiplier++;
                if (vertCollisionRect.Count > 0 && vertCollisionRect.ElementAt<RectangleSpawner>(0).LocationRect.X < -100)
                    vertCollisionRect.RemoveAt(0);

                vertCollisionRect.Add(new RectangleSpawner(this.Game, rectangleCount, RectangleSpawner.RectangleType.VERTICAL));
            }

            foreach (RectangleSpawner rectangle in collisionRect)
            {
                rectangle.LocationRect = new Rectangle((int)(location.X + rectangle.X), rectangle.LocationRect.Y,
                    rectangle.LocationRect.Width, rectangle.LocationRect.Height);
            }

            if (vertCollisionRect.Count > 0)
            {
                foreach (RectangleSpawner vertRectangle in vertCollisionRect)
                {
                    vertRectangle.LocationRect = new Rectangle((int)(location.X + vertRectangle.X), vertRectangle.LocationRect.Y,
                    vertRectangle.LocationRect.Width, vertRectangle.LocationRect.Height);
                }
            }
        }

        protected override Vector2 acceleration(Vector2 direction)
        {
            direction.X = direction.X - accel;
            return direction;
        }

        protected override Vector2 braking(Vector2 direction)
        {
            return direction;
        }

        protected override Vector2 normalize(Vector2 direction)
        {
            if (direction != Vector2.Zero)
                Vector2.Normalize(direction);

            return direction;
        }
        protected override Vector2 facing()
        {
            direction += new Vector2(-1, 0);
            return direction;
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            foreach (RectangleSpawner rectangle in collisionRect)
	        {
                spriteBatch.Draw(rectangle.horizontalRect.SpriteTexture, rectangle.horizontalRect.LocationRect, Color.White);
	        }

            foreach (RectangleSpawner vertRectangle in vertCollisionRect)
            {
                spriteBatch.Draw(vertRectangle.verticleRect.SpriteTexture, vertRectangle.verticleRect.LocationRect, Color.White);
            }
            spriteBatch.End();  
            //spriteBatch.Begin();
            //base.Draw(gameTime);
            //spriteBatch.End();
        }

        public static void DrawRect(SpriteBatch spriteBatch, Rectangle rect, int count, Color color)
        {
            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    
                }
            }
        }
    }
}
