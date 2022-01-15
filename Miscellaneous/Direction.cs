using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toolset
{
    public enum Direction : byte
    {
        Right = 0,
        Up = 1,
        Left = 2,
        Down = 3,

        _Size
    }
    public enum Direction8 : byte
    {
        Right = 0,
        UpRight = 1,
        Up = 2,
        UpLeft = 3,
        Left = 4,
        DownLeft = 5,
        Down = 6,
        DownRight = 7,

        _Size
    }

    public static class DirectionExtension
    {
        private static class Vector2IntDirection
        {
            public static readonly Vector2Int upRight = Vector2Int.up + Vector2Int.right;
            public static readonly Vector2Int upLeft = Vector2Int.up + Vector2Int.left;
            public static readonly Vector2Int downLeft = Vector2Int.down + Vector2Int.left;
            public static readonly Vector2Int downRight = Vector2Int.down + Vector2Int.right;
        }

        private static class Vector2Direction
        {
            public static readonly Vector2 upRight = Vector2.up + Vector2.right;
            public static readonly Vector2 upLeft = Vector2.up + Vector2.left;
            public static readonly Vector2 downLeft = Vector2.down + Vector2.left;
            public static readonly Vector2 downRight = Vector2.down + Vector2.right;

            public static class Normalized
            {
                public static readonly Vector2 upRight = (Vector2.up + Vector2.right).normalized;
                public static readonly Vector2 upLeft = (Vector2.up + Vector2.left).normalized;
                public static readonly Vector2 downLeft = (Vector2.down + Vector2.left).normalized;
                public static readonly Vector2 downRight = (Vector2.down + Vector2.right).normalized;
            }
        }

        private static class Vector3Direction
        {
            public static readonly Vector3 upRight = Vector3.up + Vector3.right;
            public static readonly Vector3 upLeft = Vector3.up + Vector3.left;
            public static readonly Vector3 downLeft = Vector3.down + Vector3.left;
            public static readonly Vector3 downRight = Vector3.down + Vector3.right;

            public static readonly Vector3 forwardRight = Vector3.forward + Vector3.right;
            public static readonly Vector3 forwardLeft = Vector3.forward + Vector3.left;
            public static readonly Vector3 backLeft = Vector3.back + Vector3.left;
            public static readonly Vector3 backRight = Vector3.back + Vector3.right;

            public static class Normalized
            {
                public static readonly Vector3 upRight = (Vector3.up + Vector3.right).normalized;
                public static readonly Vector3 upLeft = (Vector3.up + Vector3.left).normalized;
                public static readonly Vector3 downLeft = (Vector3.down + Vector3.left).normalized;
                public static readonly Vector3 downRight = (Vector3.down + Vector3.right).normalized;

                public static readonly Vector3 forwardRight = (Vector3.forward + Vector3.right).normalized;
                public static readonly Vector3 forwardLeft = (Vector3.forward + Vector3.left).normalized;
                public static readonly Vector3 backLeft = (Vector3.back + Vector3.left).normalized;
                public static readonly Vector3 backRight = (Vector3.back + Vector3.right).normalized;
            }
        }


        public static Vector2 ToVector2(this Direction direction)
        {
            switch (direction)
            {
                case Direction.Right:
                    return Vector2.right;
                case Direction.Up:
                    return Vector2.up;
                case Direction.Left:
                    return Vector2.left;
                case Direction.Down:
                    return Vector2.down;
            }
            return Vector2.zero;
        }

        public static Vector2 ToVector2(this Direction8 direction)
        {
            switch (direction)
            {
                case Direction8.Right:
                    return Vector2.right;
                case Direction8.UpRight:
                    return Vector2Direction.upRight;
                case Direction8.Up:
                    return Vector2.up;
                case Direction8.UpLeft:
                    return Vector2Direction.upLeft;
                case Direction8.Left:
                    return Vector2.left;
                case Direction8.DownLeft:
                    return Vector2Direction.downLeft;
                case Direction8.Down:
                    return Vector2.down;
                case Direction8.DownRight:
                    return Vector2Direction.downRight;
            }
            return Vector2.zero;
        }

        public static Vector2 ToVector2Normalized(this Direction8 direction)
        {
            switch (direction)
            {
                case Direction8.Right:
                    return Vector2.right;
                case Direction8.UpRight:
                    return Vector2Direction.Normalized.upRight;
                case Direction8.Up:
                    return Vector2.up;
                case Direction8.UpLeft:
                    return Vector2Direction.Normalized.upLeft;
                case Direction8.Left:
                    return Vector2.left;
                case Direction8.DownLeft:
                    return Vector2Direction.Normalized.downLeft;
                case Direction8.Down:
                    return Vector2.down;
                case Direction8.DownRight:
                    return Vector2Direction.Normalized.downRight;
            }
            return Vector2.zero;
        }

        public static Vector3 ToVector3(this Direction direction)
        {
            switch (direction)
            {
                case Direction.Right:
                    return Vector3.right;
                case Direction.Up:
                    return Vector3.up;
                case Direction.Left:
                    return Vector3.left;
                case Direction.Down:
                    return Vector3.down;
            }
            return Vector3.zero;
        }

        public static Vector3 ToVector3(this Direction8 direction)
        {
            switch (direction)
            {
                case Direction8.Right:
                    return Vector3.right;
                case Direction8.UpRight:
                    return Vector3Direction.upRight;
                case Direction8.Up:
                    return Vector3.up;
                case Direction8.UpLeft:
                    return Vector3Direction.upLeft;
                case Direction8.Left:
                    return Vector3.left;
                case Direction8.DownLeft:
                    return Vector3Direction.downLeft;
                case Direction8.Down:
                    return Vector3.down;
                case Direction8.DownRight:
                    return Vector3Direction.downRight;
            }
            return Vector3.zero;
        }

        public static Vector3 ToVector3Normalized(this Direction8 direction)
        {
            switch (direction)
            {
                case Direction8.Right:
                    return Vector3.right;
                case Direction8.UpRight:
                    return Vector3Direction.Normalized.upRight;
                case Direction8.Up:
                    return Vector3.up;
                case Direction8.UpLeft:
                    return Vector3Direction.Normalized.upLeft;
                case Direction8.Left:
                    return Vector3.left;
                case Direction8.DownLeft:
                    return Vector3Direction.Normalized.downLeft;
                case Direction8.Down:
                    return Vector3.down;
                case Direction8.DownRight:
                    return Vector3Direction.Normalized.downRight;
            }
            return Vector3.zero;
        }

        public static Vector3 XZ(this Direction direction)
        {
            switch (direction)
            {
                case Direction.Right:
                    return Vector3.right;
                case Direction.Up:
                    return Vector3.forward;
                case Direction.Left:
                    return Vector3.left;
                case Direction.Down:
                    return Vector3.back;
            }
            return Vector3.zero;
        }

        public static Vector3 XZ(this Direction8 direction)
        {
            switch (direction)
            {
                case Direction8.Right:
                    return Vector3.right;
                case Direction8.UpRight:
                    return Vector3Direction.forwardRight;
                case Direction8.Up:
                    return Vector3.forward;
                case Direction8.UpLeft:
                    return Vector3Direction.forwardLeft;
                case Direction8.Left:
                    return Vector3.left;
                case Direction8.DownLeft:
                    return Vector3Direction.backLeft;
                case Direction8.Down:
                    return Vector3.back;
                case Direction8.DownRight:
                    return Vector3Direction.backRight;
            }
            return Vector3.zero;
        }

        public static Vector3 XZNormalized(this Direction8 direction)
        {
            switch (direction)
            {
                case Direction8.Right:
                    return Vector3.right;
                case Direction8.UpRight:
                    return Vector3Direction.Normalized.forwardRight;
                case Direction8.Up:
                    return Vector3.forward;
                case Direction8.UpLeft:
                    return Vector3Direction.Normalized.forwardLeft;
                case Direction8.Left:
                    return Vector3.left;
                case Direction8.DownLeft:
                    return Vector3Direction.Normalized.backLeft;
                case Direction8.Down:
                    return Vector3.back;
                case Direction8.DownRight:
                    return Vector3Direction.Normalized.backRight;
            }
            return Vector3.zero;
        }

        public static Vector2Int ToVector2Int(this Direction direction)
        {
            switch (direction)
            {
                case Direction.Right:
                    return Vector2Int.right;
                case Direction.Up:
                    return Vector2Int.up;
                case Direction.Left:
                    return Vector2Int.left;
                case Direction.Down:
                    return Vector2Int.down;
            }
            return Vector2Int.zero;
        }

        public static Vector2Int ToVector2Int(this Direction8 direction)
        {
            switch (direction)
            {
                case Direction8.Right:
                    return Vector2Int.right;
                case Direction8.UpRight:
                    return Vector2IntDirection.upRight;
                case Direction8.Up:
                    return Vector2Int.up;
                case Direction8.UpLeft:
                    return Vector2IntDirection.upLeft;
                case Direction8.Left:
                    return Vector2Int.left;
                case Direction8.DownLeft:
                    return Vector2IntDirection.downLeft;
                case Direction8.Down:
                    return Vector2Int.down;
                case Direction8.DownRight:
                    return Vector2IntDirection.downRight;
            }
            return Vector2Int.zero;
        }

        public static Direction Left(this Direction direction)
        {
            return (Direction)(((byte)direction + 1) % (byte)Direction._Size);
        }

        public static Direction Left(this Direction direction, int count)
        {
            return (Direction)MathfExtension.Repeat((byte)direction + count, (byte)Direction._Size);
        }

        public static Direction8 Left(this Direction8 direction)
        {
            return (Direction8)(((byte)direction + 1) % (byte)Direction8._Size);
        }

        public static Direction8 Left(this Direction8 direction, int count)
        {
            return (Direction8)MathfExtension.Repeat((byte)direction + count, (byte)Direction8._Size);
        }

        public static Direction Right(this Direction direction)
        {
            const byte size = (byte)Direction._Size;
            return (Direction)(((byte)direction + size - 1) % size);
        }

        public static Direction Right(this Direction direction, int count)
        {
            const byte size = (byte)Direction._Size;
            return (Direction)MathfExtension.Repeat((byte)direction - count, size);
        }

        public static Direction8 Right(this Direction8 direction)
        {
            const byte size = (byte)Direction8._Size;
            return (Direction8)(((byte)direction + size - 1) % size);
        }

        public static Direction8 Right(this Direction8 direction, int count)
        {
            const byte size = (byte)Direction8._Size;
            return (Direction8)MathfExtension.Repeat((byte)direction - count, size);
        }

        public static Direction Opposite(this Direction direction)
        {
            const byte size = (byte)Direction._Size;
            const byte rotation = (byte)(size >> 1);
            return (Direction)(((byte)direction + rotation) % size);
        }

        public static Direction8 Opposite(this Direction8 direction)
        {
            const byte size = (byte)Direction8._Size;
            const byte rotation = (byte)(size >> 1);
            return (Direction8)(((byte)direction + rotation) % size);
        }

        public static Direction8 ToDirection8(this Direction direction)
        {
            return (Direction8)((byte)direction << 1);
        }

        public static Direction ToDirection(this Direction8 direction)
        {
            return (Direction)((byte)direction >> 1);
        }

        public static Bitset8 ToBitset(this Direction direction)
        {
            return (byte)(1 << (byte)direction);
        }

        public static Bitset8 ToBitset(this Direction8 direction)
        {
            return (byte)(1 << (byte)direction);
        }
    }
}
