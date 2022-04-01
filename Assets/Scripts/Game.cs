using System;
using UnityEngine;
using Random = UnityEngine.Random;


public class Game : MonoBehaviour
{
    public const int MapSize = 10;
    public const float HexSize = 10;
    public const int StairHeight = 8;

    private const float Smoothness = 7f;
    private const int HeightVariation = 7;

    public GameObject freeCamera;
    public GameObject topDownCamera;

    private HexMap hexMap;

    private void Start()
    {
        hexMap = HexMapGenerator.GenerateMap(MapSize, Smoothness, HeightVariation);

        freeCamera.transform.position = new Vector3(0, MapSize * HexSize, -MapSize * HexSize * 0.75f);
        topDownCamera.GetComponent<Camera>().orthographicSize = MapSize * HexSize * 0.8f;
    }
}