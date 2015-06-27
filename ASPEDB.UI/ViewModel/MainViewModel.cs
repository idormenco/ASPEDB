using ASPEDB.UI.DBOperationsService;
using ASPEDB.UI.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Windows;

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
        private DBOperationsClient dboc;
        public RelayCommand InsertCommand { get; private set; }
        public RelayCommand ColumnNameKeyPressed { get; set; }
        public List<ComboBoxItem> DataTypes { get; private set; }
        public List<ComboBoxItem> OperationTypes { get; private set; }
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
        public int SelectedOperationType { get; set; }
        public decimal? _columnName;
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
            dboc = new DBOperationsClient();
            DataTypes = new List<ComboBoxItem>(){
                new ComboBoxItem(1,"Number"),
                new ComboBoxItem(2,"String"),
                new ComboBoxItem(3,"Date")
            };
            SelectedDataType = 1;
            InsertCommand = new RelayCommand(InsertCommandExecuted, CanInsert);
        }

        private void InsertCommandExecuted()
        {
            MessageBox.Show(SelectedValue.ToString());
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