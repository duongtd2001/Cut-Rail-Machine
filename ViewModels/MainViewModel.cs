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
using System.Windows.Forms.VisualStyles;

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
        private HomeViewModel homeViewModel;
        private SettingViewModel settingViewModel;

        private ModbusTCP modbusTCP;

        private MachineData mMachineData;
        private MasterPosition mMasterPosition;
        private Process_Inspection mProcess_Inspection;

        private ExcelRW excelRW;
        private UserRepository userRepository;


        public MainViewModel()
        {

            mMachineData = new MachineData();
            mMachineData.ReadData();

            mMasterPosition = new MasterPosition();
            mMasterPosition.ReadData();

            excelRW = new ExcelRW(mMachineData);
            userRepository = new UserRepository();

            modbusTCP = new ModbusTCP(mMachineData);

            mProcess_Inspection = new Process_Inspection(modbusTCP);

            homeViewModel = new HomeViewModel(modbusTCP, ref excelRW, ref mProcess_Inspection, userRepository);
            
            settingViewModel = new SettingViewModel(homeViewModel, ref modbusTCP, ref mProcess_Inspection);
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
    }
}
