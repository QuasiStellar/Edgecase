using System;
using HeightMaps;
using UnityEngine;

public class HexMap
{
    private readonly GameObject _go;
    private readonly GameObject[,] _hexMap;
    private readonly int _mapSize;
    
    public GameObject this[int i, int j]
    {
        get
        {
            if (Math.Abs(i - j) >= _mapSize)
            {
                throw new IndexOutOfRangeException("Index was out of the bounds of the hexagonal map.");
            }
            return _hexMap[i, j];
        }
    }

    public HexMap
    (
        int mapSize,
        HexagonalHeightMap heightMap,
        Material hexMaterial,
        float gameScale,
        Camera cam
    )
    {
        _mapSize = mapSize;
        
        _hexMap = new GameObject[mapSize * 2 - 1, mapSize * 2 - 1];
        
        _go = new GameObject("HexMap");

        _go.AddComponent<HexMapController>().cam = cam;

        for (var i = 0; i < 2 * mapSize - 1; i++)
        {
            for (var j = 0; j < 2 * mapSize - 1; j++)
            {
                if (Math.Abs(i - j) >= mapSize) continue;

                var hex = HexBuilder.Hex
                (
                    i - mapSize + 1,
                    j - mapSize + 1,
                    heightMap[i, j],
                    hexMaterial,
                    gameScale
                );
                
                hex.GetComponent<HexController>().SetParent(_go);

                _hexMap[i, j] = hex;
            }
        }
    }
}