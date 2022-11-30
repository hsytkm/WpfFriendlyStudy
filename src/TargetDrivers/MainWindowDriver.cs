using Codeer.Friendly;
using Codeer.Friendly.Windows.Grasp;
using RM.Friendly.WPFStandardControls;
using System.Windows.Controls;

namespace TargetDrivers;

public sealed class MainWindowDriver
{
    //public IWPFDependencyObjectCollection<DependencyObject> LogicalTree { get; }
    public WPFTextBox Name { get; }
    public WPFComboBox Job { get; }
    public WPFTextBox Mp { get; }
    public WPFToggleButton HasMp { get; }
    public WPFButtonBase SaveFile { get; }
    public WPFTextBlock Base64 { get; }

    public MainWindowDriver(AppVar windowObject)
    {
        WindowControl window = new(windowObject);
        var logicalTree = window.LogicalTree();
        //var visualTree = window.VisualTree();

        Name = new WPFTextBox(logicalTree.ByType<TextBox>().ByBinding("Name").Single());
        Job = new WPFComboBox(logicalTree.ByType<ComboBox>().ByBinding("SelectedJob").Single());
        Mp = new WPFTextBox(logicalTree.ByType<TextBox>().ByBinding("MpText").Single());
        HasMp = new WPFToggleButton(logicalTree.ByType<CheckBox>().ByBinding("RequireMp").Single());

        // dynamic で取得することもできる(けどちょっとキモー)
        //Name = logicalTree.ByType<TextBox>().ByBinding("Name").Single().Dynamic();

        SaveFile = new WPFButtonBase(logicalTree.ByType<Button>().Single());
        Base64 = new WPFTextBlock(logicalTree.ByType<TextBlock>().ByBinding("Base64String").Single());
    }
}
