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

namespace Editty.Models
{
    public class FileHandler : IFileHandler
    {
        public async Task<bool> CreateFileAsync(object parameter, TextDocument document)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Text File (*.txt)|*.txt|RTF File (*.rtf)|*.rtf|PDF File (*.pdf)|*.pdf",
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
                    case 3:
                        if (fileExtension != ".pdf")
                        {
                            filePath = System.IO.Path.ChangeExtension(filePath, ".pdf");
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
                    /*case ".pdf":
                        await SavePdfFileAsync(filePath, document);
                        break;*/
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
                        await OpenPdfFileAsync(filePath, document);
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

        private async Task OpenTxtFileAsync(string filePath, TextDocument document)
        {
            string text = await File.ReadAllTextAsync(filePath);
            document.Content.Blocks.Clear();
            document.Content.Blocks.Add(new Paragraph(new Run(text)));
        }

        private async Task OpenRtfFileAsync(string filePath, TextDocument document)
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

        private async Task OpenPdfFileAsync(string filePath, TextDocument document)
        {
            StringBuilder pdfText = new StringBuilder();
            await Task.Run(() =>
            {
                using (PdfReader pdfReader = new PdfReader(filePath))
                {
                    using (PdfDocument pdfDocument = new PdfDocument(pdfReader))
                    {
                        for (int page = 1; page <= pdfDocument.GetNumberOfPages(); page++)
                        {
                            string currentText = PdfTextExtractor.GetTextFromPage(pdfDocument.GetPage(page));
                            pdfText.AppendLine(currentText);
                        }
                    }
                }

                Application.Current.Dispatcher.Invoke(() =>
                {
                    document.Content.Blocks.Clear();
                    document.Content.Blocks.Add(new Paragraph(new Run(pdfText.ToString())));
                });
            });
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
                /*case ".pdf":
                    await SavePdfFileAsync(filePath, document);
                    break;*/
                default:
                    MessageBox.Show("Неподдерживаемый формат файла.");
                    break;
            }
        }
        public async Task<bool> SaveAsFileAsync(TextDocument document)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Text File (*.txt)|*.txt|RTF File (*.rtf)|*.rtf|PDF File (*.pdf)|*.pdf",
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
                    case 3:
                        if (fileExtension != ".pdf")
                        {
                            filePath = System.IO.Path.ChangeExtension(filePath, ".pdf");
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
                    /*case ".pdf":
                        await SavePdfFileAsync(filePath, document);
                        break;*/
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
        private async Task SaveTxtFileAsync(string filePath, TextDocument document)
        {
            string text = new TextRange(document.Content.ContentStart, document.Content.ContentEnd).Text;
            await File.WriteAllTextAsync(filePath, text);
        }

        private async Task SaveRtfFileAsync(string filePath, TextDocument document)
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

        /*private async Task SavePdfFileAsync(string filePath, TextDocument document)
        {
            await Task.Run(() =>
            {
                using (PdfWriter writer = new PdfWriter(filePath))
                using (PdfDocument pdfDoc = new PdfDocument(writer))
                {
                    Document pdfDocument = new Document(pdfDoc);
                    string text = new TextRange(document.ContentStart, document.ContentEnd).Text;

                    pdfDocument.Add(new Paragraph(text));

                    pdfDocument.Close();
                }
            });
        }*/
    }
}
