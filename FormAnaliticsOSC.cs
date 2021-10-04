using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace OSC_New_Conсept
{
    public partial class FormAnaliticsOSC : Form
    {

        // все считанные из файла данные 
        private List<List<double>> OSC_Arhive = new List<List<double>>();

        // объект возращающий данные из архива
        Prepare_Data PrepareFromArhive = new Prepare_Data();

        // работа с мышью
        MouseState GetDataMouse = null;

        readonly Timer timer = new Timer();

        // текущее/предыдущее значение для масштрабирования графика функции
        int CurRange, PrevRange;

        // массив значений оси Х
        private double[] x = null;
        // массив значений оси У канал 0
        private double[] ch_0 = null;
        private double[] ch_1 = null;
        private double[] ch_2 = null;
        private double[] ch_3 = null;
        private double[] ch_4 = null;
        private double[] ch_5 = null;
        private double[] ch_6 = null;
        private double[] ch_7 = null;
        private double[] ch_8 = null;
        private double[] ch_9 = null;
        private double[] ch_10 = null;
        private double[] ch_11 = null;
        private double[] ch_12 = null;
        private double[] ch_13 = null;
        private double[] ch_14 = null;
        private double[] ch_15 = null;
        private double[] ch_16 = null;
        private double[] ch_17 = null;
        private double[] ch_18 = null;
        private double[] ch_19 = null;


        public FormAnaliticsOSC()
        {
            InitializeComponent();
            timer.Tick += new EventHandler(RefreshLabel);            
            timer.Interval = 42;
            timer.Start();
                        
            GetDataMouse = new MouseState(this);

            OSC_Arhive = PrepareFromArhive.ReturnArhive;

        }

        
        // =========================================================================================================
        // Рабочая область
        private void RefreshLabel(object sender, EventArgs e)
        {

            textBox2.Text = "x = " + Convert.ToString(GetDataMouse.GetMouseX);
            textBox3.Text = "y = " + Convert.ToString(GetDataMouse.GetMouseY);

            // Отслеживаем чё там за команду подали мышью через выделение области на графиках
            CurRange = GetDataMouse.RangeX;
            textBox1.Text = Convert.ToString(CurRange);
            if (CurRange != PrevRange)
            {
                if (CurRange > 0)
                {
                    View_Arhive_OSC(GetDataMouse.BeginRangeX, GetDataMouse.RangeX);
                }
                else if (CurRange == -1)
                {
                    View_OSC_Source();
                }
                
                PrevRange = CurRange;
            }

        }

        // Увеличиваем масштаб в выбраном мышью диапазоне
        private void View_Arhive_OSC(int _BeginX, int _Range)
        {
            // начальная точка графика
            double Xmin = Math.Round(_BeginX * ((OSC.ChartAreas[0].AxisX.Maximum - OSC.ChartAreas[0].AxisX.Minimum) / 1000) + OSC.ChartAreas[0].AxisX.Minimum);
            
            // конечная точка графика: OSC_Arhive.Count - 1 - конечная точка Х считанного из файла архива
            // проверяем чтобы при масштабировании графика правая граница не вылезла за крайний предел графиков 
            double Xmax = Xmin + Math.Round(_Range*((OSC.ChartAreas[0].AxisX.Maximum - OSC.ChartAreas[0].AxisX.Minimum) / 1000));
            if (Xmax > OSC_Arhive.Count - 1)
            {
                Xmax = OSC_Arhive.Count - 1;
            }
                        
            // шаг отриовки ...пока так, по факту посмотрим чё почём
            int Step = 2;
            // количество точек графика
            int count = (int)Math.Ceiling((Xmax - Xmin) / Step) + 1;
            // массив значений оси Х
            x = new double[count];
            
            // массив значений оси У канал 0
            ch_0 = new double[count];
            ch_1 = new double[count];
            ch_2 = new double[count];
            ch_3 = new double[count];
            ch_4 = new double[count];
            ch_5 = new double[count];
            ch_6 = new double[count];
            ch_7 = new double[count];
            ch_8 = new double[count];
            ch_9 = new double[count];
            ch_10 = new double[count];
            ch_11 = new double[count];
            ch_12 = new double[count];
            ch_13 = new double[count];
            ch_14 = new double[count];
            ch_15 = new double[count];
            ch_16 = new double[count];
            ch_17 = new double[count];
            ch_18 = new double[count];
            ch_19 = new double[count];

            // пересчитываем точки графика
            PointGraph(Xmin, Step, count);

            //ShowGraph(Xmin, Xmax, Step);
            for (int i = 0; i < count; i++)
            {
                x[i] = Xmin + Step * i;
            }
 
                // настройка осей графика
            OSC.ChartAreas[0].AxisX.Minimum = Xmin;
            OSC.ChartAreas[0].AxisX.Maximum = Xmax;
            // определение шага сетки
            OSC.ChartAreas[0].AxisX.MajorGrid.Interval = Step;
            // Добавляем вычиленные значения в графики
            OSC.Series[0].Points.DataBindXY(x, ch_0);

        }

        // Возвращаем отображение графиков в исходном масштабе
        private void View_OSC_Source()
        {
            // начальная точка графика
            double Xmin = 0;
            // конечная точка графика
            double Xmax = OSC_Arhive.Count - 1;
            // шаг отриовки ...пока так, по факту посмотрим чё почём
            int Step = 1;
            // количество точек графика
            int count = (int)Math.Ceiling((Xmax - Xmin) / Step) + 1;
            // массив значений оси Х
            x = new double[count];

            // массив значений оси У канал 0
            ch_0 = new double[count];
            ch_1 = new double[count];
            ch_2 = new double[count];
            ch_3 = new double[count];
            ch_4 = new double[count];
            ch_5 = new double[count];
            ch_6 = new double[count];
            ch_7 = new double[count];
            ch_8 = new double[count];
            ch_9 = new double[count];
            ch_10 = new double[count];
            ch_11 = new double[count];
            ch_12 = new double[count];
            ch_13 = new double[count];
            ch_14 = new double[count];
            ch_15 = new double[count];
            ch_16 = new double[count];
            ch_17 = new double[count];
            ch_18 = new double[count];
            ch_19 = new double[count];


            // пересчитываем точки графика
            PointGraph(Xmin, Step, count);
            // отрисовываем
            ShowGraph(Xmin, Xmax, Step);
        }

        // Уменьшаем масштаб в два раза
        private void View_Double_OSC()
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            View_OSC_Source();

            /*
            // начальная точка графика
            double Xmin = 0;
            // конечная точка графика
            double Xmax = OSC_Arhive.Count - 1;
            // шаг отриовки ...пока так, по факту посмотрим чё почём
            int Step = 1;
            // количество точек графика
            int count = (int)Math.Ceiling((Xmax - Xmin) / Step) + 1;
            // массив значений оси Х
            x = new double[count];

            // массив значений оси У канал 0
            ch_0 = new double[count];
            ch_1 = new double[count];
            ch_2 = new double[count];
            ch_3 = new double[count];
            ch_4 = new double[count];
            ch_5 = new double[count];
            ch_6 = new double[count];
            ch_7 = new double[count];
            ch_8 = new double[count];
            ch_9 = new double[count];
            ch_10 = new double[count];
            ch_11 = new double[count];
            ch_12 = new double[count];
            ch_13 = new double[count];
            ch_14 = new double[count];
            ch_15 = new double[count];
            ch_16 = new double[count];
            ch_17 = new double[count];
            ch_18 = new double[count];
            ch_19 = new double[count];


            // пересчитываем точки графика
            PointGraph(Xmin, Step, count);
            // отрисовываем
            ShowGraph(Xmin, Xmax, Step);
            */
        }


        // Рассчёт точек графика
        private void PointGraph(double _Xmin, int _Step, int _count)
        {
            for (int i = 0; i < _count-1; i++)
            {
                x[i] = _Xmin + _Step * i;
                
                ch_0[i] = OSC_Arhive[i][0];
                ch_1[i] = OSC_Arhive[i][1];
                ch_2[i] = OSC_Arhive[i][2];
                ch_3[i] = OSC_Arhive[i][3];
                ch_4[i] = OSC_Arhive[i][4];
                ch_5[i] = OSC_Arhive[i][5];
                ch_6[i] = OSC_Arhive[i][6];
                ch_7[i] = OSC_Arhive[i][7];
                ch_8[i] = OSC_Arhive[i][8];
                ch_9[i] = OSC_Arhive[i][9];
                ch_10[i] = OSC_Arhive[i][10];
                ch_11[i] = OSC_Arhive[i][11];
                ch_12[i] = OSC_Arhive[i][12];
                ch_13[i] = OSC_Arhive[i][13];
                ch_14[i] = OSC_Arhive[i][14];
                ch_15[i] = OSC_Arhive[i][15];
                ch_16[i] = OSC_Arhive[i][16];
                ch_17[i] = OSC_Arhive[i][17];
                ch_18[i] = OSC_Arhive[i][18];
                ch_19[i] = OSC_Arhive[i][19];                
            }
        }

        // показываем график
        private void ShowGraph(double _Xmin, double _Xmax, double _Step)
        {
            // настройка осей графика
            OSC.ChartAreas[0].AxisX.Minimum = _Xmin;
            OSC.ChartAreas[0].AxisX.Maximum = _Xmax;
            // определение шага сетки
            OSC.ChartAreas[0].AxisX.MajorGrid.Interval = _Step;
            // Добавляем вычиленные значения в графики
            OSC.Series[0].Points.DataBindXY(x, ch_0);
            OSC.Series[1].Points.DataBindXY(x, ch_1);
            OSC.Series[2].Points.DataBindXY(x, ch_2);
            OSC.Series[3].Points.DataBindXY(x, ch_3);
            OSC.Series[4].Points.DataBindXY(x, ch_4);
            OSC.Series[5].Points.DataBindXY(x, ch_5);
            OSC.Series[6].Points.DataBindXY(x, ch_6);
            OSC.Series[7].Points.DataBindXY(x, ch_7);
            OSC.Series[8].Points.DataBindXY(x, ch_8);
            OSC.Series[9].Points.DataBindXY(x, ch_9);
            OSC.Series[10].Points.DataBindXY(x, ch_10);
            OSC.Series[11].Points.DataBindXY(x, ch_11);
            OSC.Series[12].Points.DataBindXY(x, ch_12);
            OSC.Series[13].Points.DataBindXY(x, ch_13);
            OSC.Series[14].Points.DataBindXY(x, ch_14);
            OSC.Series[15].Points.DataBindXY(x, ch_15);
            OSC.Series[16].Points.DataBindXY(x, ch_16);
            OSC.Series[17].Points.DataBindXY(x, ch_17);
            OSC.Series[18].Points.DataBindXY(x, ch_18);
            OSC.Series[19].Points.DataBindXY(x, ch_19);
        }

        private void OSC_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }
    }


}
