using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Windows.Data;

namespace WpfDemoApp;

public sealed class EnumDescriptionConverter : IValueConverter
{
    public static readonly EnumDescriptionConverter Shared = new();

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var valueText = value?.ToString() ?? "";
        if (value is Enum)
        {
            var fieldInfo = value.GetType().GetField(valueText);
            var attributes = fieldInfo?.GetCustomAttributes<DescriptionAttribute>(false);
            return attributes?.FirstOrDefault()?.Description ?? valueText;
        }
        return valueText;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
        throw new NotImplementedException();
}
