using Codeer.Friendly;
using Codeer.Friendly.Dynamic;
using Codeer.Friendly.Windows;
using System;
using System.Diagnostics;
using System.Windows;
using TargetDrivers;
using Xunit;

namespace XunitUITest;

public class UITest : IDisposable
{
    // テスト対象 exe は Build の PostProcess で同フォルダにコピー済み
    const string TestAppExeName = "WpfDemoApp.exe";

    readonly WindowsAppFriend _app = default!;
    readonly MainWindowDriver _driver = default!;

    public UITest()
    {
        var process = Process.Start(TestAppExeName);
        _app = new WindowsAppFriend(process);
        AppVar windowObject = _app.Type<Application>().Current.MainWindow;
        _driver = new MainWindowDriver(windowObject);
    }

    public void Dispose()
    {
        using var process = Process.GetProcessById(_app.ProcessId);
        _app.Dispose();
        process.CloseMainWindow();
    }

    [Fact]
    public void SaveButton_CanExecute()
    {
        // 起動直後は妥当性のある設定
        Assert.True(_driver.SaveFile.IsEnabled);

        // Name が空はNG
        _driver.Name.EmulateChangeText("");
        Assert.False(_driver.SaveFile.IsEnabled);
    }

    [Fact]
    public void DotNETFwとFriendlyとXUnitの組み合わせは動作しないようです()
    {
        // Friendly + XUnit の組み合わせは動作させられませんでした。
        // xunit.runner.visualstudio が原因のようです。
        //  - Ver2.4.1 以前だとテストで Exception が発生します。
        //    Codeer.Friendly.Windows.Grasp.WindowIdentifyException : 指定のGUI要素はWindowハンドルを持たないため、
        //    指定のメソッド、もしくはコンストラクタは使用できません。
        //  - Ver2.4.2 以降だとテストが開始されません。
        //    こちらのテストプロジェクトが含まていると、他の Friendly テストも開始しなくなるようです。
        //    Friendly を導入するソリューションでは、Nunit を使うようにしましょう。(XUnit派なのでイヤだなぁ…）
        Assert.True(true);
    }
}
