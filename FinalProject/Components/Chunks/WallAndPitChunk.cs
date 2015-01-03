using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace FinalProject.Components
{
    public partial class WallAndPitChunk : Chunk
    {
        public WallAndPitChunk(Game game)
            : base(game)
        {
            tiles = new List<RectParent>();
        }
    }

    public partial class WallAndPitChunk : IChunk
    {
        public void CreateChunk(uint chunkSize)
        {
            for (int i = 0; i < chunkSize; i++)
            {
                if (i == 5 || i == 18 || i == 31)
                {
                    tiles.Add(new Rects.VerticalRect(Game, i));
                }

                if (i < 3  || (i > 4 && i < 9 ) || 
                    (i > 11 && i < 16) || (i > 17 && i < 22) ||  
                    (i > 24 && i < 29) || (i > 30 && i < 35))
                {
                    tiles.Add(new HorizontalRect(Game, i));
                }

                if ((i > 2 && i < 5) || (i > 8 && i < 12 ) ||
                    (i > 15 && i < 18) || (i > 21 && i < 25) ||
                    (i > 28 && i < 32) || i == 35)
                {
                    tiles.Add(new Rects.BlankRect(Game, i));
                }
            }
        }
    }
}