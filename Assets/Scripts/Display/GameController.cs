using Kernel.HeightMapFactories;
using Kernel.MapShapeFactories;
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
        public const float CameraHeight = MapSize * GameScale * Numbers.Sqrt3X2;

        public Material hexMaterial;

        private HexBoard _hexBoard;

        private void Start()
        {
            var mapShape = new HexagonalShapeFactory(MapSize).CreateShape();
            var heightMapGenerator = new PerlinHeightMapFactory(
                smoothness: 7f,
                minHeight: 0,
                heightVariation: 7
            );
            var heightMap = heightMapGenerator.BuildHeightMap(mapShape);

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
                CameraHeight,
                -MapSize * GameScale * Numbers.Sqrt3By2
            );
            cam.transform.position = position;
            debugCam.transform.position = position;
        }
    }
}
