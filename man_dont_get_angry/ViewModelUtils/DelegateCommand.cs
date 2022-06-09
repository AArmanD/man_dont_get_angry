using System;
using System.Windows.Input;

#pragma warning disable CS8767

namespace man_dont_get_angry.ViewModelUtils
{
    /// <summary>
    /// Class that implements an ICommand interface for handling commands which are called by gui elements
    /// </summary>
    public class DelegateCommand : ICommand
    {
        /// <summary>
        /// Saves the action which is executed when the command is Run
        /// </summary>
        private Action<object> _execute;

        /// <summary>
        /// Saves the Function which checks whether a command is runnable
        /// </summary>
        private Func<object, bool> _canExecute;

        /// <summary>
        /// Creates a new DelegateCommand with the provided action.
        /// </summary>
        /// <param name="execute">Action which should be executed when the command is Run</param>
        public DelegateCommand(Action<object> execute)
        {
            _execute = execute;
            _canExecute = (x) => { return true; };
        }

        /// <summary>
        /// Creates a new DelegateCommand with the provided action and condition.
        /// </summary>
        /// <param name="execute">Action which should be executed when the command is Run</param>
        /// <param name="canExecute">Function which checks wheter a command can be Run</param>
        public DelegateCommand(Action<object> execute, Func<object, bool> canExecute)
        {
            _execute = execute ?? throw new NullReferenceException();
            _canExecute = canExecute ?? throw new NullReferenceException();
        }

        /// <summary>
        /// Event which is to be raised when the CanExecute has changed
        /// </summary>
        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// The CanExecute method is called to inform if this command can be executed.
        /// </summary>
        /// <param name="parameter">Parameter which can be handed over at calling</param>
        /// <returns>Whether command can be executed or not</returns>
        public bool CanExecute(object parameter)
        {
            return _canExecute(parameter);
        }

        /// <summary>
        /// The Execute command is called to execute this command.
        /// </summary>
        /// <param name="parameter">Parameter which can be handed over with the command call</param>
        public void Execute(object parameter)
        {
            _execute(parameter);
        }
    }
}
