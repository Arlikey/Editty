using Editty.Helpers;
using Editty.Models;
using Editty.UserControls;
using Editty.Views;
using iText.IO.Font.Constants;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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

        private TextDocument _document;
        private FileHandler _fileHandler;
        private ImageHandler _imageHandler;
        private ListHandler _listHandler;
        private TextFormatter _textFormatter;
        private SearchManager _searchManager;

        public int DefaultFontSize;
        public IEnumerable<FontFamily> FontFamilies { get; set; }
        public EditorViewModel(RichTextBox textBox)
        {
            DefaultFontSize = 12;
            CurrentFontSize = DefaultFontSize;
            FontFamilies = Fonts.SystemFontFamilies;

            TextBox = textBox;
            _document = new TextDocument();
            _fileHandler = new FileHandler();
            _imageHandler = new ImageHandler(textBox);
            _listHandler = new ListHandler(textBox);
            _textFormatter = new TextFormatter(textBox);
            _searchManager = new SearchManager(textBox);


            CreateFileCommand = new RelayCommand(CreateFileAsync);
            OpenFileCommand = new RelayCommand(OpenFileAsync);
            SaveFileCommand = new RelayCommand(SaveFileAsync, CanExecute);
            SaveAsFileCommand = new RelayCommand(SaveAsFileAsync, CanExecute);
            InsertImageCommand = new RelayCommand(InsertImage, CanExecute);
            ToggleBoldCommand = new RelayCommand(ApplyBold);
            ToggleItalicCommand = new RelayCommand(ApplyItalic);
            ToggleUnderlineCommand = new RelayCommand(ApplyUnderline);
            ToggleAlignLeftCommand = new RelayCommand(ApplyAlignLeft);
            ToggleAlignCenterCommand = new RelayCommand(ApplyAlignCenter);
            ToggleAlignRightCommand = new RelayCommand(ApplyAlignRight);
            ApplyFontSizeCommand = new RelayCommand(ApplyFontSize);
            ChangeTextColorCommand = new RelayCommand(ApplyFontColor);
            ChangeFontFamilyCommand = new RelayCommand(ApplyFontFamily);
            CreateOrderedListCommand = new RelayCommand(CreateOrderedList, CanExecute);
            CreateUnorderedListCommand = new RelayCommand(CreateUnorderedList, CanExecute);
            FindSubstringCommand = new RelayCommand(FindSubstring);
        }

        public ICommand CreateFileCommand { get; }
        public ICommand OpenFileCommand { get; }
        public ICommand SaveFileCommand { get; }
        public ICommand SaveAsFileCommand { get; }
        public ICommand InsertImageCommand { get; }
        public ICommand ToggleBoldCommand { get; }
        public ICommand ToggleItalicCommand { get; }
        public ICommand ToggleUnderlineCommand { get; }
        public ICommand ToggleAlignLeftCommand { get; }
        public ICommand ToggleAlignCenterCommand { get; }
        public ICommand ToggleAlignRightCommand { get; }
        public ICommand ApplyFontSizeCommand { get; }
        public ICommand ChangeTextColorCommand { get; }
        public ICommand ChangeFontSizeCommand { get; }
        public ICommand ChangeFontFamilyCommand { get; }
        public ICommand CreateOrderedListCommand { get; }
        public ICommand CreateUnorderedListCommand { get; }
        public ICommand FindSubstringCommand { get; }
        public FlowDocument Content
        {
            get => _document.Content;
            set
            {
                _document.Content = value;
                OnPropertyChanged(nameof(Content));
            }
        }
        public bool DocumentIsOpen
        {
            get => _document.IsOpen;
            set
            {
                _document.IsOpen = value;
                OnPropertyChanged(nameof(DocumentIsOpen));
            }
        }
        public string FileName
        {
            get => _isDocumentChanged ? $"{Path.GetFileName(_document.FilePath)}*" : Path.GetFileName(_document.FilePath);
            set
            {
                _document.FileName = value;
                OnPropertyChanged(nameof(FileName));
            }
        }
        private bool _isDocumentChanged;
        public bool IsDocumentChanged
        {
            get => _isDocumentChanged;
            set
            {
                _isDocumentChanged = value;
                OnPropertyChanged(nameof(IsDocumentChanged));
                OnPropertyChanged(nameof(FileName));
            }
        }
        private bool _isBold;
        public bool IsBold
        {
            get => _isBold;
            set
            {
                _isBold = value;
                OnPropertyChanged(nameof(IsBold));
            }
        }
        private bool _isItalic;
        public bool IsItalic
        {
            get => _isItalic;
            set
            {
                _isItalic = value;
                OnPropertyChanged(nameof(IsItalic));
            }
        }
        private bool _isUnderline;
        public bool IsUnderline
        {
            get => _isUnderline;
            set
            {
                _isUnderline = value;
                OnPropertyChanged(nameof(IsUnderline));
            }
        }
        private bool _isAlignedLeft;
        public bool IsAlignedLeft
        {
            get => _isAlignedLeft;
            set
            {
                _isAlignedLeft = value;
                if (_isAlignedLeft)
                {
                    IsAlignedCenter = false;
                    IsAlignedRight = false;
                }
                OnPropertyChanged(nameof(IsAlignedLeft));
            }
        }
        private bool _isAlignedCenter;
        public bool IsAlignedCenter
        {
            get => _isAlignedCenter;
            set
            {
                _isAlignedCenter = value;
                if (_isAlignedCenter)
                {
                    IsAlignedLeft = false;
                    IsAlignedRight = false;
                }
                OnPropertyChanged(nameof(IsAlignedCenter));
            }
        }
        private bool _isAlignedRight;
        public bool IsAlignedRight
        {
            get => _isAlignedRight;
            set
            {
                _isAlignedRight = value;
                if (_isAlignedRight)
                {
                    IsAlignedLeft = false;
                    IsAlignedCenter = false;
                }
                OnPropertyChanged(nameof(IsAlignedRight));
            }
        }
        private double _currentFontSize;
        public double CurrentFontSize
        {
            get => _currentFontSize;
            set
            {
                _currentFontSize = value;
                OnPropertyChanged(nameof(CurrentFontSize));
            }
        }
        private SolidColorBrush _backgroundColor = Brushes.White;
        public SolidColorBrush BackgroundColor
        {
            get => _backgroundColor;
            set
            {
                _backgroundColor = value;
                OnPropertyChanged(nameof(BackgroundColor));
            }
        }
        private Color _foregroundColor = Colors.Black;
        public Color ForegroundColor
        {
            get => _foregroundColor;
            set
            {
                _foregroundColor = value;
                OnPropertyChanged(nameof(ForegroundColor));
            }
        }
        private FontFamily _fontFamily = new FontFamily("Arial");
        public FontFamily CurrentFontFamily
        {
            get => _fontFamily;
            set
            {
                _fontFamily = value;
                OnPropertyChanged(nameof(CurrentFontFamily));
            }
        }
        private bool CanExecute(object parameter) => DocumentIsOpen;
        public RichTextBox TextBox { get; set; }
        private async void CreateFileAsync(object parameter)
        {
            DocumentIsOpen = await _fileHandler.CreateFileAsync(parameter, _document);
            IsDocumentChanged = false;
        }
        private async void OpenFileAsync(object parameter)
        {
            DocumentIsOpen = await _fileHandler.OpenFileAsync(parameter, _document);
            IsDocumentChanged = false;
        }
        private async void SaveFileAsync(object parameter)
        {
            await _fileHandler.SaveFileAsync(_document);
            IsDocumentChanged = false;
        }
        private async void SaveAsFileAsync(object parameter)
        {
            await _fileHandler.SaveAsFileAsync(_document);
            IsDocumentChanged = false;
        }
        private void InsertImage(object parameter)
        {
            _imageHandler.InsertImage();
        }
        private void CreateOrderedList(object parameter)
        {
            _listHandler.CreateOrderedList();
        }
        private void CreateUnorderedList(object parameter)
        {
            _listHandler.CreateUnorderedList();
        }
        private void FindSubstring(object parameter)
        {
            SearchWindow searchWindow = new SearchWindow(_searchManager);
            searchWindow.Owner = Application.Current.MainWindow;
            searchWindow.Show();
        }
        private void ApplyBold(object parameter)
        {
            _textFormatter.ToggleBold();
        }
        private void ApplyItalic(object parameter)
        {
            _textFormatter.ToggleItalic();
        }
        private void ApplyUnderline(object parameter)
        {
            _textFormatter.ToggleUnderline();
        }
        private void ApplyAlignLeft(object parameter)
        {
            _textFormatter.ToggleAlignLeft();
        }
        private void ApplyAlignCenter(object parameter)
        {
            _textFormatter.ToggleAlignCenter();
        }
        private void ApplyAlignRight(object parameter)
        {
            _textFormatter.ToggleAlignRight();
        }
        private void ApplyFontSize(object parameter)
        {
            _textFormatter.ApplyFontSize(CurrentFontSize);
        }
        private void ApplyFontColor(object parameter)
        {
            _textFormatter.ApplyTextColor(ForegroundColor);
        }
        private void ApplyFontFamily(object parameter)
        {
            _textFormatter.ApplyFontFamily(CurrentFontFamily);
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string property = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}