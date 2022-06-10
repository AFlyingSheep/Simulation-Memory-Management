using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OS_4
{
    public partial class Form1 : Form
    {
        int[] nums;
        Zhuangtai[] fifo_zhuangtai;
        Zhuangtai[] lru_zhuangtai;

        Target fifo_target;
        Target lru_target;

        int index;

        public Form1()
        {
            InitializeComponent();
            Oper oper = new Oper();
            Zhuangtai[] zhuangtai = oper.FIFO(
                new int[] { 7, 0, 1, 2, 0, 3, 0, 4, 2, 3, 0, 3, 2, 1, 2, 0, 1, 7, 0, 1 }, 3);
            oper.Caclu(zhuangtai, zhuangtai.Length);
            Zhuangtai[] zhuangtai2 = oper.LRU(
                new int[] { 4, 7, 0, 7, 1, 0, 1, 2, 1, 2, 6 }, 5);
            oper.Caclu(zhuangtai2, zhuangtai2.Length);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int num = (int)numericUpDown1.Value;
            string[] inputNum_string = textBox1.Text.Split(' ');
            index = 0;
            nums = new int[1000];
            for (int i = 0; i < inputNum_string.Length; i++)
            {
                if (inputNum_string[i] != "")
                {
                    foreach (byte c in inputNum_string[i])
                    {
                        if (c != 46)//小数点的ASCII码为46
                        {
                            if (!(c >= 48 && c <= 57))//非0~9的ASCII码
                            {
                                MessageBox.Show("存在非法字符！");
                                return;
                            }
                        }
                    }
                    nums[index++] = int.Parse(inputNum_string[i]);
                }
            }
            Oper oper = new Oper();
            fifo_zhuangtai = oper.FIFO(nums, num);
            lru_zhuangtai = oper.LRU(nums, num);

            fifo_target = oper.Caclu(fifo_zhuangtai, index);
            lru_target = oper.Caclu(lru_zhuangtai, index);

            textBox2.Clear();
            textBox3.Clear();

            textBox3.AppendText("执行队列：");
            textBox2.AppendText("执行队列：");
            for (int i = 0; i < index; i++)
            {
                textBox2.AppendText(nums[i] + " ");
                textBox3.AppendText(nums[i] + " ");
            }
            

            textBox2.AppendText("\r\n缺页次数：" + fifo_target.queyecishu);
            textBox3.AppendText("\r\n缺页次数：" + lru_target.queyecishu);

            textBox2.AppendText("\r\n置换次数：" + fifo_target.zhihuancishu);
            textBox3.AppendText("\r\n置换次数：" + lru_target.zhihuancishu);

            textBox2.AppendText("\r\n缺页率：" + fifo_target.queyelv);
            textBox3.AppendText("\r\n缺页率：" + lru_target.queyelv);

            oper.display(textBox2, fifo_zhuangtai, index);
            oper.display(textBox3, lru_zhuangtai, index, 2);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            Oper oper = new Oper();
            oper.readFile(textBox1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            detail d = new detail(fifo_zhuangtai, lru_zhuangtai, fifo_target, lru_target, index, nums);
            d.Visible = true;
        }
    }

    public class Oper
    {
        /*
        public Zhuangtai[] FIFO(int[] inputs, int num)
        {
            Zhuangtai[] zhuangtais = new Zhuangtai[inputs.Length];
            Queue<int> q = new Queue<int>();
            bool[] hash = new bool[1000];
            int[] neicun = new int[num];
            int neicunWei = 0;
            int neicunFront = 0;
            for (int i = 0; i < 1000; i++) hash[i] = false;
            for (int i = 0; i < inputs.Length; i++)
            {
                int input = inputs[i];
                Zhuangtai zhuangtai = new Zhuangtai("Queue", num);
                zhuangtai.index = i + 1;
                if (hash[input] == false)
                {
                    zhuangtai.isHit = false;
                    if (q.Count == num)
                    {
                        hash[input] = true;
                        hash[q.Peek()] = false;

                        zhuangtai.isOut = true;
                        zhuangtai.outIndex = q.Dequeue();

                        zhuangtai.isIn = true;
                        zhuangtai.inIndex = input;
                        q.Enqueue(input);
                    }
                    else
                    {
                        hash[input] = true;
                        zhuangtai.isIn = true;
                        zhuangtai.inIndex = input;
                        q.Enqueue(input);
                    }
                }
                else
                {
                    zhuangtai.isHit = true;
                }
                zhuangtais[i] = zhuangtai;
                zhuangtai.nowQueue = new Queue<int>(q);
            }
            return zhuangtais;
        }
        */
        public Zhuangtai[] FIFO(int[] inputs, int num)
        {
            Zhuangtai[] zhuangtais = new Zhuangtai[inputs.Length];
            bool[] hash = new bool[1001];
            int[] neicun = new int[num];
            for (int i = 0; i < neicun.Length; i++) neicun[i] = -1;
            int neicunWei = 0;
            int neicunFront = -1;
            for (int i = 0; i <= 1000; i++) hash[i] = false;
            int listNum = 0;
            for (int i = 0; i < inputs.Length; i++)
            {
                int input = inputs[i];
                Zhuangtai zhuangtai = new Zhuangtai("Queue", num);
                zhuangtai.index = i + 1;
                if (hash[input + 1] == false)
                {
                    zhuangtai.isHit = false;
                    if (listNum == num)
                    {
                        hash[input + 1] = true;
                        hash[neicun[neicunWei] + 1] = false;
                        

                        zhuangtai.isOut = true;
                        zhuangtai.outIndex = neicun[neicunWei];

                        zhuangtai.isIn = true;
                        zhuangtai.inIndex = input;

                        neicunWei = (neicunWei + 1) % num;
                        neicunFront = (neicunFront + 1) % num;
                        neicun[neicunFront] = input;
                    }
                    else
                    {
                        hash[input + 1] = true;
                        zhuangtai.isIn = true;
                        zhuangtai.inIndex = input;
                        neicunFront = (neicunFront + 1) % num;
                        neicun[neicunFront] = input;
                        listNum++;
                    }
                }
                else
                {
                    zhuangtai.isHit = true;
                }
                zhuangtai.duitouIndex = neicunFront;
                zhuangtai.duiweiIndex = neicunWei;
                zhuangtais[i] = zhuangtai;

                Array.Copy(neicun, zhuangtais[i].nowNeicun, num);
            }
            return zhuangtais;
        }

        public Zhuangtai[] LRU(int[] inputs, int num)
        {
            Zhuangtai[] zhuangtais = new Zhuangtai[inputs.Length];
            Stack<int> s = new Stack<int>();
            bool[] hash = new bool[1001];
            for (int i = 0; i <= 1000; i++) hash[i] = false;
            for (int i = 0; i < inputs.Length; i++)
            {
                Zhuangtai zhuangtai = new Zhuangtai("Stack", num);
                int input = inputs[i];
                if (hash[input + 1] == true)
                {
                    zhuangtai.isHit = true;
                    Stack<int> temp = new Stack<int>();
                    while (s.Peek() != input)
                    {
                        temp.Push(s.Pop());
                    }
                    s.Pop();
                    while (temp.Count > 0) s.Push(temp.Pop());
                    s.Push(input);
                }
                else if (s.Count == num)
                {
                    Stack<int> temp = new Stack<int>();
                    while (s.Count > 0) temp.Push(s.Pop());
                    int popnum = temp.Pop();
                    while (temp.Count > 0) s.Push(temp.Pop());
                    s.Push(input);
                    hash[popnum + 1] = false;
                    hash[input + 1] = true;

                    zhuangtai.isOut = true;
                    zhuangtai.isIn = true;
                    zhuangtai.outIndex = popnum;
                    zhuangtai.inIndex = input;
                }

                else
                {
                    hash[input + 1] = true;
                    s.Push(input);
                    zhuangtai.isIn = true;
                    zhuangtai.inIndex = input;
                }
                zhuangtai.nowStack = new Stack<int>(s);
                zhuangtais[i] = zhuangtai;
            }
            return zhuangtais;
        }

        public Target Caclu(Zhuangtai[] zhuangtais, int index)
        {
            Target target = new Target();
            for (int i = 0; i < index; i++) 
            {
                Zhuangtai zhuangtai = zhuangtais[i];
                if (zhuangtai.isHit == false)
                {
                    target.queyecishu++;
                    if (zhuangtai.isOut == true)
                    {
                        target.zhihuancishu++;
                    }
                }
            }
            target.queyelv = ((float)target.queyecishu / (float)index);
            return target;
        }

        public void display(TextBox textBox, Zhuangtai[] zhuangtai, int index, int mode = 0)
        {
            
            if (mode == 0)
            {
                textBox.AppendText("\r\n-------内存状态---------");
                for (int i = 0; i < index; i++)
                {
                    textBox.AppendText("\r\nt=" + zhuangtai[i].index + ":");
                    for (int j = 0; j < zhuangtai[i].nowNeicun.Length; j++)
                    {
                        int x = zhuangtai[i].nowNeicun[j];
                        if (x != -1)
                            textBox.AppendText(x.ToString() + " ");
                    }
                    string s = zhuangtai[i].isOut ? zhuangtai[i].outIndex.ToString() : "无淘汰";
                    textBox.AppendText(" 淘汰页面：" + s);
                }
            }
            else
            {
                textBox.AppendText("\r\n-------栈状态---------");
                for (int i = 0; i < index; i++)
                {
                    textBox.AppendText("\r\nt=" + zhuangtai[i].index + ":（栈顶）");
                    Stack<int> temp = new Stack<int>(zhuangtai[i].nowStack);
                    while (temp.Count > 0)
                    {
                        int x = temp.Pop();
                        if (x != -1)
                            textBox.AppendText(x.ToString() + " ");
                    }
                    string s = zhuangtai[i].isOut ? zhuangtai[i].outIndex.ToString() : "无淘汰";
                    textBox.AppendText(" 淘汰页面：" + s);
                }
            }
            
        }

        private string selectPath()
        {
            string path = string.Empty;
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Filter = "Files (*.txt)|*.txt"//如果需要筛选txt文件（"Files (*.txt)|*.txt"）
            };

            //var result = openFileDialog.ShowDialog();
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                path = openFileDialog.FileName;
            }

            return path;
        }

        public static bool digitjdg(string x)
        {
            const string pattern = "^[0-9]*$";
            Regex rx = new Regex(pattern);
            bool IsDigit = rx.IsMatch(x);
            return IsDigit;//是数字返回true,不是返回false
        }

        public bool errorDetect(string[] s)
        {
            foreach (string str in s)
            {
                if (!digitjdg(str)) return false;
            }
            return true;
        }

        public void readFile(TextBox text)
        {
            string path = selectPath();
            if (path == String.Empty)
            {
                MessageBox.Show("读取失败！");
                return;
            }
            StreamReader sr = new StreamReader(path, Encoding.Default);

            String line;
            if ((line = sr.ReadLine()) != null)
            {
                string[] s = line.Split('\t', ' ');

                if (!errorDetect(s))
                {
                    MessageBox.Show("读取错误！请检查后输入。");
                    return;
                }
                for (int i = 0; i < s.Length; i++) text.AppendText(s[i] + " ");
                
            }
            MessageBox.Show("读取成功！");
        }

    }

    public class Zhuangtai
    {
        public int index;           // 序列号
        public bool isIn;           // 是否有元素进入
        public bool isOut;          // 是否有元素换出
        public bool isHit;          // 是否命中
        public int inIndex;         // 进入序号
        public int outIndex;        // 换出序号
        public bool hitIndex;       // 命中序号
        public Stack<int> nowStack; // 模拟栈
        public int[] nowNeicun;     // 模拟内存
        public int duitouIndex;     // 内存队首序号
        public int duiweiIndex;     // 内存队尾序号

        public Zhuangtai (string input, int num)
        {
            isIn = false;
            isOut = false;
            isHit = false;
            if (input == "Stack") nowStack = new Stack<int>();
            else
            {
                nowNeicun = new int[num];
            }
            duitouIndex = 0;
            duiweiIndex = 0;
        }
    }

    public class Target
    {
        public float queyelv;
        public int queyecishu;
        public int zhihuancishu;
        public Target()
        {
            queyecishu = 0;
            zhihuancishu = 0;
        }
    }
}


