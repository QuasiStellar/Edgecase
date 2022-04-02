using UnityEngine;


public class GameController : MonoBehaviour
{
    private const int MapSize = 10;
    private const float HexSize = 10;
    private const int StairHeight = 8;

    public GameObject freeCamera;
    public GameObject topDownCamera;

    private GameObject hexMap;

    private void Start()
    {
        hexMap = HexMapGenerator.HexMap(
            MapSize,
            HexSize,
            StairHeight,
            new PerlinHeightMapGenerator()
        );

        freeCamera.transform.position = new Vector3(0, MapSize * HexSize, -MapSize * HexSize * 0.75f);
        topDownCamera.GetComponent<Camera>().orthographicSize = MapSize * HexSize * 0.8f;
    }
}