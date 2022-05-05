using System;

namespace Utils
{
    public readonly struct HexPos
    {
        private readonly int _aPos;
        private readonly int _bPos;

        public HexPos(int aPos, int bPos)
        {
            _aPos = aPos;
            _bPos = bPos;
        }

        public Tuple<int, int> ToCoords()
        {
            return Tuple.Create(_aPos, _bPos);
        }

        public HexPos Neighbour(Direction direction)
        {
            return direction switch
            {
                Direction.UpLeft => new HexPos(_aPos, _bPos + 1),
                Direction.Up => new HexPos(_aPos + 1, _bPos + 1),
                Direction.UpRight => new HexPos(_aPos + 1, _bPos),
                Direction.DownRight => new HexPos(_aPos, _bPos - 1),
                Direction.Down => new HexPos(_aPos - 1, _bPos - 1),
                Direction.DownLeft => new HexPos(_aPos - 1, _bPos),
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            };
        }
    }
}