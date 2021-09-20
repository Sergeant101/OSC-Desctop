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
using System.Windows.Documents;
using System.Windows.Input;

namespace OSC_New_Conсept
{
    public partial class FormAnaliticsOSC : Form
    {
 
        // Массив для распарсенной текстовой строки
        private double[] OSC_Arr_Slise = new double[20];

        // Все считанные из файла данные !!!
        private List<List<double>> OSC_Arhive = new List<List<double>>();

        // отслеживаем фокус мыши на графике осциллограмм
        private bool MutexTrakingMouse = false;

        // координаты мыши !!!
        Point MouseTrakingXY = new Point();

        // типа объект возращающий данные из архива
        Prepare_Data PrepareToArhive = new Prepare_Data();

        // работа с мышью
        Mouse GetDataMouse = null;


        public FormAnaliticsOSC()
        {
            InitializeComponent();
            timer.Tick += new EventHandler(RefreshLabel);            
            timer.Interval = 100;
            timer.Start();

            //this.OSC.MouseMove += new System.Windows.Forms.MouseEventHandler(CursorPos);
            this.OSC.MouseEnter += new System.EventHandler(this.TrackingMouseFocus);
            this.OSC.MouseLeave += new System.EventHandler(this.TrakingMouseLeave);
                        
            GetDataMouse = new Mouse(this);

            OSC_Arhive = PrepareToArhive.ReturnArhive;
            
        }


        readonly Timer timer = new Timer();

        // =========================================================================================================
        // Отслеживаем курсор мыши в поле графиков
        private void RefreshLabel(object sender, EventArgs e)
        {
            // если фокус на графике отслеживаем координаты мыши
            if (MutexTrakingMouse)
            {
                //MouseTrakingXY = CursorPos();
                textBox2.Text = "x = " + Convert.ToString(MouseTrakingXY.X);
                textBox3.Text = "y = " + Convert.ToString(MouseTrakingXY.Y);
            }
        }

        private void View_Arhive_OSC()
        {
            // начальная точка графика
            double Xmin = 0;
            // конечная точка графика
            double Xmax = OSC_Arhive.Count - 1;
            // шаг отриовки ...пока так, по факту посмотрим чё почём
            double Step = 1;
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

        // Фокус пойман на графиках
        private void TrackingMouseFocus(object sender, System.EventArgs e)
        {
            textBox1.Text = "Фокус";

            MutexTrakingMouse = true;
        }

        // Фокус ушел с графиков
        private void TrakingMouseLeave(object sender, System.EventArgs e)
        {
            textBox1.Text = "Потерян";

            MutexTrakingMouse = false;
        }
/*
        //Координаты курсора
        private void CursorPos(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            MouseTrakingXY = e.Location;
        }
*/
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
