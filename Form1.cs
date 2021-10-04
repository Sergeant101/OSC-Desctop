using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace OSC_New_Conсept
{
    public partial class Form1 : Form
    {
        ReadWriteDataPLC OSC_From_PLC = new ReadWriteDataPLC();
        FormAnaliticsOSC WindowsAnalitics = null;
        Form2 WindowsAnaliticsOxyPlot = null;

        // Данные для отображения каналов
        double[] AxisY_Sh0 = new double[200];
        double[] AxisY_Sh1 = new double[200];
        double[] AxisY_Sh2 = new double[200];
        double[] AxisY_Sh3 = new double[200];

        // предыдущее значение готового для чтения массива данных
        int ReadyArrayPrev;

        // текущее значение готового для чтения массива данных
        int ReadyArrayCurr;

        // Путь к файлу для записи
        string Path = @"C:\OSC.csv";

// =========================================================================================================
        public Form1()
        {
            InitializeComponent();
            timer.Tick += new EventHandler(RefreshLabel);
            timer.Interval = 100;
            timer.Start();
        }

        readonly Timer timer = new Timer();

        private void Form1_Load(object sender, EventArgs e)
        {

        }

// =========================================================================================================
// Цикл обновления данных

        private void RefreshLabel(object sender, EventArgs e)
        {

// =========================================================================================================            
// индикатор соединения

            ConnectPLC.BackColor = Color.Orchid;
            if (OSC_From_PLC.ConnectOK)
            {
                ConnectPLC.BackColor = Color.DeepSkyBlue;
            }

// =========================================================================================================            
// индикатор включенного в ПЛК осциллографирования

            OSC_Run.BackColor = Color.Orchid;
            if (OSC_From_PLC.GetTagInt(505, 0) == 1)
            {
                OSC_Run.BackColor = Color.DeepSkyBlue;
            }
            if (!OSC_From_PLC.ConnectOK)
            {
                OSC_Run.BackColor = Color.Gray;
            }

// =========================================================================================================
// делаем графики

            double Xmin = 0;
            double Xmax = 199;
            double Step = 1;
            double Count = 200;
            double[] AxisX = new double[200];

            // подготовка значений сетки оси Х
            for (int i = 0; i < Count; i++)
            {
                AxisX[i] = Xmin + Step * i;
            }

            // здесь читаются значения текущего готового массива, и если готов новый - обрабатываем его
            ReadyArrayCurr = OSC_From_PLC.GetTagInt( 505, 2);
            if (ReadyArrayCurr != ReadyArrayPrev)
            {


                // для отладки, можно удалить потом
                richTextBox1.Clear();

                // получаем данные из ПЛК
                short[] OSC_Arr_Sh0 = OSC_From_PLC.Read_OSC_Array(505, 56 + 100 * (ReadyArrayCurr - 1));
                short[] OSC_Arr_Sh1 = OSC_From_PLC.Read_OSC_Array(505, 456 + 100 * (ReadyArrayCurr - 1));
                short[] OSC_Arr_Sh2 = OSC_From_PLC.Read_OSC_Array(505, 856 + 100 * (ReadyArrayCurr - 1));
                short[] OSC_Arr_Sh3 = OSC_From_PLC.Read_OSC_Array(505, 1256 + 100 * (ReadyArrayCurr - 1));

                //сдвигаем исходный массив влево для добавления считанного куска данных
                //для нормального движения графика при приращении данных
                for (int i = 0; i < 50; i++)
                {
                    // здесь просто заполняем текстовое окно значениями
                    richTextBox1.Text += Convert.ToString(OSC_Arr_Sh1[i]) + "\r\n";

                    // добавляем новые данные в массив визуализации осциллограмм
                    Shift_Arr(AxisY_Sh0, OSC_Arr_Sh0, i);
                    Shift_Arr(AxisY_Sh1, OSC_Arr_Sh1, i);
                    Shift_Arr(AxisY_Sh2, OSC_Arr_Sh2, i);
                    Shift_Arr(AxisY_Sh3, OSC_Arr_Sh3, i);
                   
                }

                chart1.ChartAreas[0].AxisX.Minimum = Xmin;
                chart1.ChartAreas[0].AxisX.Maximum = Xmax;
                chart1.ChartAreas[0].AxisX.MajorGrid.LineWidth = 1;
                chart1.Series[0].Points.DataBindXY(AxisX, AxisY_Sh0);
                chart1.Series[1].Points.DataBindXY(AxisX, AxisY_Sh1);
                chart1.Series[2].Points.DataBindXY(AxisX, AxisY_Sh2);
                chart1.Series[3].Points.DataBindXY(AxisX, AxisY_Sh3);

                // =========================================================================================
                // Пишем данные в файл
                // Шаг 1: Проверяем наличие файла для записи
                try
                {
                    // Проверяем существует ли файл для записи попыткой считать из него данные
                    using (StreamReader sr = new StreamReader(Path))
                    {
                        int test_file = sr.Peek();
                    }
                }
                catch
                {
                    // Если при чтении возникло исключение создаём новый файл с пустой строкой       
                    // Если открыть при записи тоже исключение
                    try
                    {
                        using (StreamWriter sw = new StreamWriter(Path, false))
                        {
                            sw.WriteLine(" test ; Sh_1 ; Sh_2 ");
                        }
                    }
                    finally
                    {

                    }
                    
                }

                // =========================================================================================
                // Добавляем в файл данные осциллографирования
                using (StreamWriter sw = new StreamWriter(Path, true))
                {
                    // Создаём строку для записи
                    // Потом конечно же создадим по-нормальному, сейчас я читаю только один канал
                    
                    for (int i = 0; i < 50; i++)
                    {
                        string LineOSC = "";
                        LineOSC = OSC_Arr_Sh0[i] + ";" + OSC_Arr_Sh1[i] + ";" + OSC_Arr_Sh2[i] + ";" + OSC_Arr_Sh3[i] + ";" + "0" + ";" + "0" 
                            + ";" + "0" + ";" + "0" + ";" + "0" + ";" + "0" + ";" + "0" + ";" + "0" + ";"
                            + "0" + ";" + "0" + ";" + "0" + ";" + "0" + ";" + "0" + ";" + "0" + ";" + "0"
                            + ";" + "0" + ";";
                        sw.WriteLine(LineOSC);
                    }
                }
                

                // сохраняем выполненный шаг чтобы не дублировать
                // его выполнение до возникновения следующего шага
                ReadyArrayPrev = ReadyArrayCurr;
            }

        }

// =========================================================================================================
// установка/разрыв соединения с ПЛК
        private void ConnectPLC_Click(object sender, EventArgs e)
        {
            if (!OSC_From_PLC.ConnectOK)
            {
                OSC_From_PLC.ConnectPLC();
            }
            else
            {
                OSC_From_PLC.Disconnect();
            }

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        // можно будет удалить
        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

// =========================================================================================================
// Включение/выключение режима осциллографирования на ПЛК
        private void OSC_Run_Click(object sender, EventArgs e)
        {
            // =========================================================================================================
            // выполняется если есть соединение с ПЛК
            if (OSC_From_PLC.ConnectOK)
            {
                if (OSC_From_PLC.GetTagInt(505, 0) == 0)
                {
                    OSC_From_PLC.SetTagInt(505, 0, 1);
                }
                else
                {
                    OSC_From_PLC.SetTagInt(505, 0, 0);
                }
            }
        }

// =========================================================================================================
// Чисто рутина по подготовке массива представления данных осциллограмм
        private void Shift_Arr(double[] _Sourcce, short[] _Add, int _index)
        {

            _Sourcce[_index] = _Sourcce[_index + 50];
            _Sourcce[_index + 50] = _Sourcce[_index + 100];
            _Sourcce[_index + 100] = _Sourcce[_index + 150];
            _Sourcce[_index + 150] = _Add[_index];
            
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            //if(WindowsAnalitics == null)
            //{
                WindowsAnalitics = new FormAnaliticsOSC();
            //}
            WindowsAnalitics.Show();
        }

        private void Channel_0_Click(object sender, EventArgs e)
        {

        }

        private void button21_Click(object sender, EventArgs e)
        {
            WindowsAnaliticsOxyPlot = new Form2();
            WindowsAnaliticsOxyPlot.Show();
        }
    }
}
