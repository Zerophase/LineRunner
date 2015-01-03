using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace FinalProject.Components
{
    public partial class FlatChunk : Chunk
    {

        public FlatChunk(Game game)
            : base(game)
        {
        }
    }

    public partial class FlatChunk : IChunk
    {
        public void CreateChunk(uint chunkSize)
        {
            for (int i = 0; i < chunkSize; i++)
            {
                tiles.Add(new HorizontalRect(Game, i));
            }
        }
    }
}