using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BullsAndCows
{
    public partial class Form1 : Form
    {
        bool step;//false-ходит игрок,true - ходит компьютер
        int[] curnum;
        int[] curnum2;
        int[] AiNum;
        List<int[]> PossibleNums = new List<int[]>();
        int stepcount=0;
        int stepcount2=0;
        public Form1()
        {
            InitializeComponent();
        }

        //Генерируем массив всех возможных чисел
        List<int[]> AllPossible()
        {

            List<int[]> nums = new List<int[]>();

            for (int i = 1; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    for (int k = 0; k < 10; k++)
                    {
                        for (int l = 0; l < 10; l++)
                        {
                            if (i == j || i == k || i == l || j == k || j == l || k == l)
                            {
                                continue;
                            }
                            else
                            {
                                nums.Add(new int[] { i, j, k, l });
                            }
                        }
                    }
                }
            }

            return nums;
        }

        //Проверяет количество быков
        int CountOfBulls(int[] number, int[] cnumber)
        {
            int n = 0;
            for (int i = 0; i < 4; i++)
            {
                if (number[i] == cnumber[i])
                {
                    n++;
                }
            }
            return n;
        }
        //Проверяет количество коров
        int CountOfCows(int[] number, int[] cnumber)
        {
            int n = 0;
            for (int i = 0; i < 4; i++)
            {
                if (number.Contains(cnumber[i]))
                {
                    n++;
                }
            }

            return n - CountOfBulls(number, cnumber);
        }

        //Проверка на соответствие числа правилам игры
        bool GoodNum(int[] num)
        {
            bool result = true;
            int[] countn = new int[4];
            if (num[0] == 0)
            {
                result = false;
                return result;
            }
            for (int i = 0; i < 4; i++)
            {
                int temp = num[i];
                for (int j = 0; j < 4; j++)
                {
                    if (temp == num[j])
                    {
                        countn[i]++;
                    }
                }

            }
            for (int i = 0; i < 4; i++)
            {
                if (countn[i] != 1)
                {
                    result = false;
                    return result;
                }
            }
            return result;
        }

        //Выкидывает неподходящие числа
        void AiMove()
        {
            
            
            int bulls = Convert.ToInt32(textBox1.Text);
            int cows = Convert.ToInt32(textBox2.Text);
            
            if (bulls == 4)
            {
                MessageBox.Show($"Ваше число{curnum[0]}{curnum[1]}{curnum[2]}{curnum[3]}");
                return;
            }
            //Если сумма быков и коров=4, то мы угадали 4 цифры, удаляем все варианты из этиъ 4 цифр
            if (bulls + cows == 4)
            {
                for (int i = 0; i < PossibleNums.Count; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        if (PossibleNums.Count != 0 &&  PossibleNums[i].Contains(curnum[j]) == false)
                        {
                            PossibleNums.RemoveAt(i);
                            if (i != 0)
                            {
                                i--;
                            }
                        }
                    }
                }
            }
            //если быков и коров 0 то данных цифр нет в нашем загаданном числе
            if (cows == 0 && bulls == 0)
            {
                for (int i = 0; i < PossibleNums.Count; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        if (PossibleNums.Count != 0 &&  PossibleNums[i].Contains(curnum[j]))
                        {
                            PossibleNums.RemoveAt(i);
                            if (i != 0)
                            {
                                i--;
                            }
                        }

                    }
                }
            }
            //Проверяем кол-во быко и коров относительно числа которое выдал компьютер
            for (int i = 0; i < PossibleNums.Count; i++)
            {
                if (PossibleNums.Count != 0 &&  CountOfBulls(PossibleNums[i], curnum) != bulls || CountOfCows(PossibleNums[i], curnum) < cows)
                {
                    PossibleNums.RemoveAt(i);
                    if (i != 0)
                    {
                        i--;
                    }
                }

            }

            if (bulls != 0 || cows != 0)
            {
                for (int i = 0; i < PossibleNums.Count; i++)
                {
                    if (PossibleNums.Count != 0 && PossibleNums[i].Contains(curnum[0]) == false && PossibleNums[i].Contains(curnum[1]) == false && PossibleNums[i].Contains(curnum[2]) == false && PossibleNums[i].Contains(curnum[3]) == false)
                    {
                        PossibleNums.RemoveAt(i);
                        if (i != 0)
                        {
                            i--;
                        }
                    }
                }
            }
            //Если быков 0 то удаляем все варианты где цифры стоят на этих позициях
            if (bulls == 0)
            {
                for (int i = 0; i < PossibleNums.Count; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        if (PossibleNums.Count!=0 && PossibleNums[i][j] == curnum[j])
                        {
                            PossibleNums.RemoveAt(i);
                            if (i != 0)
                            {
                                i--;
                            }

                        }
                    }
                }
            }
            //если коров 0, 
            if (bulls != 0 && cows == 0)
            {
                for (int i = 0; i < PossibleNums.Count; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        if (PossibleNums.Count != 0 &&  PossibleNums[i].Contains(curnum[j]))
                        {
                            for (int k = 0; k < 4; k++)
                            {
                                if (PossibleNums.Count != 0 && PossibleNums[i][k] == curnum[j] && j != k)
                                {
                                    PossibleNums.RemoveAt(i);
                                    if (i != 0)
                                    {
                                        i--;
                                    }
                                }
                            }
                        }

                    }
                }
            }
            for (int i = 0; i < PossibleNums.Count; i++)
            {
                if (PossibleNums.Count != 0 && PossibleNums[i] == curnum)
                {
                    PossibleNums.RemoveAt(i);
                    return;
                }
            }
            if (PossibleNums.Count == 0 && bulls != 4)
            {
                MessageBox.Show($"Играйте честно или проверьте введенные ответы");
                return;

            }
        }
        //Генерация числа
        int[] Make_a_number()
        {
            List<int> digits = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            int[] number = new int[4];
            Random ran = new Random();
            for (int i = 0; i < 4; i++)
            {
                int next = ran.Next(0, digits.Count);
                number[i] = digits[next];
                digits.RemoveAt(next);
            }
            return number;
        }

        //получает строку возвращает массив интов
        int[] StringToArray(string number)
        {
            int[] result = new int[number.Length];
            for (int i = 0; i < 4; i++)
            {
                result[i] = Convert.ToInt32(number[i].ToString());
            }
            return result;
        }
        
        //Проверка победы игрока
        void PlayerWin(int[] number, int[] number2)
        {
            if (CountOfBulls(number, number2) == 4)
            {
                MessageBox.Show($"Вы выиграли, моё число {number[0].ToString() + number[1].ToString() + number[2].ToString() + number[3].ToString()}");
            }
        }

        //Очищаем поля для ввода-вывода
        void clear()
        {
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            listBox3.Items.Clear();
            listBox4.Items.Clear();
            listBox5.Items.Clear();
            listBox6.Items.Clear();
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            PossibleNums.Clear();
            stepcount = 0;
            stepcount2 = 0;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            groupBox2.Visible = true;
            textBox4.Visible = false;
            textBox5.Visible = false;
            textBox6.Visible = false;

        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            
            if (radioButton2.Checked && step==true )//Первый ходит компьютер
            {
                if(textBox1.Text!="" && textBox2.Text != "" && textBox3.Text != "")
                {
                    listBox2.Items.Add(textBox1.Text);
                    listBox3.Items.Add(textBox2.Text);
                    AiMove();
                }
                else
                {
                    MessageBox.Show("Введите кол-во быков и коров");
                    
                    goto End;
                }
                if (textBox3.Text != "")
                {
                    curnum2 = StringToArray(textBox3.Text);
                    if (GoodNum(curnum))
                    {
                        listBox4.Items.Add(curnum2[0].ToString() + curnum2[1].ToString() + curnum2[2].ToString() + curnum2[3].ToString());
                        listBox5.Items.Add(CountOfBulls(AiNum, curnum2).ToString());
                        listBox6.Items.Add(CountOfCows(AiNum, curnum2).ToString());
                        PlayerWin(AiNum,curnum2);
                        
                    }
                    else
                    {
                        MessageBox.Show("Введите корректное  число");
                        goto End ;
                    }
                }
                else
                {
                    MessageBox.Show("Введите число");
                    goto End;
                }
                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                if (PossibleNums.Count != 0)
                {
                    Random rnd = new Random();
                    curnum = PossibleNums[rnd.Next(0, PossibleNums.Count)];
                    listBox1.Items.Add(curnum[0].ToString() + curnum[1].ToString() + curnum[2].ToString() + curnum[3].ToString());

                }
                label7.Text = PossibleNums.Count.ToString();
            }
             
            if (radioButton2.Checked && step == false) //если первый ходит игрок
            {
                if (textBox3.Text != "")
                {
                    curnum2 = StringToArray(textBox3.Text);
                    if (GoodNum(curnum2))
                    {
                        listBox4.Items.Add(curnum2[0].ToString() + curnum2[1].ToString() + curnum2[2].ToString() + curnum2[3].ToString());
                        listBox5.Items.Add(CountOfBulls(AiNum, curnum2).ToString());
                        listBox6.Items.Add(CountOfCows(AiNum, curnum2).ToString());
                        PlayerWin(AiNum, curnum2);
                    }
                    else
                    {
                        MessageBox.Show("Введите корректное  число");
                        goto End;
                    }
                }
                else
                {
                    MessageBox.Show("Введите число");
                    goto End;
                    
                }
                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                step = true;
                Random rnd = new Random();
                curnum = PossibleNums[rnd.Next(0, PossibleNums.Count)];
                listBox1.Items.Add(curnum[0].ToString() + curnum[1].ToString() + curnum[2].ToString() + curnum[3].ToString());
            }
            if (radioButton2.Checked == false)//игрок вс игрок
            {
                curnum2 = StringToArray(textBox3.Text);
                listBox4.Items.Add(curnum2[0].ToString() + curnum2[1].ToString() + curnum2[2].ToString() + curnum2[3].ToString());
                listBox2.Items.Add(textBox4.Text);
                listBox3.Items.Add(textBox5.Text);
                stepcount++;
                button3.Visible = true;
                button4.Visible = false;
            }
             End: return;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            clear();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            groupBox2.Visible = false;
            textBox4.Visible = true;
            textBox5.Visible = true;
            textBox6.Visible = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (radioButton2.Checked)//Игрок вс Компьютер
            {
                button3.Visible = false;
                label5.Text = "Компьютер";
                label6.Text = "Игрок";
                groupBox2.Visible = true;
                textBox4.Visible = false;
                textBox5.Visible = false;
                textBox6.Visible = false;
                AiNum = Make_a_number();
                PossibleNums = AllPossible();
                if (radioButton3.Checked)//если первый ходит игрок
                {
                    step = false;
                }
                else// если первый ходит компьютер
                {
                    step = true;
                    Random rnd = new Random();
                    curnum = PossibleNums[rnd.Next(0, PossibleNums.Count)];
                    listBox1.Items.Add(curnum[0].ToString() + curnum[1].ToString() + curnum[2].ToString() + curnum[3].ToString()); 
                    
                    
                }
            }
            else if (radioButton1.Checked)//Игрок вс Игрок
            {
                label5.Text = "Игрок 1";
                label6.Text = "Игрок 2";
                button4.Visible = false;
                stepcount = 0;

            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (stepcount == 0)
            {
                curnum = StringToArray(textBox6.Text);
                listBox1.Items.Add(curnum[0].ToString() + curnum[1].ToString() + curnum[2].ToString() + curnum[3].ToString());
                stepcount++;
                button3.Visible = false;
                button4.Visible = true;
            }
            else
            {
                curnum = StringToArray(textBox6.Text);
                listBox1.Items.Add(curnum[0].ToString() + curnum[1].ToString() + curnum[2].ToString() + curnum[3].ToString());
                listBox5.Items.Add(textBox1.Text);
                listBox6.Items.Add(textBox2.Text);
                stepcount++;
                button3.Visible = false;
                button4.Visible = true;
            }
        }
    }
}
