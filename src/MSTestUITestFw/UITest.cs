using Codeer.Friendly;
using Codeer.Friendly.Dynamic;
using Codeer.Friendly.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Windows;
using TargetDrivers;

namespace MSTestUITest;

[TestClass]
public class UITest
{
    // テスト対象 exe は Build の PostProcess で同フォルダにコピー済み
    const string TestAppExeName = "WpfDemoApp.exe";

    WindowsAppFriend _app = default!;
    MainWindowDriver _driver = default!;

    [TestInitialize]
    public void Initialize()
    {
        var process = Process.Start(TestAppExeName);
        _app = new WindowsAppFriend(process);
        AppVar windowObject = _app.Type<Application>().Current.MainWindow;
        _driver = new MainWindowDriver(windowObject);
    }

    [TestCleanup]
    public void Cleanup()
    {
        using var process = Process.GetProcessById(_app.ProcessId);
        _app.Dispose();
        process.CloseMainWindow();
    }

    [TestMethod]
    public void SaveButton_CanExecute()
    {
        // 起動直後は妥当性のある設定
        Assert.AreEqual(true, _driver.SaveFile.IsEnabled);

        // Name が空はNG
        _driver.Name.EmulateChangeText("");
        Assert.AreEqual(false, _driver.SaveFile.IsEnabled);
    }

    [TestMethod]
    public void DotNETとFriendlyとMSTestの組み合わせは動作しないようです()
    {
        Assert.AreEqual(true, true);
    }
}
