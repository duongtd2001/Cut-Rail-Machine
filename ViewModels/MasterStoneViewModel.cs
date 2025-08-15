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
using System.Threading;

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
            DrawingChart();
            Thread thread = new Thread(() =>
            {
                while (true)
                {
                    Thread.Sleep(50);
                    if(_values < 60)
                    {
                        _values++;
                        if(_values == 60)
                        {
                            _values = 0;
                        }
                    }
                }
            });
            thread.IsBackground = true;
            thread.Start();
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

            var barSeries = new BarSeries
            {
                StrokeColor = OxyColors.Transparent,
                BarWidth = 1,
                
            };
            barSeries.Items.Add(new BarItem { Value = _values, Color = ValueColor });

            MyModel.Axes.Add(new CategoryAxis
            {
                Position = AxisPosition.Left,
                ItemsSource = new[] { "VALUE" },
                IsPanEnabled = false,
                IsZoomEnabled = false,
                TextColor = OxyColors.White,
                FontSize = 11,
                FontWeight = FontWeights.Bold,

            });

            MyModel.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Bottom,
                Minimum = 0,
                Maximum = 60,
                MajorStep = 10,
                TicklineColor = OxyColors.White,
                AxislineColor = OxyColors.White,
                TitleColor = OxyColors.White,
                TextColor = OxyColors.White,
                IsPanEnabled = false,
                IsZoomEnabled = false,
                LabelFormatter = value =>
                {
                    switch ((int)value)
                    {
                        case 0: return "START";
                        case 10: return "LX15";
                        case 20: return "LX20";
                        case 30: return "LX26";
                        case 40: return "LX30";
                        case 50: return "LX45";
                        case 60: return "END";
                        default: return "";
                    }
                }
            });

            MyModel.Series.Add(barSeries);
        }
    }
}
