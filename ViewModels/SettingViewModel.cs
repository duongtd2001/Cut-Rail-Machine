using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using Caliburn.Micro;
using CUT_RAIL_MACHINE.Services;
using CUT_RAIL_MACHINE.Models;

namespace CUT_RAIL_MACHINE.ViewModels
{
    public class SettingViewModel : Screen
    {
        #region MyRegion
        private int _MinWidth = 150;
        public int MinWidthColums { get => _MinWidth; set { _MinWidth = value; NotifyOfPropertyChange(() => MinWidthColums); } }

        private string positionServo;
        public string PositionServo { get => positionServo; set { positionServo = value; NotifyOfPropertyChange(() => PositionServo); } }

        private string speedCut;
        public string SpeedCut { get => speedCut; set { speedCut = value; NotifyOfPropertyChange(() => SpeedCut); } }

        private string posEndCut;
        public string PosEndCut { get => posEndCut; set { posEndCut = value; NotifyOfPropertyChange(() => PosEndCut); } }

        private string posOffset;
        public string PosOffset { get => posOffset; set { posOffset = value; NotifyOfPropertyChange(() => PosOffset); } }

        private string speedJog;
        public string SpeedJog { get => speedJog; set { speedJog = value; NotifyOfPropertyChange(() => SpeedJog); } }

        private string posTarget;
        public string PosTarget { get => posTarget; set { posTarget = value; NotifyOfPropertyChange(() => PosTarget); } }

        private System.Windows.Media.Brush isOpenDoor;
        public System.Windows.Media.Brush IsOpenDoor { get => isOpenDoor; set { isOpenDoor = value; NotifyOfPropertyChange(() => IsOpenDoor); } }

        private System.Windows.Media.Brush isCloseDoor;
        public System.Windows.Media.Brush IsCloseDoor { get => isCloseDoor; set { isCloseDoor = value; NotifyOfPropertyChange(() => IsCloseDoor); } }

        private System.Windows.Media.Brush isHydraulicPump;
        public System.Windows.Media.Brush IsHydraulicPump { get => isHydraulicPump; set { isHydraulicPump = value; NotifyOfPropertyChange(() => IsHydraulicPump); } }

        private System.Windows.Media.Brush isMainShaftPump;
        public System.Windows.Media.Brush IsMainShaftPump { get => isMainShaftPump; set { isMainShaftPump = value; NotifyOfPropertyChange(() => IsMainShaftPump); } }

        private System.Windows.Media.Brush isCoolantWater;
        public System.Windows.Media.Brush IsCoolantWater { get => isCoolantWater; set { isCoolantWater = value; NotifyOfPropertyChange(() => IsCoolantWater); } }

        private System.Windows.Media.Brush isMotorCut;
        public System.Windows.Media.Brush IsMotorCut { get => isMotorCut; set { isMotorCut = value; NotifyOfPropertyChange(() => IsMotorCut); } }

        private System.Windows.Media.Brush isClamp;
        public System.Windows.Media.Brush IsClamp { get => isClamp; set { isClamp = value; NotifyOfPropertyChange(() => IsClamp); } }

        private System.Windows.Media.Brush isUnclamp;
        public System.Windows.Media.Brush IsUnclamp { get => isUnclamp; set { isUnclamp = value; NotifyOfPropertyChange(() => IsUnclamp); } }

        private System.Windows.Media.Brush isJogForward;
        public System.Windows.Media.Brush IsJogForward { get => isJogForward; set { isJogForward = value; NotifyOfPropertyChange(() => IsJogForward); } }

        private System.Windows.Media.Brush isJogBackward;
        public System.Windows.Media.Brush IsJogBackward { get => isJogBackward; set { isJogBackward = value; NotifyOfPropertyChange(() => IsJogBackward); } }

        private System.Windows.Media.Brush isManual;
        public System.Windows.Media.Brush IsManual { get => isManual; set { isManual = value; NotifyOfPropertyChange(() => IsManual); } }

        private string resolutionRl;
        public string ResolutionRl { get => resolutionRl; set { resolutionRl = value; NotifyOfPropertyChange(() => ResolutionRl); } }

        private string setOffsetRuler;
        public string SetOffsetRuler { get => setOffsetRuler; set { setOffsetRuler = value; NotifyOfPropertyChange(() => SetOffsetRuler); } }
        #endregion

        private ModbusTCP modbusTCP;
        HomeViewModel _homeViewModel;
        private Process_Inspection mProcessInspection;

        public SettingViewModel(HomeViewModel homeViewModel, ref ModbusTCP _modbusTCP, ref Process_Inspection inspection)
        {
            _homeViewModel = homeViewModel;
            modbusTCP = _modbusTCP;
            mProcessInspection = inspection;
        }

