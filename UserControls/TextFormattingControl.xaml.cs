using ColorPicker;
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
using System.Windows.Media.Imaging;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Editty.UserControls
{
    /// <summary>
    /// Interaction logic for TextFormattingControl.xaml
    /// </summary>
    public partial class TextFormattingControl : BaseAsideControl
    {
        private EditorViewModel _viewModel;
        public TextFormattingControl(EditorViewModel editorViewModel)
        {
            InitializeComponent();
            _viewModel = editorViewModel;
            DataContext = _viewModel;
            backgroundColorPicker.ColorChanged += SquarePicker_ColorChanged;
            fontSizeNumeric.ValueChanged += fontSizeNumeric_ValueChanged;
            
        }

        private void SquarePicker_ColorChanged(object sender, RoutedEventArgs e)
        {
            _viewModel.BackgroundColor = new SolidColorBrush(backgroundColorPicker.SelectedColor);
        }

        private void fontSizeNumeric_ValueChanged(object sender, RoutedPropertyChangedEventArgs<int> e)
        {
            _viewModel.ApplyFontSizeCommand.Execute(e.NewValue);
        }

        private void foregroundColorPicker_ColorChanged(object sender, RoutedEventArgs e)
        {
            var colorPicker = sender as SquarePicker;
            if (colorPicker != null)
            {
                var selectedColor = colorPicker.SelectedColor;
                _viewModel.ChangeTextColorCommand.Execute(selectedColor);
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            if (comboBox != null)
            {
                var selectedFamily = comboBox.SelectedItem;
                _viewModel.ChangeFontFamilyCommand.Execute(selectedFamily);
            }
        }
    }
}
