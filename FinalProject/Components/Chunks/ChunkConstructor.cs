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
        public List<RectParent> CollisionRect;
        private Dictionary<chunkType, Chunk> chunks =
            new Dictionary<chunkType, Chunk>();

        private readonly Random rand;

        private const int minChunk = 0;
        private const int maxChunk = 3;

        private enum chunkType { FLAT = 0, WALLS, WALLNPITS };
        private chunkType currentChunk;
        private chunkType previousChunk;

        private GameConsole console;
        private InputHandler input;

        private FlatChunk flatChunk;
        private WallChunk wallChunk;
        private WallAndPitChunk wallAndPitChunk;

        private bool gameStart = true;

        public ChunkConstructor(Game game)
            : base(game)
        {
            game.Services.AddService(typeof(IChunkConstructor), this);

            console = (GameConsole)game.Services.GetService(typeof(IGameConsole));

            input = (InputHandler)game.Services.GetService(typeof(IInputHandler));

            flatChunk = new FlatChunk(game);
            wallChunk = new WallChunk(game);
            wallAndPitChunk = new WallAndPitChunk(game);

            CollisionRect = new List<RectParent>();

            speedMax = 200f;
            rand = new Random();
        }

         public override void Initialize()
        {
            this.Location = new Vector2(0, 0);
            this.accel = 40;

            this.Direction = new Vector2(0, 0);

            previousChunk = currentChunk = 0;
            if(gameStart)
                generateChunk();

            gameStart = false;
            AddChunkToCollisionRect(chunks[currentChunk].Tiles);
            base.Initialize();
        }

         public void Reset()
         {
             wallChunk.Reset();
             wallAndPitChunk.Reset();
             flatChunk.Reset();
             this.Initialize();
         }

        private void AddChunkToCollisionRect(List<RectParent> rects)
        {
            CollisionRect.AddRange(rects);
        }

        
        public override void Update(GameTime gameTime)
        {
            if (chunks[currentChunk].DrawNextChunk())
            {
                previousChunk = currentChunk;
                var exclude = new HashSet<int>{(int)currentChunk};
                var range = Enumerable.Range(minChunk, maxChunk).Where(i => !exclude.Contains(i));
                int index = rand.Next(minChunk, maxChunk - exclude.Count);
                currentChunk = (chunkType)range.ElementAt(index);
                
                chunks[currentChunk].UpdateLocation(chunks[previousChunk].Location + new Vector2( 
                    (100f * 36f), 0f));

                AddChunkToCollisionRect(chunks[currentChunk].Tiles);
            }


            if (CollisionRect.First().X + 100 < 0)
                CollisionRect.RemoveAt(0);

            float time = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (previousChunk != currentChunk)
                chunks[previousChunk].Move(time);
            chunks[currentChunk].Move(time);

            base.Update(gameTime);
        }

        private void generateChunk()
        {
            flatChunk.CreateChunk(36);
            wallChunk.CreateChunk(36);
            wallAndPitChunk.CreateChunk(36);
            chunks.Add(chunkType.FLAT, flatChunk);
            chunks.Add(chunkType.WALLS, wallChunk);
            chunks.Add(chunkType.WALLNPITS, wallAndPitChunk);
        }

        public override void Draw(GameTime gameTime)
        {
            if (previousChunk != currentChunk)
                chunks[previousChunk].Draw(gameTime);
            chunks[currentChunk].Draw(gameTime);
        }
    }
}
