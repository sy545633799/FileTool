using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace StarEdit.MyComponents
{
    class DoubleBufferedPanel : Panel
    {
        public DoubleBufferedPanel()
        {
            this.DoubleBuffered = true;
        }
    }
}
