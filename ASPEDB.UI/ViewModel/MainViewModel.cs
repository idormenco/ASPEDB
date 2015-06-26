using ASPEDB.UI.DBOperationsService;
using ASPEDB.UI.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
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
        public List<ComboBoxItem> DataTypes { get; private set; }
        public List<ComboBoxItem> OperationTypes { get; private set; }
        public int SelectedDataType { get; set; }
        public int SelectedOperationType { get; set; }
        public decimal? ColumnName { get; set; }
        public decimal? SelectedValue { get; set; }
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
                if (value == Visibility.Visible)
                {
                    StringTextboxVisibility = Visibility.Collapsed;
                    NumericTextBoxVisibility = Visibility.Collapsed;
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
                if (value == Visibility.Visible)
                {
                    DatePickerVisibility = Visibility.Collapsed;
                    NumericTextBoxVisibility = Visibility.Collapsed;
                }
            }
        }

        private Visibility _numericTextBoxVisibility;
        public Visibility NumericTextBoxVisibility
        {
            get
            {
                return _numericTextBoxVisibility;
            }
            set
            {
                _numericTextBoxVisibility = value;
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
            InsertCommand = new RelayCommand(() =>
            {
                EncryptedPoint e = new EncryptedPoint();
                MessageBox.Show(dboc.Hello());
            },CanInsert);
            NumericTextBoxVisibility = Visibility.Visible;
            ////if (IsInDesignMode)
            ////{
            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////    // Code runs "for real"
            ////}
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