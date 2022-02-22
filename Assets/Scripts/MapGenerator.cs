using System;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    private const int MapSize = 10;

    private void Start()
    {
        var map = new Hex[MapSize * 2 - 1, MapSize * 2 - 1];
        for (var i = 0; i < MapSize * 2 - 1; i++)
        {
            for (var j = 0; j < MapSize * 2 - 1; j++)
            {
                if (Math.Abs(i - j) >= MapSize) continue;
                var hex = new Hex(
                    i - MapSize + 1,
                    j - MapSize + 1,
                    Hex.GetRandomHeight()
                );
                hex.SetParent(transform);
                hex.UpdateColor();
                map[i, j] = hex;
            }
        }
    }
}