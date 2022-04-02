using System;
using UnityEngine;
using Random = UnityEngine.Random;

public static class HeightMapGenerator
{
    private const int NoiseShiftMin = 10000;
    private const int NoiseShiftMax = 100000;
    
    public static int[,] HeightMap(int mapSize, float smoothness, int heightVariation)
    {
        var heightMap = new int[mapSize * 2 - 1, mapSize * 2 - 1];
        var noiseShift = Random.Range(NoiseShiftMin, NoiseShiftMax);
        for (var i = 0; i < mapSize * 2 - 1; i++)
        {
            for (var j = 0; j < mapSize * 2 - 1; j++)
            {
                if (Math.Abs(i - j) >= mapSize) continue;
                var aPos = i - mapSize + 1;
                var bPos = j - mapSize + 1;
                var height = (int)(Mathf.PerlinNoise((aPos / smoothness) + noiseShift,
                    (bPos / smoothness) + noiseShift) * heightVariation);
                if (height >= heightVariation)
                    height = heightVariation - 1;
                else if (height < 0)
                    height = 0;
                heightMap[aPos + mapSize - 1, bPos + mapSize - 1] = height;
            }
        }

        return heightMap;
    }
}