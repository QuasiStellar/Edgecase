using System;
using UnityEngine;
using Random = UnityEngine.Random;


public class Game : MonoBehaviour
{
    public const int MapSize = 5;
    public const float HexSize = 10;
    public const int StairHeight = 3;

    public const float Smoothness = 7f;
    public const int HeightVariation = 7;

    private static int _noiseShift;

    public GameObject freeCamera;
    public GameObject topDownCamera;

    private void Start()
    {
        var mesh = new Carcass();
        _noiseShift = Random.Range(10000, 100000);
        for (var i = 0; i < MapSize * 2 - 1; i++)
        {
            for (var j = 0; j < MapSize * 2 - 1; j++)
            {
                if (Math.Abs(i - j) >= MapSize) continue;
                var aPos = i - MapSize + 1;
                var bPos = j - MapSize + 1;
                var height = (int) (Mathf.PerlinNoise((aPos / Smoothness) + _noiseShift,
                    (bPos / Smoothness) + _noiseShift) * HeightVariation);
                if (height >= HeightVariation)
                    height = HeightVariation - 1;
                else if (height < 0)
                    height = 0;
                mesh.SetHeight(aPos, bPos, height);
            }
        }

        freeCamera.transform.position = new Vector3(0, MapSize * Hex.HexSize, - MapSize * Hex.HexSize * 0.75f);
        topDownCamera.GetComponent<Camera>().orthographicSize = MapSize * Hex.HexSize * 0.8f;
    }
}