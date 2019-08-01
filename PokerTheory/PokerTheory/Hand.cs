using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HaLi.PokerTheory
{
    public enum HandType
    {
        None = 0,
        HighCard = 0x001,
        OnePair = 0x002,
        TwoPair = 0x004,
        ThreeOfAKind = 0x008,
        Straight = 0x010,
        Flush = 0x020,
        FullHouse = 0x040,
        FourOfAKind = 0x080,
        StraightFlush = 0x100,
        RoyalFlush = 0x200,
    }

    public class Hand : List<Card>
        ,IEquatable<Hand> ,IComparable<Hand>
    {
        public int      _Bits_CardD = 0; // 13 bits
        public int      _Bits_CardC = 0; // 13 bits
        public int      _Bits_CardH = 0; // 13 bits
        public int      _Bits_CardS = 0; // 13 bits

        public int      _Bits_Rank = 0; // 13 bits
        public int      _Bits_Suit = 0; // 4 bits

        public long     _Bits_CntRank = 0; // 52 bits
        public int      _Bits_CntSuit = 0; // 16 bits

        public HandType _Type = HandType.None;
        public byte _TypeRank = 0;

        public static string CardsToString(List<Card> cards)
        {
            return cards.Aggregate<Card, string>(string.Empty,
                (seed, nxt) => seed + nxt.ToString() + ",");
        }

        public override string ToString()
        {
            return CardsToString(this);
        }

        public void Reset()
        {
            _Bits_CardD = 0;
            _Bits_CardC = 0;
            _Bits_CardH = 0;
            _Bits_CardS = 0;
            _Bits_Rank = 0;
            _Bits_Suit = 0;
            _Bits_CntRank = 0;
            _Bits_CntSuit = 0;
            
            Card c;
            int[] rank = new int[13];
            int[] suit = new int[4];

            for (int i = 0; i < Count; i++)
            {
                c = this[i];
                rank[c._iRank]++;
                switch (c._eSUIT)
                {
                    default: break;
                    case Card.SUIT.D: _Bits_CardD |= (1 << c._iRank); suit[0]++; break;
                    case Card.SUIT.C: _Bits_CardC |= (1 << c._iRank); suit[1]++; break;
                    case Card.SUIT.H: _Bits_CardH |= (1 << c._iRank); suit[2]++; break;
                    case Card.SUIT.S: _Bits_CardS |= (1 << c._iRank); suit[3]++; break;
                }
            }

            if (_Bits_CardD != 0) { _Bits_Rank |= _Bits_CardD; _Bits_Suit |= 0x1; }
            if (_Bits_CardC != 0) { _Bits_Rank |= _Bits_CardC; _Bits_Suit |= 0x2; }
            if (_Bits_CardH != 0) { _Bits_Rank |= _Bits_CardH; _Bits_Suit |= 0x4; }
            if (_Bits_CardS != 0) { _Bits_Rank |= _Bits_CardS; _Bits_Suit |= 0x8; }

            for (int i=0; i < 13; i++)
            {
                // 用4bits配合16進制，方便檢測方面的編程
                // 5張封頂, 牌型5張同階沒意義
                if (rank[i] > 5)
                    rank[i] = 5;
                _Bits_CntRank |= (long)rank[i] << (i * 4);
            }

            for (int i=0; i < 4; i++)
            {
                // 用4bits配合16進制，方便檢測方面的編程
                // 5張封頂, 牌型5張同花沒意義
                if (suit[i] > 5)
                    suit[i] = 5;
                _Bits_CntSuit |= suit[i] << (i * 4);
            }

            _Type = HandType.None;
            _TypeRank = 0;
        }

        public bool Equals(Hand other)
        {
            return
                (_Type == other._Type) &&
                (_TypeRank == other._TypeRank);
        }

        public int CompareTo(Hand other)
        {
            if (_Type > other._Type)
                return +1;
            if (_Type < other._Type)
                return -1;
            return _TypeRank.CompareTo(other._TypeRank);
        }
    }
}
