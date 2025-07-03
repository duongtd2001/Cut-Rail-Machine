using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using FontAwesome.Sharp;
using CUT_RAIL_MACHINE.Models;
using CUT_RAIL_MACHINE.Repositories;
using Caliburn.Micro;
using System.Windows.Controls;
using CUT_RAIL_MACHINE.Services;
using CUT_RAIL_MACHINE.Views;
using CUT_RAIL_MACHINE.Messages;
using System.Xml.Linq;
using System.Drawing;
using System.Data.SqlClient;
using CUT_RAIL_MACHINE.Helpers;
using System.IO;

namespace CUT_RAIL_MACHINE.ViewModels
{
    public class MainViewModel : Conductor<object>, IViewAware /*IHandle<AutoDoneEvent>*/
    {
        //Fields
        private string _currentUserAccount;
        private string _CurrentUserAccountIMG;
        private string _caption;
        private IconChar _icon;
        private Visibility _IsManualCommand = Visibility.Collapsed;

        
        // properties
        public string CurrentUserAccountIMG { get => _CurrentUserAccountIMG; set { _CurrentUserAccountIMG = value; NotifyOfPropertyChange(() => CurrentUserAccountIMG); } }

        public string CurrentUserAccount { get => _currentUserAccount; set { _currentUserAccount = value; NotifyOfPropertyChange(() => CurrentUserAccount); } }

        public string Caption { get => _caption; set { _caption = value; NotifyOfPropertyChange(() => Caption); } }

        public IconChar Icon { get => _icon; set { _icon = value; NotifyOfPropertyChange(() => Icon); } }

        public Visibility IsManualCommand { get => _IsManualCommand; set { _IsManualCommand = value; NotifyOfPropertyChange(() => IsManualCommand); } }

        private Visibility _isAccess = Visibility.Collapsed;
        public Visibility IsAccess { get => _isAccess; set { _isAccess = value; NotifyOfPropertyChange(() => IsAccess); } }

        private object _view;

        public void AttachView(object view, object context = null)
        {
            _view = view;
        }

        public object GetView()
        {
            return _view;
        }

        private UserRepository userRepository;
        private PLCComm S7Comm;
        private HomeViewModel homeViewModel;
        private SettingViewModel settingViewModel;
        private readonly IEventAggregator _eventAggregator;
        private readonly IWindowManager _windowManager;
        private SerialPortPLC plcCom;
        private ModbusTCP modbusTCP;
        private Thread _plcThread;
        private Thread _sqlThread;
        private bool _isRunning = false;
        public bool _isStatusPLC = false;

        private string basePathConfig;

        private PulseConverter _convert;

        public MainViewModel()
        {
            LoadDataConfig();
            modbusTCP = new ModbusTCP();
            userRepository = new UserRepository();
            homeViewModel = new HomeViewModel(this, modbusTCP);
            
            settingViewModel = new SettingViewModel(homeViewModel, ref modbusTCP);
            ShowHomeViewCommand();
        }

