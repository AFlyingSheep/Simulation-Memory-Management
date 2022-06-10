using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OS_4
{
    public partial class detail : Form
    {
        int nowIndex;
        int nowIndex2;
        int[] nums;
        Zhuangtai[] fifo_zhuangtai;
        Zhuangtai[] lru_zhuangtai;

        string xulie;

        Target fifo_target;
        Target lru_target;

        int index;

        public detail(Zhuangtai[] fifo_zhuangtai, Zhuangtai[] lru_zhuangtai, Target fifo_target, Target lru_target, int index, int[] xulie)
        {
            this.fifo_zhuangtai = fifo_zhuangtai;
            this.lru_zhuangtai = lru_zhuangtai;
            this.fifo_target = fifo_target;
            this.lru_target = lru_target;

            this.index = index;

            InitializeComponent();
            string s = "";
            for (int i = 0; i < index; i++)
            {
                s += xulie[i] + " ";
            }
            label7.Text = s;
            label20.Text = s;
            label8.Text = "↑";
            if (index > 0) {
                button2.Enabled = true;
                button3.Enabled = true;
            }
            textBox1.Text = fifo_zhuangtai[0].nowNeicun[0].ToString();
            textBox2.Text = lru_zhuangtai[0].nowStack.Peek().ToString();
            label12.Text = fifo_zhuangtai[0].isOut ? fifo_zhuangtai[0].outIndex.ToString() : "无淘汰";
            nowIndex = 0;
            nowIndex2 = 0;

            label38.Text = fifo_target.zhihuancishu.ToString();
            label39.Text = lru_target.zhihuancishu.ToString();
            label40.Text = fifo_target.queyecishu.ToString();
            label41.Text = lru_target.queyecishu.ToString();
            label42.Text = fifo_target.queyelv.ToString();
            label43.Text = lru_target.queyelv.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            nowIndex++;
            if (nowIndex + 1 == index) button2.Enabled = false;
            button1.Enabled = true;

            string s = "";
            for (int i = 0; i < nowIndex; i++) s += "  ";

            this.label8.Text = s + "↑";

            s = "";
            for (int i = 1; i <= fifo_zhuangtai[nowIndex].duitouIndex % index; i++) s += "  ";
            this.label6.Text = s + "↓";

            s = "";
            for (int i = 1; i <= fifo_zhuangtai[nowIndex].duiweiIndex % index; i++) s += "  ";
            this.label5.Text = s + "↑";

            s = "";
            for (int i = 0; i < fifo_zhuangtai[nowIndex].nowNeicun.Length; i++)
            {
                int x = fifo_zhuangtai[nowIndex].nowNeicun[i];
                if (x != -1) s += x + " ";
            }
            textBox1.Text = s;
            label12.Text = fifo_zhuangtai[nowIndex].isOut ? fifo_zhuangtai[nowIndex].outIndex.ToString() : "无淘汰";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            nowIndex--;
            if (nowIndex == 0) button1.Enabled = false;
            button2.Enabled = true;

            string s = "";
            for (int i = 0; i < nowIndex; i++) s += "  ";

            this.label8.Text = s + "↑";

            s = "";
            for (int i = 1; i < fifo_zhuangtai[nowIndex].duitouIndex % index; i++) s += "  ";
            this.label6.Text = s + "↓";

            s = "";
            for (int i = 1; i < fifo_zhuangtai[nowIndex].duiweiIndex % index; i++) s += "  ";
            this.label5.Text = s + "↑";

            s = "";
            for (int i = 0; i < fifo_zhuangtai[nowIndex].nowNeicun.Length; i++)
            {
                int x = fifo_zhuangtai[nowIndex].nowNeicun[i];
                if (x != -1) s += x + " ";
            }
            textBox1.Text = s;
            label12.Text = fifo_zhuangtai[nowIndex].isOut ? fifo_zhuangtai[nowIndex].outIndex.ToString() : "无淘汰";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            nowIndex2++;
            if (nowIndex2 + 1 == index) button3.Enabled = false;
            button4.Enabled = true;

            string s = "";
            for (int i = 0; i < nowIndex2; i++) s += "  ";

            this.label19.Text = s + "↑";

            s = "";
            for (int i = 1; i < lru_zhuangtai[nowIndex2].nowStack.Count; i++) s += "  ";
            this.label16.Text = s + "↑";


            s = "";
            Stack<int> temp = new Stack<int>(lru_zhuangtai[nowIndex2].nowStack);
            while (temp.Count > 0) s += temp.Pop() + " ";

            textBox2.Text = s;
            label17.Text = lru_zhuangtai[nowIndex2].isOut ? lru_zhuangtai[nowIndex2].outIndex.ToString() : "无淘汰";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            nowIndex2--;
            if (nowIndex2 == 0) button4.Enabled = false;
            button3.Enabled = true;

            string s = "";
            for (int i = 0; i < nowIndex2; i++) s += "  ";

            this.label19.Text = s + "↑";

            s = "";
            for (int i = 1; i < lru_zhuangtai[nowIndex2].nowStack.Count; i++) s += "  ";
            this.label16.Text = s + "↑";


            s = "";
            Stack<int> temp = new Stack<int>(lru_zhuangtai[nowIndex2].nowStack);
            while (temp.Count > 0) s += temp.Pop() + " ";

            textBox2.Text = s;
            label17.Text = lru_zhuangtai[nowIndex2].isOut ? lru_zhuangtai[nowIndex2].outIndex.ToString() : "无淘汰";
        }
    }
}
