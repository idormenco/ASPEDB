using ASPEDB.UI.DBOperationsService;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
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
        public RelayCommand ClickCommand { get; private set; }
        //private DBOperations
        public MainViewModel()
        {
            dboc = new DBOperationsClient();
            
            ClickCommand = new RelayCommand(() =>
            {
                EncryptedPoint e = new EncryptedPoint();
                MessageBox.Show(dboc.Hello());
            });
            ////if (IsInDesignMode)
            ////{
            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////    // Code runs "for real"
            ////}
        }
    }
}