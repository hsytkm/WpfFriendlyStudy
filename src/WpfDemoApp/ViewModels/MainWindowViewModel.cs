namespace WpfDemoApp;

internal sealed class MainWindowViewModel : BindableBase
{
    public string Name
    {
        get => _name;
        set
        {
            if (SetProperty(ref _name, value))
            {
                SaveFileCommand.UpdateCanExecute();
            }
        }
    }
    string _name = "bob";

    public Enums.Job SelectedJob
    {
        get => _selectedJob;
        set
        {
            if (SetProperty(ref _selectedJob, value))
            {
                NotifyPropertyChanged(nameof(RequireMp));
                SaveFileCommand.UpdateCanExecute();
            }
        }
    }
    Enums.Job _selectedJob = Enums.Job.Warrior;

    public bool RequireMp => Player.RequireMp(SelectedJob);

    public string MpText
    {
        get => _mpText;
        set
        {
            if (SetProperty(ref _mpText, value))
            {
                SaveFileCommand.UpdateCanExecute();
            }
        }
    }
    string _mpText = "0";

    public DelegateCommand SaveFileCommand => _saveFileCommand ??= new(SaveFile, CanSaveFile);
    DelegateCommand? _saveFileCommand;

    void SaveFile()
    {
        var dialog = new Microsoft.Win32.SaveFileDialog
        {
            Title = "Save base64 text",
            Filter = "Text file|*.txt"
        };
        bool? result = dialog.ShowDialog();

        if (result.HasValue && result.Value && dialog.FileName is string saveFilePath)
        {
            if (CreatePlayer() is Player player)
                player.Save(saveFilePath);
        }
    }

    bool CanSaveFile() => CreatePlayer()?.Valid ?? false;

    Player? CreatePlayer()
    {
        int mp = 0;

        if (RequireMp)
        {
            if (!int.TryParse(MpText, out mp))
                return null;
        }

        return new Player()
        {
            Name = Name,
            Job = SelectedJob,
            Mp = mp
        };
    }
}
