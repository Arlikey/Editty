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
        private string currentFilePath = "temp.rtf";
        private string currentFileExtension = ".rtf";

        public async Task OpenFileAsync(object parameter, TextDocument document)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "All files (*.*)|*.*|Text Files (*.txt)|*.txt|Rich Text Format (*.rtf)|*.rtf|PDF Files (*.pdf)|*.pdf"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                string fileExtension = System.IO.Path.GetExtension(filePath).ToLower();

                switch (fileExtension)
                {
                    case ".txt":
                        await OpenTxtFileAsync(filePath, document);
                        currentFilePath = filePath;
                        currentFileExtension = fileExtension;
                        break;
                    case ".rtf":
                        await OpenRtfFileAsync(filePath, document);
                        currentFilePath = filePath;
                        currentFileExtension = fileExtension;
                        break;
                    case ".pdf":
                        await OpenPdfFileAsync(filePath, document);
                        break;
                    default:
                        MessageBox.Show("Неподдерживаемый формат файла.");
                        break;
                }
                
            }
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
            switch (currentFileExtension.ToLower())
            {
                case ".txt":
                    await SaveTxtFileAsync(currentFilePath, document);
                    break;
                case ".rtf":
                    await SaveRtfFileAsync(currentFilePath, document);
                    break;
                /*case ".pdf":
                    await SavePdfFileAsync(filePath, document);
                    break;*/
                default:
                    MessageBox.Show("Неподдерживаемый формат файла.");
                    break;
            }
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

                    // Добавляем текст в PDF
                    pdfDocument.Add(new Paragraph(text));

                    // Закрываем документ
                    pdfDocument.Close();
                }
            });
        }*/
    }
}
