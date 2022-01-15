using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Toolset;

public class BitsetTest
{
    // A Test behaves as an ordinary method
    [Test]
    public void BitsetSimplePasses()
    {
        IBitset bitset = Bitset.Zero;
        Assert.IsTrue(bitset.None());
        Assert.IsFalse(bitset.Any());
        Assert.IsFalse(bitset.All());
        for (int i = 0; i < Bitset.Size; ++i)
        {
            Assert.IsFalse(bitset.Test(i));
        }

        bitset.Flip();
        Assert.IsFalse(bitset.None());
        Assert.IsTrue(bitset.Any());
        Assert.IsTrue(bitset.All());

        bitset.Flip();
        bitset.Set(1, true);
        Assert.IsFalse(bitset.None());
        Assert.IsTrue(bitset.Any());
        Assert.IsFalse(bitset.All());
        Assert.IsTrue(bitset.Test(1));

        bitset.Flip(1);
        Assert.IsTrue(bitset.None());

        bitset.Flip(0);
        Assert.IsTrue(bitset.Test(0));

        bitset.Flip(0);
        bitset.Flip();
        Assert.IsTrue(bitset.All());
        bitset.Reset();
        Assert.IsTrue(bitset.None());
        Assert.AreEqual(Bitset.Zero.Flip(), Bitset.Full);
    }

    [Test]
    public void Bitset64SimplePasses()
    {
        Bitset64 bitset = new Bitset64();
        bitset.Set(60, true);
        Assert.AreNotEqual(Bitset64.Zero, bitset);

        Assert.AreEqual(Bitset64.Zero.Flip(), Bitset64.Full);

        Assert.IsTrue(!Bitset64.Full.Equals(Bitset16.Full));
        Assert.AreNotEqual(Bitset64.Full, Bitset16.Full);
    }
}
