using System.Windows;
using ASPEDB.UI.Helpers;
using ASPEDB.UI.ViewModel;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;

namespace ASPEDB.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Closing += (s, e) => ViewModelLocator.Cleanup();
            Messenger.Default.Register<OpenWindowMessage>(
              this,
              message =>
              {
                  if (message.Type == WindowType.kModal)
                  {
                      var modalWindowVM = SimpleIoc.Default.GetInstance<ModalWindowViewModel>();
                      modalWindowVM.Points = message.Argument;
                      var modalWindow = new ModalWindow()
                      {
                          DataContext = modalWindowVM
                      };
                      var result = modalWindow.ShowDialog() ?? false;
                      Messenger.Default.Send(result ? "Accepted" : "Rejected");
                  }
                  
              });
        }
    }
}
