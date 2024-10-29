using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using Editty.Interfaces;
using Editty.UserControls;
using System.Windows.Threading;
using static MaterialDesignThemes.Wpf.Theme;
using Paragraph = System.Windows.Documents.Paragraph;
using Run = System.Windows.Documents.Run;
using iText.Layout;

namespace Editty.Models
{
    public class FileHandler : IFileHandler
    {
        public async Task<bool> CreateFileAsync(object parameter, TextDocument document)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Text File (*.txt)|*.txt|RTF File (*.rtf)|*.rtf",
                DefaultExt = ".txt"
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                string filePath = saveFileDialog.FileName;
                int selectedFilter = saveFileDialog.FilterIndex;
                string fileExtension = System.IO.Path.GetExtension(filePath).ToLower();

                switch (selectedFilter)
                {
                    case 1:
                        if (fileExtension != ".txt")
                        {
                            filePath = System.IO.Path.ChangeExtension(filePath, ".txt");
                        }
                        break;
                    case 2:
                        if (fileExtension != ".rtf")
                        {
                            filePath = System.IO.Path.ChangeExtension(filePath, ".rtf");
                        }
                        break;
                }
                fileExtension = System.IO.Path.GetExtension(filePath).ToLower();

                document.Content.Blocks.Clear();

                switch (fileExtension)
                {
                    case ".txt":
                        await SaveTxtFileAsync(filePath, document);
                        document.FilePath = filePath;
                        document.FileExtension = fileExtension;
                        break;
                    case ".rtf":
                        await SaveRtfFileAsync(filePath, document);
                        document.FilePath = filePath;
                        document.FileExtension = fileExtension;
                        break;
                    default:
                        MessageBox.Show("Неподдерживаемый формат файла.");
                        return false;
                }

                return true;
            }
            if (!document.IsOpen)
            {
                return false;
            }
            return true;
        }
        public async Task<bool> OpenFileAsync(object parameter, TextDocument document)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "All files (*.txt;*.rtf;*.pdf)|*.txt;*.rtf;*.pdf|Text Files (*.txt)|*.txt|Rich Text Format (*.rtf)|*.rtf|PDF Files (*.pdf)|*.pdf"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                string fileExtension = System.IO.Path.GetExtension(filePath).ToLower();


                //  Получаем элемент для отображения к загрузки
                var loadingLabel = Application.Current.MainWindow.FindName("loadingLabel") as LoadingLabelControl;

                loadingLabel.Visibility = Visibility.Visible;
                //  Небольшая заддержка перед тем как основной поток заблокируется, чтобы успел отобразиться элемент
                await Task.Delay(25);

                switch (fileExtension)
                {
                    case ".txt":
                        await OpenTxtFileAsync(filePath, document);
                        document.FilePath = filePath;
                        document.FileExtension = fileExtension;
                        break;
                    case ".rtf":
                        await OpenRtfFileAsync(filePath, document);
                        document.FilePath = filePath;
                        document.FileExtension = fileExtension;
                        break;
                    case ".pdf":
                        await OpenPdfFileAsync(filePath);
                        document.FilePath = filePath;
                        document.FileExtension = fileExtension;
                        break;
                    default:
                        MessageBox.Show("Неподдерживаемый формат файла.");
                        loadingLabel.Visibility = Visibility.Collapsed;
                        return false;
                }
                loadingLabel.Visibility = Visibility.Collapsed;
                return true;
            }
            if (!document.IsOpen)
            {
                return false;
            }
            return true;
        }

        public async Task OpenTxtFileAsync(string filePath, TextDocument document)
        {
            try
            {
                string text = await File.ReadAllTextAsync(filePath);
                document.Content.Blocks.Clear();
                string[] lines = text.Split(Environment.NewLine);

                foreach (string line in lines)
                {
                    Paragraph paragraph = new Paragraph(new Run(line));
                    document.Content.Blocks.Add(paragraph);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Во время открытия файла что-то пошло не так!\nОшибка: {ex.Message}");
            }
        }

        public async Task OpenRtfFileAsync(string filePath, TextDocument document)
        {
            try
            {
                await Task.Run(() =>
                {
                    using (FileStream rtfStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                    {
                        TextRange textRange = new TextRange(document.Content.ContentStart, document.Content.ContentEnd);
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            textRange.Load(rtfStream, DataFormats.Rtf);
                        });
                    }
                    GC.Collect();
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Во время открытия файла что-то пошло не так!\nОшибка: {ex.Message}");
            }
        }

        public async Task OpenPdfFileAsync(string filePath)
        {
            try
            {
                var pdfWebBrowser = Application.Current.MainWindow.FindName("pdfWebBrowser") as WebBrowser;
                if (File.Exists(filePath))
                {
                    var pdfViewerUrl = new Uri(filePath).AbsoluteUri;

                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        pdfWebBrowser.Source = new Uri(pdfViewerUrl);
                    });
                }
                else
                {
                    MessageBox.Show("Во время загрузки файла произошла ошибка.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Во время открытия файла что-то пошло не так!\nОшибка: {ex.Message}");
            }
        }

        public async Task SaveFileAsync(TextDocument document)
        {
            switch (document.FileExtension.ToLower())
            {
                case ".txt":
                    await SaveTxtFileAsync(document.FilePath, document);
                    break;
                case ".rtf":
                    await SaveRtfFileAsync(document.FilePath, document);
                    break;
                default:
                    MessageBox.Show("Неподдерживаемый формат файла.");
                    break;
            }
        }
        public async Task<bool> SaveAsFileAsync(TextDocument document)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Text File (*.txt)|*.txt|RTF File (*.rtf)|*.rtf",
                DefaultExt = ".txt"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                string filePath = saveFileDialog.FileName;
                int selectedFilter = saveFileDialog.FilterIndex;
                string fileExtension = System.IO.Path.GetExtension(filePath).ToLower();

                switch (selectedFilter)
                {
                    case 1:
                        if (fileExtension != ".txt")
                        {
                            filePath = System.IO.Path.ChangeExtension(filePath, ".txt");
                        }
                        break;
                    case 2:
                        if (fileExtension != ".rtf")
                        {
                            filePath = System.IO.Path.ChangeExtension(filePath, ".rtf");
                        }
                        break;
                }

                switch (System.IO.Path.GetExtension(filePath).ToLower())
                {
                    case ".txt":
                        await SaveTxtFileAsync(filePath, document);
                        break;
                    case ".rtf":
                        await SaveRtfFileAsync(filePath, document);
                        break;
                    default:
                        MessageBox.Show("Неподдерживаемый формат файла.");
                        return false;
                }

                document.FilePath = filePath;
                document.FileExtension = System.IO.Path.GetExtension(filePath).ToLower();

                return true;
            }
            return false;
        }
        public async Task SaveTxtFileAsync(string filePath, TextDocument document)
        {
            string text = new TextRange(document.Content.ContentStart, document.Content.ContentEnd).Text;
            await File.WriteAllTextAsync(filePath, text);
        }

        public async Task SaveRtfFileAsync(string filePath, TextDocument document)
        {
            await Task.Run(() =>
            {
                using (FileStream rtfStream = new FileStream(filePath, FileMode.Create))
                {
                    TextRange textRange = new TextRange(document.Content.ContentStart, document.Content.ContentEnd);
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        textRange.Save(rtfStream, DataFormats.Rtf);
                    });
                }
            });
        }
    }
}
