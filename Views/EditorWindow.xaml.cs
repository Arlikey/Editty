using Editty.Helpers;
using Editty.Models;
using Editty.ViewModels;
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
        public EditorWindow()
        {
            InitializeComponent();
            this.DataContext = new EditorViewModel();
        }

        private void TextFormattingControl_MouseEnter(object sender, MouseEventArgs e)
        {
            AnimationHelper.BackgroundColorFade(textFormattingControl, Color.FromArgb(255, 230, 230, 230), 0.25);
        }

        private void TextFormattingControl_MouseLeave(object sender, MouseEventArgs e)
        {
            AnimationHelper.BackgroundColorFade(textFormattingControl, Color.FromArgb(255, 241, 241, 241), 0.25);

        }

        private void textBox_GotFocus(object sender, RoutedEventArgs e)
        {
            AnimationHelper.BackgroundColorFade(textBox, Color.FromArgb(255, 246, 246, 246), 0.25);
        }

        private void textBox_LostFocus(object sender, RoutedEventArgs e)
        {
            AnimationHelper.BackgroundColorFade(textBox, Color.FromArgb(255, 255, 255, 255), 0.25);
        }

        private void textBox_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is RichTextBox richTextBox && e.NewValue is EditorViewModel viewModel)
            {
                richTextBox.Document = viewModel.Content;
            }
        }
    }
}
