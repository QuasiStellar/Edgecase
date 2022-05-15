using System;
using System.Collections.Generic;
using System.Linq;
using Utils;

namespace Kernel.MapShapeFactories
{
    public class HexagonalShapeFactory
    {
        private readonly int _sideSize;

        public HexagonalShapeFactory(int sideSize)
        {
            if (sideSize < 1)
            {
                throw new ArgumentOutOfRangeException("sideSize should be positive.");
            }
            _sideSize = sideSize;
        }

        public HashSet<HexPos> Build()
        {
            var shape = new HashSet<HexPos>();
            foreach (var aPos in Enumerable.Range(0, _sideSize * 2 - 1))
            {
                foreach (var bPos in Enumerable.Range(0, _sideSize * 2 - 1))
                {
                    var distanceFromDiagonal = Math.Abs(aPos - bPos);
                    if (distanceFromDiagonal < _sideSize)
                    {
                        shape.Add(new HexPos(aPos, bPos));
                    }
                }
            }
            return shape;
        }
    }
}
