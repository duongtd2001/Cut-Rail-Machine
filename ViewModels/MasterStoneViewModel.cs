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
using CUT_RAIL_MACHINE.Services;

namespace CUT_RAIL_MACHINE.ViewModels
{
    public class MasterStoneViewModel : Screen
    {

        private enum _mMasterStone
        {
            NONE,
            FULLSIZE,
            LX30,
            LX26,
            LX20,
            LX15,
            REPLACE
        }

        private _mMasterStone mMasterStone = _mMasterStone.NONE;

        private PlotModel _plotModel = null;
        public PlotModel MyModel { get => _plotModel; set { _plotModel = value; NotifyOfPropertyChange(() => MyModel); } }

        private BarSeries barSeries;

        //private double _values = 1200000;
        private double dVal = 0;
        private double dSaveVal = 0;

        private Thread mMainThread;
        private bool mThreadFlag = false;

        private OxyColor ValueColor;
        private MasterStone masterStone;
        private Process_Inspection ProcessIns;
        private ModbusTCP modbusTCP;
        private EmployeesChildViewModel employeesChildViewModel;

        public MasterStoneViewModel(ref MasterStone master, Process_Inspection process, ref ModbusTCP modbus, ref EmployeesChildViewModel employeesChild)
        {
            masterStone = master;
            ProcessIns = process;
            modbusTCP = modbus;
            employeesChildViewModel = employeesChild;
            DrawingChart();
            
            FirstLoad();

            ThreadRun();
        }

        public void ThreadRun()
        {
            if (mThreadFlag) return;

            if (mMainThread != null)
            {
                mMainThread.Join(500);
                mMainThread.Abort();
                mMainThread = null;
            }
            mMainThread = new Thread(() =>
            {
                DoRunProcess();
            });
            //mMainThread.SetApartmentState(ApartmentState.STA);
            //mMainThread.Priority = ThreadPriority.Highest;
            mMainThread.IsBackground = true;
            mThreadFlag = true;

            mMainThread.Start();
        }

        public void ThreadStop()
        {
            mThreadFlag = false;

            if (mMainThread != null)
            {
                mMainThread.Join(500);
                mMainThread.Abort();
                mMainThread = null;
            }
        }

