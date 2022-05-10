using UnityEngine;
using UnityEngine.InputSystem;
using Utils;

namespace Display
{
    public class HexBoard
    {
        private readonly GameObject _go;
        private readonly HexMap<GameObject> _hexBoard;
        private readonly int _mapSize;

        public GameObject this[HexPos hexPos] => _hexBoard[hexPos];

        public HexBoard
        (
            int mapSize,
            HexMap<int> heightMap,
            Material hexMaterial,
            float gameScale,
            Camera cam,
            PlayerInput playerInput
        )
        {
            _mapSize = mapSize;

            _hexBoard = new HexMap<GameObject>(mapSize);

            _go = new GameObject("HexBoard");
            var controller = _go.AddComponent<HexBoardController>();
            controller.cam = cam;
            controller.playerInput = playerInput;

            foreach (var _ in heightMap)
            {
                var hexPos = _.Key;
                var height = _.Value;
                var (a, b) = hexPos.ToCoords();

                var hex = HexBuilder.Hex
                (
                    a - mapSize + 1,
                    b - mapSize + 1,
                    height,
                    hexMaterial,
                    gameScale
                );

                hex.GetComponent<HexController>().SetParent(_go);

                _hexBoard[hexPos] = hex;
            }
        }
    }
}
