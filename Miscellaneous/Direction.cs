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
        private static Vector2Int upRight = Vector2Int.up + Vector2Int.right;
        private static Vector2Int upLeft = Vector2Int.up + Vector2Int.left;
        private static Vector2Int downLeft = Vector2Int.down + Vector2Int.left;
        private static Vector2Int downRight = Vector2Int.down + Vector2Int.right;

        private static Vector2 upRight2 = Vector2.up + Vector2.right;
        private static Vector2 upLeft2 = Vector2.up + Vector2.left;
        private static Vector2 downLeft2 = Vector2.down + Vector2.left;
        private static Vector2 downRight2 = Vector2.down + Vector2.right;

        private static Vector3 upRight3 = Vector3.up + Vector3.right;
        private static Vector3 upLeft3 = Vector3.up + Vector3.left;
        private static Vector3 downLeft3 = Vector3.down + Vector3.left;
        private static Vector3 downRight3 = Vector3.down + Vector3.right;

        private static Vector3 forwardRight = Vector3.forward + Vector3.right;
        private static Vector3 forwardLeft = Vector3.forward + Vector3.left;
        private static Vector3 backLeft = Vector3.back + Vector3.left;
        private static Vector3 backRight = Vector3.back + Vector3.right;

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
                    return upRight2;
                case Direction8.Up:
                    return Vector2.up;
                case Direction8.UpLeft:
                    return upLeft2;
                case Direction8.Left:
                    return Vector2.left;
                case Direction8.DownLeft:
                    return downLeft2;
                case Direction8.Down:
                    return Vector2.down;
                case Direction8.DownRight:
                    return downRight2;
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
                    return upRight3;
                case Direction8.Up:
                    return Vector3.up;
                case Direction8.UpLeft:
                    return upLeft3;
                case Direction8.Left:
                    return Vector3.left;
                case Direction8.DownLeft:
                    return downLeft3;
                case Direction8.Down:
                    return Vector3.down;
                case Direction8.DownRight:
                    return downRight3;
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
                    return forwardRight;
                case Direction8.Up:
                    return Vector3.forward;
                case Direction8.UpLeft:
                    return forwardLeft;
                case Direction8.Left:
                    return Vector3.left;
                case Direction8.DownLeft:
                    return backLeft;
                case Direction8.Down:
                    return Vector3.back;
                case Direction8.DownRight:
                    return backRight;
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
                    return upRight;
                case Direction8.Up:
                    return Vector2Int.up;
                case Direction8.UpLeft:
                    return upLeft;
                case Direction8.Left:
                    return Vector2Int.left;
                case Direction8.DownLeft:
                    return downLeft;
                case Direction8.Down:
                    return Vector2Int.down;
                case Direction8.DownRight:
                    return downRight;
            }
            return Vector2Int.zero;
        }

        public static Direction Left(this Direction direction)
        {
            return (Direction)(((byte)direction + 1) % (byte)Direction._Size);
        }

        public static Direction8 Left(this Direction8 direction)
        {
            return (Direction8)(((byte)direction + 1) % (byte)Direction8._Size);
        }

        public static Direction Right(this Direction direction)
        {
            byte size = (byte)Direction._Size;
            return (Direction)(((byte)direction + size - 1) % size);
        }

        public static Direction8 Right(this Direction8 direction)
        {
            byte size = (byte)Direction8._Size;
            return (Direction8)(((byte)direction + size - 1) % size);
        }

        public static Direction Opposite(this Direction direction)
        {
            byte size = (byte)Direction._Size;
            byte rotation = (byte)(size >> 1);
            return (Direction)(((byte)direction + rotation) % size);
        }

        public static Direction8 Opposite(this Direction8 direction)
        {
            byte size = (byte)Direction8._Size;
            byte rotation = (byte)(size >> 1);
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

        public static byte ToByte(this Direction direction)
        {
            return (byte)(1 << (byte)direction);
        }

        public static byte ToByte(this Direction8 direction)
        {
            return (byte)(1 << (byte)direction);
        }
    }
}
