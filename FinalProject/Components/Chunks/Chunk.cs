using System;
using System.Collections.Generic;
using FinalProject.Components;
using Microsoft.Xna.Framework;

namespace FinalProject
{
    public abstract class Chunk : SpriteCharacter
    {
        protected List<RectParent> tiles;
        public List<RectParent> Tiles {get { return tiles; }} 

        public Chunk(Game game)
            : base(game)
        {
            tiles = new List<RectParent>();
            LoadContent();

            accel = 40f;
            speedMax = 200f;
        }

        public void Move(float time)
        {
            if (!PlayerSprite.StopScreenMovement)
            {
                Direction = facing();
                Direction = normalize(Direction);
                //just returns direction
                Direction = braking(Direction);
                Direction = acceleration(Direction);

                this.Location = this.Location + (Direction * (time / 1000));

                moveChunk();
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

        private void moveChunk()
        {
            foreach (var tile in tiles)
            {
                Rectangle r = tile.LocationRect;
                r.X = (int)(this.Location.X + tile.X);
                tile.LocationRect = r;
            }
        }

        public bool DrawNextChunk()
        {
            return Location.X < -2500;
        }

        public void UpdateLocation(Vector2 previousChunkPosition)
        {
            Location = previousChunkPosition;
        }

        public void Reset()
        {
            Location = Vector2.Zero;
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            foreach (var tile in tiles)
            {
                spriteBatch.Draw(tile.SpriteTexture, tile.LocationRect,
                    Color.White);
            }
            spriteBatch.End();
        }
    }
}