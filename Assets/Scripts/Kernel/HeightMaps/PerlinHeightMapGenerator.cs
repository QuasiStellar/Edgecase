using System;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

namespace Kernel.HeightMaps
{
    public class PerlinHeightMapGenerator
    {
        private const float Smoothness = 7f;
        private const int HeightVariation = 7;

        private const int NoiseShiftMin = 10000;
        private const int NoiseShiftMax = 100000;

        public HexMap<int> GenerateHeightMap(int mapSize)
        {
            var map = new HexMap<int>(mapSize);
            var noiseShift = Random.Range(NoiseShiftMin, NoiseShiftMax);
            for (var i = 0; i < mapSize * 2 - 1; i++)
            {
                for (var j = 0; j < mapSize * 2 - 1; j++)
                {
                    if (Math.Abs(i - j) >= mapSize) continue;
                    var aPos = i - mapSize + 1;
                    var bPos = j - mapSize + 1;
                    var height = (int)(Mathf.PerlinNoise(aPos / Smoothness + noiseShift,
                        bPos / Smoothness + noiseShift) * HeightVariation);
                    if (height >= HeightVariation)
                        height = HeightVariation - 1;
                    else if (height < 0)
                        height = 0;
                    map[new HexPos(aPos + mapSize - 1, bPos + mapSize - 1)] = height;
                }
            }
            return map;
        }
    }
}
