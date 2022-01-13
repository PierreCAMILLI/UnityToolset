using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toolset
{
    public static class MathfExtension
    {
        public static int Repeat(int t, int length)
        {
            int r = t % length;
            return r < 0 ? r + length : r;
        }
    }
}
