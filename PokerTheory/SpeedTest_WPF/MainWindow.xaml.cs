using HaLi.PokerTheory;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace SpeedTest
{
    public class GameRecord
    {
        public string P1 { get; set; }
        public string P2 { get; set; }
        public string P3 { get; set; }
        public string P4 { get; set; }
        public string P5 { get; set; }
        public string P6 { get; set; }
        public string P7 { get; set; }
        public string P8 { get; set; }
        public string Common { get; set; }
        public string Winner { get; set; }
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Deck _Deck;

        private ulong _CntDeck = 0;
        private ulong _CntHand = 0;

        private long _TotalTime = 0L;

        private long _DeckTime = 0L;
        private long _CardTime = 0L;
        private long _CheckTime = 0L;
        private long _WinTime = 0L;

        private DispatcherTimer _Timer;
        private Stopwatch _Watch;

        // not max speed at start
        // simulate running long time
        private int _Skip = 100;

        private List<GameRecord> _GameRec = new List<GameRecord>();

        public MainWindow()
        {
            InitializeComponent();

            //_ui_Record.DataContext = _GameRec;
            _ui_Record.ItemsSource = new CollectionView(_GameRec);

            _Deck = new Deck();
            _Deck._Cards.AddRange(Deck.Standard(false));

            _Timer = new DispatcherTimer();
            _Timer.Interval = TimeSpan.FromTicks(1);
            _Timer.Tick += _Timer_Tick;

            _Watch = new Stopwatch();
        }

        void _Timer_Tick(object sender, EventArgs e)
        {
            GameRecord rec=null;
            if (_Skip <= 0)
                rec = new GameRecord();

            _Watch.Restart();
            _Deck.EndOfDeck();
            _Watch.Stop();

            if (rec != null)
                _DeckTime += _Watch.ElapsedTicks;

            var common = _Deck.Next(5);

            if (rec != null)
            {
                rec.Common = Hand.CardsToString(common);
            }

            List<Hand> handlist = new List<Hand>();

            Action<int,List<Card>> fnPlyCards = (i, cards) =>
            {
                string str = string.Format("{0},{1}", cards[0], cards[1]);
                switch (i)
                {
                    default: break;
                    case 0: rec.P1 = str; break;
                    case 1: rec.P2 = str; break;
                    case 2: rec.P3 = str; break;
                    case 3: rec.P4 = str; break;
                    case 4: rec.P5 = str; break;
                    case 5: rec.P6 = str; break;
                    case 6: rec.P7 = str; break;
                    case 7: rec.P8 = str; break;
                }
            };

            for (int i = 0; i < 8; i++)
            {
                Hand hand = new Hand();
                _Watch.Restart();
                var plyCards = _Deck.Next(2);
                hand.AddRange(plyCards);
                hand.AddRange(common);
                _Watch.Stop();

                if (rec != null)
                {
                    fnPlyCards(i, plyCards);
                    _CardTime += _Watch.ElapsedTicks;
                }

                _Watch.Restart();
                HandChecker.Check(hand);
                _Watch.Stop();

                if (rec != null)
                    _CheckTime += _Watch.ElapsedTicks;

                handlist.Add(hand);
            }
            
            _Watch.Restart();
            Hand winHand = handlist.Max();
            _Watch.Stop();

            if (rec != null)
            {
                string str = "";
                for (int i = 0; i < 8; i++)
                {
                    if (handlist[i].Equals(winHand))
                        str += string.Format("P{0},", i + 1);
                }
                str = str.Remove(str.Length - 1);
                str += "(" + winHand._Type.ToString() + ")";
                rec.Winner = str;
                _WinTime += _Watch.ElapsedTicks;
            }

            if (rec != null)
            {
                _GameRec.Add(rec);

                _CntDeck++;
                _CntHand += 8;
                _ui_CntDeck.Text = _CntDeck.ToString();
                _ui_CntHand.Text = _CntHand.ToString();

                _TotalTime = _DeckTime + _CardTime + _CheckTime + _WinTime;
                Action<TextBlock,TextBlock,long,ulong> fnTime = (Time, Avg, tm, cnt) =>
                {
                    double sec = TimeSpan.FromTicks(tm).TotalSeconds;
                    Time.Text = sec.ToString("F7");
                    Avg.Text = (sec / cnt).ToString("F10");
                };
                fnTime(_ui_DeckTime, _ui_DeckAvg, _DeckTime, _CntDeck);
                fnTime(_ui_CardTime, _ui_CardAvg, _CardTime, _CntHand);
                fnTime(_ui_CheckTime, _ui_CheckAvg, _CheckTime, _CntHand);
                fnTime(_ui_WinTime, _ui_WinAvg, _WinTime, _CntDeck);
                fnTime(_ui_GameTime, _ui_GameAvg, _TotalTime, _CntDeck);
            }

            if (_Skip > 0)
                _Skip--;

            if (_ui_Mode.SelectedIndex == 0)
            {
                if (_CntDeck >= ulong.Parse(_ui_Setting.Text))
                {
                    _Timer.Stop();

                    CollectionView dgcv = _ui_Record.ItemsSource as CollectionView;
                    if (dgcv != null)
                        dgcv.Refresh();
                }
            }
            else if (_ui_Mode.SelectedIndex == 1)
            {
                double sec = TimeSpan.FromTicks(_TotalTime).TotalSeconds;
                if (sec >= double.Parse(_ui_Setting.Text))
                    _Timer.Stop();
            }
        }

        private void Evt_StartTest(object sender, RoutedEventArgs e)
        {
            if (_Timer.IsEnabled)
                _Timer.Stop();
            else
                _Timer.Start();

            _ui_Start.Content = (_Timer.IsEnabled) ? "Stop" : "Start";
        }
    }
}
