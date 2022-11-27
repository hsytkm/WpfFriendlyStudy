using System.Windows.Input;

namespace WpfDemoApp;

internal sealed class DelegateCommand : ICommand
{
    private readonly Action _execute;
    private readonly Func<bool>? _canExecute;

    public event EventHandler? CanExecuteChanged;

    public DelegateCommand(Action action, Func<bool>? canExecute = null)
    {
        _execute = action;
        _canExecute = canExecute;
    }

    public void Execute(object? parameter) => _execute();

    public bool CanExecute(object? parameter) => (_canExecute is null) || _canExecute();

    public void UpdateCanExecute() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
}
