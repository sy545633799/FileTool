using System;
using System.Windows.Forms;
using StarEdit.Interface;
using StarEdit.Tools;

namespace StarEdit.MyComponents
{
    public partial class UCUnixTimeLabel : UserControl, IAutoControl
    {
        private string timeData;

        public UCUnixTimeLabel()
        {
            InitializeComponent();
        }

        public String Value
        {
            get { return timeData; }
            set
            {
                timeData = value;
                labelData.Text = TimeTool.UnixTimeToDateTime(int.Parse(timeData)).ToString();
            }
        }
    }
}
