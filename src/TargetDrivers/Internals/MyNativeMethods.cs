using Codeer.Friendly.Windows.Grasp;
using Codeer.Friendly.Windows.NativeStandardControls;
using System;
using System.Runtime.InteropServices;

namespace TargetDrivers.Internals;

internal static class MyNativeMethods
{
    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool GetWindowRect(IntPtr hwnd, out RECT lpRect);

    internal static int GetWindowTop(WindowControl w)
    {
        _ = GetWindowRect(w.Handle, out RECT rc);
        return rc.Top;
    }
}
