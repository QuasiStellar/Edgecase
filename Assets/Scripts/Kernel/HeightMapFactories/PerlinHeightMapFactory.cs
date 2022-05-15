using System;
using System.Collections.Generic;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

namespace Kernel.HeightMapFactories
{
    public class PerlinHeightMapFactory : IHeightMapFactory
    {
        private readonly float _smoothness;
        private readonly int _minHeight;
        private readonly int _heightVariation;

        private readonly int _noiseShiftMin;
        private readonly int _noiseShiftMax;

        public PerlinHeightMapFactory(float smoothness, int minHeight, int heightVariation)
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

        public HexMap<int> BuildHeightMap(ISet<HexPos> mapShape)
        {
            var mapContent = new Dictionary<HexPos, int>();
            var noiseShift = Random.Range(_noiseShiftMin, _noiseShiftMax);
            foreach (var hexPos in mapShape)
            {
                var (aPos, bPos) = hexPos.ToCoords();
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
                mapContent[hexPos] = height;
            }
            return new HexMap<int>(mapContent);
        }
    }
}
