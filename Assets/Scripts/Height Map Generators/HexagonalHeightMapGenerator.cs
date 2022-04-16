using System;

public abstract class HexagonalHeightMapGenerator : HeightMapGenerator
{
    private const int BottomHeight = -1000;
    
    public override int[,] HeightMap(int mapSize)
    {
        return new int[mapSize * 2 - 1, mapSize * 2 - 1];
    }

    protected static int[,] LowerEdges(int[,] heightMap, int mapSize)
    {
        for (var i = 0; i < mapSize * 2 - 1; i++)
        {
            for (var j = 0; j < mapSize * 2 - 1; j++)
            {
                if (Math.Abs(i - j) >= mapSize) continue;
                if (i == 0 ||
                    j == 0 ||
                    i - j == mapSize - 1 ||
                    j - i == mapSize - 1 ||
                    i == (mapSize - 1) * 2 ||
                    j == (mapSize - 1) * 2)
                {
                    heightMap[i, j] = BottomHeight;
                }
            }
        }
        return heightMap;
    }
}