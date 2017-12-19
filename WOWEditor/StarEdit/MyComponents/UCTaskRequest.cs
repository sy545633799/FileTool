using System;
using System.Windows.Forms;
using StarEdit.Interface;

namespace StarEdit.MyComponents
{
    public partial class UCTaskRequest : UserControl, IAutoControl
    {
        public UCTaskRequest()
        {
            InitializeComponent();
        }

        public int SelectIndex
        {
            set
            {
                viewStack1.SelectedIndex = value - 1;
            }
        }

        public string Value
        {
            get
            {
                switch (viewStack1.SelectedIndex + 1)                    
                {
                    case 1: return "";
                    case 2: return String.Format("{0}:{1}:{2}", textBoxItemid.Text, textBoxType.Text, textBoxItemNum.Text);
                    case 3: return textBoxItemUse.Text;
                    case 4: return String.Format("{0}:{1}", textBoxMonid.Text, textBoxMonNum.Text);
                    case 5: return String.Format("{0}:{1}", textBoxMonLevel.Text, textBoxMonNum2.Text);
                    case 6: return String.Format("{0}:{1}", reincarnTextBox1.Text, textBoxLevel2.Text); 
                    case 7: return String.Format("{0}:{1}:{2}", textBoxMonsterGroupId.Text, textBoxCount.Text, textBoxProb.Text);
                    case 8: return String.Format("{0}:{1}:{2}", textBoxMapid8.Text, textBoxX8.Text, textBoxY8.Text);
                    case 9: return String.Format("{0}:{1}", textBoxType9.Text, textBoxSend9.Text);
                    case 10: return String.Format("{0}", textBoxnpc10.Text);
                    case 11: return "";
                    case 12: return "";
                    case 13: return String.Format("{0}:{1}", textBoxID13.Text, textBoxNum13.Text);
                    case 14: return String.Format("{0}:{1}", textBoxId14.Text, textBoxNum14.Text);
                    case 15: return String.Format("{0}:{1}", textBoxMapId.Text,textBoxMineNum.Text);
                    case 16: return String.Format("{0}:{1}", textBoxId16.Text, textBoxNum16.Text);
                    case 17: return String.Format("{0}",textBoxNum17.Text);
                    case 18: return String.Format("{0}:{1}", textBoxNum18.Text, textBoxNum19.Text);
                    case 19: return String.Format("{0}", textBoxNum20.Text);
                    case 20: return String.Format("{0}", textBoxNum21.Text);
                    case 21: return String.Format("{0}:{1}", textBoxRingId.Text,textBoxRingLevel.Text);
                    case 22: return String.Format("{0}", textBoxNum23.Text);
                    case 23: return String.Format("{0}", textBoxNum24.Text);
                    case 24: return String.Format("{0}", textBoxNum25.Text);
                    case 25: return String.Format("{0}", textBoxTaskId.Text);
                    case 26: return "";
                    case 27: return String.Format("{0}", textBoxGuanWei.Text);
                    case 28: return "";
                    case 29: return String.Format("{0}", guanYinId.Text);
                    case 30: return "";
                    case 31: return String.Format("{0}", duiJietextBox1.Text);
                    case 32: return String.Format("{0}:{1}", jointHaloId.Text, jointHaloLevel.Text);
                    case 33: return String.Format("{0}:{1}:{2}", textBox3.Text,textBox2.Text,textBox1.Text);
                    case 34: return String.Format("{0}:{1}:{2}", textBox4.Text, textBox5.Text, textBox6.Text);
                    case 35: return String.Format("{0}:{1}:{2}", playerReincarn.Text, playerLevel.Text, killNum.Text);
                    case 36: return String.Format("{0}:{1}:{2}", textBox7.Text, textBox9.Text, textBox8.Text);
                    case 37: return String.Format("{0}:{1}", textBox10.Text, textBox11.Text);
                    case 38: return String.Format("{0}:{1}", textBox12.Text, textBox13.Text);
                    case 39: return String.Format("{0}:{1}", textBox14.Text, textBox15.Text);
                    default: return "";
                }                
            }
            set
            {
                String[] datab = value.Split(':');
                String[] data = new String[] { "0", "0", "0" };
                for (int i = 0; i < Math.Min(datab.Length, data.Length); i++)
                {
                    data[i] = datab[i];
                }

                switch (viewStack1.SelectedIndex + 1)
                {
                    case 2: textBoxItemid.Text = data[0]; textBoxType.Text = data[1]; textBoxItemNum.Text = data[2]; break;
                    case 3: textBoxItemUse.Text = data[0]; break;
                    case 4: textBoxMonid.Text = data[0]; textBoxMonNum.Text = data[1]; break;
                    case 5: textBoxMonLevel.Text = data[0]; textBoxMonNum2.Text = data[1]; break;
                    case 6: reincarnTextBox1.Text = data[0]; textBoxLevel2.Text = data[1]; break;
                    case 7: textBoxMonsterGroupId.Text = data[0]; textBoxCount.Text = data[1]; textBoxProb.Text = data[2]; break;
                    case 8: textBoxMapid8.Text = data[0]; textBoxX8.Text = data[1]; textBoxY8.Text = data[2]; break;
                    case 9: textBoxType9.Text = data[0]; textBoxSend9.Text = data[1]; break;
                    case 10: textBoxnpc10.Text = data[0]; break;
                    case 13: textBoxID13.Text = data[0]; textBoxNum13.Text = data[1]; break;
                    case 14: textBoxId14.Text = data[0]; textBoxNum14.Text = data[1]; break;
                    case 15: textBoxMapId.Text = data[0]; textBoxMineNum.Text = data[1]; break;
                    case 16: textBoxId16.Text = data[0]; textBoxNum16.Text = data[1];break;
                    case 17: textBoxNum17.Text = data[0]; break;
                    case 18: textBoxNum18.Text = data[0]; textBoxNum19.Text = data[1]; break;
                    case 19: textBoxNum20.Text = data[0]; break;
                    case 20: textBoxNum21.Text = data[0]; break;
                    case 21: textBoxRingId.Text = data[0]; textBoxRingLevel.Text = data[1]; break;
                    case 22: textBoxNum23.Text = data[0]; break;
                    case 23: textBoxNum24.Text = data[0]; break;
                    case 24: textBoxNum25.Text = data[0]; break;
                    case 25: textBoxTaskId.Text = data[0]; break;
                    case 26: break;
                    case 27: textBoxGuanWei.Text = data[0]; break;
                    case 28: break;
                    case 29: guanYinId.Text = data[0]; break;
                    case 30: break;
                    case 31: duiJietextBox1.Text = data[0]; break; ;
                    case 32: jointHaloId.Text = data[0]; jointHaloLevel.Text = data[1]; break;
                    case 33: textBox3.Text = data[0]; textBox2.Text = data[1]; textBox1.Text = data[2]; break;
                    case 34: textBox4.Text = data[0]; textBox5.Text = data[1]; textBox6.Text = data[2]; break;
                    case 35: playerReincarn.Text = data[0]; playerLevel.Text = data[1]; killNum.Text = data[2]; break;
                    case 36: textBox7.Text = data[0]; textBox9.Text = data[1]; textBox8.Text = data[2]; break;
                    case 37: textBox10.Text = data[0]; textBox11.Text = data[1]; break;
                    case 38: textBox12.Text = data[0]; textBox13.Text = data[1]; break;
                    case 39: textBox14.Text = data[0]; textBox15.Text = data[1]; break;
                }
            }
        }
    }
}
