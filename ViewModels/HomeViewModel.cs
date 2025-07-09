using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.Expando;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using CUT_RAIL_MACHINE.Services;
using CUT_RAIL_MACHINE.Models;
using System.Windows;
using System.Diagnostics;
using System.Threading;
using S7.Net;
using Modbus.Device;
using System.Net.Sockets;
using project.Services;

namespace CUT_RAIL_MACHINE.ViewModels
{
    public class HomeViewModel : Screen
    {
        #region Varialble
        private int _ExpandResult = 50;
        public int ExpandResult { get => _ExpandResult; set { _ExpandResult = value; NotifyOfPropertyChange(() => ExpandResult); } }

        private int _MinWidth = 150;
        public int MinWidthColums { get => _MinWidth; set { _MinWidth = value; NotifyOfPropertyChange(() => MinWidthColums); } }

        private bool _IsExpand;
        public bool IsExpand { get => _IsExpand; set { _IsExpand = value; NotifyOfPropertyChange(() => IsExpand); } }

        private bool _IsIDFocused = true;
        public bool IsIDFocused { get => _IsIDFocused; set { _IsIDFocused = value; NotifyOfPropertyChange(() => IsIDFocused); } }

        private bool _IsPOFocused;
        public bool IsPOFocused { get => _IsPOFocused; set { _IsPOFocused = value; NotifyOfPropertyChange(() => IsPOFocused); } }

        private string _aResults;
        public string aResults { get => _aResults; set { _aResults = value; NotifyOfPropertyChange(() => aResults); } }

        private string rulerValue;
        public string RulerValue { get => rulerValue; set { rulerValue = value; NotifyOfPropertyChange(() => RulerValue); } }

        private string resolutionRuler;
        public string ResolutionRuler { get => resolutionRuler; set { resolutionRuler = value; NotifyOfPropertyChange(() => ResolutionRuler); } }

        private string valueChangeLenght;
        public string ValueChangeLenght { get => valueChangeLenght; set { valueChangeLenght = value; NotifyOfPropertyChange(() => ValueChangeLenght); } }

        private string cuttingLenght;
        private string CuttingLenght { get => cuttingLenght; set { cuttingLenght = value; NotifyOfPropertyChange(() => CuttingLenght); } }

        private string typeX;
        public string TypeX { get => typeX; set { typeX = value; NotifyOfPropertyChange(() => TypeX); } }

        private string quantity;
        public string Quantity { get => quantity; set { quantity = value; NotifyOfPropertyChange(() => Quantity); } }

        private string lotNo;
        public string LotNo { get => lotNo; set { lotNo = value; NotifyOfPropertyChange(() => LotNo); } }

        
        #endregion

        