        public void HOME()
        {
            Thread t = new Thread(() =>
            {
                if (modbusTCP.IsModbus())
                {
                    modbusTCP.WriteSingleRegis(10, 1);
                    modbusTCP.WriteSingleRegis(10, 0);
                    //_homeViewModel.aResults += $"[{DateTime.Now.ToString("yyyy MM dd - HH mm ss")}]     START GO HOME\n";
                }
            });
            t.IsBackground = true;
            t.Start();
        }

        public void SetPosEndCut()
        {
            Thread t = new Thread(() =>
            {
                if (modbusTCP.IsModbus())
                {
                    modbusTCP.WriteSingleRegis(8, 1);
                    modbusTCP.WriteSingleRegis(8, 0);
                    //_homeViewModel.aResults += $"[{DateTime.Now.ToString("yyyy MM dd - HH mm ss")}]     SET POSITITON END CUT DONE\n";
                }
            });
            t.IsBackground = true;
            t.Start();
        }

        public void SetPosOffset()
        {
            Thread t = new Thread(() =>
            {
                if (modbusTCP.IsModbus())
                {
                    modbusTCP.WriteSingleRegis(9, 1);
                    modbusTCP.WriteSingleRegis(9, 0);
                    //_homeViewModel.aResults += $"[{DateTime.Now.ToString("yyyy MM dd - HH mm ss")}]     SET POSITION OFFSET DONE\n";
                }
            });
            t.IsBackground = true;
            t.Start();
        }

        public void SetSpeedJog()
        {
            Thread t = new Thread(() =>
            {
                if (!string.IsNullOrEmpty(SpeedJog))
                {
                    modbusTCP.WriteSingleRegsReal(46, Convert.ToDouble(SpeedJog));
                    //_homeViewModel.aResults += $"[{DateTime.Now.ToString("yyyy MM dd - HH mm ss")}]     SET SPEED JOG DONE: {SpeedJog}\n";
                }
                //else
                    //_homeViewModel.aResults += $"[{DateTime.Now.ToString("yyyy MM dd - HH mm ss")}]     PLEASE ENTER VALUE\n";
            });
            t.IsBackground = true;
            t.Start();
        }

        public void SetSpeedCut()
        {
            Thread t = new Thread(() =>
            {
                if (!string.IsNullOrEmpty(SpeedCut))
                {
                    modbusTCP.WriteSingleRegsReal(48, Convert.ToDouble(SpeedCut));
                    //_homeViewModel.aResults += $"[{DateTime.Now.ToString("yyyy MM dd - HH mm ss")}]     SET SPEED CUT DONE: {SpeedCut}\n";
                }
                //else
                    //_homeViewModel.aResults += $"[{DateTime.Now.ToString("yyyy MM dd - HH mm ss")}]     PLEASE ENTER VALUE\n";
            });
            t.IsBackground = true;
            t.Start();
        }

        public void JogForwardABS()
        {
            Thread t = new Thread(() =>
            {
                if (DictinaryInfo.EmployeesInfo["Access"].Equals("PE"))
                {
                    if (modbusTCP.IsModbus() && !string.IsNullOrEmpty(PosTarget))
                    {
                        modbusTCP.WriteSingleRegsReal(50, mProcessInspection.dataDouble[9] + Convert.ToDouble(PosTarget));
                        Thread.Sleep(20);
                        modbusTCP.WriteSingleRegis(11, 1);
                        modbusTCP.WriteSingleRegis(11, 0);
                        //_homeViewModel.aResults += $"[{DateTime.Now.ToString("yyyy MM dd - HH mm ss")}]     JOG FORWARD + {PosTarget}\n";
                    }
                    //else
                        //_homeViewModel.aResults += $"[{DateTime.Now.ToString("yyyy MM dd - HH mm ss")}]     PLEASE ENTER VALUE\n";
                }
            });
            t.IsBackground = true;
            t.Start();
        }

        public void JogBackwardABS()
        {
            Thread t = new Thread(() =>
            {
                if (DictinaryInfo.EmployeesInfo["Access"].Equals("PE"))
                {
                    if (modbusTCP.IsModbus() && !string.IsNullOrEmpty(PosTarget))
                    {
                        modbusTCP.WriteSingleRegsReal(50, mProcessInspection.dataDouble[9] - Convert.ToDouble(PosTarget));
                        Thread.Sleep(100);
                        modbusTCP.WriteSingleRegis(11, 1);
                        modbusTCP.WriteSingleRegis(11, 0);
                        //_homeViewModel.aResults += $"[{DateTime.Now.ToString("yyyy MM dd - HH mm ss")}]     JOG FORWARD - {PosTarget}\n";
                    }
                    //else
                        //_homeViewModel.aResults += $"[{DateTime.Now.ToString("yyyy MM dd - HH mm ss")}]     PLEASE ENTER VALUE\n";
                }
            });
            t.IsBackground = true;
            t.Start();
        }

