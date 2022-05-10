using System;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

namespace Kernel.HeightMaps
{
    public class PerlinHeightMapGenerator
    {
        private readonly float _smoothness;
        private readonly int _heightVariation;

        private readonly int _noiseShiftMin;
        private readonly int _noiseShiftMax;

        public PerlinHeightMapGenerator(float smoothness, int heightVariation)
        {
            _smoothness = smoothness;
            _heightVariation = heightVariation;
            _noiseShiftMin = 10000;
            _noiseShiftMax = 100000;
        }

        public HexMap<int> GenerateHeightMap(int mapSize)
        {
            var map = new HexMap<int>(mapSize);
            var noiseShift = Random.Range(_noiseShiftMin, _noiseShiftMax);
            for (var i = 0; i < mapSize * 2 - 1; i++)
            {
                for (var j = 0; j < mapSize * 2 - 1; j++)
                {
                    if (Math.Abs(i - j) >= mapSize) continue;
                    var aPos = i - mapSize + 1;
                    var bPos = j - mapSize + 1;
                    var height = (int)(Mathf.PerlinNoise(aPos / _smoothness + noiseShift,
                        bPos / _smoothness + noiseShift) * _heightVariation);
                    if (height >= _heightVariation)
                        height = _heightVariation - 1;
                    else if (height < 0)
                        height = 0;
                    map[new HexPos(aPos + mapSize - 1, bPos + mapSize - 1)] = height;
                }
            }
            return map;
        }
    }
}
