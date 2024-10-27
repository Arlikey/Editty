using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows;

namespace Editty.Models
{
    public class TextFormatter
    {
        private readonly RichTextBox _richTextBox;

        public TextFormatter(RichTextBox richTextBox)
        {
            _richTextBox = richTextBox;
        }

        public void ToggleBold()
        {
            EditingCommands.ToggleBold.Execute(null, _richTextBox);
        }

        public void ToggleItalic()
        {
            EditingCommands.ToggleItalic.Execute(null, _richTextBox);
        }

        public void ToggleUnderline()
        {
            EditingCommands.ToggleUnderline.Execute(null, _richTextBox);
        }

        public void ApplyTextColor(Color color)
        {
            ApplyTextFormatting(TextElement.ForegroundProperty, new SolidColorBrush(color));
        }
        public void ApplyBackgroundColor(Color color)
        {
            ApplyTextFormatting(TextElement.BackgroundProperty, new SolidColorBrush(color));
        }
        public void ApplyFontFamily(string fontFamily)
        {
            ApplyTextFormatting(TextElement.FontFamilyProperty, new FontFamily(fontFamily));
        }

        public void ApplyFontSize(double size)
        {
            ApplyTextFormatting(TextElement.FontSizeProperty, size);
        }

        private void ApplyTextFormatting(DependencyProperty property, object value)
        {
            var selectedText = _richTextBox.Selection;
            if (!selectedText.IsEmpty)
            {
                selectedText.ApplyPropertyValue(property, value);
            }
        }
    }
}
