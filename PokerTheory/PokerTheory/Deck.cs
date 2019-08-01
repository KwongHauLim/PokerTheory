using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HaLi.PokerTheory
{
    public class Deck
    {
        public List<Card> _Cards = new List<Card>();
        public int _Ptr = 0; // Point next card position

        public Card this[int index]
        {
            get { return _Cards[index]; }
        }

        public static List<Card> Standard(bool withJoker)
        {
            List<Card> list = new List<Card>();
            var suit = Enum.GetValues(typeof(Card.SUIT));
            var rank = Enum.GetValues(typeof(Card.RANK));

            foreach (Card.SUIT s in suit)
            {
                foreach (Card.RANK r in rank)
                {
                    var c = new Card(s, r);
                    list.Add(c);
                }
            }

            if (withJoker)
            {
            }

            return list;
        }

        public virtual void Shuffle()
        {
            Random rand = new Random();
            
	        // Fisher-Yates shuffle
	        // http://en.wikipedia.org/wiki/Fisher-Yates_shuffle

            Card tmp;
            int idx = 0;
            for (int i = _Cards.Count - 1; i >= 2; i--)
            {
                idx = rand.Next(i);
                tmp = _Cards[idx];
                _Cards[idx] = _Cards[i];
                _Cards[i] = tmp;
            }
        }

        public virtual Card Next()
        {
            var card = _Cards[_Ptr];
            if (++_Ptr >= _Cards.Count)
                EndOfDeck();
            return card;
        }

        public List<Card> Next(int cnt)
        {
            List<Card> list = new List<Card>();
            for (int i = 0; i < cnt; i++)
                list.Add(Next());
            return list;
        }

        public virtual void EndOfDeck()
        {
            _Ptr = 0;
            Shuffle();
        }
    }
}
