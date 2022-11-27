using Codeer.Friendly;
using Codeer.Friendly.Dynamic;
using Codeer.Friendly.Windows;
using Codeer.Friendly.Windows.Grasp;
using NUnit.Framework;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows;
using TargetDrivers;

namespace NunitUITest;

[TestFixture]
public class UITest
{
    // テスト対象 exe は Build の PostProcess で同フォルダにコピー済み
    const string TestAppExeName = "WpfDemoApp.exe";
    const string SaveFileName = "temp.txt";
    const string SaveDialogWindowTitle = "Save base64 text";

    WindowsAppFriend _app = default!;
    MainWindowDriver _driver = default!;

    [SetUp]
    public void Setup()
    {
        var process = Process.Start(TestAppExeName);
        _app = new WindowsAppFriend(process);
        AppVar windowObject = _app.Type<Application>().Current.MainWindow;
        _driver = new MainWindowDriver(windowObject);
    }

    [TearDown]
    public void TearDown()
    {
        using var process = Process.GetProcessById(_app.ProcessId);
        _app.Dispose();
        process.CloseMainWindow();
    }

    [Test]
    public void SaveButton_CanExecute()
    {
        // 起動直後は妥当性のある設定
        Assert.That(_driver.SaveFile.IsEnabled, Is.True);

        _driver.Name.EmulateChangeText("1234");
        _driver.Job.EmulateChangeSelectedIndex(1);  // Warrior
        Assert.That(_driver.SaveFile.IsEnabled, Is.True);

        // Name が空はNG
        _driver.Name.EmulateChangeText("");
        Assert.That(_driver.SaveFile.IsEnabled, Is.False);

        // Name は4文字までOK
        _driver.Name.EmulateChangeText("1234");
        Assert.That(_driver.SaveFile.IsEnabled, Is.True);

        // Wizard は MP が1以上でないとNG
        _driver.Job.EmulateChangeSelectedIndex(2);  // Wizard
        _driver.Mp.EmulateChangeText("0");
        Assert.That(_driver.SaveFile.IsEnabled, Is.False);

        // Wizard で MP が設定されていればOK
        _driver.Mp.EmulateChangeText("999");
        Assert.That(_driver.SaveFile.IsEnabled, Is.True);

        // Wizard の MP は半角数値以外NG
        _driver.Mp.EmulateChangeText("１");
        Assert.That(_driver.SaveFile.IsEnabled, Is.False);

        // Warrior は魔法を使えない仕様なので MP に数値以外が入っててもOK
        _driver.Mp.EmulateChangeText("foge");
        _driver.Job.EmulateChangeSelectedIndex(1);  // Warrior
        Assert.That(_driver.SaveFile.IsEnabled, Is.True);
    }

    [Test]
    public void SaveFile()
    {
        const string expected = @"dGVzdA0KMQ0K";    //test/Warrior
        _driver.Name.EmulateChangeText("test");
        _driver.Job.EmulateChangeSelectedIndex(1);
        Assert.That(_driver.SaveFile.IsEnabled, Is.True);

        // ファイル保存
        string saveFilePath = Path.Combine(Path.GetTempPath(), SaveFileName);
        File.Delete(saveFilePath);  // 事前に削除しておきます
        _driver.SaveFile.EmulateClick(new Async());
        var windowControl = WindowControl.WaitForIdentifyFromWindowText(_driver.SaveFile.App, SaveDialogWindowTitle);
        var saveDialog = new SaveFileDialogDriver(windowControl);
        saveDialog.FilePathComboBox.EmulateChangeEditText(saveFilePath);
        saveDialog.SaveButton.EmulateClick();
        Thread.Sleep(500);          // ファイル保存待ち

        // ファイル読み出しテスト
        var actual = File.ReadAllText(saveFilePath);
        Assert.That(actual, Is.EqualTo(expected));
        File.Delete(saveFilePath);
    }

}
