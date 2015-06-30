using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using ASPEDB.Utils;
using Xceed.Wpf.Toolkit;

namespace ASPEDB.UI
{
    public class StringToDecimalConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                if (value == null) return string.Empty;
                string number = value.ToString();
                var cleanNumbers = number.SplitByLength(2).Where(x => x != "00");
                foreach (var nr in cleanNumbers)
                {
                    //In order to recover initial text. 'a' in ascii is 97 in my encoding its 10.
                    sb.Append((char) (Byte.Parse(nr) + 87));
                }
                return sb.ToString();
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char ch in value.ToString())
            {
                if (char.IsLetter(ch) && ch >= 'a' && ch <= 'z')
                {
                    //In order to have encoded all lower case letters in two digit numbers.'a' in ascii is 97 in my encoding its 10.
                    sb.Append(((int)ch - 87).ToString());
                }
            }
            if (sb.Length == 0) return null as decimal?;
            return decimal.Parse(sb.ToString().PadRight(12));
        }
    }

    public class NameToDecimalConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            StringBuilder sb = new StringBuilder();
            if (value == null) return string.Empty;
            string number = value.ToString();
            var cleanNumbers = number.SplitByLength(2).Where(x => x != "00");
            foreach (var nr in cleanNumbers)
            {
                //In order to recover initial text. 'a' in ascii is 97 in my encoding its 10.
                sb.Append((char)(Byte.Parse(nr) + 87));
            }
            return sb.ToString();
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char ch in value.ToString())
            {
                if (char.IsLetter(ch) && ch >= 'a' && ch <= 'z')
                {
                    //In order to have encoded all lower case letters in two digit numbers.'a' in ascii is 97 in my encoding its 10.
                    sb.Append(((int)ch - 87).ToString());
                }
            }
            if (sb.Length == 0) return null as decimal?;
            return decimal.Parse(sb.ToString());
        }
    }

    public class DateToDecimalConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds((double)value).ToLocalTime();
            return dtDateTime;
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            return ((DateTime)value - new DateTime(1970, 1, 1).ToLocalTime()).TotalSeconds;
        }
    }
}
