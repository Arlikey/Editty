using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Editty.Models
{
    public class BaseWindow : Window
    {
        public BaseWindow() {
            MinHeight = 400;
            MinWidth = 400;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            Width = 1200;
            Height = 800;
            Title = "Eddity";
            Icon = new BitmapImage(new Uri("pack://application:,,,/images/logo/logo.ico"));
        }

    }
}
