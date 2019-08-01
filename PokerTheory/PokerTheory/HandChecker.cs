using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HaLi.PokerTheory
{
    public static class HandChecker
    {
        public static void Check(Hand hand)
        {
            hand.Reset();

            int rank = default(int);
            if ((rank = StraightFlush(hand)) >= 0)
            {
                if (rank == 8)
                    hand._Type = HandType.RoyalFlush;
                else
                    hand._Type = HandType.StraightFlush;
            }
            else if ((rank = FourOfAKind(hand)) >= 0)
            {
                hand._Type = HandType.FourOfAKind;
            }
            else if ((rank = FullHouse(hand)) >= 0)
            {
                hand._Type = HandType.FullHouse;
            }
            else if ((rank = Flush(hand)) >= 0)
            {
                hand._Type = HandType.Flush;
            }
            else if ((rank = ThreeOfAKind(hand)) >= 0)
            {
                hand._Type = HandType.ThreeOfAKind;
            }
            else if ((rank = TwoPair(hand)) >= 0)
            {
                hand._Type = HandType.TwoPair;
            }
            else if ((rank = OnePair(hand)) >= 0)
            {
                hand._Type = HandType.OnePair;
            }
            else
            {
                hand._Type = HandType.HighCard;
                rank = HighCard(hand);
            }
            hand._TypeRank = (byte)rank;
        }

        public static int StraightFlush(Hand hand)
        {
            #region fast filter
            // 限5張牌時可快速檢查
            if (hand.Count == 5)
            {
                // 如果沒有5張同色可能，不可能為Straight flush
                // 5(二進制101), 應該有2bit才對
                // 這裡的檢查4條是可以過的
                if (BitsFunction.CountBits(hand._Bits_CntSuit & 0x5555) != 2)
                    return -1;

                // 如果有某種牌超過一張，不可能為Straight flush
                if ((hand._Bits_CntRank & 0xEEEEEEEEEEEEEL) != 0)
                    return -1;
            }
            #endregion


            // Check
            Func<int,int> fnCheck = (bits) =>
            {
                // fast filter
                if (bits < 0x001F)
                    return -1;
                // fast filter
                if (BitsFunction.CountBits(bits) < 5)
                    return -1;

                if ((bits & 0x1F00) == 0x1F00) return 8; // AKQJT
                if ((bits & 0x0F80) == 0x0F80) return 7; // 9TJQK
                if ((bits & 0x07C0) == 0x07C0) return 6; // 89TJQ
                if ((bits & 0x03E0) == 0x03E0) return 5; // 789TJ
                if ((bits & 0x01F0) == 0x01F0) return 4; // 6789T
                if ((bits & 0x00F8) == 0x00F8) return 3; // 56789
                if ((bits & 0x007C) == 0x007C) return 2; // 45678
                if ((bits & 0x003E) == 0x003E) return 1; // 34567
                if ((bits & 0x001F) == 0x001F) return 0; // 23456

                return -1;
            };

            // 大小不計花式
            if (hand._Bits_CntSuit == 0x5000) return fnCheck(hand._Bits_CardS);
            if (hand._Bits_CntSuit == 0x0500) return fnCheck(hand._Bits_CardH);
            if (hand._Bits_CntSuit == 0x0050) return fnCheck(hand._Bits_CardC);
            if (hand._Bits_CntSuit == 0x0005) return fnCheck(hand._Bits_CardD);

            return -1;
        }

        public static int FourOfAKind(Hand hand)
        {
            // 假設只用1副牌
            long bits = (hand._Bits_CntRank & 0x4444444444444L);
            if (bits == 0)
                return -1;

            return BitsFunction.LargeBit(bits, 52) / 4;
        }

        public static int FullHouse(Hand hand)
        {
            int three = ThreeOfAKind(hand);
            if (three >= 0)
            {
                // 有3條的情況
                long bits = (hand._Bits_CntRank & 0x2222222222222L);
                bits ^= 0x2L << (three * 4);

                // 有另一Pair
                if (bits != 0)
                    return three;
            }
            return -1;
        }

        public static int Flush(Hand hand)
        {
            // 不計花式大小, 只計牌大小
            if (hand._Bits_CntSuit == 0x5000) return BitsFunction.LargeBit(hand._Bits_CardS, 13);
            if (hand._Bits_CntSuit == 0x0500) return BitsFunction.LargeBit(hand._Bits_CardH, 13);
            if (hand._Bits_CntSuit == 0x0050) return BitsFunction.LargeBit(hand._Bits_CardC, 13);
            if (hand._Bits_CntSuit == 0x0005) return BitsFunction.LargeBit(hand._Bits_CardD, 13);
            return -1;
        }

        public static int Straight(Hand hand)
        {
            if (BitsFunction.CountBits(hand._Bits_Rank) < 5)
                return -1;

            if ((hand._Bits_Rank & 0x1F00) == 0x1F00) return 8; // AKQJT
            if ((hand._Bits_Rank & 0x0F80) == 0x0F80) return 7; // 9TJQK
            if ((hand._Bits_Rank & 0x07C0) == 0x07C0) return 6; // 89TJQ
            if ((hand._Bits_Rank & 0x03E0) == 0x03E0) return 5; // 789TJ
            if ((hand._Bits_Rank & 0x01F0) == 0x01F0) return 4; // 6789T
            if ((hand._Bits_Rank & 0x00F8) == 0x00F8) return 3; // 56789
            if ((hand._Bits_Rank & 0x007C) == 0x007C) return 2; // 45678
            if ((hand._Bits_Rank & 0x003E) == 0x003E) return 1; // 34567
            if ((hand._Bits_Rank & 0x001F) == 0x001F) return 0; // 23456

            return -1;
        }

        public static int ThreeOfAKind(Hand hand)
        {
            // 檢查2牌的可能, 跳過5張不同的情況
            long bits = (hand._Bits_CntRank & 0x2222222222L);
            if (bits == 0)
                return -1;

            if ((hand._Bits_CntRank & 0x3000000000000L) == 0x3000000000000L) return 12;
            if ((hand._Bits_CntRank & 0x0300000000000L) == 0x0300000000000L) return 11;
            if ((hand._Bits_CntRank & 0x0030000000000L) == 0x0030000000000L) return 10;
            if ((hand._Bits_CntRank & 0x0003000000000L) == 0x0003000000000L) return 9;
            if ((hand._Bits_CntRank & 0x0000300000000L) == 0x0000300000000L) return 8;
            if ((hand._Bits_CntRank & 0x0000030000000L) == 0x0000030000000L) return 7;
            if ((hand._Bits_CntRank & 0x0000003000000L) == 0x0000003000000L) return 6;
            if ((hand._Bits_CntRank & 0x0000000300000L) == 0x0000000300000L) return 5;
            if ((hand._Bits_CntRank & 0x0000000030000L) == 0x0000000030000L) return 4;
            if ((hand._Bits_CntRank & 0x0000000003000L) == 0x0000000003000L) return 3;
            if ((hand._Bits_CntRank & 0x0000000000300L) == 0x0000000000300L) return 2;
            if ((hand._Bits_CntRank & 0x0000000000030L) == 0x0000000000030L) return 1;
            if ((hand._Bits_CntRank & 0x0000000000003L) == 0x0000000000003L) return 0;

            return -1;
        }

        public static int TwoPair(Hand hand)
        {
            long bits = (hand._Bits_CntRank & 0x2222222222L);
            if (BitsFunction.CountBits(bits) >= 2)
            {
                return BitsFunction.LargeBit(bits, 52) / 4;
            }
            return -1;
        }

        public static int OnePair(Hand hand)
        {
            long bits = (hand._Bits_CntRank & 0x2222222222222L);
            if (bits == 0)
                return -1;
            return BitsFunction.LargeBit(bits, 52) / 4;
        }

        public static int HighCard(Hand hand)
        {
            return BitsFunction.LargeBit(hand._Bits_Rank, 13);
        }
    }
}
