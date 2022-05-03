using System;

namespace HeightMaps
{
    public abstract class HexagonalHeightMap : HeightMap
    {
        public override int this[int i, int j]
        {
            get
            {
                if (Math.Abs(i - j) >= MapSize)
                {
                    throw new IndexOutOfRangeException("Index was out of the bounds of the hexagonal map.");
                }
                return Map[i, j];
            }
        }

        protected HexagonalHeightMap(int mapSize) : base(mapSize)
        {
            Map = new int[mapSize * 2 - 1, mapSize * 2 - 1];
        }
    }
}