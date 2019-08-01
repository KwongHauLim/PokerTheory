using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HaLi.PokerTheory
{
    internal static class BitsFunction
    {
        public static int CountBits(int bits)
        {
            bits = (0x55555555 & bits) + (0x55555555 & (bits >> 1));
            bits = (0x33333333 & bits) + (0x33333333 & (bits >> 2));
            bits = (0x0f0f0f0f & bits) + (0x0f0f0f0f & (bits >> 4));
            bits = (0x00ff00ff & bits) + (0x00ff00ff & (bits >> 8));
            bits = (0x0000ffff & bits) + (0x0000ffff & (bits >> 16));
            return bits;
        }

        public static int CountBits(long bits)
        {
            bits = (0x5555555555555555 & bits) + (0x5555555555555555 & (bits >> 1));
            bits = (0x3333333333333333 & bits) + (0x3333333333333333 & (bits >> 2));
            bits = (0x0f0f0f0f0f0f0f0f & bits) + (0x0f0f0f0f0f0f0f0f & (bits >> 4));
            bits = (0x00ff00ff00ff00ff & bits) + (0x00ff00ff00ff00ff & (bits >> 8));
            bits = (0x0000ffff0000ffff & bits) + (0x0000ffff0000ffff & (bits >> 16));
            bits = (0x00000000ffffffff & bits) + (0x00000000ffffffff & (bits >> 32));
            return (int)bits;
        }
        
        public static int SmallBit(int bits, int numOfBit)
        {
            // 檢查對應bit位
            for (int i = 0; i < numOfBit; i++)
            {
                if ((bits & 0x1) != 0)
                    return i;
                bits >>= 1; 
            }
            return 0;
        }
        public static int SmallBit(long bits, int numOfBit)
        {
            // 檢查對應bit位
            for (int i = 0; i < numOfBit; i++)
            {
                if ((bits & 0x1L) != 0)
                    return i;
                bits >>= 1; 
            }
            return 0;
        }
        
        public static int LargeBit(int bits, int numOfBit)
        {
            // 檢查對應bit位
            int largeBit = 1 << (numOfBit-1);
            for (int i = numOfBit - 1; i >= 0; i--)
            {
                if ((bits & largeBit) != 0)
                    return i;
                bits <<= 1; 
            }
            return 0;
        }
        public static int LargeBit(long bits, int numOfBit)
        {
            // 檢查對應bit位
            long largeBit = 1L << (numOfBit-1);
            for (int i = numOfBit - 1; i >= 0; i--)
            {
                if ((bits & largeBit) != 0)
                    return i;
                bits <<= 1; 
            }
            return 0;
        }
    }
}
