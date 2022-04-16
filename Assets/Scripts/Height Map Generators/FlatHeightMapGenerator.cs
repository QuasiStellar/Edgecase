using System;

public class FlatHeightMapGenerator : HexagonalHeightMapGenerator
{
    public override int[,] HeightMap(int mapSize)
    {
        return LowerEdges(base.HeightMap(mapSize), mapSize);
    }
}