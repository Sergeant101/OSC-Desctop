using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace OSC_New_Conсept
{
    class Mouse
    {

        // точки графика
        private System.Drawing.Point MouseTrakingXY = new System.Drawing.Point();

        // 

        public Mouse(FormAnaliticsOSC _ObjTraking)
        {
            FormAnaliticsOSC ObjTraking = _ObjTraking;
            ObjTraking.OSC.MouseMove += new System.Windows.Forms.MouseEventHandler(CursorPosOSC);
            //ObjTraking.OSC.MouseEnter += new System.EventHandler(this.TrackingMouseFocus);
            //ObjTraking.OSC.MouseLeave += new System.EventHandler(this.TrakingMouseLeave);
        }

        // Координаты мыши на графике функций - Х
        public int GetMouseX_OSC
        {
            get
            {
                return MouseTrakingXY.X;
            }
        }

        // Координаты мыши на графике функций - У
        public int GetMouseY_OSC
        {
            get
            {
                return MouseTrakingXY.Y;
            }
        }


        private void StartTraking( object sender, System.Windows.Forms.MouseEventArgs e)
        {

        }

        // Получаем координаты мыши в окне графиков
        private void CursorPosOSC(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            MouseTrakingXY = e.Location;
        }



    }
}