        public void ShowHomeViewCommand()
        {
            ActivateItemAsync(homeViewModel);
            Caption = "Dashboard";
            Icon = IconChar.Home;
            settingViewModel.IsUpdate = false;
        }
        public void ShowSettingCommand()
        {
            ActivateItemAsync(settingViewModel);
            Caption = "Setting";
            Icon = IconChar.Tools;
        }
        public void bnClose()
        {
            if (MessageBox.Show("Are you sure you want to exit?", "Warning", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
            {
                if (_view is MainView window)
                {
                    window.ShutdownWithFade();
                }
            }
            else
                return;

        }

        private void LoadDataConfig()
        {
            // Path File
            basePathConfig = Path.Combine(AppContext.BaseDirectory, "config.ini");

            // Data Modbus
            List<string> listModbus = IniFile.ReadSectionRawValue(basePathConfig, "ModbusTCPIP");
            DataConfigModel.Modbus_IP = listModbus[0];
            DataConfigModel.Modbus_Port = listModbus[1];

            // Data PLC Siemen
            List<string> listPLC = IniFile.ReadSectionRawValue(basePathConfig, "PLCSiemen");
            DataConfigModel.CPUTypes = listPLC[0];
            DataConfigModel.IP_PLC = listPLC[1];
            DataConfigModel._Rack = listPLC[2];
            DataConfigModel._Slot = listPLC[3];

            // Data PLC FX Serial
            List<string> listPLCFx = IniFile.ReadSectionRawValue(basePathConfig, "PLCFXSerial");
            DataConfigModel._Port = listPLCFx[0];
            DataConfigModel._BaudRate = listPLCFx[1];
            DataConfigModel._Parity = listPLCFx[2];
            DataConfigModel._DataBits = listPLCFx[3];
            DataConfigModel._StopBits = listPLCFx[4];

            // Data Employees
            List<string> listEmp = IniFile.ReadSectionRawValue(basePathConfig, "Employees");
            DataConfigModel.PathEmployees = listEmp[0];
            DataConfigModel.FileNameEmp = listEmp[1];

            // Data SQL Server
            List<string> listSQL = IniFile.ReadSectionRaw(basePathConfig, "SQLServer");
            DataConfigModel.DataSource = listSQL[0];
            DataConfigModel.InitialCatalog = listSQL[1];
            DataConfigModel.PersistSecurityInfo = listSQL[2];
            DataConfigModel.UserID = listSQL[3];
            DataConfigModel.Password = listSQL[4];
            DataConfigModel.SaveSQL = listSQL[5];
            //string[] _isSave = DataConfigModel.SaveSQL.Split('=');
            //bool _boolSQL = Convert.ToBoolean(_isSave[1]);

            // Data Save Excel
            List<string> listSaveData = IniFile.ReadSectionRawValue(basePathConfig, "SaveDataExcel");
            DataConfigModel.PathSaveData = listSaveData[0];
            DataConfigModel.FileSaveData = listSaveData[1];
            DataConfigModel.SaveExcel = listSaveData[2];
        }
        private string _access = "";
        
        
        private void StartPlcThread()
        {
            _plcThread = new Thread(() =>
            {
                bool previousStatus = false;

                while (!_isRunning)
                {
                    bool currentStatus = S7Comm.IsConnect();

                    if (!currentStatus)
                    {
                        _eventAggregator.PublishOnUIThreadAsync(new PlcDataMessage { IsConnectPLC = 0 });
                        S7Comm.ConnectPLC();
                        if (UserSession.NumberOfLoginTimes == 1)
                        {
                            if (plcCom.CheckConnectPLC())
                            {
                                plcCom.WriteDataToPLC("M330", true);
                                plcCom.WriteDataToPLC("M0", true);
                            }
                        }
                        previousStatus = false;
                        Thread.Sleep(2500);
                    }
                    else
                    {
                        if (!previousStatus)
                        {
                            _eventAggregator.PublishOnUIThreadAsync(new PlcDataMessage { IsConnectPLC = 1 });
                            previousStatus = true;
                        }
                        else
                        {
                            _eventAggregator.PublishOnUIThreadAsync(new PlcDataMessage
                            {
                                IsConnectPLC = 2,
                                IsPosition = _convert.ConvertPulseToMM(plcCom.ReadDataFromPLC2("D248")).ToString("F2"),
                                IsSpeed = _convert.ConvertSpeedPulseToMMperSec(plcCom.ReadDataFromPLC2("D252")).ToString("F2")
                            });
                            Thread.Sleep(100);
                        }
                    }
                }
            });
            _plcThread.IsBackground = true;
            _plcThread.Start();
        }
        /*
        private void SQLConn()
        {
            _sqlThread = new Thread(() =>
            {
                bool previousStatusSQL = false;

                while (!_isRunning)
                {
                    bool _statusSQLServer = userRepository.StatusConnectSQL();

                    if (!_statusSQLServer)
                    {
                        _eventAggregator.PublishOnUIThreadAsync(new SqlDataMessage { SQLConnect = 0 });
                        previousStatusSQL = false;
                        Thread.Sleep(2500);
                    }
                    else
                    {
                        if (!previousStatusSQL)
                        {
                            _eventAggregator.PublishOnUIThreadAsync(new SqlDataMessage { SQLConnect = 1 });
                            previousStatusSQL = true;
                        }
                    }
                }
            });
            _sqlThread.IsBackground = true;
            _sqlThread.Start();
        }
        
        public Task HandleAsync(AutoDoneEvent message, CancellationToken cancellationToken)
        {
            if (message.Message == 1)
            {
                var loginVM = IoC.Get<LoginViewModel>();
                _windowManager.ShowWindowAsync(loginVM);
                if (_view is MainView window)
                {
                    window.CloseWithFade();
                    _isRunning = true;
                    _sqlThread.Abort();
                    _plcThread.Abort();
                    plcCom.DisconnectPLC();
                }
            }
            return Task.CompletedTask;
        }
        */
    }
}
