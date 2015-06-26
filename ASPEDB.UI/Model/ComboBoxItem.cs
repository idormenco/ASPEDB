using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPEDB.UI.Model
{
    public class ComboBoxItem
    {
        public int Index { get; set; }
        public string Value { get; set; }

        public ComboBoxItem(int index, string value)
        {
            this.Index = index;
            this.Value = value;
        }
    }
}
