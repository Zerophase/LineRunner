using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace FinalProject.Components
{
    public partial class WallChunk : GameComponent
    {
        private List<RectParent> rects;
 
        public WallChunk(Game game)
            : base(game)
        {
            rects = new List<RectParent>();
        }
    }

    public partial class WallChunk : IChunk
    {

        public IEnumerable<RectParent> CreateChunk(uint chunkSize)
        {
            for (int i = 0; i < chunkSize; i++)
            {
                if (i % 4 == 0)
                    rects.Add(new Rects.VerticalRect(Game, i));
                
               rects.Add(new HorizontalRect(Game, i));
            }

            return rects;
        }
    }
}