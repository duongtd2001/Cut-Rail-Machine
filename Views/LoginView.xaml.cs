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
using System.Windows.Shapes;

namespace CUT_RAIL_MACHINE.Views
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : Window
    {
        public LoginView()
        {
            InitializeComponent();
            Storyboard fadeIn = (Storyboard)this.Resources["FadeInStoryboard"];
            fadeIn.Begin(this);
            StateChanged += Window_StateChanged;
        }

        public void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }
        public void MinimizeWithFade()
        {
            Storyboard fadeOut = (Storyboard)this.Resources["FadeOutStoryboard"];
            fadeOut.Completed += (s, e) =>
            {
                this.WindowState = WindowState.Minimized;
                this.Opacity = 1;
            };
            fadeOut.Begin(this);
        }
        public void CloseWithFade()
        {
            Storyboard fadeOut = (Storyboard)this.Resources["FadeOutStoryboard"];
            fadeOut.Completed += (s, e) =>
            {
                this.Close();
            };
            fadeOut.Begin(this);
        }
        private void Window_StateChanged(object sender, EventArgs e)
        {
            Storyboard fadeIn = (Storyboard)this.Resources["FadeInStoryboard"];
            fadeIn.Begin(this);
        }
    }
}
