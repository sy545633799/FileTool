using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using StarEdit.Interface;
using StarEdit.MysqlServices;

namespace StarEdit.MyComponents
{
    public partial class UCAttrBox : UserControl, IAutoControl
    {
        public string Value
        {
            get { return textBoxData.Text; }
            set { textBoxData.Text = value; }
        }

        public UCAttrBox()
        {
            InitializeComponent();
        }

        private void textBoxData_TextChanged(object sender, EventArgs e)
        {
            string data = textBoxData.Text;
            richTextBox1.Text = "";
            try
            {
                string[] attrs = data.Split('|');
                foreach (string attr in attrs)
                {
                    string[] attrinfo = attr.Split(':');

                    if (attrinfo.Length == 3)
                    {
                        if (attrinfo[1] == "1")
                        {
                            richTextBox1.AppendText(String.Format("{0}+{1} ", getAttribNameByAttribType(int.Parse(attrinfo[0])), attrinfo[2]));
                        }
                        else if (attrinfo[1] == "2")
                        {
                            richTextBox1.AppendText(String.Format("{0}+{1}‰ ", getAttribNameByAttribType(int.Parse(attrinfo[0])), attrinfo[2]));
                        }
                    }
                    else
                    {
                        richTextBox1.Text = "错误数据";
                    }
                }
            }
            catch
            {
                richTextBox1.Text = "错误数据";
            }
        }

        public static string getAttribNameByAttribType(int type)
        {
            switch (type)
            {
                case 1:
                    return "生命上限";
                case 2:
                    return "魔法上限";
                case 3:
                    return "物理攻击下限";
                case 4:
                    return "物理攻击上限";
                case 5:
                    return "魔法攻击下限";
                case 6:
                    return "魔法攻击上限";
                case 7:
                    return "道术攻击下限";
                case 8:
                    return "道术攻击上限";
                case 9:
                    return "物理防御下限";
                case 10:
                    return "物理防御上限";
                case 11:
                    return "魔法防御下限";
                case 12:
                    return "魔法防御上限";
                case 13:
                    return "准确";
                case 14:
                    return "敏捷";
                case 15:
                    return "魔法闪避(已弃用)";
                case 16:
                    return "暴击";
                case 17:
                    return "暴击伤害";
                case 18:
                    return "韧性";
                case 19:
                    return "会心一击(已弃用)";
                case 20:
                    return "会心闪避（已弃用）";
                case 21:
                    return "幸运";
                case 22:
                    return "抵抗幸运";
                case 23:
                    return "诅咒";
                case 24:
                    return "神圣";
                case 25:
                    return "攻速";
                case 26:
                    return "生命回复";
                case 27:
                    return "魔法回复";
                case 28:
                    return "中毒回复";
                case 29:
                    return "负重";
                case 30:
                    return "移动速度";
                case 31:
                    return "物理免伤";
                case 32:
                    return "魔法免伤";
                case 33:
                    return "免伤穿透";
                case 34:
                    return "伤害增益";
                case 35:
                    return "麻痹几率";
                case 36:
                    return "抗麻痹几率";
                case 37:
                    return "麻痹恢复";
                case 38:
                    return "合击威力";
                case 39:
                    return "反射几率";
                case 40:
                    return "反射比例";
                case 41:
                    return "格挡几率";
                case 42:
                    return "格挡值";
                case 43:
                    return "吸血几率";
                case 44:
                    return "吸血比例";
                case 45:
                    return "吸魔几率";
                case 46:
                    return "吸魔比例";
                case 47:
                    return "击杀回血";
                case 48:
                    return "击杀回魔";
                case 49:
                    return "药效增益";
                case 50:
                    return "复活增益";
                case 51:
                    return "抑生几率";
                case 52:
                    return "人形杀手";
                case 53:
                    return "野兽杀手";
                case 54:
                    return "亡灵杀手";
                case 55:
                    return "恶魔杀手";
                case 56:
                    return "掉宝几率";
                case 57:
                    return "探测宝物几率";
                case 58:
                    return "吸取技力几率";
                case 59:
                    return "杀怪金钱";
                case 60:
                    return "主角杀怪经验";
                case 61:
                    return "英雄杀怪经验";
                case 62:
                    return "怒气恢复速度";
                case 63:
                    return "攻击下限";
                case 64:
                    return "攻击上限";
                case 65:
                    return "减少英雄伤害";
                case 66:
                    return "最终伤害减免";
                case 67:
                    return "物理防御增益";
                case 68:
                    return "魔法防御增益";
                case 70:
                    return "对战士伤害增加";
                case 71:
                    return "受战士伤害减少";
                case 72:
                    return "对法师伤害增加";
                case 73:
                    return "受法师伤害减少";
                case 74:
                    return "对道士伤害增加";
                case 75:
                    return "受道士伤害减少";
                case 76:
                    return "对怪物伤害增加";
                case 77:
                    return "受怪物伤害减少";
                case 78:
                    return "对BOSS伤害增加";
                case 79:
                    return "受BOSS伤害减少";
                case 80:
                    return "内力上限(已弃用)";
                case 81:
                    return "内力回复(已弃用)";
                case 82:
                    return "内力免伤(已弃用)";
                case 83:
                    return "破防点数";
                case 84:
                    return "内功最大值";
                case 85:
                    return "内功当前值";
                case 86:
                    return "内功抵消伤害比例";
                case 87:
                    return "内功每三秒恢复值";
                case 88:
                    return "闪避率";
                case 89:
                    return "暴击率";
                case 91:
                    return "生命值";
                case 92:
                    return "魔法值";
                case 93:
                    return "怒气值";
                case 94:
                    return "内力值";
                case 96:
                    return "英雄经脉激活加成";
                case 98:
                    return "对英雄伤害增加";
                case 99:
                    return "减少受到英雄伤害";
                case 100:
                    return "回血增益";

            }
            return "";
        }
    }
}
