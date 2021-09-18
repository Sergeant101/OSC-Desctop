using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;



namespace OSC_New_Conсept
{
    class Prepare_Data
    {

        // Путь к файлу с данными 
        private string PathData = @"C:\OSC.csv";

        // Все считанные из файла данные
        private List<List<double>> OSC_Arhive = new List<List<double>>();


        public Prepare_Data()
        {
            PathData = @"C:\OSC.csv";

            Prepare(PathData, OSC_Arhive);

        }


        public Prepare_Data(string _Path)
        {
            PathData = _Path;

            Prepare(PathData, OSC_Arhive);

        }


 // =========================================================================================================
// Считываем данные из файла и формируем динамический список из данных
        private void Prepare(string _Path, List<List<double>> _OSC_Arhive)
        {
            // Параметры:   _Path - путь до папки с данными
            //              _OSC_Arhive - все считанные данные

            // !!! всё равно выбивает, если файл открыть другим приложением, даже в обёртке !!!
            try
            {
                using (StreamReader sr = new StreamReader(_Path))
                {
                    string line;
                    int i = 0;
                    double[] OSC_Arr_Slise = new double[20];

                    // убираем первую строку заголовка
                    sr.ReadLine();

                    while ((line = sr.ReadLine()) != null)
                    {
                        _OSC_Arhive.Add(new List<double>());
                        this.Parse_20_Elem(line,
                                           OSC_Arr_Slise);

                        foreach (double elem in OSC_Arr_Slise)
                        {
                            _OSC_Arhive[i].Add(elem);
                        }

                        i++;
                    }
                }
            }
            finally
            {

            }
        }


        public List<List<double>> ReturnArhive
        {
            get
            {
                return OSC_Arhive;
            }
        }

        // =========================================================================================================
        // Парсим считанную строку на двадцать элементов 
        private void Parse_20_Elem(string _Source, double[] _arr)
        {
            int _i = 0;

            // Это очень смешной костылик: при чтении из файла крайний в строке делимитер не читается
            // посему последнее число в строке не парсится, поэтому добавляем "свой" конечный делимитер
            // если он отсутствует
            if (_Source.LastIndexOf(";") != (_Source.Length - 1))
            {
                _Source += ";";
            }

            do
            {

                int AddrDelimiter = _Source.IndexOf(";");

                // если делимитеры кончились или элементов больше двадцати - стоп!
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



    }        
}
