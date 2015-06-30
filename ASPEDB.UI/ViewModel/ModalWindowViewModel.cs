using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASPEDB.DTO.DB;
using ASPEDB.UI.Model;
using GalaSoft.MvvmLight;
using DBPoint = ASPEDB.UI.Model.DisplayDBPoint;

namespace ASPEDB.UI.ViewModel
{
    class ModalWindowViewModel : ViewModelBase 
    {
        public List<DisplayDBPoint> Points { get; set; }
        public ModalWindowViewModel(IDataService dataService)
        {
            //OpenModalDialog
        }
    }
}
