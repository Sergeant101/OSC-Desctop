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

namespace OSC_New_Conсept
{
    public partial class Form_OSC_Analitics : Form
    {
        // Путь к файлу с данными
        string Path = @"C:\OSC.csv";
        // Массив для распарсенной текстовой строки
        double[] OSC_Arr_Slise = new double[20]; 
        // Все считанные из файла данные
        List<List<double>> OSC_Arhive = new List<List<double>>();

        public Form_OSC_Analitics()
        {
            InitializeComponent();
            timer.Tick += new EventHandler(RefreshLabel);
            timer.Interval = 100;
            timer.Start();

// =========================================================================================================
// Считываем данные из файла и формируем динамический список из данных
            
            // !!! всё равно выбивает, если файл открыть другим приложением, даже в обёртке !!!
            try
            {
                using(StreamReader sr = new StreamReader(Path))
                {
                    string line;
                    int i = 0;

                    // убираем первую строку заголовка
                    sr.ReadLine();

                    // тестируем
                                       
                    //foreach ( double elem in OSC_Arr_Slise)
                    //{
                        //richTextBox1.Text += Convert.ToString(elem) + "\r\n";
                    //}
                    // конец теста

                    while ((line = sr.ReadLine()) != null)
                    {
                        OSC_Arhive.Add(new List<double>());  
                        Parse_20_Elem(line, OSC_Arr_Slise);

                        foreach( double elem in OSC_Arr_Slise)
                        {
                            OSC_Arhive[i].Add( elem );                            
                        }

                        i++;
                    }
                }
            }
            finally
            {

            }
        }

        readonly Timer timer = new Timer();

        // =========================================================================================================
        // Обработка графиков из архива осциллографа

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
            chart1.ChartAreas[0].AxisX.Minimum = Xmin;
            chart1.ChartAreas[0].AxisX.Maximum = Xmax;
            // определение шага сетки
            chart1.ChartAreas[0].AxisX.MajorGrid.Interval = Step;
            // Добавляем вычиленные значения в графики
            chart1.Series[0].Points.DataBindXY(x, ch_0);
        }

// =========================================================================================================
// Парсим считанную строку на двадцать элементов
        private void Parse_20_Elem(string _Source, double[] _arr)
        {
            int _i = 0;

            // Это очень смешной костылик: при чтении из файла крайний в строке делимитер не читается
            // посему последнее число в строке не парсится, поэтому добавляем "свой" конечный делимитер
            // если он отсутствует
            if ( _Source.LastIndexOf(";") != (_Source.Length - 1)){
                _Source += ";";
            }
                        
            do
            {

                int AddrDelimiter = _Source.IndexOf(";");

                if (AddrDelimiter == -1 || _i >= 20)
                {                    
                    break;
                }

                // забираем число
                _arr[_i] = Convert.ToDouble(_Source.Substring(0, AddrDelimiter));

                // обрезаем по делимитер
                _Source = _Source.Remove(0, AddrDelimiter + 1);

                _i++;

            } while (true);

        }

        private void RefreshLabel(object sender, EventArgs e)
        {

        }

        private void OSC_Analitics_Load(object sender, EventArgs e)
        {
            
        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
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

                /*
                int j = i * 5;
                if (j < count/5)
                {
                    ch_0[j] = OSC_Arhive[j][0];
                    ch_0[j + 1] = OSC_Arhive[j + 1][0];
                    ch_0[j + 2] = OSC_Arhive[j + 2][0];
                    ch_0[j + 3] = OSC_Arhive[j + 3][0];
                    ch_0[j + 4] = OSC_Arhive[j + 4][0];
                }
             */
            }

            // настройка осей графика
            chart1.ChartAreas[0].AxisX.Minimum = Xmin;
            chart1.ChartAreas[0].AxisX.Maximum = Xmax;
            // определение шага сетки
            chart1.ChartAreas[0].AxisX.MajorGrid.Interval = Step;
            // Добавляем вычиленные значения в графики
            chart1.Series[0].Points.DataBindXY(x, ch_0);
            chart1.Series[1].Points.DataBindXY(x, ch_1);
            chart1.Series[2].Points.DataBindXY(x, ch_2);
        }

        private void button21_Click(object sender, EventArgs e)
        {

        }
    }
}
