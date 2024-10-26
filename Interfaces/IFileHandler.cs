using Editty.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Editty.Interfaces
{
    interface IFileHandler
    {
        public Task<bool> OpenFileAsync(object parameter, TextDocument document);
    }
}
