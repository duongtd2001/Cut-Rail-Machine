using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;
using CUT_RAIL_MACHINE.Models;
using CUT_RAIL_MACHINE.Repositories;
using CUT_RAIL_MACHINE.Services;
using Action = System.Action;

namespace CUT_RAIL_MACHINE.ViewModels
{
    public class EmployeesChildViewModel : Screen
    {

        private bool _IsIDFocused = true;
        public bool IsIDFocused { get => _IsIDFocused; set { _IsIDFocused = value; NotifyOfPropertyChange(() => IsIDFocused); } }

        private bool _IsPOFocused;
        public bool IsPOFocused { get => _IsPOFocused; set { _IsPOFocused = value; NotifyOfPropertyChange(() => IsPOFocused); } }

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
                    DEmployees.Clear();
                    DEmployees = excelRW.FindEmployeesByID(Username);
                    DictinaryInfo.EmployeesInfo = DEmployees;
                    if (DEmployees.Count != 0)
                    {
                        FullName = DEmployees["Name"];
                        ErrorMessage = "";
                        IsIDFocused = false;
                        IsPOFocused = true;

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
                        FullName = "";
                        ErrorMessage = "* Invalid username or password";
                        Thread t = new Thread(() =>
                        {
                            if (modbusTCP.IsModbus())
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

        private string _PO;
        public string PO
        {
            get => _PO;
            set
            {
                _PO = value;
                NotifyOfPropertyChange(() => PO);
                //userRepository.StatusConnectOST();
                //userRepository.StatusConnectERP();
                if (!string.IsNullOrWhiteSpace(_PO) && _PO.Length == 12)
                {
                    Thread FindsqlERP = new Thread(() =>
                    {
                        try
                        {
                            ProductName = userRepository.GetProductNameByPO(PO);

                            if (!string.IsNullOrEmpty(ProductName))
                            {
                                DictinaryInfo.EmployeesInfo["PO"] = PO;
                                DictinaryInfo.EmployeesInfo["ProductName"] = ProductName;
                                MasterProductInfo = excelRW.FindProductByName(ProductName);
                                if (MasterProductInfo != null && MasterProductInfo.Count > 0)
                                {
                                    ErrorMessage = "";
                                    DictinaryInfo.ProductInfo = MasterProductInfo;
                                    mTypex = MasterProductInfo["Type"];
                                    mLenght = MasterProductInfo["Lenght"];
                                    if (modbusTCP.IsModbus())
                                    {
                                        modbusTCP.WriteSingleRegis(1, 1);
                                    }
                                }
                                else
                                {
                                    ErrorMessage = "* Master information is incorrect";
                                }
                            }
                            else
                            {
                                if (userRepository.Error == null)
                                {
                                    ErrorMessage = "* PO information is incorrect";
                                }
                                else
                                {
                                    ErrorMessage = userRepository.Error;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Application.Current.Dispatcher.Invoke(new Action(() =>
                            {
                                MessageBox.Show($"ERROR: {ex.ToString()}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                            }));
                        }
                    });
                    FindsqlERP.IsBackground = true;
                    FindsqlERP.Start();
                    
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
                    ErrorMessage = "* PO information is incorrect";
                }
            }
        }

        private string ProductName = null;

        private string _lenght;
        public string mLenght { get => _lenght; set { _lenght = value; NotifyOfPropertyChange(() => mLenght); } }

        private string _mTypex;
        public string mTypex
        { 
            get => _mTypex; 
            set 
            { 
                _mTypex = value; 
                NotifyOfPropertyChange(() => mTypex);
                Thread TypeDistance = new Thread(() =>
                {
                    try
                    {
                        if (mTypex.Contains("R15"))
                        {
                            modbusTCP.WriteSingleRegis(27, 1);
                            Thread.Sleep(20);
                            modbusTCP.WriteSingleRegis(27, 0);
                        }
                        if (mTypex.Contains("R20"))
                        {
                            modbusTCP.WriteSingleRegis(28, 1);
                            Thread.Sleep(20);
                            modbusTCP.WriteSingleRegis(28, 0);
                        }
                        if (mTypex.Contains("R26"))
                        {
                            modbusTCP.WriteSingleRegis(29, 1);
                            Thread.Sleep(20);
                            modbusTCP.WriteSingleRegis(29, 0);
                        }
                        if (mTypex.Contains("R30"))
                        {
                            modbusTCP.WriteSingleRegis(30, 1);
                            Thread.Sleep(20);
                            modbusTCP.WriteSingleRegis(30, 0);
                        }
                        if (mTypex.Contains("R45"))
                        {
                            modbusTCP.WriteSingleRegis(31, 1);
                            Thread.Sleep(20);
                            modbusTCP.WriteSingleRegis(31, 0);
                        }
                    }
                    catch (Exception)
                    {
                    }
                });

            } 
        }

        private ModbusTCP modbusTCP;
        public Dictionary<string, string> DEmployees = new Dictionary<string, string>();
        public Dictionary<string, string> MasterProductInfo = new Dictionary<string, string>();
        private ExcelRW excelRW;
        private UserRepository userRepository;

        //public event EventHandler<(string, string)> POInfo;

        public EmployeesChildViewModel(ModbusTCP modbus, ref ExcelRW _excelRW, UserRepository repository)
        {
            modbusTCP = modbus;
            excelRW = _excelRW;
            userRepository = repository;
        }

        public void FinishPO()
        {
            Username = "";
            PO = "";
        }
    }
}
