using System;
using RevLib;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Windows.Input;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ReversiUniversal
{
    public class GameViewModel : ViewModelBase
    {
        public ICommand CreateGameCommand { get; private set; }
        public RevGame Game { get; private set; }
        public RelayCommand<Ellipse> PlayCommand { get; private set; }
        public RelayCommand UndoCommand{ get; private set; }
        public RelayCommand RedoCommand { get; private set; }
        public RelayCommand OpenPlayerNameChangerCommand { get; private set; }
        public RelayCommand ToggleSidePanelCommand { get; private set; }
        public ISnapshotContainer<Turn> SnapshotContainer { get; private set; }
        public IPlayerQueue Queue { get; private set; }
        public Player Black { get; private set; }
        public Player White { get; private set; }

        private Visibility sidePanelVisibility;
        public Visibility SidePanelVisibility { get { return sidePanelVisibility; } set { Set(() => SidePanelVisibility, ref sidePanelVisibility, value); } }

        public GameViewModel()
        {
            CreateGameCommand = new RelayCommand(CreateGame);
            PlayCommand = new RelayCommand<Ellipse>(Play, CanPlay);
            MessengerInstance.Register<PlayerSettingsChangedMessage>(this, x =>
            {
                Black.Name = x.BlackName;
                White.Name = x.WhiteName;
            });


            OpenPlayerNameChangerCommand = new RelayCommand(OpenPlayerNameChangerWindow);
            SidePanelVisibility = Visibility.Visible;

            UndoCommand = new RelayCommand(Undo, () => SnapshotContainer.CanUndo());
            RedoCommand = new RelayCommand(Redo, () => SnapshotContainer.CanRedo());
            ToggleSidePanelCommand = new RelayCommand(() => SidePanelVisibility = SidePanelVisibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible);

            CreateGame();
        }

        private void Undo()
        {
            var turn = SnapshotContainer.Undo();
            Game.LoadBoard(turn.Board.Clone(), turn.PlayerToPlay);
            
            RaisePropertyChanged(() => Game);
        }

        private void Redo()
        {
            var turn = SnapshotContainer.Redo();
            Game.LoadBoard(turn.Board.Clone(), turn.PlayerToPlay);

            RaisePropertyChanged(() => Game);
        }

        private void CreateGame()
        {
            Black = new Player(Token.Black);
            White = new Player(Token.White);
            
            var queue = new PlayerQueue(Black, White);
            Queue = queue;

            Game = new RevGame(Queue);
            SnapshotContainer = Game.SnapshotContainer;
            SnapshotContainer.SetInitialState(new OnePartTurn() { Board = Game.CloneBoard(), PlayerThatPlayed = White, PlayerToPlay = Black });

            RaisePropertyChanged(() => Game);
            RaisePropertyChanged(() => SnapshotContainer);
        }

        private void Play(Ellipse tok)
        {
            int x = Grid.GetColumn(tok);
            int y = Grid.GetRow(tok);

            Game.Play(Game.CurrentPlayer, x, y);
            RaisePropertyChanged(() => Game);
        }

        private bool CanPlay(Ellipse tok)
        {
            return tok.Opacity < 1;
        }

        private void OpenPlayerNameChangerWindow()
        {
            MessengerInstance.Send(new RequestPlayerSettingsMessage() { PlayerNames = new PlayerNamesViewModel() { BlackName = Black.Name, WhiteName = White.Name } });
        }

        public override void Cleanup()
        {
            base.Cleanup();
            Game = null;
            Queue = null;
            Black = null;
            White = null;
            PlayCommand = null;
            CreateGameCommand = null;
        }
    }
}
