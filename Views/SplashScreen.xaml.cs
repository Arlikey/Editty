using Editty.Models;
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

namespace Editty.Views
{
    /// <summary>
    /// Interaction logic for SplashScreen.xaml
    /// </summary>
    public partial class SplashScreen : BaseWindow
    {
        public SplashScreen()
        {
            InitializeComponent();
            Loaded += SplashScreen_Loaded;
        }

        private void SplashScreen_Loaded(object sender, RoutedEventArgs e)
        {
            DoubleAnimation fadeIn = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(1));
            fadeIn.Completed += FadeIn_Completed;
            splash.BeginAnimation(OpacityProperty, fadeIn);

        }

        private void FadeIn_Completed(object sender, EventArgs e)
        {
            DoubleAnimation fadeOut = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(1));
            fadeOut.BeginTime = TimeSpan.FromSeconds(2);

            fadeOut.Completed += FadeOut_Completed;
            splash.BeginAnimation(OpacityProperty, fadeOut);
        }

        private void FadeOut_Completed(object sender, EventArgs e)
        {
            EditorWindow editorWindow = new EditorWindow();
            editorWindow.Show();

            DoubleAnimation splashDissapear = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(1));
            splashDissapear.Completed += (s, ev) => this.Hide();;
            this.BeginAnimation(OpacityProperty, splashDissapear);
        }

    }
}
