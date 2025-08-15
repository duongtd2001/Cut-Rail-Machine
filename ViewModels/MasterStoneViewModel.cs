using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Caliburn.Micro;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.Annotations;

namespace CUT_RAIL_MACHINE.ViewModels
{
    public class MasterStoneViewModel : Screen
    {

        private PlotModel _plotModel = null;
        public PlotModel MyModel { get => _plotModel; set { _plotModel = value; NotifyOfPropertyChange(() => MyModel); } }

        private double _values = 20;

        private OxyColor ValueColor;

        public MasterStoneViewModel()
        {
            
        }

        public void DrawingChart()
        {
            MyModel = new PlotModel
            {
                Title = "STONE",
                TitleFontSize = 11,
                TitleColor = OxyColors.White,
                Background = OxyColors.Transparent,
                PlotAreaBackground = OxyColors.Transparent,
                PlotAreaBorderColor = OxyColors.White,
            };

            ValueColor = OxyColors.Red;

            MyModel.Series.Add(new BarSeries
            {
                StrokeColor = OxyColors.Black,
                StrokeThickness = 1,
                BarWidth = 1,
                Items = { new BarItem { Value = _values, Color = ValueColor} }
            });

            MyModel.Axes.Add(new CategoryAxis
            {

            });
        }
    }
}
