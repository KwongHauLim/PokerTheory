using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HaLi.PokerTheory
{
    public class Card :
        IEquatable<Card>
    {
        public enum SUIT : int
        {
            D = 0x10000, // Diamonds
            C = 0x20000, // Clubs
            H = 0x40000, // Hearts
            S = 0x80000, // Spades
        }

        public enum RANK : int
        {
            _2 = 0x0001,
            _3 = 0x0002,
            _4 = 0x0004,
            _5 = 0x0008,
            _6 = 0x0010,
            _7 = 0x0020,
            _8 = 0x0040,
            _9 = 0x0080,
            _T = 0x0100,
            _J = 0x0200,
            _Q = 0x0400,
            _K = 0x0800,
            _A = 0x1000,
        }

        public readonly int _iCard = 0;
        public readonly int _iSuit = 0;
        public readonly int _iRank = 0;
        public readonly SUIT _eSUIT = SUIT.D;
        public readonly RANK _eRANK = RANK._2;

        public int Value { get { return (int)_eSUIT | (int)_eRANK; } }

        public bool Equals(Card other)
        {
            return _iCard.Equals(other._iCard);
        }

        public Card(int idx)
        {
            _iCard = idx;
            _iSuit = idx / 13;
            _iRank = idx % 13;
            _eSUIT = (SUIT)(0x10000 << _iSuit);
            _eRANK = (RANK)(0x1 << _iRank);
        }

        public Card(SUIT suit, RANK rank)
        {
            _eSUIT = suit;
            _eRANK = rank;

            switch (suit)
            {
                case SUIT.D: _iSuit = 0; break;
                case SUIT.C: _iSuit = 1; break;
                case SUIT.H: _iSuit = 2; break;
                case SUIT.S: _iSuit = 3; break;
            }

            switch (rank)
            {
                case RANK._2: _iRank = 0; break;
                case RANK._3: _iRank = 1; break;
                case RANK._4: _iRank = 2; break;
                case RANK._5: _iRank = 3; break;
                case RANK._6: _iRank = 4; break;
                case RANK._7: _iRank = 5; break;
                case RANK._8: _iRank = 6; break;
                case RANK._9: _iRank = 7; break;
                case RANK._T: _iRank = 8; break;
                case RANK._J: _iRank = 9; break;
                case RANK._Q: _iRank = 10; break;
                case RANK._K: _iRank = 11; break;
                case RANK._A: _iRank = 12; break;
            }

            _iCard = _iSuit * 13 + _iRank;
        }

        public override int GetHashCode()
        {
            return Value;
        }

        public string RankString(RANK rank)
        {
            switch (rank)
            {
                default: return "";
                case RANK._2: return "2";
                case RANK._3: return "3";
                case RANK._4: return "4";
                case RANK._5: return "5";
                case RANK._6: return "6";
                case RANK._7: return "7";
                case RANK._8: return "8";
                case RANK._9: return "9";
                case RANK._T: return "T";
                case RANK._J: return "J";
                case RANK._Q: return "Q";
                case RANK._K: return "K";
                case RANK._A: return "A";
            }
        }

        public string SuitString(SUIT suit)
        {
            switch (suit)
            {
                default: return "";
                case SUIT.D: return "D";// "♦";
                case SUIT.C: return "C";// "♣";
                case SUIT.H: return "H";// "♥";
                case SUIT.S: return "S";// "♠";
            }
        }

        public override string ToString()
        {
            return RankString(_eRANK) + SuitString(_eSUIT);
        }
    }
}