        public void OPEN_DOOR()
        {
            Thread t = new Thread(() =>
            {
                if (DictinaryInfo.EmployeesInfo["Access"].Equals("PE"))
                {
                    if (modbusTCP.IsModbus())
                    {
                        modbusTCP.WriteSingleRegis(14, 1);
                    }
                } 
            });
            t.IsBackground = true;
            t.Start();
        }

        public void CLOSE_DOOR()
        {
            Thread t = new Thread(() =>
            {
                if (DictinaryInfo.EmployeesInfo["Access"].Equals("PE"))
                {
                    if (modbusTCP.IsModbus())
                    {
                        modbusTCP.WriteSingleRegis(14, 2);
                    }
                }
            });
            t.IsBackground = true;
            t.Start();
        }
        bool _IsHydraulicPump = false;
        public void HYDRAULIC_PUMP()
        {
            _IsHydraulicPump = !_IsHydraulicPump;
            Thread t = new Thread(() =>
            {
                if (DictinaryInfo.EmployeesInfo["Access"].Equals("PE"))
                {
                    if (modbusTCP.IsModbus())
                    {
                        if(_IsHydraulicPump)
                        {
                            modbusTCP.WriteSingleRegis(15, 1);
                            IsHydraulicPump = System.Windows.Media.Brushes.LightGreen;
                        }
                        else
                        {
                            modbusTCP.WriteSingleRegis(15, 0);
                            IsHydraulicPump = System.Windows.Media.Brushes.LightGray;
                        }
                            
                    }
                }
            });
            t.IsBackground = true;
            t.Start();
        }
        bool _IsMainShaftPump = false;
        public void MAIN_SHAFT_PUMP()
        {
            _IsMainShaftPump= !_IsMainShaftPump;
            Thread t = new Thread(() =>
            {
                if (DictinaryInfo.EmployeesInfo["Access"].Equals("PE"))
                {
                    if (modbusTCP.IsModbus())
                    {
                        if (_IsMainShaftPump)
                        {
                            modbusTCP.WriteSingleRegis(16, 1);
                            IsMainShaftPump = System.Windows.Media.Brushes.LightGreen;
                        }
                        else
                        {
                            modbusTCP.WriteSingleRegis(16, 0);
                            IsMainShaftPump = System.Windows.Media.Brushes.LightGray;
                        }

                    }
                }
            });
            t.IsBackground = true;
            t.Start();
        }
        bool _IsCoolantWater = false;
        public void COOLANT_WATER()
        {
            _IsCoolantWater= !_IsCoolantWater;
            Thread t = new Thread(() =>
            {
                if (DictinaryInfo.EmployeesInfo["Access"].Equals("PE"))
                {
                    if (modbusTCP.IsModbus())
                    {
                        if (_IsCoolantWater)
                        {
                            modbusTCP.WriteSingleRegis(17, 1);
                            IsCoolantWater = System.Windows.Media.Brushes.LightGreen;
                        }
                        else
                        {
                            modbusTCP.WriteSingleRegis(17, 0);
                            IsCoolantWater = System.Windows.Media.Brushes.LightGray;
                        }

                    }
                }
            });
            t.IsBackground = true;
            t.Start();
        }
        bool _IsMotorCut = false;
        public void MOTOR_CUT()
        {
            _IsMotorCut= !_IsMotorCut;
            Thread t = new Thread(() =>
            {
                if (DictinaryInfo.EmployeesInfo["Access"].Equals("PE"))
                {
                    if (modbusTCP.IsModbus())
                    {
                        if (_IsMotorCut)
                        {
                            modbusTCP.WriteSingleRegis(18, 1);
                            IsMotorCut = System.Windows.Media.Brushes.LightGreen;
                        }
                        else
                        {
                            modbusTCP.WriteSingleRegis(18, 0);
                            IsMotorCut = System.Windows.Media.Brushes.LightGray;
                        }

                    }
                }
            });
            t.IsBackground = true;
            t.Start();
        }

        public void CLAMP()
        {
            Thread t = new Thread(() =>
            {
                if (DictinaryInfo.EmployeesInfo["Access"].Equals("PE"))
                {
                    if (modbusTCP.IsModbus())
                    {
                        modbusTCP.WriteSingleRegis(19, 1);
                        //IsHydraulicPump = System.Windows.Media.Brushes.LightGreen;
                    }
                }
            });
            t.IsBackground = true;
            t.Start();
        }

