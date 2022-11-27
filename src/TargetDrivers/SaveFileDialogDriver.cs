using Codeer.Friendly.Windows.Grasp;
using Codeer.Friendly.Windows.NativeStandardControls;
using System.Linq;
using System.Windows.Controls;
using TargetDrivers.Internals;

namespace TargetDrivers;

public sealed class SaveFileDialogDriver
{
    public NativeButton SaveButton { get; }
    public NativeButton CancelButton { get; }
    public NativeComboBox FilePathComboBox { get; }

    public SaveFileDialogDriver(WindowControl window)
    {
        var combos = window.GetFromWindowClass(nameof(ComboBox))
            .OrderBy(MyNativeMethods.GetWindowTop).ToArray();
        FilePathComboBox = new NativeComboBox(combos[1]);    // ファイル名

        SaveButton = new NativeButton(window.IdentifyFromWindowText("保存(&S)"));
        CancelButton = new NativeButton(window.IdentifyFromWindowText("キャンセル"));
    }
}
