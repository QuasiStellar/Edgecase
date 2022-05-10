using Kernel.HeightMaps;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils;

namespace Display
{
    public class GameController : MonoBehaviour
    {
        private const int MapSize = 10;
        private const float GameScale = 10;

        public Camera cam;
        public Camera debugCam;
        public PlayerInput playerInput;

        public Material hexMaterial;

        private HexBoard _hexBoard;

        private void Start()
        {
            var heightMap = new PerlinHeightMapGenerator().GenerateHeightMap(MapSize);
            _hexBoard = new HexBoard(
                MapSize,
                heightMap,
                hexMaterial,
                GameScale,
                cam,
                playerInput
            );

            var position = new Vector3(
                -MapSize * GameScale * 0.5f,
                MapSize * GameScale * Numbers.Sqrt3X2,
                -MapSize * GameScale * Numbers.Sqrt3By2
            );
            cam.transform.position = position;
            debugCam.transform.position = position;
        }
    }
}
