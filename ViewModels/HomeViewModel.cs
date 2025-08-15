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
using CUT_RAIL_MACHINE.Repositories;
using Action = System.Action;

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

        private string _aResults;
        public string aResults { get => _aResults; set { _aResults = value; NotifyOfPropertyChange(() => aResults); } }

        private string rulerValue;
        public string RulerValue { get => rulerValue; set { rulerValue = value; NotifyOfPropertyChange(() => RulerValue); } }

        //private string _mLenght;
        //public string mLenght { get => _mLenght; set { _mLenght = value; NotifyOfPropertyChange(() => mLenght); } }

        //private string mType;
        //public string mTypeX { get => mType; set { mType = value; NotifyOfPropertyChange(() => mTypeX); } }

        private object _mEmployeesChildView;
        public object mEmployeesChildView { get => _mEmployeesChildView; set { _mEmployeesChildView = value; NotifyOfPropertyChange(() => mEmployeesChildView); } }

        private EmployeesChildViewModel _employeesChildViewModel;
        public EmployeesChildViewModel employeesChildViewModel { get => _employeesChildViewModel; set { _employeesChildViewModel = value; NotifyOfPropertyChange(() => employeesChildViewModel); } }

        private object _MasterStoneChildView;
        public object MasterStoneChildView { get => _MasterStoneChildView; set { _MasterStoneChildView = value; NotifyOfPropertyChange(() => MasterStoneChildView); } }
        #endregion


        private Thread mMainThread;

        private ModbusTCP modbusTCP;
        private ExcelRW excelRW;

        private Process_Inspection mProcess_Inspection;
        private MasterStoneViewModel mMasterStoneViewModel;

        private UserRepository mUserRepository;

        public HomeViewModel(ModbusTCP _modbusTCP, ref ExcelRW excel, ref Process_Inspection process_Inspection, UserRepository repository)
        {

            modbusTCP = _modbusTCP;
            excelRW = excel;
            mUserRepository = repository;

            mProcess_Inspection = process_Inspection;

            employeesChildViewModel = new EmployeesChildViewModel(modbusTCP, ref excelRW, mUserRepository);
            mMasterStoneViewModel = new MasterStoneViewModel();

            mEmployeesChildView = employeesChildViewModel;
            MasterStoneChildView = mMasterStoneViewModel;
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
            mMainThread.IsBackground = true;
            mThreadFlag = true;

            mMainThread.Start();
        }

        private enum ProcessStatus
        {
            CHECK_VALUE,
            SHOW_VALUE,
            WRITE_DATA,
            LENGHT_OK,
            LENGHT_NG
        }

        private ProcessStatus mProcessStatus = ProcessStatus.CHECK_VALUE;

        private void DoRunProcess()
        {
            mProcessStatus = ProcessStatus.CHECK_VALUE;
            while (mThreadFlag)
            {
                GC.Collect();
                Thread.Sleep(20);
                try
                {
                    switch (mProcessStatus)
                    {
                        case ProcessStatus.CHECK_VALUE:
                            if (mProcess_Inspection.dataDouble.Count != 0)
                            {
                                mProcessStatus = ProcessStatus.SHOW_VALUE;
                            }
                            break;
                        case ProcessStatus.SHOW_VALUE:
                            RulerValue = mProcess_Inspection.dataDouble[0].ToString("F2");
                            mProcessStatus = ProcessStatus.WRITE_DATA;
                            break;
                        case ProcessStatus.WRITE_DATA:
                            if (mProcess_Inspection.dataDouble[0].ToString("F2") == employeesChildViewModel.mLenght)
                            {
                                mProcessStatus = ProcessStatus.LENGHT_OK;
                                break;
                            }
                            mProcessStatus = ProcessStatus.LENGHT_NG;
                            break;
                        case ProcessStatus.LENGHT_OK:
                            modbusTCP.WriteSingleRegis(26, 1);
                            mProcessStatus = ProcessStatus.CHECK_VALUE;
                            break;
                        case ProcessStatus.LENGHT_NG:
                            modbusTCP.WriteSingleRegis(26, 0);
                            mProcessStatus = ProcessStatus.CHECK_VALUE;
                            break;
                    }
                }
                catch (Exception)
                {
                }
            }
        }
        public void ResetRuler()
        {
            Thread t = new Thread(() =>
            {
                if (modbusTCP.IsModbus())
                {
                    modbusTCP.WriteSingleRegis(4, 1);
                    Thread.Sleep(20);
                    modbusTCP.WriteSingleRegis(4, 0);

                    //aResults += $"[{DateTime.Now.ToString("yyyy MM dd - HH mm ss")}]     RESET VALUE RULER DONE\n";
                }

            });
            t.IsBackground = true;
            t.Start();
        }
    }
}
