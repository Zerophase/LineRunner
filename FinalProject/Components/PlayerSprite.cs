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
using FinalProject.Components;

namespace FinalProject
{
    //TODO Work on Collision for all Chunks
    interface IPlayerSprite
    { 
    
    }
    public class PlayerSprite : SpriteCharacter, IPlayerSprite
    {
        InputHandler input;
        int jumpHeight = -500;
        int fallHeight = 500;
        Vector2 gravityDirection;
        float gravityAccel;
        bool jump;
        int collisionHolder;
        float tossBack = 200;
        float fallTo;
        Texture2D player;

        name currenName;

        CollisionManager collisionManager;

        ChunkConstructor chunkConstructor;

        name prevRectName;
        private enum movement { FORWARD, BACKWARD, STILL, FALL };

        private movement currentMovement;

        public static bool StopScreenMovement = false;

        public PlayerSprite(Game game)
            : base(game)
        {
            input = (InputHandler)Game.Services.GetService(typeof(IInputHandler));
            collisionManager = (CollisionManager)game.Services.GetService(typeof(ICollisionManager));
            chunkConstructor = (ChunkConstructor)game.Services.GetService(typeof(IChunkConstructor));

            currentMovement = movement.FORWARD;

            this.X = 100;
            this.Y = GraphicsDevice.PresentationParameters.BackBufferHeight;
            this.accel = 4f;
            this.speedMax = 5f;
            this.friction = 3f;
            this.gravityDirection = new Vector2(0, 1);
            this.gravityAccel = 5f;
        }

        public void Reset()
        {
            currentMovement = movement.FORWARD;

            this.X = 100;
            this.Y = 480;
            this.accel = 4f;
            this.speedMax = 5f;
            this.friction = 3f;
            this.gravityDirection = new Vector2(0, 1);
            this.gravityAccel = 5f;
        }

        protected override void LoadContent()
        {
            player = content.Load<Texture2D>("Player");
            Location = new Vector2(x, y);
            
            SpriteTexture = player;
            locationRect = SpriteTexture.Bounds;
            //this.origin = new Vector2(this.spriteTexture.Width / 2, this.spriteTexture.Height / 2);
            
            base.LoadContent();
            
            this.color = Color.White;
                    
        }
        
        public override void Update(GameTime gameTime)
        {
            SetTransform();
            if (GraphicsDevice.PresentationParameters.BackBufferHeight < Location.Y)
            {
                //resets the game if the player falls off screen.
            }

            if (currentMovement != movement.BACKWARD)
                fallTo = (GraphicsDevice.PresentationParameters.BackBufferWidth / 2);

            float time = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            move(gameTime, time);
            
            for (int i = 0; i < chunkConstructor.CollisionRect.Count; i++)
            {
                
                if (collisionManager.Intersects(this, chunkConstructor.CollisionRect.ElementAt<Components.RectParent>(i)))
                {
                    //Per pixel collision is broken.  It is broken because I'm stretching a 1 x 1 texture through out the shape.  Don't know how to fix.
                    //if (collisionManager.PerPixelCollision(this, chunkConstructor.CollisionRect.ElementAt<Components.RectParent>(i)))
                    //{
                    currenName = chunkConstructor.CollisionRect.ElementAt<Components.RectParent>(i).RectName;
                    //prevBlock =
                    switch (currenName)
                    {
                        case FinalProject.Components.name.GROUND:
                            jump = false;
                            if (Location.X < chunkConstructor.CollisionRect.ElementAt<Components.RectParent>(i).LocationRect.X
                                && (Location.Y - spriteTexture.Height) > chunkConstructor.CollisionRect.ElementAt<Components.RectParent>(i).LocationRect.Y)
                            {
                                StopScreenMovement = true;
                                fallTo = this.Location.X - tossBack;
                            }

                            if (currentMovement != movement.STILL && currentMovement != movement.BACKWARD
                                && currentMovement != movement.FALL)
                                currentMovement = movement.FORWARD;
                            //Location.Y =  //new Vector2(Location.X, chunkConstructor.CollisionRect.ElementAt<Components.RectParent>(i).LocationRect.Y -
                            //chunkConstructor.CollisionRect.ElementAt<Components.RectParent>(i).LocationRect.Height);
                            //chunkConstructor.CollisionRect.ElementAt<Components.RectParent>(i).LocationRect.Y
                            //- (this.spriteTexture.Height);
                            Direction.Y = 0.0f;

                            
                            break;
                        case FinalProject.Components.name.WALL:
                            if (Location.Y < chunkConstructor.CollisionRect.ElementAt<Components.RectParent>(i).LocationRect.Y
                                && Location.X > chunkConstructor.CollisionRect.ElementAt<Components.RectParent>(i).LocationRect.X
                                && Location.X < (chunkConstructor.CollisionRect.ElementAt<Components.RectParent>(i).LocationRect.X +
                                chunkConstructor.CollisionRect.ElementAt<Components.RectParent>(i).LocationRect.Width))
                            {
                                collisionHolder = i;
                                jump = false;
                                Direction.Y = 0.0f;
                            }
                            else if (Location.X > (chunkConstructor.CollisionRect.ElementAt<Components.RectParent>(i).LocationRect.X 
                                + chunkConstructor.CollisionRect.ElementAt<Components.RectParent>(i).LocationRect.Width))
                            {
                                //jump = false;
                                Direction.Y = 1;
                            }
                            else if (Location.X < chunkConstructor.CollisionRect.ElementAt<Components.RectParent>(i).LocationRect.X)
                            {
                                //Doesn't toss player back fast enough.
                                currentMovement = movement.BACKWARD;
                                fallTo = this.Location.X - tossBack;

                                StopScreenMovement = true;
                            }
                            else if (Location.X > chunkConstructor.CollisionRect.ElementAt<Components.RectParent>(i).LocationRect.X)
                            {
                                currentMovement = movement.FORWARD;
                            }
                            break;
                        case FinalProject.Components.name.PIT:
                            if (Location.X > chunkConstructor.CollisionRect.ElementAt<RectParent>(i).LocationRect.X)
                            {
                                //Need to play with fall speed.
                                currentMovement = movement.FALL;
                                Direction.Y = 400f;
                            }
                            
                            break;
                        default:
                            throw new Exception("Missed all Collision ending up in default.");
                    }
                 //}  
                }
                else if (!collisionManager.Intersects(this, chunkConstructor.CollisionRect.ElementAt<Components.RectParent>(i)))
                {

                    prevRectName = currenName;
                    if (prevRectName == Components.name.WALL &&
                        Location.X > (chunkConstructor.CollisionRect.ElementAt<Components.RectParent>(collisionHolder).LocationRect.X
                        + chunkConstructor.CollisionRect.ElementAt<Components.RectParent>(collisionHolder).LocationRect.Width) && collisionHolder != 0)
                    {
                        gravityAccel = 5f;
                        jump = true;
                        //Direction.Y = fallHeight;
                    }
                }
            }

            base.Update(gameTime);
        }

