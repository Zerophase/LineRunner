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
    interface IGameObjects
    { 
    
    }

    public class GameObjects : SpriteCharacter, IGameObjects
    {
        public ChunkConstructor Chunks;

        private bool selectChunk = true;
        public GameObjects(Game game)
            : base(game)
        {
            game.Services.AddService(typeof(IGameObjects), this);
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            if (selectChunk)
            {
                Chunks = new ChunkConstructor(Game);
                selectChunk = false;
            }
           
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


            base.move(gameTime, time);
        }
    }
}
