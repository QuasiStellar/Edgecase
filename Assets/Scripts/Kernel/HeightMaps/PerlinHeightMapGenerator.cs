using System;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

namespace Kernel.HeightMaps
{
    public class PerlinHeightMapGenerator
    {
        private readonly float _smoothness;
        private readonly int _minHeight;
        private readonly int _heightVariation;

        private readonly int _noiseShiftMin;
        private readonly int _noiseShiftMax;

        public PerlinHeightMapGenerator(float smoothness, int minHeight, int heightVariation)
        {
            if (smoothness <= 0)
            {
                throw new ArgumentOutOfRangeException("smoothness should be positive.");
            }
            if (heightVariation <= 0)
            {
                throw new ArgumentOutOfRangeException("heightVariation should be positive.");
            }
            _smoothness = smoothness;
            _minHeight = minHeight;
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
                    var noiseValue = Mathf.PerlinNoise
                    (
                        aPos / _smoothness + noiseShift,
                        bPos / _smoothness + noiseShift
                    );
                    var relativeHeight = (int)(_heightVariation * noiseValue);
                    if (relativeHeight >= _heightVariation)
                        relativeHeight = _heightVariation - 1;
                    else if (relativeHeight < 0)
                        relativeHeight = 0;
                    var height = _minHeight + relativeHeight;
                    map[new HexPos(aPos + mapSize - 1, bPos + mapSize - 1)] = height;
                }
            }
            return map;
        }
    }
}
