namespace AnimationTesting
{
    using System;
    using System.Windows.Input;

    public class BasicCommand : ICommand
    {
        private Func<object, Boolean> _CanExecute;
        private Action<object> _Execute;

        public bool CanExecute(object parameter)
        {
            if (_CanExecute != null)
                return _CanExecute.Invoke(parameter);

            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            if (_Execute != null)
                _Execute.Invoke(parameter);
        }

        /// <summary>
        /// Initializes a new instance of the BasicCommand class.
        /// </summary>
        /// <param name="canExecute"></param>
        /// <param name="execute"></param>
        public BasicCommand(Func<object, Boolean> canExecute, Action<object> execute)
        {
            _CanExecute = canExecute;
            _Execute = execute;
        }

        /// <summary>
        /// Initializes a new instance of the BasicCommand class.
        /// </summary>
        /// <param name="execute"></param>
        public BasicCommand(Action<object> execute)
        {
            _Execute = execute;
        }

        public void OnCanExecuteChanged()
        {
            var evt = CanExecuteChanged;

            if (evt != null)
                evt.Invoke(this, new EventArgs());
        }
    }
}
