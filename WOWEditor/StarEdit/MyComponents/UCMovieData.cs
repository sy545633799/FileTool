using System;
using System.Windows.Forms;
using StarEdit.Interface;

namespace StarEdit.MyComponents
{
    public partial class UCMovieData : UserControl, IAutoControl
    {
        public UCMovieData()
        {
            InitializeComponent();
        }

        public int SelectIndex
        {
            set
            {
                viewStack1.SelectedIndex = GetIndex(value);
            }
        }

        public string Value
        {
            get
            {
                switch (viewStack1.SelectedIndex + 1)
                {
                    case 1: return String.Format("{0}", textBoxShow1.Text);
                    case 2: return String.Format("1:{0}:{1}:{2}:{3}:100:100:{4}", textBoxMid2.Text, textBoxMapId2.Text, textBoxX2.Text, textBoxY2.Text, textBoxSp2.Text);
                    case 3: return String.Format("{0}:{1}:{2}:{3}:{4}:{5}:{4}:{5}:-1", textBoxId3.Text, textBoxType3.Text, textBoxX13.Text, textBoxY13.Text, textBoxX23.Text, textBoxY23.Text);
                    case 4: return String.Format("{0}", textBoxMonid4.Text);
                    case 5: return String.Format("{0}:{1}", textBoxId5.Text, textBoxType5.Text);
                    case 6: return String.Format("{0}:{1}:{2}", textBoxId6.Text, textBoxType6.Text, textBoxSay6.Text);
                    case 7: return String.Format("{0}:{1}:{2}:0:{3}:{4}:1:{5}:{6}:{7}:0:0", textBoxSid7.Text, textBoxSrcId7.Text, textBoxSrcType7.Text, textBoxDestX7.Text, textBoxDestY7.Text, textBoxTargetId7.Text, textBoxTargetType7.Text, textBoxDamage7.Text);
                    case 8: return String.Format("{0}", textBoxId8.Text);
                    case 9: return String.Format("{0}:{1}", textBoxNpcid9.Text, textBoxTalk9.Text);
                    case 10: return String.Format("{0}", textBoxEnd10.Text);
                    case 11: return String.Format("{0}:{1}", textBoxMid11.Text, textBoxDirect11.Text);
                    case 12: return String.Format("{0}:{1}:{2}:{3}:{4}:{5}:{6}:{7}", textBoxEid12.Text, textBoxEtype12.Text, textBoxDirect12.Text, textBoxId12.Text, textBoxType12.Text, textBoxX12.Text, textBoxY12.Text, textBoxTime12.Text);
                    case 13: return String.Format("{0}:{1}:{2}:{3}", textBoxMapId1.Text, textBoxPisitionX2.Text, textBoxPisitionY2.Text, textBoxParameter.Text);
                    case 14: return String.Format("{0}:{1}:{2}", textBoxColor1.Text,textBoxNum141.Text,textBoxNum142.Text);
                    case 15: return String.Format("{0}:{1}:{2}", textBoxText151.Text,textBoxTime151.Text,textBoxColor151.Text);
                    default: return "";
                }
            }
            set
            {
                String[] datab = value.Split(':');
                String[] data = new String[] { "0", "0", "0", "0", "0", "0", "0", "0", "0", "0" };
                for (int i = 0; i < Math.Min(datab.Length, data.Length); i++)
                {
                    data[i] = datab[i];
                }

                switch (viewStack1.SelectedIndex + 1)
                {
                    case 1: textBoxShow1.Text = data[0]; break;
                    case 2: textBoxMid2.Text = data[1]; textBoxMapId2.Text = data[2]; textBoxX2.Text = data[3]; textBoxY2.Text = data[4]; textBoxSp2.Text = data[7]; break;
                    case 3: textBoxId3.Text = data[0]; textBoxType3.Text = data[1]; textBoxX13.Text = data[2]; textBoxY13.Text = data[3]; textBoxX23.Text = data[4]; textBoxY23.Text = data[5]; break;
                    case 4: textBoxMonid4.Text = data[0]; break;
                    case 5: textBoxId5.Text = data[0]; textBoxType5.Text = data[1]; break;
                    case 6: textBoxId6.Text = data[0]; textBoxType6.Text = data[1]; textBoxSay6.Text = data[2]; break;
                    case 7: textBoxSid7.Text = data[0]; textBoxSrcId7.Text = data[1]; textBoxSrcType7.Text = data[2]; textBoxDestX7.Text = data[4]; textBoxDestY7.Text = data[5]; textBoxTargetId7.Text = data[7]; textBoxTargetType7.Text = data[8]; textBoxDamage7.Text = data[9]; break;
                    case 8: textBoxId8.Text = data[0]; break;
                    case 9: textBoxNpcid9.Text = data[0]; textBoxTalk9.Text = data[1]; break;
                    case 10: textBoxEnd10.Text = data[0]; break;
                    case 11: textBoxMid11.Text = data[0]; textBoxDirect11.Text = data[1]; break;
                    case 12: textBoxEid12.Text = data[0]; textBoxEtype12.Text = data[1]; textBoxDirect12.Text = data[2]; textBoxId12.Text = data[3]; textBoxType12.Text = data[4]; textBoxX12.Text = data[5]; textBoxY12.Text = data[6]; textBoxTime12.Text = data[7]; break;
                    case 13: textBoxMapId1.Text = data[0]; textBoxPisitionX2.Text = data[1]; textBoxPisitionY2.Text = data[2]; textBoxParameter.Text = data[3]; break;
                    case 14: textBoxColor1.Text = data[0]; textBoxNum141.Text = data[1]; textBoxNum142.Text = data[2]; break;
                    case 15: textBoxText151.Text = data[0]; textBoxTime151.Text = data[1]; textBoxColor151.Text = data[2]; break;
                }
            }
        }

        private int GetIndex(int type)
        {
            switch (type)
            {
                case 1: return 0;
                case 2: return 9;
                case 3: return 7;
                case 11:
                case 21: return 1;
                case 12: return 2;
                case 13:
                case 14:
                case 18: return 3;
                case 15: return 4;
                case 16: return 5;
                case 17: return 6;
                case 19: return 10;
                case 22: return 8;
                case 23: return 13;
                case 24: return 14;
                case 25: return 15;
                case 31: return 11;
                case 32: return 12;
                case 33: return 12;
                
            }
            return 0;
        }
    }
}