        protected override void move(GameTime gameTime, float time)
        {
            movementType();

            Direction = facing();
            jumping(); //Play around with position of Jump

            

            this.Location = Location + (Direction * (time / 1000));
            locationRect.X = (int)Location.X;
            locationRect.Y = (int)Location.Y;
            Direction = Direction + (gravityDirection * gravityAccel);
        }

       

        private void movementType()
        {
            if (Location.X < (GraphicsDevice.PresentationParameters.BackBufferWidth / 2) && Location.X < fallTo)
            {
                currentMovement = movement.FORWARD;
                StopScreenMovement = false;
            }    
            else if (currentMovement != movement.BACKWARD)
                currentMovement = movement.STILL;
        }

        protected override Vector2 facing()
        {
            if (currentMovement == movement.FORWARD)
            {
                Direction += new Vector2(1, 0);
            }
            if (currentMovement == movement.BACKWARD)
            {
                Direction += new Vector2(-1, 0);
            }
            if (currentMovement == movement.STILL)
            {
                Direction += new Vector2(0, 0);
            }
            if (currentMovement == movement.FALL)
            {
                Direction += new Vector2(0, 1);
            }
            return normalize(Direction);
        }

        protected override Vector2 normalize(Vector2 direction)
        {
            if (direction != Vector2.Zero)
            {
                Vector2.Normalize(direction);
            }
            return acceleration(direction);
        }

        //should probably make a seperate method for gravityAccel
        protected override Vector2 acceleration(Vector2 direction)
        {
            if (currentMovement == movement.BACKWARD)
            {
                gravityAccel = 5f;
                accel = 10;
                direction.X = Math.Min((speedMax * -1.0f), direction.X - accel);
            }
            if (currentMovement == movement.FORWARD && currentMovement != movement.FALL)
            {
                gravityAccel = 5f;
                accel = 4;
                direction.X = Math.Max(speedMax, direction.X + accel);
            }
            if (currentMovement == movement.STILL)
            {
                if (!jump)
                    gravityAccel = 0.0f;
              
                direction.X = 0.0f;
            }
            
            return direction;
        }

        private void jumping()
        {
            if (input.KeyboardState.IsKeyDown(Keys.Up) && !jump)//IF player holds key down a little jump is done followed by a big jump.
            {
                //Jump gets stuck on the ground. Every other time. Meaning player falls through the ground a bit and then triggers collision.
                //Keeps reverting between landing on the edge of the rect and passing slighly through.
                gravityAccel = 5f;
                jump = true;
                Direction.Y += jumpHeight;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            base.Draw(gameTime);
            spriteBatch.End();
        }
    }
}
