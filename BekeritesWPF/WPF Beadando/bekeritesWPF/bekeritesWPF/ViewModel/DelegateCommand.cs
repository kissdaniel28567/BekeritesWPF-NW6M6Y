using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace bekeritesWPF.ViewModel {
    public class DelegateCommand : ICommand {
        private readonly Action<object?> _execute;
        private readonly Predicate<object?>? _canExecute;

        public event EventHandler? CanExecuteChanged;

        public DelegateCommand(Action<object?> execute, Predicate<object?>? canExecute = null) {
            if (execute == null) {
                throw new ArgumentNullException(nameof(execute));
            }

            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object? parameter) {
            return _canExecute == null ? true : _canExecute(parameter);
        }

        public void Execute(object? parameter) {
            if (!CanExecute(parameter)) {
                throw new InvalidOperationException("Command execution is disabled");
            }

            _execute(parameter);
        }

        public void RaiseCanExecuteChanged() {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }


        //private readonly Action<Object>? _execute;
        //private readonly Predicate<Object>? _canExecute;
        //
        //public event EventHandler? CanExecuteChanged;
        //
        //public DelegateCommand(Action<Object>? execute, Predicate<Object>? canExecute = null) {
        //    if (execute == null) throw new ArgumentException();
        //
        //    _execute = execute;
        //    _canExecute = canExecute;
        //}
        //
        //public bool CanExecute(object? parameter) {
        //    return parameter != null && (_canExecute == null || _canExecute(parameter));
        //}
        //
        //public void Execute(object? parameter) {
        //    if (!CanExecute(parameter)) {
        //        throw new InvalidOperationException("Execution not disabled");
        //    }
        //
        //    if (_execute == null || parameter == null) throw new ArgumentException();
        //
        //    _execute(parameter);
        //}
        //
        //public void RaiseCanExecute() {
        //    CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        //}
    }
}
