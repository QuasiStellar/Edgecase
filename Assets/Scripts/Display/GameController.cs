using HeightMaps;
using UnityEngine;
using Utils;

public class GameController : MonoBehaviour
{
    private const int MapSize = 10;
    private const float GameScale = 10;

    public Camera cam;

    public Material hexMaterial;
    
    private HexBoard _hexBoard;

    private void Start()
    {
        _hexBoard = new HexBoard(
            MapSize,
            new PerlinHeightMap(MapSize),
            hexMaterial,
            GameScale,
            cam
        );

        cam.transform.position = new Vector3(
            -MapSize * GameScale * 0.5f,
            MapSize * GameScale * Numbers.Sqrt3X2,
            -MapSize * GameScale * Numbers.Sqrt3By2
        );
    }
}