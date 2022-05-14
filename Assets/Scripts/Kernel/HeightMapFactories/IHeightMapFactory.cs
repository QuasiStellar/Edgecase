using System.Collections.Generic;
using Utils;

namespace Kernel.HeightMapFactories
{
    public interface IHeightMapFactory
    {
        HexMap<int> BuildHeightMap(ISet<HexPos> mapShape);
    }
}
