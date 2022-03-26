using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;


public class MapGenerator : MonoBehaviour
{
    public const int MapSize = 5;
    public const int TriangleSize = 5;
    public const float HexSize = 10;
    public const int StairHeight = 3;

    public const float Smoothness = 7f;
    public const int HeightVariation = 7;
    private static int _noiseShift;

    public GameObject freeCamera;
    public GameObject topDownCamera;
    public Map map;
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
                mesh.SetHeight(aPos, bPos, (int) (Mathf.PerlinNoise((aPos / Smoothness) + _noiseShift,
                    (bPos / Smoothness) + _noiseShift) * HeightVariation));
            }
        }
        map = new Map(mesh);

        freeCamera.transform.position = new Vector3(0, MapSize * HexSize, -MapSize * HexSize * 0.75f);
        topDownCamera.GetComponent<Camera>().orthographicSize = MapSize * HexSize * 0.8f;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(Elevate());
        }
    }
    private IEnumerator Elevate()
    {
        var elapsedTime = 0f;
        while (elapsedTime < 1)
        {
            for (var i = 0; i < MapSize * 2 - 1; i++)
            {
                for (var j = 0; j < MapSize * 2 - 1; j++)
                {
                    if (Math.Abs(i - j) >= MapSize) continue;
                    map._hexGameObject.transform.Translate(Vector3.up * StairHeight  * Time.deltaTime);
                }
            }
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }
}