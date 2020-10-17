# PokerTheory
A simple library of playing card check

This is my old project, wrote at serveral years ago.<br/>
At that time I work in a game company which make many poker game, I write library many time, again and again when sometime I feel not satisfied, and this is most final version<br/>
(Maybe I will make some change, this style is my past style, modify to more morden if i am have some time)<br/>
<br/>
This library is suit to teach junior about programming skill. <br/>
<br/>
>If Looking for library about poker / playing card, you may see these before</br>
>Google search may find this:<br/>
>https://www.codeproject.com/Articles/12279/Fast-Texas-Holdem-Hand-Evaluation-and-Analysis<br/>
>or this:<br/>
>https://github.com/NikolayIT/TexasHoldemGameEngine<br/>
<br/>
it is nice if you just need a library to use, but it is not easy to read, especially the one on codeproject<br/>
<br/>
I think nice code should be:<br/>
- clean (less useless code)<br/>
- readable (use meaningful words and leave comment)<br/>
- functions (extract into many simple, dont a big function do everything)<br/>

And last thing is perfermance, if coding a simple, perfermance is high

## Concept
ok, too many words about my past... back to the theory

#### 1. Define Card
Rank: 2~9,10,J,Q,K,A</br>
Suit: ♦♣♥♠</br>
Use bits to present which card it is</br>
| 20 | 19 | 18 | 17 | 16 | 15 | 14 | 13 | 12 | 11 | 10 | 9  | 8 | 7 | 6 | 5 | 4 | 3 | 2 | 1 |
|----|----|----|----|----|----|----|----|----|----|----|----|---|---|---|---|---|---|---|---|
| ♠  | ♥  | ♣  | ♦  |    |    |    | A  | K  | Q  | J  | 10 | 9 | 8 | 7 | 6 | 5 | 4 | 3 | 2 |

> e.g.</br>
> A♠ = 1000 0001 0000 0000 0000 = 0x81000</br>
> Q♥ = 0100 0000 0100 0000 0000 = 0x40400</br>
> 8♣ = 0010 0000 0000 0100 0000 = 0x20040</br>
> 3♦ = 0001 0000 0000 0000 0010 = 0x10002</br>

#### 2. Hand value
- Count every rank and every suit
- Combine all card bits in hand by OR operation
> e.g.</br>
> A♠K♠Q♠J♠10♠</br>
> = 1000 0001 1111 0000 0000</br>
> Bits 17\~20 represent suit, only one bit have 1 mean it is a flush</br>
> Bits 1\~13 represent rank, it show 5 continue 1 mean it is a straight </br>
