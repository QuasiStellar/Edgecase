using System;
using UnityEngine;
using Random = UnityEngine.Random;
public class MapGenerator : MonoBehaviour
{
    private const int MapSize = 10;
    private const float Smoothness = 10f;
    private const int HeightVariation = 7;
    private static int _noiseShift;
    public GameObject freeCamera;
    public GameObject topDownCamera;

    private void Start()
    {
        var map = new Hex[MapSize * 2 - 1, MapSize * 2 - 1];
        _noiseShift = Random.Range(10000,100000);
        for (var i = 0; i < MapSize * 2 - 1; i++)
        {
            for (var j = 0; j < MapSize * 2 - 1; j++)
            {
                if (Math.Abs(i - j) >= MapSize) continue;
                int aPos = i - MapSize + 1;
                int bPos = j - MapSize + 1;
                int height = (int) (Mathf.PerlinNoise((aPos/Smoothness) + _noiseShift, 
                    (bPos/Smoothness) + _noiseShift) * HeightVariation);
                var hex = new Hex(
                    aPos, bPos, height
                );
                hex.SetParent(transform);
                hex.UpdateColor();
                map[i, j] = hex;
            }
        }

        freeCamera.transform.position = new Vector3(0, MapSize * Hex.HexSize, - MapSize * Hex.HexSize * 0.75f);
        topDownCamera.GetComponent<Camera>().orthographicSize = MapSize * Hex.HexSize * 0.8f;
    }
}