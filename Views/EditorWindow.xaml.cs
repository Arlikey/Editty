using Editty.Helpers;
using Editty.Models;
using Editty.UserControls;
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
            this.DataContext = new EditorViewModel(textBox);
            mainControl.Content = new TextFormattingControl();
            textBox.Document.PageWidth = 800;
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

        private void listHandlerButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void mediaHandlerButton_Click(object sender, RoutedEventArgs e)
        {
            mainControl.Content = new MediaHandlerControl();
        }

        private void textFormattingButton_Click(object sender, RoutedEventArgs e)
        {
            mainControl.Content = new TextFormattingControl();
        }
    }
}
