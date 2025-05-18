using GameMechanics.Model;
using GameMechanics.Persistence;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Drawing;
using System.Windows;
using Color = System.Drawing.Color;
using Size = System.Drawing.Size;

namespace bekeritesWPF.ViewModel {
    public class MainViewModel : ViewModelBase, IDisposable {
        #region Fields

        private readonly String[] DefaultNames = { "Player One Name", "Player One Color", "Player Two Name", "Player Two Color" };
        private readonly Size GameBoardSize = new Size(400, 400);
        private GameModel? _model;
        private bool _disposed = false;
        private Size _buttonSize;

        #endregion

        #region Commands
        public DelegateCommand StartGame { get; set; }
        public DelegateCommand LoadGameCommand { get; set; }
        public DelegateCommand SaveGameCommand { get; set; }
        public DelegateCommand RadioChange { get; set; }
        //public DelegateCommand ButtonSelectedCommand { get; set; }
        public DelegateCommand AddPlayerCommand { get; set; }

        #endregion


        #region Events
        //public event EventHandler<GameField>? ButtonSelected;
        public event EventHandler? SaveGame;
        public event EventHandler? LoadGame;
        //public event EventHandler<int>? NewGame;
        //public event EventHandler? AddPlayer;

        #endregion

        #region Properties
        public ObservableCollection<GameField> GameBoard { get; private set; }
        public String testLabelText { get; set; }

        public int BoardSize { get; set; }

        public Size ButtonSize {
            get => _buttonSize;
            private set {
                _buttonSize = value;
                OnPropertyChanged();
                Console.WriteLine("ButtonSizeChanged");
            }
        }

        public bool IsPlayerInputEnabled { get; private set; }
        public bool IsSizeInputEnabled { get; private set; }

        public int Player1Point { get; set; }
        public int Player2Point { get; set; }

        public String Player1Name { get; set; }
        public String Player2Name { get; set; }

        public String Player1Color { get; set; }
        public String Player2Color { get; set; }
        #endregion


        #region public functions

        // Color has been set, user will set the players at the beggining of the game and the player will set the new player onece the game ends
        public MainViewModel(GameModel model) {
            _model = model;
            AddEvents();
            IsPlayerInputEnabled = true;
            IsSizeInputEnabled = true;
            OnPropertyChanged(nameof(IsPlayerInputEnabled));
            OnPropertyChanged(nameof(IsSizeInputEnabled));
            Player1Name  = DefaultNames[0];
            Player1Color = DefaultNames[1];
            Player2Name  = DefaultNames[2];
            Player2Color = DefaultNames[3];
            testLabelText = "fasdlf";
            GameBoard = new ObservableCollection<GameField>();
            BoardSize = 8;
            StartGame = new DelegateCommand(_ => {
                try {
                    OnNewGame();
                } catch (Exception e) {
                    MessageBox.Show($"Add players first or load game {e}", "Error", MessageBoxButton.OK);
                }
            });
            AddPlayerCommand = new DelegateCommand(_ => {
                if(DefaultNames.Contains(Player1Name) && DefaultNames.Contains(Player2Name)) {
                    MessageBox.Show("Don't leave playername default!", "Error!", MessageBoxButton.OK);
                    return;
                }
                if (!PlayerValidator.IsCorrectPlayer(Player1Name, Player1Color)) { 
                    MessageBox.Show($"Don't leave player1 fields empty and give a valid colorname {Player1Color}, {Player1Name} please!", "Error!", MessageBoxButton.OK);
                    return;
                }
                if (!PlayerValidator.IsCorrectPlayer(Player2Name, Player2Color)) { 
                    MessageBox.Show("Don't leave player2 fields empty and give a valid colorname please!", "Error!", MessageBoxButton.OK);
                    return;
                }
                MessageBox.Show("Player adding was successful", "Success!", MessageBoxButton.OK);
                IsPlayerInputEnabled = false;
                OnPropertyChanged(nameof(IsPlayerInputEnabled));
            });
            RadioChange = new DelegateCommand(param => {
                if (param is String radioText) {
                    int size = int.Parse(radioText.Split("X")[0]);
                    if (BoardSize != size) {
                        BoardSize = size;
                        OnPropertyChanged(nameof(ButtonSize));
                        testLabelText = $"{size}";
                        OnPropertyChanged(nameof(testLabelText));
                    }
                }
            });
            //ButtonSelectedCommand = new DelegateCommand(param => {
            //    if (param is GameField selectedField) {
            //        try {
            //            (int x, int y) = selectedField.XY;
            //            _model?.Step(x, y);
            //        } catch (Exception) {
            //            MessageBox.Show("Click adjament buttons and do not step on the other fields!");
            //        }
            //    }
            //});
            //save and load in app.xaml.cs
            SaveGameCommand = new DelegateCommand(param => {
                if(_model == null) { throw new NullReferenceException(); }
                if (GameBoard.Count > 0) {
                    SaveGame?.Invoke(this, EventArgs.Empty);
                } else {
                    MessageBox.Show("Cannot save empty game!", "Error!", MessageBoxButton.OK);
                }
            });

            LoadGameCommand = new DelegateCommand(param => {
                if (_model != null) {
                    LoadGame?.Invoke(this, EventArgs.Empty);
                }
            });

        }

