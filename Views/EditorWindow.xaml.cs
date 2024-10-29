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
        private EditorViewModel _viewModel;
        public EditorWindow()
        {
            InitializeComponent();
            textBox.SelectionChanged += TextBox_SelectionChanged;
            _viewModel = new EditorViewModel(textBox);
            this.DataContext = _viewModel;
            mainControl.Content = new TextFormattingControl(_viewModel);
            textBox.Document.PageWidth = 800;
            textBox.SelectionBrush = new SolidColorBrush(Color.FromArgb(255, 0, 171, 40));
        }

        private void TextBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            var selectedText = textBox.Selection;

            var isBold = selectedText.GetPropertyValue(TextElement.FontWeightProperty).Equals(FontWeights.Bold);
            _viewModel.IsBold = isBold;

            var isItalic = selectedText.GetPropertyValue(TextElement.FontStyleProperty).Equals(FontStyles.Italic);
            _viewModel.IsItalic = isItalic;

            if (selectedText.Start.Paragraph != null && selectedText.Text != "")
            {
                var isUnderline = selectedText.GetPropertyValue(Inline.TextDecorationsProperty).Equals(TextDecorations.Underline);
                _viewModel.IsUnderline = isUnderline;
            }
            else
            {
                _viewModel.IsUnderline = false;
            }
            var isAlignedLeft = selectedText.GetPropertyValue(Paragraph.TextAlignmentProperty).Equals(TextAlignment.Left);
            _viewModel.IsAlignedLeft = isAlignedLeft;

            var isAlignedCenter = selectedText.GetPropertyValue(Paragraph.TextAlignmentProperty).Equals(TextAlignment.Center);
            _viewModel.IsAlignedCenter = isAlignedCenter;

            var isAlignedRight = selectedText.GetPropertyValue(Paragraph.TextAlignmentProperty).Equals(TextAlignment.Right);
            _viewModel.IsAlignedRight = isAlignedRight;
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
            mainControl.Content = new ListHandlerControl();
        }

        private void mediaHandlerButton_Click(object sender, RoutedEventArgs e)
        {
            mainControl.Content = new MediaHandlerControl();
        }

        private void textFormattingButton_Click(object sender, RoutedEventArgs e)
        {
            mainControl.Content = new TextFormattingControl(_viewModel);
        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (DataContext is EditorViewModel viewModel)
            {
                viewModel.IsDocumentChanged = true;
            }
        }
    }
}
