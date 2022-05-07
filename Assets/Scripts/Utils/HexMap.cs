using System;
using System.Collections;
using System.Collections.Generic;

namespace Utils
{
    public class HexMap<T> : IEnumerable
    {
        private readonly Dictionary<HexPos, T> _hexDictionary;
        private readonly int _size;

        public T this[HexPos hexPos]
        {
            get => _hexDictionary[ValidatePos(hexPos)];
            set => _hexDictionary[ValidatePos(hexPos)] = value;
        }

        public HexMap(int size)
        {
            _size = size;
            _hexDictionary = new Dictionary<HexPos, T>();
        }

        public IEnumerator<KeyValuePair<HexPos, T>> GetEnumerator()
        {
            return _hexDictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool HexExistsAtPos(HexPos hexPos)
        {
            var (a, b) = hexPos.ToCoords();
            bool aFits = 0 <= a && a < _size * 2 - 1;
            bool bFits = 0 <= b && b < _size * 2 - 1;
            bool distanceFromDiagonalFits = Math.Abs(a - b) < _size;
            return aFits && bFits && distanceFromDiagonalFits;
        }

        private HexPos ValidatePos(HexPos hexPos)
        {
            if (HexExistsAtPos(hexPos))
            {
                return hexPos;
            }
            else
            {
                throw new IndexOutOfRangeException("Index was out of the bounds of the hexagonal map.");
            }
        }
    }
}