        private bool UserUI = false;
        private string _Username;
        public string Username
        {
            get => _Username;
            set
            {
                _Username = value;
                NotifyOfPropertyChange(() => Username);
                try
                {
                    UserModel userModel = excelData.FindProductByID(Username);
                    if (userModel != null)
                    {
                        UserSession.CurrentUser = userModel.Name;
                        UserSession.CurrentAccess = userModel.Access;
                        UserSession.CurrentID = userModel.ID;
                        _mainViewModel.CurrentUserAccount = userModel.Name;
                        ErrorMessage = "";
                        FullName = userModel.Name;
                        IsIDFocused = false;
                        IsPOFocused = true;
                        if (UserSession.CurrentAccess.Equals("PE"))
                        {
                            _mainViewModel.IsAccess = Visibility.Visible;
                            currentAccess = true;
                        }
                        Thread t = new Thread(() =>
                        {
                            if (modbusTCP.IsModbus())
                            {
                                modbusTCP.WriteSingleRegis(0, 1);
                            }
                        });
                        t.IsBackground = true;
                        t.Start();
                    }
                    else
                    {
                        UserSession.CurrentUser = "";
                        UserSession.CurrentAccess = "";
                        UserSession.CurrentID = "";
                        FullName = "";
                        ErrorMessage = "* Invalid username or password";
                        if (UserSession.CurrentAccess == null || !UserSession.CurrentAccess.Equals("PE"))
                        {
                            _mainViewModel.IsAccess = Visibility.Collapsed;
                            currentAccess = false;
                        }
                        Thread t = new Thread(() =>
                        {
                            if(modbusTCP.IsModbus())
                            {
                                modbusTCP.WriteSingleRegis(0, 0);
                            }
                        });
                        t.IsBackground = true;
                        t.Start();
                    }

                }
                catch (Exception)
                {
                    ErrorMessage = "* Employee data file not found";
                }
            }
        }
        private string _FullName;
        public string FullName
        {
            get => _FullName;
            set
            {
                _FullName = value;
                NotifyOfPropertyChange(() => FullName);
            }
        }

        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                NotifyOfPropertyChange(() => ErrorMessage);
            }
        }

        private bool PO_UI = false;
        private string _PO;
        public string PO
        {
            get => _PO;
            set
            {
                _PO = value;
                NotifyOfPropertyChange(() => PO);
                if (!string.IsNullOrWhiteSpace(_PO) && _PO.Length == 12)
                {
                    UserSession.CurrentPO = PO;
                    ErrorMessage = "";
                    startTime = DateTime.Now.ToString();
                    Thread t = new Thread(() =>
                    {
                        if(modbusTCP.IsModbus())
                        {
                            modbusTCP.WriteSingleRegis(1, 1);
                        }
                    });
                    t.IsBackground = true;
                    t.Start();

                }
                else
                {
                    Thread t = new Thread(() =>
                    {
                        if (modbusTCP.IsModbus())
                        {
                            modbusTCP.WriteSingleRegis(1, 0);
                        }
                    });
                    t.IsBackground = true;
                    t.Start();
                    UserSession.CurrentPO = "";
                    ErrorMessage = "* PO information is incorrect";
                }
            }
        }

        Thread mMainThread;
        Thread watchdog;
        private string startTime;
        private bool IsWatchDog = false;
        ModbusTCP modbusTCP;
        private ReadExcelData excelData;
        private MainViewModel _mainViewModel;
        private SaveDataTestExcel saveDataTestExcel;
        public HomeViewModel(MainViewModel mainViewModel, ModbusTCP _modbusTCP)
        {
            
            excelData = new ReadExcelData();
            saveDataTestExcel = new SaveDataTestExcel();
            _mainViewModel = mainViewModel;
            modbusTCP = _modbusTCP;
           
            ThreadRun();
            watchdog = new Thread(() =>
            {
                while (true)
                {
                    if (previousStatus)
                    {
                        if (sw.ElapsedMilliseconds > 2000)
                        {
                            aResults += $"[{DateTime.Now.ToString("yyyy MM dd - HH mm ss")}]     TIMEOUT ERROR READING DATA FORM PLC\n";
                            Thread.Sleep(2000);
                        }
                    }
                    Thread.Sleep(200);
                }
            });
            watchdog.IsBackground = true;
            watchdog.Start();
            
        }

        public void Expand_CKB()
        {
            ExpandResult = IsExpand ? 200 : 50;
        }
        private bool mThreadFlag = false;

        public void ThreadRun()
        {
            if (mThreadFlag) return;

            if (mMainThread != null)
            {
                mMainThread.Join(500);
                mMainThread.Abort();
                mMainThread = null;
            }

            mMainThread = new Thread(() => DoRunProcess());
            mMainThread.SetApartmentState(ApartmentState.STA);
            mMainThread.Priority = ThreadPriority.Highest;
            mMainThread.IsBackground = true;
            mThreadFlag = true;

            mMainThread.Start();

        }
        
        bool previousStatus = false;
        public bool currentAccess = false;
        private bool _isRunning = false;
        private ushort[] _ushorts = new ushort[] {1, 0};
        public List<double> rulers = new List<double>();
        List<int> intValue = new List<int>();
        Stopwatch sw = new Stopwatch();
        private void DoRunProcess()
        {
            while (!_isRunning)
            {
                try
                {
                    bool currentStatus = modbusTCP.IsModbus();
                    if (!currentStatus)
                    {
                        aResults += $"[{DateTime.Now.ToString("yyyy MM dd - HH mm ss")}]     PLC DISCONNECTED\n";
                        modbusTCP = new ModbusTCP();
                        previousStatus = false;
                        Thread.Sleep(1500);
                    }
                    else
                    {
                        if (!previousStatus)
                        {
                            modbusTCP.WriteSingleRegis(2, 1);
                            intValue = modbusTCP.ReadMutilHoldingInt(0, 15);
                            if (intValue[3] >= 1 && intValue != null)
                            {
                                aResults += $"[{DateTime.Now.ToString("yyyy MM dd - HH mm ss")}]     PLC CONNECTED\n";
                                previousStatus = true;
                            }
                        }
                        else
                        {
                            sw.Reset();
                            sw.Start();
                            bool _restart = false;
                            modbusTCP.WriteMutilRegisters(2, _ushorts);
                            rulers = modbusTCP.ReadMutilHoldingReal(32, 22);
                            intValue = modbusTCP.ReadMutilHoldingInt(0, 15);
                            if (rulers != null)
                            {
                                RulerValue = rulers[0].ToString("F3");
                                ResolutionRuler = rulers[3].ToString("F3");
                            }
                            if (intValue != null)
                            {
                                if (intValue[13] == 1)
                                {
                                    if(!_restart)
                                    {
                                        aResults += $"[{DateTime.Now.ToString("yyyy MM dd - HH mm ss")}]     CUTTING FINISH\n";
                                        aResults += $"[{DateTime.Now.ToString("yyyy MM dd - HH mm ss")}]     RESTART\n";
                                        _restart = !_restart;
                                    }
                                }
                            }
                            Thread.Sleep(50);
                            sw.Stop();
                        }
                    }
                }
                catch (Exception ex)
                {
                    aResults += $"[{DateTime.Now.ToString("yyyy MM dd - HH mm ss")}]     ERROR: {ex.ToString()}\n";
                    Thread.Sleep(2000);
                }
            }
        }
        public void ResetRuler()
        {
            Thread t = new Thread(() =>
            {
                if(modbusTCP.IsModbus())
                {
                    modbusTCP.WriteSingleRegis(4, 1);
                    modbusTCP.WriteSingleRegis(4, 0);

                    aResults += $"[{DateTime.Now.ToString("yyyy MM dd - HH mm ss")}]     RESET VALUE RULER DONE\n";
                }
                
            });
            t.IsBackground = true;
            t.Start();
        }

        public void ChangeLenght()
        {
            
        }
        private string endTime;
        public void FinishPO()
        {
            
        }
    }
}
