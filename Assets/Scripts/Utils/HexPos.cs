using System;
using System.Collections.Generic;
using System.Linq;

namespace Utils
{
    public readonly struct HexPos : IEquatable<HexPos>
    {
        private readonly int _aPos;
        private readonly int _bPos;

        public HexPos(int aPos, int bPos)
        {
            _aPos = aPos;
            _bPos = bPos;
        }

        public (int, int) ToCoords() => (_aPos, _bPos);

        public override bool Equals(object obj) => obj is HexPos otherPos && Equals(otherPos);

        public bool Equals(HexPos otherPos) => this.ToCoords() == otherPos.ToCoords();

        public override int GetHashCode() => ToCoords().GetHashCode();

        public static bool operator ==(HexPos thisPos, HexPos otherPos) => thisPos.Equals(otherPos);

        public static bool operator !=(HexPos thisPos, HexPos otherPos) => !(thisPos == otherPos);

        public bool IsNeighbour(HexPos otherPos) => Neighbours.Contains(otherPos);

        public HexPos Shift(Direction direction, int offset)
            => Enumerable.Range(1, offset).Aggregate(this, (current, i)
                => current.NeighbourByDirection(direction));

        private HexPos NeighbourByDirection(Direction direction)
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

        private IEnumerable<HexPos> Neighbours
        {
            get
            {
                var allDirections = Enum.GetValues(typeof(Direction)).Cast<Direction>();
                return allDirections.Select(NeighbourByDirection);
            }
        }
    }
}
