using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CUT_RAIL_MACHINE.Services
{
    public class Process_Inspection
    {
        private enum ProcessStatus
        {
            INITIALIZE,
            CHECK_PLC_CONNECT,
            READ_DATA,
            CUTTING_DONE,
            RESTART,
            SAVE_DATA,
            WAIT_COM_ACK
        }

        private ProcessStatus mProcessStatus = ProcessStatus.INITIALIZE;

        private Thread mMainThread;
        private bool mThreadFlag = false;

        private Thread mMainThread2;
        private bool mThreadFlag2 = false;

        private ModbusTCP PLCComm;

        public Process_Inspection(ModbusTCP modbusTCP)
        {
            PLCComm = modbusTCP;
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
            mMainThread.SetApartmentState(ApartmentState.STA);
            mMainThread.Priority = ThreadPriority.Highest;
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

        public List<int> dataInt = new List<int>();
        public List<double> dataDouble = new List<double>();

        private void DoRunProcess()
        {
            mProcessStatus = ProcessStatus.INITIALIZE;

            while(mThreadFlag)
            {
                GC.Collect();
                Thread.Sleep(50);
                try
                {

                    switch (mProcessStatus)
                    {
                        case ProcessStatus.INITIALIZE:
                            // clear data

                            break;
                        case ProcessStatus.CHECK_PLC_CONNECT:
                            // check connect plc
                            //mProcessStatus = ProcessStatus.READ_DATA;


                            break;
                        case ProcessStatus.READ_DATA:
                            // read data from plc to ui
                            dataInt = PLCComm.ReadMutilHoldingInt(0, 61);
                            dataDouble = PLCComm.ReadMutilHoldingReal(62, 32);
                            break;
                        case ProcessStatus.CUTTING_DONE:
                            // cutting done
                            break;
                        case ProcessStatus.RESTART:
                            // restart
                            
                            break;
                        case ProcessStatus.SAVE_DATA:
                            // save data sql, excel ...

                            break;
                        case ProcessStatus.WAIT_COM_ACK:
                            // intitalize
                            break;
                    }
                }
                catch (Exception)
                {
                }
            }
        }

        public void ThreadRun2()
        {
            if (mThreadFlag2) return;

            if (mMainThread2 != null)
            {
                mMainThread2.Join(500);
                mMainThread2.Abort();
                mMainThread2 = null;
            }
            mMainThread2 = new Thread(() =>
            {
                DoRunProcess2();
            });
            mMainThread2.IsBackground = true;
            mThreadFlag2 = true;

            mMainThread2.Start();
        }

        public void ThreadStop2()
        {
            mThreadFlag2 = false;

            if (mMainThread2 != null)
            {
                mMainThread2.Join(500);
                mMainThread2.Abort();
                mMainThread2 = null;
            }
        }

        private void DoRunProcess2()
        {
            bool _connect;
            while (mThreadFlag2)
            {
                try
                {
                    _connect = PLCComm.IsModbus();
                    if (!_connect)
                    {
                        mProcessStatus = ProcessStatus.CHECK_PLC_CONNECT;
                        PLCComm.Connect();
                    }
                    else
                    {
                        mProcessStatus = ProcessStatus.READ_DATA;
                    }
                }
                catch (Exception)
                {
                }
            }
        }
    }
}
