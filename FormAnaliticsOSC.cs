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
            if (CurRange != PrevRange)
            {
                if (CurRange > 0)
                {
                    View_Arhive_OSC(GetDataMouse.BeginRangeX, GetDataMouse.RangeX);
                }
                
                PrevRange = CurRange;
            }

        }

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
            double Step = 10;
            // количество точек графика
            int count = (int)Math.Ceiling((Xmax - Xmin) / Step) + 1;
            // массив значений оси Х
            double[] x = new double[count];
            // массив значений оси У канал 0
            double[] ch_0 = new double[count];

            // рассчитываем точки графика
            for (int i = 0; i < count; i++)
            {
                x[i] = Xmin + Step * i;
                ch_0[i] = OSC_Arhive[i][0];
            }

            // настройка осей графика
            OSC.ChartAreas[0].AxisX.Minimum = Xmin;
            OSC.ChartAreas[0].AxisX.Maximum = Xmax;
            // определение шага сетки
            OSC.ChartAreas[0].AxisX.MajorGrid.Interval = Step;
            // Добавляем вычиленные значения в графики
            OSC.Series[0].Points.DataBindXY(x, ch_0);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // начальная точка графика
            double Xmin = 0;
            // конечная точка графика
            double Xmax = OSC_Arhive.Count - 1;
            // шаг отриовки ...пока так, по факту посмотрим чё почём
            double Step = 10;
            // количество точек графика
            int count = (int)Math.Ceiling((Xmax - Xmin) / Step) + 1;
            // массив значений оси Х
            double[] x = new double[count];
            // массив значений оси У канал 0
            double[] ch_0 = new double[count];
            // массив значений оси У канал 1
            double[] ch_1 = new double[count];
            // массив значений оси У канал 2
            double[] ch_2 = new double[count];

            // рассчитываем точки графика
            for (int i = 0; i < count; i++)
            {
                x[i] = Xmin + Step * i;

                ch_0[i] = OSC_Arhive[i][0];
                ch_1[i] = OSC_Arhive[i][1];
                ch_2[i] = OSC_Arhive[i][2];

            }

            // настройка осей графика
            OSC.ChartAreas[0].AxisX.Minimum = Xmin;
            OSC.ChartAreas[0].AxisX.Maximum = Xmax;
            // определение шага сетки
            OSC.ChartAreas[0].AxisX.MajorGrid.Interval = Step;
            // Добавляем вычиленные значения в графики
            OSC.Series[0].Points.DataBindXY(x, ch_0);
            OSC.Series[1].Points.DataBindXY(x, ch_1);
            OSC.Series[2].Points.DataBindXY(x, ch_2);
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
