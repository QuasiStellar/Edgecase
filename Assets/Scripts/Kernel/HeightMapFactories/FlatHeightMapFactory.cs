using System;
using System.Collections.Generic;
using Utils;

namespace Kernel.HeightMapFactories
{
    public class FlatHeightMapFactory : IHeightMapFactory
    {
        private readonly int _height;

        public FlatHeightMapFactory(int height)
        {
            _height = height;
        }

        public HexMap<int> BuildHeightMap(ISet<HexPos> mapShape)
        {
            var mapContent = new Dictionary<HexPos, int>();
            foreach (var hexPos in mapShape)
            {
                mapContent[hexPos] = _height;
            }
            return new HexMap<int>(mapContent);
        }
    }
}
