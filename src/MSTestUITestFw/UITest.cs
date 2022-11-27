using Codeer.Friendly;
using Codeer.Friendly.Dynamic;
using Codeer.Friendly.Windows;
using Codeer.Friendly.Windows.Grasp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows;
using TargetDrivers;

namespace MSTestUITest;

[TestClass]
public class UITest
{
    // テスト対象 exe は Build の PostProcess で同フォルダにコピー済み
    const string TestAppExeName = "WpfDemoApp.exe";
    const string SaveFileName = "temp.txt";
    const string SaveDialogWindowTitle = "Save base64 text";

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

        _driver.Name.EmulateChangeText("1234");
        _driver.Job.EmulateChangeSelectedIndex(1);  // Warrior
        Assert.AreEqual(true, _driver.SaveFile.IsEnabled);

        // Name が空はNG
        _driver.Name.EmulateChangeText("");
        Assert.AreEqual(false, _driver.SaveFile.IsEnabled);

        // Name は4文字までOK
        _driver.Name.EmulateChangeText("1234");
        Assert.AreEqual(true, _driver.SaveFile.IsEnabled);

        // Wizard は MP が1以上でないとNG
        _driver.Job.EmulateChangeSelectedIndex(2);  // Wizard
        _driver.Mp.EmulateChangeText("0");
        Assert.AreEqual(false, _driver.SaveFile.IsEnabled);

        // Wizard で MP が設定されていればOK
        _driver.Mp.EmulateChangeText("999");
        Assert.AreEqual(true, _driver.SaveFile.IsEnabled);

        // Wizard の MP は半角数値以外NG
        _driver.Mp.EmulateChangeText("１");
        Assert.AreEqual(false, _driver.SaveFile.IsEnabled);

        // Warrior は魔法を使えない仕様なので MP に数値以外が入っててもOK
        _driver.Mp.EmulateChangeText("foge");
        _driver.Job.EmulateChangeSelectedIndex(1);  // Warrior
        Assert.AreEqual(true, _driver.SaveFile.IsEnabled);
    }

    [TestMethod]
    public void SaveFile()
    {
        const string expected = @"dGVzdA0KMQ0K";    //test/Warrior
        _driver.Name.EmulateChangeText("test");
        _driver.Job.EmulateChangeSelectedIndex(1);
        Assert.AreEqual(true, _driver.SaveFile.IsEnabled);

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
        Assert.AreEqual(expected, actual);
        File.Delete(saveFilePath);
    }

    [TestMethod]
    public void dotNETFwとFriendlyとXUnitの組み合わせは動作しないようです()
    {
        // Friendly + XUnit の組み合わせは動作させられませんでした。
        // xunit.runner.visualstudio が原因のようです。
        //  - Ver2.4.1 以前だとテストで Exception が発生します。
        //    Codeer.Friendly.Windows.Grasp.WindowIdentifyException : 指定のGUI要素はWindowハンドルを持たないため、
        //    指定のメソッド、もしくはコンストラクタは使用できません。
        //  - Ver2.4.2 以降だとテストが開始されません。
        //    こちらのテストプロジェクトが含まていると、他の Friendly テストも開始しなくなるようです。
        //    Friendly を導入するソリューションでは、Nunit を使うようにしましょう。(XUnit派なのでイヤだなぁ…）
        Assert.AreEqual(true, true);
    }
}
