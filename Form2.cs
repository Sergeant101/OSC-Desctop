

namespace OSC_New_Conсept
{
    using System;
    using System.Windows.Forms;
    using OxyPlot;
    using OxyPlot.Series;
    public partial class Form2 : Form
    {
        public Form2()
        {
            this.InitializeComponent();
            var myModel = new PlotModel { Title = "Example 1" };
            myModel.Series.Add(new FunctionSeries(Math.Cos, 0, 50, 0.1, "cos(x)"));
            //this.plot1.Model = myModel;

        }

        private void Form2_Load(object sender, EventArgs e)
        {
            
        }
    }
}
