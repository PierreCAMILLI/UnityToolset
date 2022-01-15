using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toolset
{
    public interface IBitset : System.IEquatable<IBitset>
    {
        public int TypeSize { get; }

        public IBitset Set(int pos, bool value = true);
        public IBitset Reset();
        public IBitset Reset(int pos);
        public bool Test(int pos);
        public IBitset Flip();
        public IBitset Flip(int pos);
        public bool Any();
        public bool None();
        public bool All();
    }

    public struct Bitset : IBitset, System.IEquatable<Bitset>
    {
        private int _bits;

        public const int Size = sizeof(int) * 8;

        public static readonly Bitset Zero = new Bitset(0);
        public static readonly Bitset Full = new Bitset(-1);

        public int TypeSize => Size;

        public Bitset(int bits)
        {
            _bits = bits;
        }

        public IBitset Set(int pos, bool value = true)
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

        public IBitset Reset()
        {
            _bits = 0;
            return this;
        }

        public IBitset Reset(int pos)
        {
            _bits &= ~(1 << pos);
            return this;
        }

        public bool Test(int pos)
        {
            return (_bits & (1 << pos)) > 0;
        }

        public IBitset Flip()
        {
            _bits = ~_bits;
            return this;
        }

        public IBitset Flip(int pos)
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
            return _bits == Full._bits;
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

        public bool Equals(IBitset other)
        {
            int maxCount = Mathf.Max(other.TypeSize, Size);
            for (int i = 0; i < maxCount; ++i)
            {
                if (Test(i) != other.Test(i))
                {
                    return false;
                }
            }
            return true;
        }

        public static implicit operator int(Bitset bitset) => bitset._bits;
        public static implicit operator Bitset(int bitset) => new Bitset(bitset);
    }

    public struct Bitset8 : IBitset, System.IEquatable<Bitset8>
    {
        private byte _bits;

        public const int Size = sizeof(byte) * 8;

        public static readonly Bitset8 Zero = new Bitset8(0);
        public static readonly Bitset8 Full = new Bitset8(255);

        int IBitset.TypeSize => Size;

        public Bitset8(byte bits)
        {
            _bits = bits;
        }

        public IBitset Set(int pos, bool value = true)
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

        public IBitset Reset()
        {
            _bits = 0;
            return this;
        }

        public IBitset Reset(int pos)
        {
            _bits = (byte)(_bits & ~(1 << pos));
            return this;
        }

        public bool Test(int pos)
        {
            return (byte)(_bits & (1 << pos)) > 0;
        }

        public IBitset Flip()
        {
            _bits = (byte)~_bits;
            return this;
        }

        public IBitset Flip(int pos)
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
            return _bits == Full._bits;
        }

        public bool Equals(Bitset8 other)
        {
            return _bits.Equals(other._bits);
        }

        public bool Equals(IBitset other)
        {
            int maxCount = Mathf.Max(other.TypeSize, Size);
            for (int i = 0; i < maxCount; ++i)
            {
                if (Test(i) != other.Test(i))
                {
                    return false;
                }
            }
            return true;
        }

        public static implicit operator byte(Bitset8 bitset) => bitset._bits;
        public static implicit operator Bitset8(byte bitset) => new Bitset8(bitset);
    }

    public struct Bitset16 : IBitset, System.IEquatable<Bitset16>
    {
        private short _bits;

        public const int Size = sizeof(short) * 8;

        public static readonly Bitset16 Zero = new Bitset16(0);
        public static readonly Bitset16 Full = new Bitset16(-1);

        int IBitset.TypeSize => Size;

        public Bitset16(short bits)
        {
            _bits = bits;
        }

        public IBitset Set(int pos, bool value = true)
        {
            if (value)
            {
                _bits = (short)(_bits | (short)(1 << pos));
            }
            else
            {
                _bits = (short)(_bits & ~(1 << pos));
            }
            return this;
        }

        public IBitset Reset()
        {
            _bits = 0;
            return this;
        }

        public IBitset Reset(int pos)
        {
            _bits = (short)(_bits & ~(1 << pos));
            return this;
        }

        public bool Test(int pos)
        {
            return (short)(_bits & (1 << pos)) > 0;
        }

        public IBitset Flip()
        {
            _bits = (short)~_bits;
            return this;
        }

        public IBitset Flip(int pos)
        {
            _bits = (short)(1 ^ (1 << pos));
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
            return _bits == Full._bits;
        }

        public bool Equals(Bitset16 other)
        {
            return _bits.Equals(other._bits);
        }

        public bool Equals(IBitset other)
        {
            int maxCount = Mathf.Max(other.TypeSize, Size);
            for (int i = 0; i < maxCount; ++i)
            {
                if (Test(i) != other.Test(i))
                {
                    return false;
                }
            }
            return true;
        }

        public static implicit operator short(Bitset16 bitset) => bitset._bits;
        public static implicit operator Bitset16(short bitset) => new Bitset16(bitset);
    }

    public struct Bitset64 : IBitset, System.IEquatable<Bitset64>
    {
        private long _bits;

        public const int Size = sizeof(long) * 8;

        public static readonly Bitset64 Zero = new Bitset64(0);
        public static readonly Bitset64 Full = new Bitset64(-1);

        int IBitset.TypeSize => Size;

        public Bitset64(long bits)
        {
            _bits = bits;
        }

        public IBitset Set(int pos, bool value = true)
        {
            if (value)
            {
                _bits = (_bits | (long)(1 << pos));
            }
            else
            {
                _bits = (_bits & ~(1 << pos));
            }
            return this;
        }

        public IBitset Reset()
        {
            _bits = 0;
            return this;
        }

        public IBitset Reset(int pos)
        {
            _bits = (_bits & ~(1 << pos));
            return this;
        }

        public bool Test(int pos)
        {
            return (_bits & (1 << pos)) > 0;
        }

        public IBitset Flip()
        {
            _bits = ~_bits;
            return this;
        }

        public IBitset Flip(int pos)
        {
            _bits = (1 ^ (1 << pos));
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
            return _bits == Full._bits;
        }

        public bool Equals(Bitset64 other)
        {
            return _bits.Equals(other._bits);
        }

        public bool Equals(IBitset other)
        {
            int maxCount = Mathf.Max(other.TypeSize, Size);
            for (int i = 0; i < maxCount; ++i)
            {
                if (Test(i) != other.Test(i))
                {
                    return false;
                }
            }
            return true;
        }

        public static implicit operator long(Bitset64 bitset) => bitset._bits;
        public static implicit operator Bitset64(long bitset) => new Bitset64(bitset);
    }
}
