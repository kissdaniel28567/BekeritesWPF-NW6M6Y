using Accessibility;
using bekeritesWPF.ViewModel;
using GameMechanics.Model;
using GameMechanics.Persistence;
using Microsoft.Win32;
using System.Configuration;
using System.Data;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace bekeritesWPF {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application, IDisposable {
        private MainViewModel? _mainViewModel = null;
        private MainWindow? _mainWindow = null;
        private String[]? AddedPlayers = null;
        private bool _disposed = false;

        private GameModel? _model = null;

        public App() {
            Startup += OnStartup;
        }
        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void OnStartup(object sender, StartupEventArgs e) {
            _model = new GameModel(new GameFileDataAccess()); 
            _mainViewModel = new MainViewModel(_model);
            AddedPlayers = new String[2];
            //Events go here
            //_mainViewModel.ButtonSelected += ButtonSelected;
            _mainViewModel.LoadGame += LoadGame;
            _mainViewModel.SaveGame += SaveGame;

            _mainWindow = new MainWindow {
                DataContext = _mainViewModel
            };
            _mainWindow.Show();
        }

        private async void SaveGame(object? sender, EventArgs e) {
            //meg kell hívni a model savegame-jét, de a png-s mókulással, csak itt txt-be
            SaveFileDialog saveDialog = new SaveFileDialog {
                Title = "Save Game",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                RestoreDirectory = true,
                Filter = "TXT files (*.txt)|*.txt"
            };

            if (saveDialog.ShowDialog() == true) {
                await SaveAsync(saveDialog.FileName);
            }
        }

        private async Task SaveAsync(string path) {
            if (_model == null) return;
            await _model.SaveGameAsync(path);
        }

        private async void LoadGame(object? sender, EventArgs e) {
            OpenFileDialog openFileDialog = new OpenFileDialog {
                Title = "Load Game",
                Filter = "TXT files (*.txt)|*.txt",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                RestoreDirectory = true
            };
            if (openFileDialog.ShowDialog() == true) {
                await LoadAsync(openFileDialog.FileName);
            }
        }

        private async Task LoadAsync(String path) {
            if (_model == null) MessageBox.Show("Game is null");
            else
                try {
                    await _model.LoadGameAsync(path);
                } catch (Exception) {
                    MessageBox.Show("Something went wrong, or corrupted file!", "error", MessageBoxButton.OK);
                    return;
                }
            MessageBox.Show("Game has loaded successfully!", "Success", MessageBoxButton.OK);
        }

        protected virtual void Dispose(bool disposing) {
            if (_disposed)
                return;

            if (disposing) {
                if (_mainViewModel != null) {
                    _mainViewModel.LoadGame -= LoadGame;
                    _mainViewModel.SaveGame -= SaveGame;
                    _mainViewModel.Dispose();
                }
            }
            _disposed = true;
        }

    }

}
