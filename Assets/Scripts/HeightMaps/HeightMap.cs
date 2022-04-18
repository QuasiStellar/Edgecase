namespace HeightMaps
{
    public abstract class HeightMap
    {
        protected int[,] Map;
        protected readonly int MapSize;

        public virtual int this[int i, int j] => Map[i, j];
        
        protected HeightMap(int mapSize)
        {
            MapSize = mapSize;
        }
    }
}