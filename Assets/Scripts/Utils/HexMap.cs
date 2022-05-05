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

        private HexPos ValidatePos(HexPos hexPos)
        {
            var (a, b) = hexPos.ToCoords();
            if (a >= _size * 2 - 1 || a < 0 || b >= _size * 2 - 1 || b < 0 || Math.Abs(a - b) >= _size)
            {
                throw new IndexOutOfRangeException("Index was out of the bounds of the hexagonal map.");
            }

            return hexPos;
        }
    }
}