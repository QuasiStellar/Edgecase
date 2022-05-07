using System;

namespace Utils
{
    public enum Direction
    {
        UpLeft,
        Up,
        UpRight,
        DownRight,
        Down,
        DownLeft
    }

    public static class DirectionMethods
    {
        public static Direction Reverse(this Direction direction)
        {
            return direction switch
            {
                Direction.UpLeft => Direction.DownRight,
                Direction.Up => Direction.Down,
                Direction.UpRight => Direction.DownLeft,
                Direction.DownRight => Direction.UpLeft,
                Direction.Down => Direction.Up,
                Direction.DownLeft => Direction.UpRight,
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            };
        }

        public static Direction Clockwise(this Direction direction)
        {
            return direction switch
            {
                Direction.UpLeft => Direction.Up,
                Direction.Up => Direction.UpRight,
                Direction.UpRight => Direction.DownRight,
                Direction.DownRight => Direction.Down,
                Direction.Down => Direction.DownLeft,
                Direction.DownLeft => Direction.UpLeft,
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            };
        }

        public static Direction Counterclockwise(this Direction direction)
        {
            return direction switch
            {
                Direction.UpLeft => Direction.DownLeft,
                Direction.Up => Direction.UpLeft,
                Direction.UpRight => Direction.Up,
                Direction.DownRight => Direction.UpRight,
                Direction.Down => Direction.DownRight,
                Direction.DownLeft => Direction.Down,
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            };
        }
    }
}