        public void UNCLAMP()
        {
            Thread t = new Thread(() =>
            {
                if (DictinaryInfo.EmployeesInfo["Access"].Equals("PE"))
                {
                    if (modbusTCP.IsModbus())
                    {
                        modbusTCP.WriteSingleRegis(19, 0);
                        //IsHydraulicPump = System.Windows.Media.Brushes.LightGreen;
                    }
                }
            });
            t.IsBackground = true;
            t.Start();
        }
        bool _IsJogForward = false;
        public void JOG_FORWARD()
        {
            _IsJogForward = !_IsJogForward;
            Thread t = new Thread(() =>
            {
                if (DictinaryInfo.EmployeesInfo["Access"].Equals("PE"))
                {
                    if (modbusTCP.IsModbus())
                    {
                        if (_IsJogForward)
                        {
                            modbusTCP.WriteSingleRegis(20, 1);
                            IsJogForward = System.Windows.Media.Brushes.LightGreen;
                        }
                        else
                        {
                            modbusTCP.WriteSingleRegis(20, 0);
                            IsJogForward = System.Windows.Media.Brushes.LightGray;
                        }

                    }
                }
            });
            t.IsBackground = true;
            t.Start();
        }
        bool _IsJogBackward = false;
        public void JOG_BACKWARD()
        {
            _IsJogBackward = !_IsJogBackward;
            Thread t = new Thread(() =>
            {
                if (DictinaryInfo.EmployeesInfo["Access"].Equals("PE"))
                {
                    if (modbusTCP.IsModbus())
                    {
                        if (_IsJogBackward)
                        {
                            modbusTCP.WriteSingleRegis(21, 1);
                            IsJogBackward = System.Windows.Media.Brushes.LightGreen;
                        }
                        else
                        {
                            modbusTCP.WriteSingleRegis(21, 0);
                            IsJogBackward = System.Windows.Media.Brushes.LightGray;
                        }

                    }
                }
            });
            t.IsBackground = true;
            t.Start();
        }

        public void SET_RESOL_RULER()
        {

            Thread t = new Thread(() =>
            {
                if (DictinaryInfo.EmployeesInfo["Access"].Equals("PE"))
                {
                    if (modbusTCP.IsModbus() && !string.IsNullOrEmpty(ResolutionRl))
                    {
                        modbusTCP.WriteSingleRegsReal(38, Convert.ToDouble(ResolutionRl));
                    }
                }
            });
            t.IsBackground = true;
            t.Start();
        }
        bool _IsManual = false;
        public void MANUAL()
        {
            _IsManual = !_IsManual;
            Thread t = new Thread(() =>
            {
                if (DictinaryInfo.EmployeesInfo["Access"].Equals("PE"))
                {
                    if (modbusTCP.IsModbus())
                    {
                        if (_IsManual)
                        {
                            modbusTCP.WriteSingleRegis(22, 1);
                            IsManual = System.Windows.Media.Brushes.LightGreen;
                        }
                        else
                        {
                            modbusTCP.WriteSingleRegis(22, 0);
                            IsManual = System.Windows.Media.Brushes.LightGray;
                        }
                    }
                }
            });
            t.IsBackground = true;
            t.Start();
        }

        public void SetOffsetRl()
        {
            Thread t = new Thread(() =>
            {
                if (DictinaryInfo.EmployeesInfo["Access"].Equals("PE"))
                {
                    if (modbusTCP.IsModbus() && !string.IsNullOrEmpty(SetOffsetRuler))
                    {
                        modbusTCP.WriteSingleRegsReal(52, Convert.ToDouble(SetOffsetRuler));
                    }
                }
            });
            t.IsBackground = true;
            t.Start();
        }
        public bool IsUpdate = false;
        public void UpdateFromPLC()
        {
            IsUpdate = !IsUpdate;
            Thread tr_update = new Thread(() => 
            {
                while (IsUpdate)
                {
                    if (modbusTCP.IsModbus())
                    {
                        if (mProcessInspection.dataDouble != null)
                        {
                            ResolutionRl = mProcessInspection.dataDouble[3].ToString("F2");
                            PositionServo = mProcessInspection.dataDouble[4].ToString("F3");
                            PosEndCut = mProcessInspection.dataDouble[11].ToString("F3");
                            PosOffset = mProcessInspection.dataDouble[6].ToString("F3");
                            SpeedJog = mProcessInspection.dataDouble[7].ToString("F2");
                            SpeedCut = mProcessInspection.dataDouble[8].ToString("F2");
                            SetOffsetRuler = mProcessInspection.dataDouble[10].ToString("F2");
                        }
                    }
                    Thread.Sleep(100);
                }
            });
            tr_update.IsBackground = true;
            tr_update.Start();
        }
    }
}
