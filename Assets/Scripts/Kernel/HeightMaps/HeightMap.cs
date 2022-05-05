using System.Collections;
using System.Collections.Generic;
using Utils;

namespace HeightMaps
{
    public abstract class HeightMap : IEnumerable
    {
        protected readonly HexMap<int> Map;

        public int this[HexPos hexPos] => Map[hexPos];

        protected HeightMap(int mapSize)
        {
            Map = new HexMap<int>(mapSize);
        }
        
        public IEnumerator<KeyValuePair<HexPos, int>> GetEnumerator()
        {
            return Map.GetEnumerator();
        }
        
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}