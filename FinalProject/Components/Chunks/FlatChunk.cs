using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace FinalProject.Components
{
    public partial class FlatChunk : GameComponent
    {
        private List<HorizontalRect> horizontalRect;

        public FlatChunk(Game game)
            : base(game)
        {
            game.Services.AddService(typeof(IChunk), this);
            horizontalRect = new List<HorizontalRect>();
        }
    }

    public partial class FlatChunk : IChunk
    {
        public IEnumerable<RectParent> CreateChunk(uint chunkSize)
        {
            for (int i = 0; i < chunkSize; i++)
            {
                horizontalRect.Add(new HorizontalRect(Game, i));
            }

            return horizontalRect;
        }
    }
}