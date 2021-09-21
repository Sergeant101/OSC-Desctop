using System;
using System.Windows.Input;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace OSC_New_Conсept
{
    class MouseState
    {

        // точки графика
        private System.Drawing.Point MouseTrakingXY = new System.Drawing.Point();

        // фокус мыши на объекте
        private bool MouseFocus = false;

        // Фиксируем нажатие/отпускание левой клавиши мыши
        private bool MouseLeftKeyDown = false;

        // Координаты при нажатии на левую кнопку мыши, при отпускании и длина вектора масштабирования графиков
        private int RangeX1, RangeY1, RangeX2, RangeY2, RangeScale;

        public MouseState(FormAnaliticsOSC _ObjTraking)
        {
            FormAnaliticsOSC ObjTraking = _ObjTraking;
            ObjTraking.OSC.MouseMove    += new System.Windows.Forms.MouseEventHandler(CursorPos);
            ObjTraking.OSC.MouseEnter   += new System.EventHandler(this.TrackingMouseFocus);
            ObjTraking.OSC.MouseLeave   += new System.EventHandler(this.TrackingMouseLeave);
            ObjTraking.OSC.MouseDown    += new System.Windows.Forms.MouseEventHandler(this.IfLeftDown);
            ObjTraking.OSC.MouseUp      += new System.Windows.Forms.MouseEventHandler(this.IfLeftUp);
        }

        // Координаты мыши на графике - Х
        public int GetMouseX
        {
            get
            {
                if (MouseFocus)
                {
                    return MouseTrakingXY.X;
                }           
                return 0;
            }
        }

        // Координаты мыши на графике - У
        public int GetMouseY
        {
            get
            {
                if (MouseFocus)
                {
                    return MouseTrakingXY.Y;
                }
                return 0;
            }
        }

        // Фокус мыши на объекте
        public bool GetMouseFocus
        {
            get
            {
                return MouseFocus;
            }
        }

        // Возвращаем значение для масштабирования оси Х
        public int RangeX
        {
            get
            {
                // значение больше нуля - увеличиваем масштаб графиков в выбранной области
                // значение ноль - не производим никаких действий
                // значение минус один - возвращаем масштаб по умолчанию
                return RangeScale;
            }
        }

        public int BeginRangeX
        {
            get
            {
                // 35 - поправка на смещение окна отображения графиков от окна chart
                return RangeX1 - 35;
            }
        }

        // для тестирования потом удалить
        public bool Test
        {
            get
            {
                return MouseLeftKeyDown;
            }
        }

        // Получаем координаты мыши в окне графиков
        private void CursorPos(object sender, System.Windows.Forms.MouseEventArgs e) => MouseTrakingXY = e.Location;

        // фокус мыши на объекте
        private void TrackingMouseFocus(object sender, EventArgs e) => MouseFocus = true;

        // фокус ушел с объекта
        private void TrackingMouseLeave(object sender, EventArgs e) => MouseFocus = false;

        // Ловится нажатие левой клавиши мыши
        private void IfLeftDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                MouseLeftKeyDown = true;
                RangeX1 = MouseTrakingXY.X;
                RangeY1 = MouseTrakingXY.Y;
            }
        }

        // Ловится отпускание левой клавиши мыши
        private void IfLeftUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                MouseLeftKeyDown = false;

                RangeX2 = MouseTrakingXY.X;
                RangeY2 = MouseTrakingXY.Y;

                // По отпускании левой клавиши мыши смотрим какое действие произвести
                // В прочих не указанных случаях возвращаем значение ноль, т.е. не выполнять никаких действий
                RangeScale = 0;
                if (RangeY1 < RangeY2)
                {
                    // Мышь переместилась вправо вниз - передаём данные для масштабирования графиков функции
                    if(RangeX1 < RangeX2)
                    {
                        RangeScale = RangeX2 - RangeX1;
                    }
                    // Мышь переместилась влево вниз - возвращаем масштаб по умолчанию графиков функции
                    else if(RangeX1 > RangeX2)
                    {
                        RangeScale = -1;
                    }
                }


            }
        }

    }
}
