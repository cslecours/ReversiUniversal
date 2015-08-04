using GalaSoft.MvvmLight.Messaging;
using ReversiUniversal.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ReversiUniversal
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        GameViewModel game;

        public MainPage()
        {
            this.InitializeComponent();

            game = (this.DataContext as GameViewModel);

            Messenger.Default.Register<RequestPlayerSettingsMessage>(this, x =>
            {
                //var result = new WindowPlayerNames() { DataContext = x.PlayerNames }.ShowDialog();
                //if (result == true)
                //{
                //    Messenger.Default.Send<PlayerSettingsChangedMessage>(new PlayerSettingsChangedMessage() { BlackName = x.PlayerNames.BlackName, WhiteName = x.PlayerNames.WhiteName });
                //}
            });

            MakeGrid();

            game.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(game_PropertyChanged);
            //game.Queue.OnTurnSkipped += (o, e) => MessageBox.Show("Turn skipped!");
            //game.Game.GameOver += (o, e) => MessageBox.Show("Game over!");
            UpdateGrid();
        }

        void game_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Game")
            {
                UpdateGrid();
            }
        }

        private void UpdateGrid()
        {
            var board = game.Game;
            var black = game.Black;
            var white = game.White;

            var blackBrush = new SolidColorBrush(Windows.UI.Colors.Black);
            var whiteBrush = new SolidColorBrush(Windows.UI.Colors.White);
            Func<RevLib.Player, Brush> func = p => p == black ? blackBrush : p == white ? whiteBrush : null;

            foreach (FrameworkElement item in g.Children.OfType<FrameworkElement>())
            {
                int x = Grid.GetColumn(item);
                int y = Grid.GetRow(item);

                if (item is Ellipse)
                {
                    var el = item as Ellipse;

                    if (board[x, y] == null)
                    {
                        if (game.Game.CanPlayThere(board.CurrentPlayer, x, y))
                        {
                            el.Fill = func(board.CurrentPlayer);
                            el.Opacity = 0.4;
                        }
                        else
                        {
                            el.Fill = null;
                            el.Opacity = 0;
                        }
                    }
                    else
                    {
                        el.Opacity = 1;
                        el.Fill = func(board[x, y]);
                    }
                }
            }
        }

        private void MakeGrid()
        {
            var creator = new GridCreator();

            creator.SetupGrid(g, 8, 8);
            creator.FillWithEllipseWithEvent(g, t_MouseLeftButtonUp,
                (o,e) =>
                {
                    Shape tok = o as Shape;
                    if (tok.Opacity < 1)
                    {
                        tok.Opacity = 0.7;
                    }
                },
                (o,e) =>
                {
                    Shape tok = o as Shape;
                    if (tok.Opacity < 1)
                    {
                        tok.Opacity = 0.4;
                    }
                });
        }

        void t_MouseLeftButtonUp(object sender, TappedRoutedEventArgs e)
        {
            var tok = sender as Ellipse;
            game.PlayCommand.Execute(tok);
        }
    }
}
