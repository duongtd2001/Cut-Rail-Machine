using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CUT_RAIL_MACHINE.ViewModels;

namespace CUT_RAIL_MACHINE.Views
{
    /// <summary>
    /// Interaction logic for HomeView.xaml
    /// </summary>
    public partial class HomeView : UserControl
    {
        public HomeView()
        {
            InitializeComponent();
            //this.DataContextChanged += HomeView_DataContextChanged;
            this.Loaded += (s, e) =>
            {
                ResultTextBox.TextChanged += (sender, args) =>
                {
                    ResultTextBox.ScrollToEnd();
                };
            };
        }
        //private void HomeView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        //{
        //    if(e.NewValue is HomeViewModel vm)
        //    {
        //        vm.PropertyChanged += (s, args) =>
        //        {
        //            if (args.PropertyName == nameof(vm.IsUnLocker))
        //            {
        //                Dispatcher.Invoke(() =>
        //                {
        //                    if (vm.IsUnLocker)
        //                    {
        //                        OverlayGrid.Visibility = Visibility.Collapsed;
        //                        OverlayGrid.BeginAnimation(UIElement.OpacityProperty, null);
        //                    }
        //                    else
        //                    {
        //                        OverlayGrid.Visibility = Visibility.Visible;
        //                        var animation = new DoubleAnimation
        //                        {
        //                            From = 0.3,
        //                            To = 1,
        //                            Duration = TimeSpan.FromSeconds(1.2),
        //                            AutoReverse = true,
        //                            RepeatBehavior = RepeatBehavior.Forever
        //                        };
        //                        BlinkingText.BeginAnimation(UIElement.OpacityProperty, animation);
        //                    }
        //                });
        //            }
        //        };
        //    }
        //}
    }
}
