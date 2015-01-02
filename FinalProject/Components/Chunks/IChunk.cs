using System.Collections.Generic;

namespace FinalProject.Components
{
    public interface IChunk
    {
        IEnumerable<RectParent> CreateChunk(uint chunkSize);
    }
}