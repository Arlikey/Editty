using Editty.Helpers;
using Editty.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace Editty.ViewModels
{
    public class EditorViewModel : INotifyPropertyChanged
    {
        public IEnumerable<FontFamily> FontFamilies { get; set; }
        private TextDocument _document;
        private FileHandler _fileHandler;

        public EditorViewModel()
        {
            FontFamilies = Fonts.SystemFontFamilies;
            _document = new TextDocument();
            _fileHandler = new FileHandler();

            OpenFileCommand = new RelayCommand(OpenFileAsync);
        }

        public ICommand OpenFileCommand { get; }

        public FlowDocument Content
        {
            get => _document.Content;
            set
            {
                _document.Content = value;
                OnPropertyChanged(nameof(Content));
            }
        }

        private async void OpenFileAsync(object parameter)
        {
            await _fileHandler.OpenFileAsync(parameter, _document);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string property = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}