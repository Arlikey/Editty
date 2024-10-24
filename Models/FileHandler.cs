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

namespace Editty.Models
{
    public class FileHandler : IFileHandler
    {
        private string currentFilePath;

        public async Task OpenFileAsync(object parameter, TextDocument document)
        {
            if (parameter is RichTextBox richTextBox)
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
                            await OpenTxtFileAsync(richTextBox, filePath, document);
                            break;
                        case ".rtf":
                            await OpenRtfFileAsync(richTextBox, filePath, document);
                            break;
                        case ".pdf":
                            await OpenPdfFileAsync(richTextBox, filePath, document);
                            break;
                        default:
                            MessageBox.Show("Неподдерживаемый формат файла.");
                            break;
                    }
                    currentFilePath = filePath;
                }
            }
        }

        private async Task OpenTxtFileAsync(RichTextBox richTextBox, string filePath, TextDocument document)
        {
            string text = await File.ReadAllTextAsync(filePath);
            document.Content.Blocks.Clear();
            document.Content.Blocks.Add(new Paragraph(new Run(text)));
        }

        private async Task OpenRtfFileAsync(RichTextBox richTextBox, string filePath, TextDocument document)
        {
            await Task.Run(() =>
            {    
                using (FileStream rtfStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        TextRange textRange = new TextRange(document.Content.ContentStart, document.Content.ContentEnd);
                        textRange.Load(rtfStream, DataFormats.Rtf);
                    });
                }
                GC.Collect();
            });
        }

        private async Task OpenPdfFileAsync(RichTextBox richTextBox, string filePath, TextDocument document)
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
    }
}
