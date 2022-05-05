using System;
using System.Collections.Generic;
using HeightMaps;
using UnityEngine;
using Utils;

public class HexBoard
{
    private readonly GameObject _go;
    private readonly HexMap<GameObject> _hexMap;
    private readonly int _mapSize;
    
    public GameObject this[HexPos hexPos] => _hexMap[hexPos];

    public HexBoard
    (
        int mapSize,
        HeightMap heightMap,
        Material hexMaterial,
        float gameScale,
        Camera cam
    )
    {
        _mapSize = mapSize;

        _hexMap = new HexMap<GameObject>(mapSize);
        
        _go = new GameObject("HexBoard");
        _go.AddComponent<HexMapController>().cam = cam;

        foreach (var _ in heightMap)
        {
            var hexPos = _.Key;
            var height = _.Value;
            var (a, b) = hexPos.ToCoords();
            
            var hex = HexBuilder.Hex
            (
                a - mapSize + 1,
                b - mapSize + 1,
                height,
                hexMaterial,
                gameScale
            );
                
            hex.GetComponent<HexController>().SetParent(_go);

            _hexMap[hexPos] = hex;
        }
    }
}