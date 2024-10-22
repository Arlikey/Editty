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
        public async Task OpenFileAsync(object parameter)
        {
            if (parameter is RichTextBox richTextBox)
            {
                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Filter = "All files (*.*)|*.*| Text Files (*.txt)|*.txt| Rich Text Format (*.rtf)|*.rtf| PDF Files (*.pdf)|*.pdf"
                };

                if (openFileDialog.ShowDialog() == true)
                {
                    string filePath = openFileDialog.FileName;
                    string fileExtension = System.IO.Path.GetExtension(filePath).ToLower();

                    switch (fileExtension)
                    {
                        case ".txt":
                            await OpenTxtFileAsync(richTextBox, filePath);
                            break;
                        case ".rtf":
                            await OpenRtfFileAsync(richTextBox, filePath);
                            break;
                        case ".pdf":
                            await OpenPdfFileAsync(richTextBox, filePath);
                            break;
                        default:
                            MessageBox.Show("Неподдерживаемый формат файла.");
                            break;
                    }
                    currentFilePath = filePath;
                }
            }
        }
        private async Task OpenTxtFileAsync(RichTextBox richTextBox, string filePath)
        {
            string text = await File.ReadAllTextAsync(filePath);
            richTextBox.Document.Blocks.Clear();
            richTextBox.Document.Blocks.Add(new Paragraph(new Run(text)));
        }
        private async Task OpenRtfFileAsync(RichTextBox richTextBox, string filePath)
        {
            /*string content = await File.ReadAllTextAsync(filePath);
            using (MemoryStream stream = new MemoryStream(Encoding.Default.GetBytes(content)))
            {
                richTextBox.Selection.Load(stream, DataFormats.Rtf);
            }*/

            TextRange rtfRange = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
            using (FileStream rtfStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                rtfRange.Load(rtfStream, DataFormats.Rtf);
            }
        }
        private async Task OpenPdfFileAsync(RichTextBox richTextBox, string filePath)
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
                    richTextBox.Document.Blocks.Clear();
                    richTextBox.Document.Blocks.Add(new Paragraph(new Run(pdfText.ToString())));
                });
            });


        }
    }
}
