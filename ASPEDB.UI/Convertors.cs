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
            return decimal.Parse(sb.ToString().PadRight(20, '0'));
        }
    }

    public class DateToDecimalConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            return null;
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public static class ModelServiceConvertor
    {
        public static DBOperationsService.EncryptedDBPoint EncryptedDBPointToServer(this DTO.DB.EncryptedDBPoint edbpModel)
        {
            DBOperationsService.EncryptedDBPoint sedbp = new DBOperationsService.EncryptedDBPoint();
            sedbp.Type = new DBOperationsService.EncryptedDBValue();
            sedbp.Name = new DBOperationsService.EncryptedDBValue();
            sedbp.Value = new DBOperationsService.EncryptedDBValue();
            #region type
            sedbp.Type.C = new DBOperationsService.EncryptedPoint();
            sedbp.Type.C.pa = new List<decimal>(edbpModel.Type.C.pa).ToArray();
            sedbp.Type.C.pb = new List<decimal>(edbpModel.Type.C.pb).ToArray();
            sedbp.Type.D = new DBOperationsService.EncryptedPoint();
            sedbp.Type.D.pa = new List<decimal>(edbpModel.Type.D.pa).ToArray();
            sedbp.Type.D.pb = new List<decimal>(edbpModel.Type.D.pb).ToArray();
            #endregion
            #region name
            sedbp.Name.C = new DBOperationsService.EncryptedPoint();
            sedbp.Name.C.pa = new List<decimal>(edbpModel.Name.C.pa).ToArray();
            sedbp.Name.C.pb = new List<decimal>(edbpModel.Name.C.pb).ToArray();
            sedbp.Name.D = new DBOperationsService.EncryptedPoint();
            sedbp.Name.D.pa = new List<decimal>(edbpModel.Name.D.pa).ToArray();
            sedbp.Name.D.pb = new List<decimal>(edbpModel.Name.D.pb).ToArray();
            #endregion
            #region value
            sedbp.Value.C = new DBOperationsService.EncryptedPoint();
            sedbp.Value.C.pa = new List<decimal>(edbpModel.Value.C.pa).ToArray();
            sedbp.Value.C.pb = new List<decimal>(edbpModel.Value.C.pb).ToArray();
            sedbp.Value.D = new DBOperationsService.EncryptedPoint();
            sedbp.Value.D.pa = new List<decimal>(edbpModel.Value.D.pa).ToArray();
            sedbp.Value.D.pb = new List<decimal>(edbpModel.Value.D.pb).ToArray();
            #endregion

            return sedbp;
        }

        public static DTO.DB.EncryptedDBPoint EncryptedDBPointToServer(this DBOperationsService.EncryptedDBPoint edbpModel)
        {
            DTO.EncryptedPoint typeC = new DTO.EncryptedPoint(edbpModel.Type.C.pa, edbpModel.Type.C.pb);
            DTO.EncryptedPoint typeD = new DTO.EncryptedPoint(edbpModel.Type.D.pa, edbpModel.Type.D.pb);
            DTO.DB.EncryptedDBValue type = new DTO.DB.EncryptedDBValue(typeC, typeD);

            DTO.EncryptedPoint nameC = new DTO.EncryptedPoint(edbpModel.Name.C.pa, edbpModel.Name.C.pb);
            DTO.EncryptedPoint nameD = new DTO.EncryptedPoint(edbpModel.Name.D.pa, edbpModel.Name.D.pb);
            DTO.DB.EncryptedDBValue name = new DTO.DB.EncryptedDBValue(nameC, nameD);

            DTO.EncryptedPoint valueC = new DTO.EncryptedPoint(edbpModel.Value.C.pa, edbpModel.Value.C.pb);
            DTO.EncryptedPoint valueD = new DTO.EncryptedPoint(edbpModel.Value.D.pa, edbpModel.Value.D.pb);
            DTO.DB.EncryptedDBValue value = new DTO.DB.EncryptedDBValue(valueC, valueD);

            DTO.DB.EncryptedDBPoint dto = new DTO.DB.EncryptedDBPoint(type, name, value);
            return dto;
        }
    }
}
