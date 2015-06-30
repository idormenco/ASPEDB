using ASPEDB.UI.DBOperationsService;
using ASPEDB.UI.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ASPEDB.DTO;
using ASPEDB.DTO.DB;
using ASPEDB.EncryptionModule;
using ASPEDB.UI.Helpers;
using ASPEDB.Utils;
using GalaSoft.MvvmLight.Messaging;
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

        public RelayCommand ReadCommand { get; private set; }
        public RelayCommand UpdateCommand { get; private set; }
        public RelayCommand DeleteCommand { get; private set; }

        public List<ComboBoxItem> DataTypes { get; private set; }
        public List<ComboBoxItem> Operators { get; private set; }
        public List<string> DataTypesAlias { get; private set; }
        public ASPE ASPE { get; set; }

        private int _selectedDataType;

        public int SelectedDataType
        {
            get { return _selectedDataType; }
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
                    default:
                        throw new Exception("Unknown data type!");
                }
            }
        }

        private int _selectedDataReadType;

        public int SelectedDataReadType
        {
            get { return _selectedDataReadType; }
            set
            {
                _selectedDataReadType = value;
                SelectedQueryValue = null;
                SelectedQueryOptionalValue = null;
                QueryColumnName = null;
                switch (DataTypes[value - 1].Value.ToLower())
                {
                    case "string":
                        StringTextboxReadVisibility = Visibility.Visible;
                        break;
                    case "number":
                        NumberTextBoxReadVisibility = Visibility.Visible;
                        break;
                    default:
                        throw new Exception("Unknown data type!");
                }
            }
        }

        private int _selectedDataUpdateType;

        public int SelectedDataUpdateType
        {
            get { return _selectedDataUpdateType; }
            set
            {
                _selectedDataUpdateType = value;
                SelectedQueryUpdateValue = null;
                SelectedQueryUOptionalValue = null;
                UpdateColumnName = null;
                SelectedUpdateValue = null;
                switch (DataTypes[value - 1].Value.ToLower())
                {
                    case "string":
                        StringTextboxUpdateVisibility = Visibility.Visible;
                        break;
                    case "number":
                        NumberTextBoxUpdateVisibility = Visibility.Visible;
                        break;
                    default:
                        throw new Exception("Unknown data type!");
                }
            }
        }

        private int _selectedDataDeleteType;

        public int SelectedDataDeleteType
        {
            get { return _selectedDataDeleteType; }
            set
            {
                _selectedDataDeleteType = value;
                SelectedQueryUpdateValue = null;
                SelectedQueryUOptionalValue = null;
                UpdateColumnName = null;
                SelectedUpdateValue = null;
                switch (DataTypes[value - 1].Value.ToLower())
                {
                    case "string":
                        StringTextboxDeleteVisibility = Visibility.Visible;
                        break;
                    case "number":
                        NumberTextBoxDeleteVisibility = Visibility.Visible;
                        break;
                    default:
                        throw new Exception("Unknown data type!");
                }
            }
        }

        private bool _readOptionalValueEnabled;

        public bool ReadOptionalValueEnabled
        {
            get { return _readOptionalValueEnabled; }
            set
            {
                _readOptionalValueEnabled = value;
                RaisePropertyChanged("ReadOptionalValueEnabled");
            }
        }

        private bool _readOptionalUValueEnabled;

        public bool ReadOptionalUValueEnabled
        {
            get { return _readOptionalUValueEnabled; }
            set
            {
                _readOptionalUValueEnabled = value;
                RaisePropertyChanged("ReadOptionalUValueEnabled");
            }
        }

        private bool _deleteOptionalValueEnabled;

        public bool DeleteOptionalValueEnabled
        {
            get { return _deleteOptionalValueEnabled; }
            set
            {
                _deleteOptionalValueEnabled = value;
                RaisePropertyChanged("DeleteOptionalValueEnabled");
            }
        }

        private int _selectedOperator;

        public int SelectedOperator
        {
            get { return _selectedOperator; }
            set
            {
                _selectedOperator = value;
                switch (value)
                {
                    case (int) Operator.NotEqual:
                    case (int) Operator.Less:
                    case (int) Operator.LessEqual:
                    case (int) Operator.Equal:
                    case (int) Operator.GreaterEqual:
                    case (int) Operator.Greater:
                        ReadOptionalValueEnabled = false;
                        SelectedQueryOptionalValue = null;
                        break;
                    case (int) Operator.ExactBetween:
                    case (int) Operator.BetweenDown:
                    case (int) Operator.Between:
                    case (int) Operator.BetweenUp:
                        ReadOptionalValueEnabled = true;
                        break;
                }
            }
        }

        private int _selectedOperatorUpdate;

        public int SelectedOperatorUpdate
        {
            get { return _selectedOperatorUpdate; }
            set
            {
                _selectedOperatorUpdate = value;
                switch (value)
                {
                    case (int) Operator.NotEqual:
                    case (int) Operator.Less:
                    case (int) Operator.LessEqual:
                    case (int) Operator.Equal:
                    case (int) Operator.GreaterEqual:
                    case (int) Operator.Greater:
                        ReadOptionalUValueEnabled = false;
                        SelectedQueryUOptionalValue = null;
                        break;
                    case (int) Operator.ExactBetween:
                    case (int) Operator.BetweenDown:
                    case (int) Operator.Between:
                    case (int) Operator.BetweenUp:
                        ReadOptionalUValueEnabled = true;
                        break;
                }
            }
        }

        private int _selectedOperatorDelete;

        public int SelectedOperatorDelete
        {
            get { return _selectedOperatorDelete; }
            set
            {
                _selectedOperatorDelete = value;
                switch (value)
                {
                    case (int) Operator.NotEqual:
                    case (int) Operator.Less:
                    case (int) Operator.LessEqual:
                    case (int) Operator.Equal:
                    case (int) Operator.GreaterEqual:
                    case (int) Operator.Greater:
                        DeleteOptionalValueEnabled = false;
                        SelectedDeleteOptionalValue = null;
                        break;
                    case (int) Operator.ExactBetween:
                    case (int) Operator.BetweenDown:
                    case (int) Operator.Between:
                    case (int) Operator.BetweenUp:
                        DeleteOptionalValueEnabled = true;
                        break;
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

        private decimal? _updateColumnName;

        public decimal? UpdateColumnName
        {
            get { return _updateColumnName; }
            set
            {
                _updateColumnName = value;
                RaisePropertyChanged("UpdateColumnName");
            }
        }

        private decimal? _queryColumnName;

        public decimal? QueryColumnName
        {
            get { return _queryColumnName; }
            set
            {
                _queryColumnName = value;
                RaisePropertyChanged("QueryColumnName");
            }
        }

        private decimal? _deleteColumnName;

        public decimal? DeleteColumnName
        {
            get { return _deleteColumnName; }
            set
            {
                _deleteColumnName = value;
                RaisePropertyChanged("DeleteColumnName");
            }
        }

        private decimal? _selectedValue;

        public decimal? SelectedValue
        {
            get { return _selectedValue; }
            set
            {
                _selectedValue = value;
                RaisePropertyChanged("SelectedValue");
            }
        }

        private decimal? _selectedUpdateValue;

        public decimal? SelectedUpdateValue
        {
            get { return _selectedUpdateValue; }
            set
            {
                _selectedUpdateValue = value;
                RaisePropertyChanged("SelectedUpdateValue");
            }
        }

        private decimal? _selectedDeleteValue;

        public decimal? SelectedDeleteValue
        {
            get { return _selectedDeleteValue; }
            set
            {
                _selectedDeleteValue = value;
                RaisePropertyChanged("SelectedDeleteValue");
            }
        }

        private decimal? _selectedQueryValue;

        public decimal? SelectedQueryValue
        {
            get { return _selectedQueryValue; }
            set
            {
                _selectedQueryValue = value;
                RaisePropertyChanged("SelectedQueryValue");
            }
        }

        private decimal? _selectedQueryOptionalValue;

        public decimal? SelectedQueryOptionalValue
        {
            get { return _selectedQueryOptionalValue; }
            set
            {
                _selectedQueryOptionalValue = value;
                RaisePropertyChanged("SelectedQueryOptionalValue");
            }
        }

        private decimal? _selectedDeleteOptionalValue;

        public decimal? SelectedDeleteOptionalValue
        {
            get { return _selectedDeleteOptionalValue; }
            set
            {
                _selectedDeleteOptionalValue = value;
                RaisePropertyChanged("SelectedDeleteOptionalValue");
            }
        }

        private decimal? _selectedQueryUpdateValue;

        public decimal? SelectedQueryUpdateValue
        {
            get { return _selectedQueryUpdateValue; }
            set
            {
                _selectedQueryUpdateValue = value;
                RaisePropertyChanged("SelectedQueryUpdateValue");
            }
        }

        private decimal? _selectedQueryUOptionalValue;

        public decimal? SelectedQueryUOptionalValue
        {
            get { return _selectedQueryUOptionalValue; }
            set
            {
                _selectedQueryUOptionalValue = value;
                RaisePropertyChanged("SelectedQueryUOptionalValue");
            }
        }

        #region visibility of controls

        private Visibility _stringTextboxVisibility;

        public Visibility StringTextboxVisibility
        {
            get { return _stringTextboxVisibility; }
            set
            {
                _stringTextboxVisibility = value;
                RaisePropertyChanged("StringTextboxVisibility");
                if (value == Visibility.Visible)
                {
                    NumberTextBoxVisibility = Visibility.Collapsed;
                }
            }
        }

        private Visibility _numberTextBoxVisibility;

        public Visibility NumberTextBoxVisibility
        {
            get { return _numberTextBoxVisibility; }
            set
            {
                _numberTextBoxVisibility = value;
                RaisePropertyChanged("NumberTextBoxVisibility");
                if (value == Visibility.Visible)
                {
                    StringTextboxVisibility = Visibility.Collapsed;
                }
            }
        }

        private Visibility _stringTextboxReadVisibility;

        public Visibility StringTextboxReadVisibility
        {
            get { return _stringTextboxReadVisibility; }
            set
            {
                _stringTextboxReadVisibility = value;
                RaisePropertyChanged("StringTextboxReadVisibility");
                if (value == Visibility.Visible)
                {
                    NumberTextBoxReadVisibility = Visibility.Collapsed;
                }
            }
        }

        private Visibility _numberTextBoxUpdateVisibility;

        public Visibility NumberTextBoxUpdateVisibility
        {
            get { return _numberTextBoxUpdateVisibility; }
            set
            {
                _numberTextBoxUpdateVisibility = value;
                RaisePropertyChanged("NumberTextBoxUpdateVisibility");
                if (value == Visibility.Visible)
                {
                    StringTextboxUpdateVisibility = Visibility.Collapsed;
                }
            }
        }

        private Visibility _stringTextboxUpdateVisibility;

        public Visibility StringTextboxUpdateVisibility
        {
            get { return _stringTextboxUpdateVisibility; }
            set
            {
                _stringTextboxUpdateVisibility = value;
                RaisePropertyChanged("StringTextboxUpdateVisibility");
                if (value == Visibility.Visible)
                {
                    NumberTextBoxUpdateVisibility = Visibility.Collapsed;
                }
            }
        }

        private Visibility _numberTextBoxDeleteVisibility;

        public Visibility NumberTextBoxDeleteVisibility
        {
            get { return _numberTextBoxDeleteVisibility; }
            set
            {
                _numberTextBoxDeleteVisibility = value;
                RaisePropertyChanged("NumberTextBoxDeleteVisibility");
                if (value == Visibility.Visible)
                {
                    StringTextboxDeleteVisibility = Visibility.Collapsed;
                }
            }
        }

        private Visibility _stringTextboxDeleteVisibility;

        public Visibility StringTextboxDeleteVisibility
        {
            get { return _stringTextboxDeleteVisibility; }
            set
            {
                _stringTextboxDeleteVisibility = value;
                RaisePropertyChanged("StringTextboxDeleteVisibility");
                if (value == Visibility.Visible)
                {
                    NumberTextBoxDeleteVisibility = Visibility.Collapsed;
                }
            }
        }

        private Visibility _numberTextBoxReadVisibility;

        public Visibility NumberTextBoxReadVisibility
        {
            get { return _numberTextBoxReadVisibility; }
            set
            {
                _numberTextBoxReadVisibility = value;
                RaisePropertyChanged("NumberTextBoxReadVisibility");
                if (value == Visibility.Visible)
                {
                    StringTextboxReadVisibility = Visibility.Collapsed;
                }
            }
        }

        private Visibility _optionalValueVisibility;
        private bool _isOpened;


        public Visibility OptionalValueVisibility
        {
            get { return _optionalValueVisibility; }
            set
            {
                _optionalValueVisibility = value;
                RaisePropertyChanged("OptionalValueVisibility");
            }
        }

        #endregion

        //private DBOperations
        public MainViewModel(IDataService dataService)
        {
            #region secretkey

            decimal[][] M1 = new decimal[][]
            {
                new decimal[] {9, 4, 3, 3, 8, 6},
                new decimal[] {6, 6, 2, 0, 2, 7},
                new decimal[] {4, 4, 4, 6, 2, 0},
                new decimal[] {5, 4, 4, 4, 3, 4},
                new decimal[] {4, 9, 2, 6, 5, 4},
                new decimal[] {2, 3, 5, 1, 2, 0}
            };
            decimal[][] M2 = new decimal[][]
            {
                new decimal[] {3, 1, 3, 2, 8, 4},
                new decimal[] {4, 4, 5, 5, 0, 9},
                new decimal[] {2, 1, 6, 4, 8, 9},
                new decimal[] {6, 7, 2, 8, 0, 7},
                new decimal[] {0, 3, 5, 9, 3, 4},
                new decimal[] {4, 8, 5, 6, 1, 6}
            };
            decimal[][] permutation = new decimal[][]
            {
                new decimal[] {0, 1, 0, 0, 0, 0},
                new decimal[] {0, 0, 0, 1, 0, 0},
                new decimal[] {1, 0, 0, 0, 0, 0},
                new decimal[] {0, 0, 0, 0, 1, 0},
                new decimal[] {0, 0, 0, 0, 0, 1},
                new decimal[] {0, 0, 1, 0, 0, 0}
            };
            Dictionary<int, decimal> wds = new Dictionary<int, decimal> {{4, 8}, {5, 2}, {6, 5}};
            var sk = new SecretKey(2, 6, "101010", wds, permutation, M1, M2, (decimal) Math.Pow(10, -10));
            ASPE = new ASPE(sk);

            #endregion

            DataTypes = new List<ComboBoxItem>()
            {
                new ComboBoxItem(1, "Number"),
                new ComboBoxItem(2, "String")
            };
            Operators = new List<ComboBoxItem>
            {
                new ComboBoxItem(1, "!="),
                new ComboBoxItem(2, "<"),
                new ComboBoxItem(3, "<="),
                new ComboBoxItem(4, "=="),
                new ComboBoxItem(5, ">="),
                new ComboBoxItem(6, ">"),
                new ComboBoxItem(7, "><"),
                new ComboBoxItem(8, ">=<"),
                new ComboBoxItem(9, ">=<="),
                new ComboBoxItem(10, "><=")
            };
            DataTypesAlias = new List<string>()
            {
                "Num",
                "Str"
            };
            SelectedDataType = 1;
            SelectedDataReadType = 1;
            SelectedOperator = 1;
            SelectedDataUpdateType = 1;
            SelectedOperatorUpdate = 1;
            SelectedDataDeleteType = 1;
            SelectedOperatorDelete = 1;
            InsertCommand = new RelayCommand(InsertCommandExecuted, CanInsert);
            ReadCommand = new RelayCommand(ReadCommandExecuted, CanRead);
            UpdateCommand = new RelayCommand(UpdateCommandExecuted, CanUpdate);
            DeleteCommand = new RelayCommand(DeleteCommandExecuted, CanDelete);
        }

        private bool CanDelete()
        {
            if (SelectedDataDeleteType > 0 && DeleteColumnName.HasValue && DeleteColumnName > 0 &&
                SelectedDeleteValue.HasValue)
            {
                switch (SelectedOperatorDelete)
                {
                    case (int) Operator.NotEqual:
                    case (int) Operator.Less:
                    case (int) Operator.LessEqual:
                    case (int) Operator.Equal:
                    case (int) Operator.GreaterEqual:
                    case (int) Operator.Greater:
                        return true;
                        break;
                    case (int) Operator.ExactBetween:
                    case (int) Operator.BetweenDown:
                    case (int) Operator.Between:
                    case (int) Operator.BetweenUp:
                        if (SelectedDeleteOptionalValue.HasValue) return true;
                        break;
                }
            }
            return false;
        }

        private void DeleteCommandExecuted()
        {
            var _dboc = new DBOperationsClient();
            decimal? dataType = (decimal?)ConvertToNumber(DataTypesAlias[SelectedDataDeleteType - 1].ToLower());
            DBQuery dbq = new DBQuery(dataType.Value, DeleteColumnName.Value, (Operator)SelectedOperatorDelete,
                SelectedDeleteValue.Value, SelectedDeleteOptionalValue);
            var encryptedDBQuery = ASPE.EncryptDBQuery(dbq);

            Task<ASPEDB.DTO.DB.DBOperationResponse> tsk = _dboc.DeleteAsync(encryptedDBQuery);
            tsk.Wait();
            if (tsk.Result.IsOperationExecuted)
            {
                MessageBox.Show(tsk.Result.Message);
            }
            else
            {
                MessageBox.Show(tsk.Result.Message, "ERROR!");
            }
        }

        private bool CanUpdate()
        {
            if (SelectedDataUpdateType > 0 && UpdateColumnName.HasValue && UpdateColumnName > 0 &&
                SelectedUpdateValue.HasValue && SelectedUpdateValue.HasValue)
            {
                switch (SelectedOperatorUpdate)
                {
                    case (int) Operator.NotEqual:
                    case (int) Operator.Less:
                    case (int) Operator.LessEqual:
                    case (int) Operator.Equal:
                    case (int) Operator.GreaterEqual:
                    case (int) Operator.Greater:
                        return true;
                        break;
                    case (int) Operator.ExactBetween:
                    case (int) Operator.BetweenDown:
                    case (int) Operator.Between:
                    case (int) Operator.BetweenUp:
                        if (SelectedQueryUOptionalValue.HasValue) return true;
                        break;
                }
            }
            return false;
        }

        private void UpdateCommandExecuted()
        {
            var _dboc = new DBOperationsClient();
            decimal? dataType = (decimal?)ConvertToNumber(DataTypesAlias[SelectedDataUpdateType- 1].ToLower());
            DBQuery dbq = new DBQuery(dataType.Value, UpdateColumnName.Value, (Operator)SelectedOperatorUpdate,
                SelectedQueryUpdateValue.Value, SelectedQueryUOptionalValue);
            var encryptedDBQuery = ASPE.EncryptDBQuery(dbq);
            var encryptedDBValue = ASPE.EncryptDBValue(DBPointsUtils.GenerateUnEncryptedDBValue(SelectedUpdateValue.Value,ASPE.sk.d));
            Task<ASPEDB.DTO.DB.DBOperationResponse> tsk = _dboc.UpdateAsync(encryptedDBQuery,encryptedDBValue);
            tsk.Wait();
            if (tsk.Result.IsOperationExecuted)
            {
                MessageBox.Show(tsk.Result.Message);
            }
            else
            {
                MessageBox.Show(tsk.Result.Message, "ERROR!");
            }
        }

        public bool IsOpened
        {
            get { return _isOpened; }
            set
            {
                _isOpened = value;
                RaisePropertyChanged("IsOpened");
            }
        }

        private void ReadCommandExecuted()
        {
            var _dboc = new DBOperationsClient();
            decimal? dataType = (decimal?) ConvertToNumber(DataTypesAlias[SelectedDataReadType - 1].ToLower());
            DBQuery dbq = new DBQuery(dataType.Value, QueryColumnName.Value, (Operator) SelectedOperator,
                SelectedQueryValue.Value, SelectedQueryOptionalValue);
            var encryptedDBQuery = ASPE.EncryptDBQuery(dbq);

            Task<ASPEDB.DTO.DB.EncryptedDBPoint[]> tsk = _dboc.SearchAsync(encryptedDBQuery);
            tsk.Wait();
            EncryptedDBPoint[] encryptedDBPoints = tsk.Result;
            List<DBPoint> points =
                encryptedDBPoints.Select(x => ASPE.DecryptDBPoint(x).RecoverDBPointValue((decimal) Math.Pow(10, -10)))
                    .ToList();
            List<DisplayDBPoint> lst = new List<DisplayDBPoint>();
            if (points.Count == 0)
                MessageBox.Show("No points covered by query");
            else
            {
                foreach (var dbPoint in points)
                {
                    DisplayDBPoint ddbp = new DisplayDBPoint();
                    ddbp.Type = ConvertToString(dbPoint.Type);
                    ddbp.Name = ConvertToString(dbPoint.Name);
                    switch (ddbp.Type)
                    {
                        case "num":
                            ddbp.Type = "Number";
                            ddbp.Value = dbPoint.Value.ToString();
                            break;
                        case "str":
                            ddbp.Type = "String";
                            StringToDecimalConvertor s = new StringToDecimalConvertor();
                            ddbp.Value = (string) s.Convert(dbPoint.Value, null, null, null);
                            break;
                    }
                    lst.Add(ddbp);
                }
                Messenger.Default.Send<OpenWindowMessage>(
                    new OpenWindowMessage() {Type = WindowType.kModal, Argument = lst});
            }
        }

        private bool CanRead()
        {
            if (SelectedDataReadType > 0 && QueryColumnName.HasValue && QueryColumnName > 0 &&
                SelectedQueryValue.HasValue)
            {
                switch (SelectedOperator)
                {
                    case (int) Operator.NotEqual:
                    case (int) Operator.Less:
                    case (int) Operator.LessEqual:
                    case (int) Operator.Equal:
                    case (int) Operator.GreaterEqual:
                    case (int) Operator.Greater:
                        return true;
                        break;
                    case (int) Operator.ExactBetween:
                    case (int) Operator.BetweenDown:
                    case (int) Operator.Between:
                    case (int) Operator.BetweenUp:
                        if (SelectedQueryOptionalValue.HasValue) return true;
                        break;
                }
            }
            return false;
        }

        public object ConvertToNumber(object value)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char ch in value.ToString())
            {
                if (char.IsLetter(ch) && ch >= 'a' && ch <= 'z')
                {
                    //In order to have encoded all lower case letters in two digit numbers.'a' in ascii is 97 in my encoding its 10.
                    sb.Append(((int) ch - 87).ToString());
                }
            }
            if (sb.Length == 0) return null as decimal?;
            return decimal.Parse(sb.ToString());
        }

        public string ConvertToString(decimal value)
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

        private void InsertCommandExecuted()
        {
            decimal? dataType = (decimal?) ConvertToNumber(DataTypesAlias[SelectedDataType - 1].ToLower());
            DBPoint dbp = new DBPoint(dataType.Value, ColumnName.Value, SelectedValue.Value);
            EncryptedDBPoint edbp = ASPE.EncryptDBPoint(dbp);
            var _dboc = new DBOperationsClient();
            var dbOperationResponse = _dboc.Insert(edbp);

            if (dbOperationResponse.IsOperationExecuted)
            {
                MessageBox.Show(dbOperationResponse.Message);
            }
            else
            {
                MessageBox.Show(dbOperationResponse.Message, "ERROR!");
            }
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