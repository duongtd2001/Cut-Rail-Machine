using Caliburn.Micro;
using CUT_RAIL_MACHINE.ViewModels;
using System.Windows;
using System;
using System.Linq;
using System.ComponentModel;
using System.Xml.Linq;

namespace CUT_RAIL_MACHINE.Services
{
    public class Bootstrapper : BootstrapperBase
    {
       private SimpleContainer _container = new SimpleContainer();
        public Bootstrapper() => Initialize();

        protected override void Configure()
        {
            _container.Instance(_container);

            // Đăng ký services
            _container
                .Singleton<IWindowManager, WindowManager>()
                .Singleton<IEventAggregator, EventAggregator>();
            // Đăng ký ViewModels
            GetType().Assembly.GetTypes()
                 .Where(type => type.IsClass)
                 .Where(type => type.Name.EndsWith("ViewModel"))
                 .ToList()
                 .ForEach(viewModelType => _container.RegisterPerRequest(
                     viewModelType, viewModelType.ToString(), viewModelType
                     ));
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            base.OnStartup(sender, e);
            DisplayRootViewForAsync<MainViewModel>();
        }

        protected override object GetInstance(Type service, string key)
            => _container.GetInstance(service, key);
    }
}
