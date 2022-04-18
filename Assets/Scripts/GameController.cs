using HeightMaps;
using UnityEngine;
using Utils;

public class GameController : MonoBehaviour
{
    private const int MapSize = 10;
    private const float HexSize = 10;
    private const float StairHeight = HexSize * 1.5f;

    public Camera cam;

    public Material hexSideMaterial;
    public Material hexTopMaterial;
    
    private HexMap hexMap;

    private void Start()
    {
        hexMap = new HexMap(
            MapSize,
            HexSize,
            StairHeight,
            new PerlinHeightMap(MapSize),
            hexSideMaterial,
            hexTopMaterial
        );

        cam.transform.position = new Vector3(
            -MapSize * HexSize * 0.5f,
            MapSize * HexSize * Numbers.Sqrt3X2,
            -MapSize * HexSize * Numbers.Sqrt3By2
        );
    }
}