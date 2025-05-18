namespace bekeritesWPF.ViewModel {
    public class GameField : ViewModelBase {
        private Boolean _isLocked;
        private int _value;

        public Boolean IsLocked {
            get { return _isLocked; }
            set {
                if (_isLocked != value) {
                    _isLocked = value;
                    OnPropertyChanged();
                }
            }
        }

        public int Value {
            get { return _value; }
            set {
                if (_value != value) {
                    _value = value;
                    OnPropertyChanged();
                }
            }
        }

        public Int32 X { get; set; }
        public Int32 Y { get; set; }
        public Tuple<Int32, Int32> XY {
            get { return new(X, Y); }
        }

        public DelegateCommand? StepCommand { get; set; }
    }
}
