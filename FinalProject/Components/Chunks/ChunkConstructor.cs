using System;
using System.CodeDom;
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
    //TODO Speed up movement speed of Ground
    //LAST TODO Change so chunk constructor creates all of the chunks saves them to a dictionary, 
    //and then a level builder selects between the saved chunks. 
    interface IChunkConstructor
    { 
    }
    public class ChunkConstructor : SpriteCharacter, IChunkConstructor
    {
        private List<Rects.BlankRect> blankRect;

        public List<RectParent> CollisionRect;
        private Dictionary<IChunk, List<RectParent>> chunks =
            new Dictionary<IChunk, List<RectParent>>();

        private Random rand;
        private int itemCount = 0;
        private int timeThrough = 36;
        private int time;

        private int minChunk = 0;
        private int maxChunk = 3;

        private Vector2 locationToChunk = new Vector2(0, 0);

        private enum chunkType { FLAT = 0, WALLS, WALLNPITS };
        private chunkType currentChunk;

        private GameConsole console;
        private InputHandler input;

        private FlatChunk flatChunk;
        private WallChunk wallChunk;

        private float screenWidth;
        public ChunkConstructor(Game game)
            : base(game)
        {
            game.Services.AddService(typeof(IChunkConstructor), this);

            console = (GameConsole)game.Services.GetService(typeof(IGameConsole));

            input = (InputHandler)game.Services.GetService(typeof(IInputHandler));

            flatChunk = new FlatChunk(game);
            wallChunk = new WallChunk(game);

           screenWidth = game.GraphicsDevice.Viewport.Bounds.Width;
            //verticalRect = new List<Rects.VerticalRect>();
            blankRect = new List<Rects.BlankRect>();

            CollisionRect = new List<RectParent>();

            speedMax = 200f;
            rand = new Random();
        }

         public override void Initialize()
        {
            if (CollisionRect.Count > 0)
            {
                CollisionRect.Clear();
            }

            //if (horizontalRect.Count > 0)
            //    horizontalRect.Clear();

            //if (verticalRect.Count > 0)
            //    verticalRect.Clear();

            if (blankRect.Count > 0)
                blankRect.Clear();

            if (timeThrough > 20)
            {
                timeThrough = 36;
                itemCount = 0;
            }
            this.Location = new Vector2(0, 0);
            this.accel = 40;

            this.Direction = new Vector2(0, 0);

            currentChunk = (chunkType)0;//(chunkType)rand.Next(minChunk, maxChunk);
            if(chunks.Count == 0)
                generateChunk();

            base.Initialize();
        }

         public void Reset()
         {
             gameStart = false;
             this.Initialize();
         }

        private bool gameStart = true;
        public override void Update(GameTime gameTime)
        {
            //generateChunk(gameTime);
            
            if (CollisionRect.Count == 0)
            {
                if (gameStart)
                {
                    this.Location = new Vector2(0f, 0f);
                    this.locationToChunk = new Vector2(0f, 0f);
                }
                else
                {
                    this.Location = new Vector2(screenWidth, 0f);
                    this.locationToChunk = new Vector2(screenWidth, 0f);
                }
                gameStart = false;
                switch ((chunkType)rand.Next(minChunk, 2))
                {
                    case chunkType.FLAT:
                        CollisionRect.AddRange(chunks[flatChunk]);
                        break;
                    case chunkType.WALLS:
                        CollisionRect.AddRange(chunks[wallChunk]);
                        break;
                }
            }

            if (blankRect.Count != 0)
            {
                CollisionRect.AddRange(blankRect);
                blankRect.Clear();
            }
            float time = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            move(gameTime, time);

            //Ask about removing items pecisely
            if (CollisionRect.First<RectParent>().LocationRect.X < -100)
            {
                CollisionRect.RemoveAt(0);
            }
            base.Update(gameTime);
        }

        private void generateChunk()
        {
            if (this.locationToChunk.X < -2000)
            {
                currentChunk = (chunkType)rand.Next(minChunk, maxChunk);

                console.GameConsoleWrite(currentChunk.ToString());

                this.locationToChunk.X = 0;

                time = 0;
                //needs 8 more tiles per chunk
            }

            chunks.Add(flatChunk, flatChunk.CreateChunk(36).ToList());
            chunks.Add(wallChunk, wallChunk.CreateChunk(36).ToList());
            switch ((chunkType)1)
            {
                case chunkType.FLAT:
                    break;
                case chunkType.WALLS:
                    //vert rects every other 200 pix, horizontal rects every 100 pix
                    //while (itemCount < timeThrough)
                    //{
                    //    if (itemCount % 4 == 0 && itemCount != timeThrough)
                    //    {
                    //        verticalRect.Add(new Rects.VerticalRect(Game, itemCount));
                    //    }
                    //    // horizontalRect.Add(new HorizontalRect(Game, itemCount++));
                    //}
                    break;
                case chunkType.WALLNPITS:
                    //3 straight rects, 2 blank rects, 1 vert rects, 3 horizontal rects
                    //3 blank rects 1 horizontal
                    while (itemCount < timeThrough)
                    {
                        if (itemCount == (timeThrough - 7) && itemCount % 12 != 0)
                        {
                            //verticalRect.Add(new Rects.VerticalRect(Game, itemCount));
                        }

                        if ((itemCount < (3 + (timeThrough - 12))) || (itemCount > (4 + (timeThrough - 12)) && itemCount < (8 + (timeThrough - 12))) || itemCount == (timeThrough - 1))
                        {
                            // horizontalRect.Add(new HorizontalRect(Game, itemCount));
                        }

                        if ((itemCount > (2 + (timeThrough - 12)) && itemCount < (5 + (timeThrough - 12))) || (itemCount > (7 + (timeThrough - 12)) && itemCount < (11 + (timeThrough - 12))))
                        {
                            blankRect.Add(new Rects.BlankRect(Game, itemCount));
                        }

                        itemCount++;
                    }

                    break;
                default:
                    break;
            }
        }

        private void generateChunk(GameTime gameTime)
        {   
            time += gameTime.ElapsedGameTime.Milliseconds;
            if (this.locationToChunk.X < -2000)
            {
                currentChunk = (chunkType)rand.Next(minChunk, maxChunk);

                console.GameConsoleWrite(currentChunk.ToString());

                this.locationToChunk.X = 0;

                time = 0;
                timeThrough += 36;
                //needs 8 more tiles per chunk
            }
            
            switch ((chunkType)0)
            {
                case chunkType.FLAT:
                    while (itemCount < timeThrough)
                    {
                        //horizontalRect.Add(new HorizontalRect(Game, itemCount++));
                    }
                    break;
                case chunkType.WALLS:
                    //vert rects every other 200 pix, horizontal rects every 100 pix
                    while (itemCount < timeThrough)
                    {
                        if (itemCount % 4 == 0 && itemCount != timeThrough)
                        {
                           // verticalRect.Add(new Rects.VerticalRect(Game, itemCount));
                        }
                       // horizontalRect.Add(new HorizontalRect(Game, itemCount++));
                    }
                    break;
                case chunkType.WALLNPITS:
                    //3 straight rects, 2 blank rects, 1 vert rects, 3 horizontal rects
                    //3 blank rects 1 horizontal
                    while (itemCount < timeThrough)
                    {
                        if (itemCount == (timeThrough - 7) && itemCount % 12 != 0)
                        {
                            //verticalRect.Add(new Rects.VerticalRect(Game, itemCount));
                        }

                        if ((itemCount < (3 + (timeThrough - 12))) || (itemCount > (4 + (timeThrough - 12)) && itemCount < (8 + (timeThrough -12))) || itemCount == (timeThrough - 1))
                        {
                           // horizontalRect.Add(new HorizontalRect(Game, itemCount));
                        }

                        if ((itemCount > (2 + (timeThrough - 12)) && itemCount < (5 + (timeThrough - 12))) || (itemCount > (7 +(timeThrough - 12)) && itemCount < (11 + (timeThrough -12))))
                        {
                            blankRect.Add(new Rects.BlankRect(Game, itemCount));
                        }

                        itemCount++;
                    }
                    
                    break;
                default:
                    break;
            }
        }

        // TODO Movemovement of chunks outside of ChunkConstructor
        protected override void move(GameTime gameTime, float time)
        {
            if (!PlayerSprite.StopScreenMovement)
            {
                Direction = facing();
                Direction = normalize(Direction);
                //just returns direction
                Direction = braking(Direction);
                Direction = acceleration(Direction);

                this.Location = this.Location + (Direction * (time / 1000));

                this.locationToChunk = this.locationToChunk + (Direction * (time / 1000));
                //Direction = Direction;

                moveCollisionRect();
                base.move(gameTime, time);
            }
        }

        private void moveCollisionRect()
        {
            foreach (RectParent rectangle in CollisionRect)
            {
                rectangle.LocationRect = new Rectangle((int)(Location.X + rectangle.X), rectangle.LocationRect.Y,
                    rectangle.LocationRect.Width, rectangle.LocationRect.Height);
            }
        }

        protected override Vector2 acceleration(Vector2 direction)
        {
            direction.X = Math.Max((speedMax * -1), direction.X - accel);
            return direction;
        }

        //TODO add logic to stop movement when player is knocked back
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
            Direction += new Vector2(-1, 0);
            return Direction;
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            foreach (RectParent rectangle in CollisionRect)
            {
                spriteBatch.Draw(rectangle.SpriteTexture, rectangle.LocationRect, Color.White);
            }
            spriteBatch.End();
        }
    }
}
