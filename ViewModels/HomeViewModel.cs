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
using CUT_RAIL_MACHINE.Services.Interfaces;

namespace CUT_RAIL_MACHINE.ViewModels
{
    public class HomeViewModel : Screen
    {
        private int _ExpandResult = 35;
        public int ExpandResult { get => _ExpandResult; set { _ExpandResult = value; NotifyOfPropertyChange(() => ExpandResult); } }

        private int _MinWidth = 200;
        public int MinWidthColums { get => _MinWidth; set { _MinWidth = value; NotifyOfPropertyChange(() => MinWidthColums); } }

        private bool _IsExpand;
        public bool IsExpand { get => _IsExpand; set { _IsExpand = value; NotifyOfPropertyChange(() => IsExpand); } }

        private bool _IsIDFocused = true;
        public bool IsIDFocused { get => _IsIDFocused; set { _IsIDFocused = value; NotifyOfPropertyChange(() => IsIDFocused); } }

        private bool _IsPOFocused;
        public bool IsPOFocused { get => _IsPOFocused; set { _IsPOFocused = value; NotifyOfPropertyChange(() => IsPOFocused); } }

        private bool _IsUnLocker = false;
        public bool IsUnLocker { get => _IsUnLocker; set { _IsUnLocker = value; NotifyOfPropertyChange(() => IsUnLocker); } }

        private Visibility _IsViewLocker = Visibility.Collapsed;
        public Visibility IsViewLocker
        {
            get => _IsViewLocker;
            set
            {
                _IsViewLocker = value;
                NotifyOfPropertyChange(() => IsViewLocker);
            }
        }

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
                        ErrorMessage = "";
                        FullName = userModel.Name;
                        IsIDFocused = false;
                        IsPOFocused = true;
                    }
                    else
                    {
                        FullName = "";
                        ErrorMessage = "* Invalid username or password";
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
                    IsUnLocker = true;
                    IsViewLocker = Visibility.Visible;
                }
                else
                {
                    ErrorMessage = "* PO information is incorrect";
                }
            }
        }

        private ReadExcelData excelData;
        private IPLCComm _S7Comm;
        public HomeViewModel(IPLCComm plcComm)
        {
            _S7Comm = plcComm;
            excelData = new ReadExcelData();
        }

        public void Expand_CKB()
        {
            ExpandResult = IsExpand ? 200 : 35;
        }
    }
}
