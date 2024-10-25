using Editty.Helpers;
using Editty.Models;
using Editty.UserControls;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace Editty.ViewModels
{
    public class EditorViewModel : INotifyPropertyChanged
    {
        public IEnumerable<FontFamily> FontFamilies { get; set; }
        
        private TextDocument _document;
        private FileHandler _fileHandler;
        private ImageHandler _imageHandler;

        public EditorViewModel(RichTextBox textBox)
        {
            FontFamilies = Fonts.SystemFontFamilies;
            TextBox = textBox;
            _document = new TextDocument();
            _fileHandler = new FileHandler();
            _imageHandler = new ImageHandler();

            OpenFileCommand = new RelayCommand(OpenFileAsync);
            SaveFileCommand = new RelayCommand(SaveFileAsync);
            InsertImageCommand = new RelayCommand(InsertImage);
        }

        public ICommand OpenFileCommand { get; }
        public ICommand SaveFileCommand { get; }
        public ICommand InsertImageCommand { get; }

        public FlowDocument Content
        {
            get => _document.Content;
            set
            {
                _document.Content = value;
                OnPropertyChanged(nameof(Content));
            }
        }
        public RichTextBox TextBox { get; set; }
        private async void OpenFileAsync(object parameter)
        {
            await _fileHandler.OpenFileAsync(parameter, _document);
        }

        private async void SaveFileAsync(object parameter)
        {
            await _fileHandler.SaveFileAsync(_document);
        }

        private void InsertImage(object parameter)
        {
            if (parameter is RichTextBox richTextBox)
            {
                {
                    _imageHandler.InsertImage(TextBox);
                }
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string property = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}