using System.Globalization;
namespace NotesMaui.Converters;
public class WrapTextConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null) return null;

        var stringValue = (string)value;

        if (int.TryParse(parameter?.ToString(), out var length))
            return stringValue.Length > length
                ? $"{stringValue[..length]}..."
                : stringValue;
        return stringValue;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value;
    }
}