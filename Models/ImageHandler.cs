using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Editty.Models
{
    public class ImageHandler
    {
        double maxImageWidth = 550;
        public void InsertImage(RichTextBox richTextBox)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image Files (*.jpg;*.jpeg;*.png;*.bmp)|*.jpg;*.jpeg;*.png;*.bmp"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                var image = new Image
                {
                    Source = new BitmapImage(new Uri(openFileDialog.FileName))
                };

                var bitmap = (BitmapImage)image.Source;
                double originalWidth = bitmap.PixelWidth;
                double originalHeight = bitmap.PixelHeight;

                if (originalWidth > maxImageWidth)
                {
                    double scale = maxImageWidth / originalWidth;
                    image.Width = maxImageWidth;
                    image.Height = originalHeight * scale;
                }
                else
                {
                    image.Width = originalWidth;
                    image.Height = originalHeight;
                }
                var insertImageUIContainer = new BlockUIContainer(image);

                if (richTextBox.CaretPosition.Paragraph == null)
                {
                    var newParagraph = new Paragraph();
                    richTextBox.Document.Blocks.Add(newParagraph);
                    richTextBox.CaretPosition = newParagraph.ContentEnd;
                }

                try
                {
                    richTextBox.Document.Blocks.InsertAfter(richTextBox.CaretPosition.Paragraph, insertImageUIContainer);
                    richTextBox.CaretPosition = insertImageUIContainer.ContentEnd;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при вставке изображения: {ex.Message}");
                }
            }
        }
    }
}
