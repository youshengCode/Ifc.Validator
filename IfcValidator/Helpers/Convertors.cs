using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace IfcValidator.Helpers
{
    public class BoolToVisible : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if ((bool)value)
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if ((Visibility)value == Visibility.Visible)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public class BoolToVisibleReverse : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if ((bool)value)
            {
                return Visibility.Collapsed;
            }
            else
            {
                return Visibility.Visible;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if ((Visibility)value == Visibility.Collapsed)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public class StringToBool : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            switch ((string)value)
            {
                case "true":
                    return true;
                case "false":
                    return false;
                default:
                    return false;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            switch ((bool)value)
            {
                case true:
                    return "true";
                case false:
                    return "false";
                default:
                    return "false";
            }
        }

    }

    public class StringToUri : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
            {
                return null;
            }
            if (Uri.IsWellFormedUriString(value.ToString(), UriKind.Absolute))
            {
                Uri uri = new Uri(value.ToString());
                return uri;
            }
            else
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
            {
                return null;
            }
            else
            {
                return value.ToString();
            }
        }
    }

    public class StringToDateTimeOffset : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
            {
                return new DateTimeOffset(DateTime.Today);
            }
            if (DateTime.TryParse(value.ToString(), out DateTime date))
            {
                return new DateTimeOffset(date);
            }
            else
            {
                return new DateTimeOffset(DateTime.Today);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            DateTimeOffset dateTimeOffset = (DateTimeOffset)value;
            return dateTimeOffset.Date.ToString("s");
        }
    }

    public class StringToInt : IValueConverter
    {
        public int EmptyValue { get; set; }
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            double d = 0f;
            if (value == null)
                return EmptyValue;
            if (double.TryParse((string)value, out d))
            {
                int i = EmptyValue;
                i = (int)Math.Round(d, 0);
                Int32.TryParse((string)value, out i);
                return i;
            }
            else
                return EmptyValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
                return EmptyValue.ToString();
            else
                return value.ToString();
        }
    }

    public class StringToDouble : IValueConverter
    {
        public double EmptyValue { get; set; }
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            double d = 0f;
            if (value == null)
                return EmptyValue;
            if (double.TryParse((string)value, out d))
                return d;
            else
                return EmptyValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
                return EmptyValue.ToString();
            else
                return value.ToString();
        }
    }

    public class StringToDoubleNullable : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            double d = 0f;
            if (value == null)
                return null;
            if (double.TryParse((string)value, out d))
                return d;
            else
                return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
                return null;
            else
                return value.ToString();
        }
    }
}