        private void DoRunProcess()
        {
            while (mThreadFlag)
            {
                GC.Collect();
                Thread.Sleep(20);
                try
                {
                    UpdatePlot();
                    UpdateDataStone();
                    WriteDataPLC();
                }
                catch (Exception)
                {
                }
            }
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

            barSeries = new BarSeries
            {
                StrokeColor = OxyColors.Transparent,
                BarWidth = 1,
                
            };
            barSeries.Items.Add(new BarItem { Value = masterStone.msStone["CURRENT"], Color = ValueColor });

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

            //MaximumVal = ;

            MyModel.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Bottom,
                Minimum = 0,
                Maximum = masterStone.msStone["REPLACE"],
                MajorStep = 100000,
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
                        case 0: return "REPLACE";
                        case 200000: return "LX15";
                        case 400000: return "LX20";
                        case 600000: return "LX26";
                        case 800000: return "LX30";
                        case 1000000: return "LX45";
                        case 1200000: return "FULL";
                        default: return "";
                    }
                }
            });

            MyModel.Series.Add(barSeries);
        }

        public void UpdatePlot()
        {
            OxyColor GetColorByValue(double value)
            {
                if (value >= masterStone.msStone["REPLACE"]) return OxyColor.Parse("#90EE90"); // 
                if (value >= masterStone.msStone["LX15"]) return OxyColor.Parse("#90EE90");    // 90EE90
                if (value >= masterStone.msStone["LX20"]) return OxyColor.Parse("#ADFF2F");    // ADFF2F
                if (value >= masterStone.msStone["LX26"]) return OxyColor.Parse("#FFFF00");    // FFFF00
                if (value >= masterStone.msStone["LX30"]) return OxyColor.Parse("#FFA500");    // FFA500
                if (value >= masterStone.msStone["LX45"]) return OxyColor.Parse("#FF0000");    // FFA500
                return OxyColor.Parse("#8B0000"); //FF0000
            }

            barSeries.Items[0] = new BarItem
            {
                Value = masterStone.msStone["FULLSIZE"] - ProcessIns.dataDouble[14],
                Color = GetColorByValue(masterStone.msStone["FULLSIZE"] - ProcessIns.dataDouble[14])
            };
            MyModel.InvalidatePlot(true);
        }

        public void UpdateDataStone()
        {
            if(ProcessIns.dataDouble.Count > 0)
            {
                dVal = ProcessIns.dataDouble[13];
                //mMasterStone = _mMasterStone.NONE;
                if (ProcessIns.dataDouble[14] <= masterStone.msStone["FULLSIZE"])
                {
                    mMasterStone = _mMasterStone.FULLSIZE;
                }
                if (ProcessIns.dataDouble[14] <= masterStone.msStone["LX30"] && ProcessIns.dataDouble[14] > masterStone.msStone["FULLSIZE"])
                {
                    mMasterStone = _mMasterStone.LX30;
                }
                if (ProcessIns.dataDouble[14] <= masterStone.msStone["LX26"] && ProcessIns.dataDouble[14] > masterStone.msStone["LX30"])
                {
                    mMasterStone = _mMasterStone.LX26;
                }
                if (ProcessIns.dataDouble[14] <= masterStone.msStone["LX20"] && ProcessIns.dataDouble[14] > masterStone.msStone["LX26"])
                {
                    mMasterStone = _mMasterStone.LX20;
                }
                if (ProcessIns.dataDouble[14] <= masterStone.msStone["LX15"] && ProcessIns.dataDouble[14] > masterStone.msStone["LX20"])
                {
                    mMasterStone = _mMasterStone.LX15;
                }
                if (ProcessIns.dataDouble[14] < masterStone.msStone["LX15"])
                {
                    mMasterStone = _mMasterStone.REPLACE;
                }

                if(dSaveVal != dVal)
                {
                    masterStone.WriteValue("", "CURRENT", "Value", dVal.ToString("F0"));
                    dSaveVal = dVal;
                }    
                
               
            }    
        }

        public void WriteDataPLC()
        {
            ushort[] uVal = new ushort[] {0, 0, 0, 0, 0, 0};
            switch (mMasterStone)
            {
                case _mMasterStone.NONE:
                    modbusTCP.WriteMutilRegisters(33, uVal);
                    break;
                case _mMasterStone.FULLSIZE:
                    modbusTCP.WriteSingleRegis(33, 1);
                    break;
                case _mMasterStone.LX30:
                    modbusTCP.WriteSingleRegis(34, 1);
                    break;
                case _mMasterStone.LX26:
                    modbusTCP.WriteSingleRegis(35, 1);
                    break;
                case _mMasterStone.LX20:
                    modbusTCP.WriteSingleRegis(36, 1);
                    break;
                case _mMasterStone.LX15:
                    modbusTCP.WriteSingleRegis(37, 1);
                    break;
                case _mMasterStone.REPLACE:
                    modbusTCP.WriteSingleRegis(38, 1);
                    break;
            }
        }

        private void FirstLoad()
        {
            OxyColor GetColorByValue(double value)
            {
                if (value >= masterStone.msStone["REPLACE"]) return OxyColor.Parse("#90EE90"); // 
                if (value >= masterStone.msStone["LX15"]) return OxyColor.Parse("#90EE90");    // 90EE90
                if (value >= masterStone.msStone["LX20"]) return OxyColor.Parse("#ADFF2F");    // ADFF2F
                if (value >= masterStone.msStone["LX26"]) return OxyColor.Parse("#FFFF00");    // FFFF00
                if (value >= masterStone.msStone["LX30"]) return OxyColor.Parse("#FFA500");    // FFA500
                if (value >= masterStone.msStone["LX45"]) return OxyColor.Parse("#FF0000");    // FFA500
                return OxyColor.Parse("#8B0000"); //FF0000
            }
            barSeries.Items[0] = new BarItem
            {
                Value = masterStone.msStone["CURRENT"],
                Color = GetColorByValue(masterStone.msStone["CURRENT"])
            };
            MyModel.InvalidatePlot(true);

            if (masterStone.msStone["CURRENT"] <= masterStone.msStone["FULLSIZE"])
            {
                mMasterStone = _mMasterStone.FULLSIZE;
            }
            else if (masterStone.msStone["CURRENT"] <= masterStone.msStone["LX30"] && masterStone.msStone["CURRENT"] > masterStone.msStone["FULLSIZE"])
            {
                mMasterStone = _mMasterStone.LX30;
            }
            else if (masterStone.msStone["CURRENT"] <= masterStone.msStone["LX26"] && masterStone.msStone["CURRENT"] > masterStone.msStone["LX30"])
            {
                mMasterStone = _mMasterStone.LX26;
            }
            else if (masterStone.msStone["CURRENT"] <= masterStone.msStone["LX20"] && masterStone.msStone["CURRENT"] > masterStone.msStone["LX26"])
            {
                mMasterStone = _mMasterStone.LX20;
            }
            else if (masterStone.msStone["CURRENT"] <= masterStone.msStone["LX15"] && masterStone.msStone["CURRENT"] > masterStone.msStone["LX20"])
            {
                mMasterStone = _mMasterStone.LX15;
            }
            else
            {
                mMasterStone = _mMasterStone.REPLACE;
            }

            if (modbusTCP.IsModbus())
            {
                modbusTCP.WriteSingleRegsReal(90, masterStone.msStone["CURRENT"]);
                WriteDataPLC();
            }
        }
    }
}