        public void LoadPropertieChange() {
            OnPropertyChanged(nameof(Player1Color));
            OnPropertyChanged(nameof(Player2Color));
            OnPropertyChanged(nameof(Player1Name));
            OnPropertyChanged(nameof(Player2Name));
            OnPropertyChanged(nameof(Player1Point));
            OnPropertyChanged(nameof(Player2Point));
            OnPropertyChanged(nameof(BoardSize));
        }
        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

        #region private functions
        private void OnNewGame() {
            if(IsPlayerInputEnabled) {
                MessageBox.Show("Add Players first!", "Error!", MessageBoxButton.OK);
            }
            else {
                _model?.NewGame(BoardSize, Player1Name, Player1Color, Player2Name, Player2Color);
                IsPlayerInputEnabled = false;
                IsSizeInputEnabled = false;
                OnPropertyChanged(nameof(IsPlayerInputEnabled));
                OnPropertyChanged(nameof(IsSizeInputEnabled));
                //AddEvents()
            }
        }



        private void AddEvents() {
            if (_model == null) throw new NullReferenceException();
            _model.PointsChanged += PointsChanged;
            _model.TableChanged += TableChanged;
            _model.TableReady += TableReady;
            _model.GameEnded += GameEnded;
            _model.PlayerChanged += PlayerChanged;
        }

        private void PlayerChanged(object? sender, PlayerEventArgs e) {
            if (e.Player1Name == null || e.Player2Name == null || e.Player1Color == null || e.Player2Color == null) return;
            Player1Name = e.Player1Name;
            Player2Name = e.Player2Name;
            OnPropertyChanged(nameof(Player1Name));
            OnPropertyChanged(nameof(Player2Name));
            Player1Color = e.Player1Color;
            Player2Color = e.Player2Color;
            OnPropertyChanged(nameof(Player1Color));
            OnPropertyChanged(nameof(Player2Color));
            IsSizeInputEnabled = false;
            IsPlayerInputEnabled = false;
            OnPropertyChanged(nameof(IsSizeInputEnabled));
            OnPropertyChanged(nameof(IsPlayerInputEnabled));
        }

        private void RemoveEvents() {
            if (_model == null) throw new NullReferenceException();
            _model.PointsChanged -= PointsChanged;
            _model.TableChanged -= TableChanged;
            _model.TableReady -= TableReady;
            _model.GameEnded -= GameEnded;
        }

        private void GameEnded(object? sender, String winner) {
            if (_model == null) throw new NullReferenceException();
            if (winner == "Draw") {
                MessageBox.Show($"Game over. The winner is... Well, both of you", "Never Gonna Give You Up",
                    MessageBoxButton.OK);
            } else {
                MessageBox.Show($"Game over. The winner is {winner}", "Game over",
                    MessageBoxButton.OK);
            }
            foreach (GameField field in GameBoard) {
                field.IsLocked = true;
            }

            IsPlayerInputEnabled = true;
            IsSizeInputEnabled = true;
            OnPropertyChanged(nameof(IsPlayerInputEnabled));
            OnPropertyChanged(nameof(IsSizeInputEnabled));
            ///_model.NewGame(BoardSize);
        }

        private void TableReady(object? sender, int tableSize) {
            if (_model == null) throw new NullReferenceException();
            GameBoard.Clear();
            BoardSize = tableSize;
            OnPropertyChanged(nameof(BoardSize));
            for (int i = 0; i < tableSize; i++) {
                for (int j = 0; j < tableSize; j++) {
                    GameField field = new GameField {
                        Value = 0,
                        IsLocked = false,
                        X=i,
                        Y=j,
                        StepCommand = new DelegateCommand(param => {
                            if (param is Tuple<Int32, Int32> position)
                                StepGame(position.Item1, position.Item2);
                        })
                    };
                    GameBoard.Add(field);
                }
            }
            OnPropertyChanged(nameof(GameBoard));
        }

        private void StepGame(int x, int y) {
            if(_model == null) { throw new NullReferenceException(); }
            try {
                
                GameBoard[y + x * BoardSize].Value = _model.Step(x, y);;
            } catch (Exception) {
                MessageBox.Show("Click adjament buttons and do not step on the other fields!");
            }
        }

        private void TableChanged(object? sender, int[][] field) {
            if (_model == null) throw new NullReferenceException();
            int size = field.GetLength(0);
            for (int i = 0; i < size; i++) {
                for (int j = 0; j < size; j++) {
                    GameBoard[j + i * size].Value = field[i][j];
                    if (field[i][j] != 0) GameBoard[j + i * size].IsLocked = true;
                }
            }
            OnPropertyChanged(nameof(GameBoard));
        }

        private void PointsChanged(object? sender, PointsEventArgs points) {
            Player1Point = points.Player1Point;
            Player2Point = points.Player2Point;
            OnPropertyChanged(nameof(Player1Point));
            OnPropertyChanged(nameof(Player2Point));
        }

        protected virtual void Dispose(bool disposing) {
            if (_disposed)
                return;

            if (disposing) {
                if (_model != null) {
                    RemoveEvents();
                }
            }
            _disposed = true;
        }

        #endregion
    }
}
