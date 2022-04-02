public class FlatHeightMapGenerator : HeightMapGenerator
{
    public override int[,] HeightMap(int mapSize, float smoothness, int heightVariation)
    {
        return new int[mapSize * 2 - 1, mapSize * 2 - 1];
    }
}