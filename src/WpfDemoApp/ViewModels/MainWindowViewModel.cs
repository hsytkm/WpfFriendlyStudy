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
                RequireMp = SelectedJob.HasMp();
                SaveFileCommand.UpdateCanExecute();
            }
        }
    }
    Enums.Job _selectedJob = Enums.Job.Warrior;

    public bool RequireMp
    {
        get => _requireMp;
        set => SetProperty(ref _requireMp, value);
    }
    bool _requireMp;

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


    public string Base64String
    {
        get => _base64String;
        set => SetProperty(ref _base64String, value);
    }
    string _base64String = "";

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
            {
                player.Save(saveFilePath);
                Base64String = player.ToBase64();
            }
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
