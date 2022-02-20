using System;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    private const int Size = 10;

    private void Start()
    {
        var map = new Hex[Size * 2 - 1, Size * 2 - 1];
        for (var i = 0; i < Size * 2 - 1; i++)
        {
            for (var j = 0; j < Size * 2 - 1; j++)
            {
                if (Math.Abs(i - j) >= Size) continue;
                var hex = new Hex(i, j, Hex.GetRandomHeight());
                hex.SetParent(transform);
                hex.UpdateColor();
                map[i, j] = hex;
            }
        }
    }
}
