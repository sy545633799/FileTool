using System;
using System.Drawing;
using System.Windows.Forms;

namespace MessageBoxExLib
{
    public class MessageBoxEx
    {
        static public DialogResult Show(String Text, String Caption, Icon Icon)
        {
            MessageBoxExForm mbef = new MessageBoxExForm();
            mbef.FulText = Text;
            mbef.Caption = Caption;
            mbef.Icon = Icon;
            mbef.ShowDialog();
            return mbef.Result;
        }
    }
}
