using ASPEDB.UI.DBOperationsService;
using ASPEDB.UI.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ASPEDB.DTO;
using ASPEDB.DTO.DB;
using ASPEDB.EncryptionModule;
using ASPEDB.Utils;
using EncryptedDBPoint = ASPEDB.DTO.DB.EncryptedDBPoint;

namespace ASPEDB.UI.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public RelayCommand InsertCommand { get; private set; }
        public RelayCommand ColumnNameKeyPressed { get; set; }
        public List<ComboBoxItem> DataTypes { get; private set; }
        public List<string> DataTypesAlias { get; private set; }
        public ASPE ASPE { get; set; }
        private int _selectedDataType;
        public int SelectedDataType
        {
            get
            {
                return _selectedDataType;
            }
            set
            {
                _selectedDataType = value;
                SelectedValue = null;
                ColumnName = null;
                switch (DataTypes[value - 1].Value.ToLower())
                {
                    case "string":
                        StringTextboxVisibility = Visibility.Visible;
                        break;
                    case "number":
                        NumberTextBoxVisibility = Visibility.Visible;
                        break;
                    case "date":
                        DatePickerVisibility = Visibility.Visible;
                        break;
                    default:
                        throw new Exception("Unknown data type!");
                }
            }
        }

        private decimal? _columnName;
        public decimal? ColumnName
        {
            get { return _columnName; }
            set
            {
                _columnName = value;
                RaisePropertyChanged("ColumnName");
            }
        }

        private decimal? _selectedValue;
        public decimal? SelectedValue
        {
            get
            {
                return _selectedValue;
            }
            set
            {
                _selectedValue = value;
                RaisePropertyChanged("SelectedValue");
            }
        }

        #region visibility of controls
        private Visibility _datePickerVisibility;
        public Visibility DatePickerVisibility
        {
            get
            {
                return _datePickerVisibility;
            }
            set
            {
                _datePickerVisibility = value;
                RaisePropertyChanged("DatePickerVisibility");
                if (value == Visibility.Visible)
                {
                    StringTextboxVisibility = Visibility.Collapsed;
                    NumberTextBoxVisibility = Visibility.Collapsed;
                }
            }
        }

        private Visibility _stringTextboxVisibility;
        public Visibility StringTextboxVisibility
        {
            get
            {
                return _stringTextboxVisibility;
            }
            set
            {
                _stringTextboxVisibility = value;
                RaisePropertyChanged("StringTextboxVisibility");
                if (value == Visibility.Visible)
                {
                    DatePickerVisibility = Visibility.Collapsed;
                    NumberTextBoxVisibility = Visibility.Collapsed;
                }
            }
        }

        private Visibility _numberTextBoxVisibility;
        public Visibility NumberTextBoxVisibility
        {
            get
            {
                return _numberTextBoxVisibility;
            }
            set
            {
                _numberTextBoxVisibility = value;
                RaisePropertyChanged("NumberTextBoxVisibility");
                if (value == Visibility.Visible)
                {
                    DatePickerVisibility = Visibility.Collapsed;
                    StringTextboxVisibility = Visibility.Collapsed;
                }
            }
        }
        #endregion
        //private DBOperations
        public MainViewModel()
        {
            #region secretkey

            decimal[][] M1 = new decimal[][]
            {
                new decimal[]{9,4,3,3,8,6},
                new decimal[]{6,6,2,0,2,7}, 
                new decimal[]{4,4,4,6,2,0},
                new decimal[]{5,4,4,4,3,4},
                new decimal[]{4,9,2,6,5,4}, 
                new decimal[]{2,3,5,1,2,0}
            };
            decimal[][] M2 = new decimal[][]
            {
                new decimal[]{3,1,3,2,8,4},
                new decimal[]{4,4,5,5,0,9}, 
                new decimal[]{2,1,6,4,8,9},
                new decimal[]{6,7,2,8,0,7},
                new decimal[]{0,3,5,9,3,4}, 
                new decimal[]{4,8,5,6,1,6}
            };
            decimal[][] permutation = new decimal[][]
            {
                new decimal[]{0,1,0,0,0,0},
                new decimal[]{0,0,0,1,0,0}, 
                new decimal[]{1,0,0,0,0,0},
                new decimal[]{0,0,0,0,1,0},
                new decimal[]{0,0,0,0,0,1}, 
                new decimal[]{0,0,1,0,0,0}
            };
            Dictionary<int, decimal> wds = new Dictionary<int, decimal> { { 4, 8 }, { 5, 2 }, { 6, 5 } };
            var sk = new SecretKey(2, 6, "101010", wds, permutation, M1, M2, (decimal)Math.Pow(10, -10));
            ASPE = new ASPE(sk);

            #endregion

            
            DataTypes = new List<ComboBoxItem>(){
                new ComboBoxItem(1,"Number"),
                new ComboBoxItem(2,"String"),
                new ComboBoxItem(3,"Date")
            };
            DataTypesAlias = new List<string>(){
                "Num",
                "Str",
                "Date"
            };
            SelectedDataType = 1;
            InsertCommand = new RelayCommand(InsertCommandExecuted, CanInsert);
        }

        public object ConvertToNumber(object value)
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

        private void InsertCommandExecuted()
        {
            decimal? dataType = (decimal?)ConvertToNumber(DataTypesAlias[SelectedDataType - 1].ToLower());
            DBPoint dbp = new DBPoint(dataType.Value, ColumnName.Value, SelectedValue.Value);
            DBQuery dbq = new DBQuery(dataType.Value, ColumnName.Value, Operator.Equal, SelectedValue.Value, null);
            EncryptedDBPoint edbp = ASPE.EncryptDBPoint(dbp);
            var encryptedDBQuery = ASPE.EncryptDBQuery(dbq);
            var _dboc = new DBOperationsClient();
            //var dbOperationResponse = _dboc.Insert(edbp);
            
            //if (dbOperationResponse.IsOperationExecuted)
            //{ MessageBox.Show(dbOperationResponse.Message); }
            //else
            //{
            //    MessageBox.Show(dbOperationResponse.Message, "ERROR!");
            //}
            Task<ASPEDB.DTO.DB.EncryptedDBPoint[]> tsk = _dboc.SearchAsync(encryptedDBQuery);
            tsk.Wait();
            MessageBox.Show("Done");
            var a = 0;
        }

        private bool CanInsert()
        {
            if (SelectedDataType > 0 && ColumnName.HasValue && ColumnName > 0 && SelectedValue.HasValue)
            {
                return true;
            }
            return false;
        }
    }
}