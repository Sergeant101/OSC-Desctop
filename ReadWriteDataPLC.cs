using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using S7.Net;

namespace OSC_New_Conсept
{

   class ReadWriteDataPLC
    {

        public enum TypeDataRW
        {
            PLC_bit = 1,
            PLC_byte,
            PLC_int,
            PLC_real
        }

        static Plc Swap_Data_PLC = null;
              

        // подключение ПЛК
        public void ConnectPLC()
        {
            if (Swap_Data_PLC == null)
            {
                Swap_Data_PLC = new Plc(CpuType.S71500, "192.168.100.101", 0, 1);
                try
                {
                    Swap_Data_PLC.Open();
                }
                catch
                {

                }
            }
            else
            {
                Swap_Data_PLC.Close();
                if (!Swap_Data_PLC.IsConnected)
                {
                    Swap_Data_PLC.OpenAsync();
                }
                else
                {
                    ConnectPLC();
                }
            }
        }

        // Отключение от контроллера
        public void Disconnect()
        {
            if (Swap_Data_PLC != null)
            {
                Swap_Data_PLC.Close();
                //Вот это чё-то как-то так себе, надо бы сделать освобождение ресурсов
                Swap_Data_PLC = null;
            }
        }

        // Проверка соединения
        public bool ConnectOK
        {
            get
            {
                // соединение ранее установливалось - экземпляр подключения существует
                if (Swap_Data_PLC != null)
                {

                    if (Swap_Data_PLC.IsConnected)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
                else
                {
                    return false;
                }
   
            }
        }

// =========================================================================================================
        // Чтение одиночных данных из памяти ПЛК
        public short GetTagInt(int _db, int _dbOffset)
        {
            var DB_Num = Convert.ToString(_db);
            var DB_Offset = Convert.ToString(_dbOffset);

            if (this.ConnectOK)
            {
                
                // читаем целое число (ПЛК - 2 байта) со знаком 
                try
                    {
                        return ((ushort)Swap_Data_PLC.Read("DB" + DB_Num + "." + "DBW" + DB_Offset)).ConvertToShort();
                    }
                    catch
                    {
                        return 0;
                    }
            }

            else
                {
                    return 0;
                }
                       
        }

// =========================================================================================================
        //  Вертаем массив канала быстрого осциллографа из Контроллера
        public short[] Read_OSC_Array(int _db, int _start_Addr)
        {

            short[] OSC_Sh = new short[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            if (this.ConnectOK)
            {
                try
                {
                    var DbBytes = Swap_Data_PLC.ReadBytes(DataType.DataBlock, _db, _start_Addr, 100);

                    for (int i = 0; i < 50; i++)
                    {
                        OSC_Sh[i] = S7.Net.Types.Int.FromByteArray(DbBytes.Skip(2 * i).Take(2).ToArray());
                    }
                }
                catch
                {
                    return OSC_Sh;
                }
                
            }
            
            return OSC_Sh;
            
        }

// =========================================================================================================
// запись в ПЛК одиночного тэга типа Int (ПЛК)
        public bool SetTagInt(int _db, int _dbOffset, short value)
        {
            var DB_Num = Convert.ToString(_db);
            var DB_Offset = Convert.ToString(_dbOffset);

            if (this.ConnectOK)
            {
                try
                {
                    Swap_Data_PLC.Write("DB" + DB_Num + ".DBW" + DB_Offset, value.ConvertToUshort());
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
            
        }

    }

}
