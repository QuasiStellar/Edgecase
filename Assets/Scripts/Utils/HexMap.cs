using System;
using System.Collections;
using System.Collections.Generic;

namespace Utils
{
    public class HexMap<T> : IEnumerable
    {
        private readonly Dictionary<HexPos, T> _content;

        public T this[HexPos hexPos]
        {
            get => _content[ValidatePos(hexPos)];
            set => _content[ValidatePos(hexPos)] = value;
        }

        public HexMap(IDictionary<HexPos, T> content)
        {
            _content = new Dictionary<HexPos, T>(content);
        }

        public IEnumerator<KeyValuePair<HexPos, T>> GetEnumerator()
        {
            return _content.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool HexExistsAtPos(HexPos hexPos)
        {
            return _content.ContainsKey(hexPos);
        }

        private HexPos ValidatePos(HexPos hexPos)
        {
            if (!HexExistsAtPos(hexPos))
            {
                throw new IndexOutOfRangeException("Index was out of the bounds of the hexagonal map.");
            }
            return hexPos;
        }
    }
}
