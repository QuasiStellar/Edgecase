using UnityEngine;


public class GameController : MonoBehaviour
{
    private const int MapSize = 10;
    private const float HexSize = 10;
    private const float StairHeight = HexSize * 1.5f;

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

        freeCamera.transform.position = new Vector3(
            -MapSize * HexSize * 0.5f,
            MapSize * HexSize * Numbers.Sqrt3X2,
            -MapSize * HexSize * Numbers.Sqrt3By2
        );
        topDownCamera.GetComponent<Camera>().orthographicSize = MapSize * HexSize * 0.8f;
    }
}