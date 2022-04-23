using System;
using HeightMaps;
using UnityEngine;

public class HexMap
{
    private readonly GameObject _go;

    public HexMap
    (
        int mapSize,
        HexagonalHeightMap heightMap,
        Material hexMaterial,
        float gameScale
    )
    {
        _go = new GameObject("HexMap");

        for (var i = 0; i < 2 * mapSize - 1; i++)
        {
            for (var j = 0; j < 2 * mapSize - 1; j++)
            {
                if (Math.Abs(i - j) >= mapSize) continue;

                var hex = new Hex
                (
                    i - mapSize + 1,
                    j - mapSize + 1,
                    heightMap[i, j],
                    hexMaterial,
                    gameScale
                );
                
                hex.SetParent(_go);
            }
        }
    }
}