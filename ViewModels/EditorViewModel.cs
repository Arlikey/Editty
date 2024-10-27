﻿using Editty.Helpers;
using Editty.Models;
using Editty.UserControls;
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

            CreateFileCommand = new RelayCommand(CreateFileAsync);
            OpenFileCommand = new RelayCommand(OpenFileAsync);
            SaveFileCommand = new RelayCommand(SaveFileAsync, CanExecute);
            SaveAsFileCommand = new RelayCommand(SaveAsFileAsync, CanExecute);
            InsertImageCommand = new RelayCommand(InsertImage, CanExecute);
        }

        public ICommand CreateFileCommand { get; }
        public ICommand OpenFileCommand { get; }
        public ICommand SaveFileCommand { get; }
        public ICommand SaveAsFileCommand { get; }
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