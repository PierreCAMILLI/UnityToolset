using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toolset
{
    public struct Bitset : System.IComparable, System.IComparable<Bitset>, System.IEquatable<Bitset>
    {
        private int _bits;

        public const int Size = sizeof(int);

        public static readonly Bitset Zero = new Bitset(0);

        public Bitset(int bits)
        {
            _bits = bits;
        }

        public Bitset Set(int pos, bool value = true)
        {
            if (value)
            {
                _bits |= (1 << pos);
            }
            else
            {
                _bits &= ~(1 << pos);
            }
            return this;
        }

        public Bitset Reset()
        {
            _bits = 0;
            return this;
        }

        public Bitset Reset(int pos)
        {
            _bits &= ~(1 << pos);
            return this;
        }

        public bool Test(int pos)
        {
            return (_bits & (1 << pos)) > 0;
        }

        public Bitset Flip()
        {
            _bits = ~_bits;
            return this;
        }

        public Bitset Flip(int pos)
        {
            _bits ^= (1 << pos);
            return this;
        }

        public bool Any()
        {
            return _bits != 0;
        }

        public bool None()
        {
            return _bits == 0;
        }

        public bool All()
        {
            return _bits == ~0;
        }

        public int CompareTo(object obj)
        {
            if (obj is Bitset)
            {
                return _bits.CompareTo(((Bitset)obj)._bits);
            }
            return _bits.CompareTo(obj);
        }

        public int CompareTo(Bitset other)
        {
            return _bits.CompareTo(other._bits);
        }

        public bool Equals(Bitset other)
        {
            return _bits.Equals(other._bits);
        }

        public static implicit operator int(Bitset bitset) => bitset._bits;
        public static implicit operator Bitset(int bitset) => new Bitset(bitset);
    }

    public struct Bitset8 : System.IComparable, System.IComparable<Bitset8>, System.IEquatable<Bitset8>
    {
        private byte _bits;

        public const int Size = sizeof(byte);

        public static readonly Bitset8 Zero = new Bitset8(0);

        public Bitset8(byte bits)
        {
            _bits = bits;
        }

        public Bitset8 Set(int pos, bool value = true)
        {
            if (value)
            {
                _bits = (byte)(_bits | (1 << pos));
            }
            else
            {
                _bits = (byte)(_bits & ~(1 << pos));
            }
            return this;
        }

        public Bitset8 Reset()
        {
            _bits = 0;
            return this;
        }

        public Bitset8 Reset(int pos)
        {
            _bits = (byte)(_bits & ~(1 << pos));
            return this;
        }

        public bool Test(int pos)
        {
            return (_bits & (1 << pos)) > 0;
        }

        public Bitset8 Flip()
        {
            _bits = (byte)~_bits;
            return this;
        }

        public Bitset8 Flip(int pos)
        {
            _bits = (byte)(1 ^ (1 << pos));
            return this;
        }

        public bool Any()
        {
            return _bits != 0;
        }

        public bool None()
        {
            return _bits == 0;
        }

        public bool All()
        {
            return _bits == unchecked((byte)~0);
        }

        public int CompareTo(object obj)
        {
            if (obj is Bitset8)
            {
                return _bits.CompareTo(((Bitset8)obj)._bits);
            }
            return _bits.CompareTo(obj);
        }

        public int CompareTo(Bitset8 other)
        {
            return _bits.CompareTo(other._bits);
        }

        public bool Equals(Bitset8 other)
        {
            return _bits.Equals(other._bits);
        }

        public static implicit operator byte(Bitset8 bitset) => bitset._bits;
        public static implicit operator Bitset8(byte bitset) => new Bitset8(bitset);
    }
}
