using Editty.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Editty.Interfaces
{
    public interface IFileHandler
    {
        public Task<bool> CreateFileAsync(object parameter, TextDocument document);
        public Task<bool> OpenFileAsync(object parameter, TextDocument document);
        public Task OpenTxtFileAsync(string filePath, TextDocument document);
        public Task OpenRtfFileAsync(string filePath, TextDocument document);
        public Task OpenPdfFileAsync(string filePath, TextDocument document);
        public Task SaveFileAsync(TextDocument document);
        public Task<bool> SaveAsFileAsync(TextDocument document);
        public Task SaveTxtFileAsync(string filePath, TextDocument document);
        public Task SaveRtfFileAsync(string filePath, TextDocument document);
        public Task SavePdfFileAsync(string filePath, TextDocument document);
    }
}
