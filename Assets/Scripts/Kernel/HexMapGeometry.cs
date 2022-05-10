using System;
using Utils;

namespace Kernel
{
    public class HexMapGeometry
    {
        private readonly HexMap<int> _heights;
        private readonly Direction _viewDirection;

        public HexMapGeometry(HexMap<int> heights, Direction viewDirection) {
            _heights = heights;
            _viewDirection = viewDirection;
        }

        public bool AreVisualNeighbors(HexPos pos1, HexPos pos2)
        {
            if (IsOverlapped(pos1) || IsOverlapped(pos2))
            {
                return false;
            }
            var visualPos1 = GetVisualPos(pos1);
            var visualPos2 = GetVisualPos(pos2);
            return visualPos1.IsNeighbour(visualPos2);
        }

        private bool IsOverlapped(HexPos hexPos)
        {
            var viewDirectionInverse = _viewDirection.Reverse();
            var maybeOverlappingHexPos = hexPos.Shift(viewDirectionInverse, 1);
            var heightEnoughToOverlap = _heights[hexPos] + 1;
            while (_heights.HexExistsAtPos(maybeOverlappingHexPos))
            {
                if (_heights[maybeOverlappingHexPos] >= heightEnoughToOverlap)
                {
                    return true;
                }
                maybeOverlappingHexPos = maybeOverlappingHexPos.Shift(viewDirectionInverse, 1);
                heightEnoughToOverlap += 1;
            }
            return false;
        }

        private HexPos GetVisualPos(HexPos hexPos)
        {
            if (IsOverlapped(hexPos))
            {
                throw new ArgumentException("Hex is overlapped.");
            }
            return hexPos.Shift(_viewDirection, _heights[hexPos]);
        }
    }
}
