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
    /// Interaction logic for EditorWindow.xaml
    /// </summary>
    public partial class EditorWindow : BaseWindow
    {
        public Editor editor;
        public EditorWindow()
        {
            InitializeComponent();
            editor = new Editor();
            DataContext = editor;
        }

        private void TextFormattingControl_MouseEnter(object sender, MouseEventArgs e)
        {
            AnimationTimeline fadeIn = new ColorAnimation(Color.FromArgb(255, 230, 230, 230), TimeSpan.FromSeconds(0.25));
            textFormattingControl.Background.BeginAnimation(SolidColorBrush.ColorProperty, fadeIn);
        }

        private void TextFormattingControl_MouseLeave(object sender, MouseEventArgs e)
        {
            AnimationTimeline fadeIn = new ColorAnimation(Color.FromArgb(255, 241, 241, 241), TimeSpan.FromSeconds(0.25));
            textFormattingControl.Background.BeginAnimation(SolidColorBrush.ColorProperty, fadeIn);

        }

        private void textBox_GotFocus(object sender, RoutedEventArgs e)
        {
            AnimationTimeline fadeIn = new ColorAnimation(Color.FromArgb(255, 246, 246, 246), TimeSpan.FromSeconds(0.25));
            textBox.Background.BeginAnimation(SolidColorBrush.ColorProperty, fadeIn);
        }

        private void textBox_LostFocus(object sender, RoutedEventArgs e)
        {
            AnimationTimeline fadeIn = new ColorAnimation(Color.FromArgb(255, 255, 255, 255), TimeSpan.FromSeconds(0.25));
            textBox.Background.BeginAnimation(SolidColorBrush.ColorProperty, fadeIn);
        }
    }
}
