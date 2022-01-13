using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Toolset
{
    public class DirectionTest
    {
        // A Test behaves as an ordinary method
        [Test]
        public void DirectionTestSimplePasses()
        {
            Direction direction = Direction.Left;

            Assert.AreEqual(Direction.Down, direction.Left());
            Assert.AreEqual(Direction.Up, direction.Right());
            Assert.AreEqual(Direction.Right, direction.Opposite());

            Assert.AreEqual(Direction.Right, direction.Left(2));
            Assert.AreEqual(Direction.Right, direction.Right(2));
            Assert.AreEqual(Direction.Up, direction.Left(3));
            Assert.AreEqual(Direction.Down, direction.Right(3));
            Assert.AreEqual(Direction.Left, direction.Left(4));
            Assert.AreEqual(Direction.Left, direction.Right(4));

            Assert.AreEqual(Direction.Down, direction.Left(25));
            Assert.AreEqual(Direction.Up, direction.Right(25));

            Assert.AreEqual(Direction.Left, direction.Left(256));
            Assert.AreEqual(Direction.Left, direction.Right(256));
        }

        [Test]
        public void Direction8TestSimplePasses()
        {
            Direction8 direction = Direction8.Left;

            Assert.AreEqual(Direction8.DownLeft, direction.Left());
            Assert.AreEqual(Direction8.UpLeft, direction.Right());
            Assert.AreEqual(Direction8.Right, direction.Opposite());

            Assert.AreEqual(Direction8.Down, direction.Left(2));
            Assert.AreEqual(Direction8.Up, direction.Right(2));
            Assert.AreEqual(Direction8.DownRight, direction.Left(3));
            Assert.AreEqual(Direction8.UpRight, direction.Right(3));
            Assert.AreEqual(Direction8.Right, direction.Left(4));
            Assert.AreEqual(Direction8.Right, direction.Right(4));

            Assert.AreEqual(Direction8.DownLeft, direction.Left(25));
            Assert.AreEqual(Direction8.UpLeft, direction.Right(25));

            Assert.AreEqual(Direction8.Left, direction.Left(256));
            Assert.AreEqual(Direction8.Left, direction.Right(256));
        }

        [Test]
        public void DirectionToDirection8Test()
        {
            Assert.AreEqual(Direction8.Right, Direction.Right.ToDirection8());
            Assert.AreEqual(Direction8.Up, Direction.Up.ToDirection8());
            Assert.AreEqual(Direction8.Left, Direction.Left.ToDirection8());
            Assert.AreEqual(Direction8.Down, Direction.Down.ToDirection8());

            Assert.AreEqual(Direction.Right, Direction8.Right.ToDirection());
            Assert.AreEqual(Direction.Up, Direction8.Up.ToDirection());
            Assert.AreEqual(Direction.Left, Direction8.Left.ToDirection());
            Assert.AreEqual(Direction.Down, Direction8.Down.ToDirection());

            Assert.AreEqual(Direction.Right, Direction8.UpRight.ToDirection());
            Assert.AreEqual(Direction.Up, Direction8.UpLeft.ToDirection());
            Assert.AreEqual(Direction.Left, Direction8.DownLeft.ToDirection());
            Assert.AreEqual(Direction.Down, Direction8.DownRight.ToDirection());
        }
    }
}
