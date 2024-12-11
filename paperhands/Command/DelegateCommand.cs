using System.Windows.Input;

namespace paperhands.Command;

public class DelegateCommand : ICommand
{
    private readonly Func<object?, bool> _canExecute;
    private readonly Action<object> _execute;

    public DelegateCommand(Action<object> execute, Func<object?, bool> canExecute = null)
    {
        ArgumentNullException.ThrowIfNull(execute);
        _execute = execute;
        _canExecute = canExecute;
    }

    public event EventHandler? CanExecuteChanged;

    public bool CanExecute(object? parameter)
    {
        return _canExecute is null ? true : _canExecute(parameter);
    }

    public void Execute(object? parameter)
    {
        _execute(parameter);
    }

    public void RaiseCanExecuteChanged()
    {
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}