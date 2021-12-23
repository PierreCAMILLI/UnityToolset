using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toolset
{
    public static class BitsetExtension
    {
        public static int SetTo1(this int value, int pos)
        {
            return value |= (1 << pos);
        }

        public static int SetTo0(this int value, int pos)
        {
            return value & ~(1 << pos);
        }

        public static bool IsSetTo1(this int value, int pos)
        {
            return (value & (1 << pos)) > 0;
        }

        public static bool IsSetTo0(this int value, int pos)
        {
            return (~value & (1 << pos)) > 0;
        }
    }
}
