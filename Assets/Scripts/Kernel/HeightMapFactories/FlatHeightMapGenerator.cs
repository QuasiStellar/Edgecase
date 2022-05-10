using System;
using System.Collections.Generic;
using Utils;

﻿namespace Kernel.HeightMapFactories
{
    public class FlatHeightMapGenerator : IHeightMapFactory
    {
        private readonly int _height;

        public FlatHeightMapGenerator(int height)
        {
            _height = height;
        }

        public HexMap<int> BuildHeightMap(int mapSize)
        {
            var mapContent = new Dictionary<HexPos, int>();
            // TODO: Fix duplicate code
            for (var aPos = 0; aPos < mapSize * 2 - 1; aPos++)
            {
                for (var bPos = 0; bPos < mapSize * 2 - 1; bPos++)
                {
                    if (Math.Abs(aPos - bPos) >= mapSize) continue;
                    mapContent[new HexPos(aPos, bPos)] = _height;
                }
            }
            return new HexMap<int>(mapContent);
        }
    }
}