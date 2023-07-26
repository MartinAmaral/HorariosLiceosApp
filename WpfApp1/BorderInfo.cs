using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    public class BorderInfo
    {
        public byte Column { get; set; }
        public byte Row { get; set; }
        public bool HasChanged { get; set; }
        public BorderInfo(byte column,byte row, bool hasChanged)
        {
            Column = column;
            Row = row;
            HasChanged = hasChanged;
        }
    }
}
