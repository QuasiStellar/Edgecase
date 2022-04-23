using HeightMaps;
using UnityEngine;
using Utils;

public class GameController : MonoBehaviour
{
    private const int MapSize = 10;
    private const float GameScale = 10;

    public Camera cam;

    public Material hexMaterial;
    
    private HexMap hexMap;

    private void Start()
    {
        hexMap = new HexMap(
            MapSize,
            new PerlinHeightMap(MapSize),
            hexMaterial,
            GameScale
        );

        cam.transform.position = new Vector3(
            -MapSize * GameScale * 0.5f,
            MapSize * GameScale * Numbers.Sqrt3X2,
            -MapSize * GameScale * Numbers.Sqrt3By2
        );
    }
}