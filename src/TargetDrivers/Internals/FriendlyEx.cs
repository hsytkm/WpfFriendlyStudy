using Codeer.Friendly;
using RM.Friendly.WPFStandardControls;
using System.Collections.Generic;
using System.Windows;

namespace TargetDrivers.Internals;

internal static class FriendlyEx
{
    internal static IEnumerable<AppVar> AsEnumerable<T>(this IWPFDependencyObjectCollection<T> collection)
        where T : DependencyObject
    {
        for (int i = 0; i < collection.Count; i++)
            yield return collection[i];
    }
}
