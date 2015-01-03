using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace FinalProject.Components
{
    public partial class WallChunk : Chunk
    {
 
        public WallChunk(Game game)
            : base(game)
        {
        }
    }

    public partial class WallChunk : IChunk
    {

        public void CreateChunk(uint chunkSize)
        {
            for (int i = 0; i < chunkSize; i++)
            {
                if (i % 4 == 0)
                    tiles.Add(new Rects.VerticalRect(Game, i));
                
               tiles.Add(new HorizontalRect(Game, i));
            }
        }
    }
}