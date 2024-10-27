using Editty.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Editty.Views
{
    /// <summary>
    /// Interaction logic for SearchWindow.xaml
    /// </summary>
    public partial class SearchWindow : Window, INotifyPropertyChanged
    {
        private readonly SearchManager _searchManager;
        private bool isFound;
        public bool IsFound
        {
            get => isFound;
            set
            {
                isFound = value;
                OnPropertyChanged(nameof(IsFound));
            }
        }
        public SearchWindow(SearchManager searchManager)
        {
            InitializeComponent();
            _searchManager = searchManager;
            DataContext = this;
            this.Closed += SearchWindow_Closed;
            IsFound = false;
        }

        private void SearchWindow_Closed(object? sender, EventArgs e)
        {
            _searchManager.ClearHighlights();
        }

        private void OnFindAllClick(object sender, RoutedEventArgs e)
        {
            string query = SearchTextBox.Text;
            if (!string.IsNullOrWhiteSpace(query))
            {
                _searchManager.FindAll(query);
                IsFound = true;
            }
            else
            {
                MessageBox.Show("Please enter a search term.", "Search", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void OnNextClick(object sender, RoutedEventArgs e)
        {
            _searchManager.SelectNextMatch();
        }

        private void OnPreviousClick(object sender, RoutedEventArgs e)
        {
            _searchManager.SelectPreviousMatch();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string property = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
