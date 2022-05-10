using Utils;

namespace Kernel.HeightMapFactories
{
    public interface IHeightMapFactory
    {
        HexMap<int> BuildHeightMap(int mapSize);
    }
}
