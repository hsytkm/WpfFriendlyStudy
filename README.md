# WpfFriendlyStudy (2022年11月)

## はじめに

- Friendly を使えば UIアプリ の E2Eテスト を実施ができます。
- キーやマウスを記録する RPA でなく、対象.exe をプロセス越しに操作が行われます。各手続きを同期的に（処理完了を確認して）実行するので、再現性の高いテストを実施できます。
- Friendly の解説記事が少ない。 Qiita の Friendly タグが 全12件（2022年11月時点）
- 作者の解説ブログは 2014年 に書かれており、最新環境での情報ではないので、少し物足りない部分があります。（.NETFw + MSTest ベース）

## Friendly 前提

- Friendly のサポート対象は .NETFw のみっぽいです。（[作者様のブログ](https://ishikawa-tatsuya.hatenablog.com/entry/2019/08/20/163402) を見ると .NET も対応されてるようですが、 Nuget パッケージは .NETFw しか対応していないようです。 .NET に導入すると [NU1701](https://learn.microsoft.com/ja-jp/nuget/reference/errors-and-warnings/nu1701) 警告が表示されます）
- テスト対象プロジェクトは .NETFw じゃなくてもOKです。（.NET7 ₊ WPF のテストを確認済み）

## テストフレームワークの選択

テストフレームワーク と .NET の組み合わせによっては テストが動作しないようです。

また、**Xunit をソリューションに含めると、他のテスト（.NETFw +Nunit）のテストが実行できなくなる謎動作** をしていました。

#### 動作確認の結果（ソリューション内で個別に有効化）

**.NETFw 4.8**

| Test framework + .NETFw 4.8 | テスト動作                |
| --------------------------- | ------------------------- |
| MSTest (2.2.10)             | OK                        |
| Nunit (3.13.3)              | OK                        |
| Xunit (2.4.2)               | **NG (テスト開始しない)** |

**.NET 7** 

| Test framework + .NET 7 | テスト動作                |
| ----------------------- | ------------------------- |
| MSTest (2.2.10)         | **NG (テスト開始しない)** |
| Nunit (3.13.3)          | OK                        |
| Xunit (2.4.2)           | OK                        |

**.NET Fw なら Nunit、 .NET なら Nunit もしくは Xunit が良いように思いました。**

## テストの実行

- テスト対象プロジェクト と テスト実行 のプラットフォームを（基本的には）揃える必要があります。VisualStudio なら [テスト - テスト設定 - AnyCPUプロジェクトのプロセッサアーキテクチャ] からテスト環境のアーキテクチャを選択できます。 x64 から x86 を操作する方法もあるようです。[Friendlyでx64のプロセスからx86のプロセスを操作する - ささいなことですが。](https://ishikawa-tatsuya.hatenablog.com/entry/2019/07/27/123301)
- テスト対象プロジェクト と テストコード のシンボル紐付けは `dynamic` により動的に行われます。 そのためテストコードの実装をミスると動的エラーとなってしまいます。 また Intellisense による入力支援が受けられません。

## ご参考

[Codeer-Software/Friendly - GitHub](https://github.com/Codeer-Software/Friendly)

[Friendly Advent Calendar 2014 - Qiita](https://qiita.com/advent-calendar/2014/friendly)  ※作者ブログのリンク集になっていますが、アベカレ以降はブログを漁る必要があります。

[Friendly ハンズオン 11 WPF用上位ライブラリ - ささいなことですが。](https://ishikawa-tatsuya.hatenablog.com/entry/2015/01/10/114835)  ※作者ブログ WPFライブラリについて

[Friendly for .NetCore - ささいなことですが。](https://ishikawa-tatsuya.hatenablog.com/entry/2019/01/05/140805)  ※作者ブログ .NET 対応について

[FriendlyでWPFアプリをテストするときのコツ - ささいなことですが。](https://ishikawa-tatsuya.hatenablog.com/entry/2020/05/17/142034)  ※作者ブログ WPFアプリについて

[Friendly カテゴリーの記事一覧 - かずきのBlog@hatena](https://blog.okazuki.jp/archive/category/Friendly)  ※WPFターゲット

[Ishikawa-Tatsuya/WPFFriendlySampleDotNetConf2016 - GitHub](https://github.com/Ishikawa-Tatsuya/WPFFriendlySampleDotNetConf2016)  ※作者様の2016デモ

[Friendlyを使ったUI自動テスト - Qiita](https://qiita.com/zuzuwen/items/b042fe501990ad29f418)  ※Qiitaの中で最も丁寧な説明

[Friendlyが対象プロセスにdllを読み込ませる(DLLインジェクション)仕組み - Qiita](https://qiita.com/mngreen/items/fb56137153142e4dd5f9)   ※作者以外が動作を解説

## 後で試す

StaticMethod の呼び出し　https://ishikawa-tatsuya.hatenablog.com/entry/2014/12/10/000326

相手のプロセスで生成されたメッセージボックス　https://fkmt5.hatenadiary.org/entry/20141213/1418400469

12/17 ハンズオン6 (Form)   https://github.com/Ishikawa-Tatsuya/HandsOn6

現在提供している上位ライブラリ    https://ishikawa-tatsuya.hatenablog.com/entry/2014/12/18/000601

論理ツリー切れる場合の対処

```cs
var window = new WindowControl(app.Type<Application>().Current.MainWindow);
// 1.Frameをとって、そのContentをとってAppVarに入れる
AppVar page = window.LogicalTree().ByType<Frame>().Single().Dynamic().Content;
// 2.PageのLogicalTreeをとってごにょごにょする
var btn = new WPFButtonBase(page.LogicalTree().ByType<Button>().Single());
```

```cs
//UIインスタンスをつかんでみる
//おなじみのApplication.Current.MainWindowですね、
//MainWindowここにきてるように見えますが、実はきてません（相手プロセスにいます）
AppVar mainWindow = app.Type<Application>().Current.MainWindow;

// 型参照できない場合は文字列のインターフェスを使う
var unkwownType = mainWindow.LogicalTree().ByType("ThirdPartyTextBox").Single();
```

[Friendly ハンズオン8 Friendly.Windows.Grasp - .Netアプリ操作でよく使う機能 - - ささいなことですが。](https://ishikawa-tatsuya.hatenablog.com/entry/2015/01/05/230614)  ※アベカレ以降の記事

```cs
// キー操作できるっぽいけど、最適か怪しい。
System.Windows.Forms.SendKeys.SendWait("{ENTER}");
```

EOF
