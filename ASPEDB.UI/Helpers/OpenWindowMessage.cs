using System.Collections.Generic;
using ASPEDB.DTO.DB;
using ASPEDB.UI.ViewModel;
using DBPoint = ASPEDB.UI.Model.DisplayDBPoint;

namespace ASPEDB.UI.Helpers
{
  public class OpenWindowMessage {
    public WindowType Type { get; set; }
    public List<DBPoint> Argument { get; set; }
  }

}