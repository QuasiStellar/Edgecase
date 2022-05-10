using System;
using Utils;

﻿namespace Kernel.HeightMaps
{
    public class FlatHeightMap : HeightMap
    {
        public FlatHeightMap(int mapSize) : base(mapSize)
        {
            // TODO: Fix duplicate code
            for (var aPos = 0; aPos < mapSize * 2 - 1; aPos++)
            {
                for (var bPos = 0; bPos < mapSize * 2 - 1; bPos++)
                {
                    if (Math.Abs(aPos - bPos) >= mapSize) continue;
                    Map[new HexPos(aPos, bPos)] = 0;
                }
            }
        }
    }
}
