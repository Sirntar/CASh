using System;
using System.Windows.Input;

namespace CASh.Core
{
    /**
     * 
     * An implementation of custom Class for binding commands to controlls.
     * This class will allow to bind code exacution to controlls directly from the code.
     * 
     **/
    class RelayCommand : ICommand
    {
        readonly private Action<object> _execute;
        readonly private Func<object, bool>? _canExecute;

        public RelayCommand(Action<object> execute, Func<object, bool>? canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object? param)
        {
            if (param != null)
                return _canExecute == null || _canExecute(param);
            else
                return _canExecute == null;
        }

        public void Execute(object? param)
        {
            if(param != null)
                _execute(param);
            else
                _execute(true);
        }
    }
}
