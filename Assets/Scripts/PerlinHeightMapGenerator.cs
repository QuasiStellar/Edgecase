using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class PerlinHeightMapGenerator : HeightMapGenerator
{
    private const float Smoothness = 7f;
    private const int HeightVariation = 7;
    
    private const int NoiseShiftMin = 10000;
    private const int NoiseShiftMax = 100000;
    
    public override int[,] HeightMap(int mapSize)
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
                var height = (int)(Mathf.PerlinNoise((aPos / Smoothness) + noiseShift,
                    (bPos / Smoothness) + noiseShift) * HeightVariation);
                if (height >= HeightVariation)
                    height = HeightVariation - 1;
                else if (height < 0)
                    height = 0;
                heightMap[aPos + mapSize - 1, bPos + mapSize - 1] = height;
            }
        }

        return heightMap;
    }
}